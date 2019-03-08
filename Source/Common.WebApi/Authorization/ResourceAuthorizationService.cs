using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Common.Monitoring;
using Dapper;

namespace Common.WebApi.Authorization
{
    /// <summary>
    /// Implémente le service d'autorisation d'accès d'un utilisateur à une ressource.
    /// </summary>
    public class ResourceAuthorizationService : IResourceAuthorizationService, IResourceAuthorizationCacheService
    {
        private static readonly ConcurrentDictionary<string, ResourceGrant> UserGrantsDictionnary = new ConcurrentDictionary<string, ResourceGrant>();
        private static readonly Func<string, string, string> Key = (email, resource) => $"{email}¤{resource}";

        private const string ConnectionStringName = "Hermes";
        private readonly string _connectionString;

        /// <summary>
        /// Crée une instance de <see cref="ResourceAuthorizationService"/> avec la configuration spécifiée.
        /// </summary>
        /// <param name="configuration">La configuration du <see cref="ResourceAuthorizationService"/></param>
        public ResourceAuthorizationService(IEnsureConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(ConnectionStringName);
        }


        /// <summary>
        /// Récupère les droits effectifs de l'utilisateur spécifié sur la ressource spécifiée.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur.</param>
        /// <param name="resourceId">L'identifiant de la ressource.</param>
        /// <returns>Les droits effectifs.</returns>
        public ResourceGrant GetGrantsForResource(string userId, string resourceId)
        {
            ResourceGrant resourgrants;
            var key = Key(userId, resourceId);
            if (UserGrantsDictionnary.TryGetValue(key, out resourgrants))
            {
                return resourgrants;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var userGrants = connection.Query<UserGrant>("HABILITATION.GetUtilisateurDroits",
                    new
                    {
                        Email = userId,
                        c_fonc = resourceId
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                resourgrants = userGrants?.ToResourceGrant() ?? ResourceGrant.None;
                UserGrantsDictionnary.TryAdd(key, resourgrants);
                return resourgrants;
            }
        }

        /// <summary>
        /// Efface le cache des habilitations.
        /// </summary>
        public void Clear()
        {
            UserGrantsDictionnary.Clear();
        }
    }
}
