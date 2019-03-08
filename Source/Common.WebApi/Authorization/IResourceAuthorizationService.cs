namespace Common.WebApi.Authorization
{
    /// <summary>
    /// Décrit le service d'autorisation d'accès d'un utilisateur à une ressource.
    /// </summary>
    public interface IResourceAuthorizationService
    {
        /// <summary>
        /// Récupère les droits effectifs de l'utilisateur spécifié sur la ressource spécifiée.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur.</param>
        /// <param name="resourceId">L'identifiant de la ressource.</param>
        /// <returns>Les droits effectifs.</returns>
        ResourceGrant GetGrantsForResource(string userId, string resourceId);
    }
}