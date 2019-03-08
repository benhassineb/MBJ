using System;
using System.Data.SqlClient;
using Common.Monitoring;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Hermes.DataAccess.Commun
{
    /// <summary>
    /// Implémente les méthodes d'accès aux données "Communes"
    /// </summary>
    public class CommunRepository: ICommunRepository
    {
        private const string ConnectionStringName = "Hermes";
        private readonly string _connectionString;

        /// <summary>
        /// Crée une instance de <see cref="CommunRepository"/> avec la configuration spécifiée.
        /// </summary>
        /// <param name="configuration">La configuration du <see cref="CommunRepository"/></param>
        public CommunRepository(IEnsureConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(ConnectionStringName);
        }

        /// <summary>
        /// Retourne l'heure actuelle UTC définie au niveau du serveur de données.
        /// </summary>
        /// <returns></returns>
        public DateTime GetUtcNow()
        {
            string strSql = "SELECT GetUtcDate()";
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<DateTime>(strSql);
            }
        }
    }
}
