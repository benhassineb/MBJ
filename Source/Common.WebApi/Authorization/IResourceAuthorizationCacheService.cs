namespace Common.WebApi.Authorization
{
    /// <summary>
    /// Décrit le service de cache des autorisations d'accès d'un utilisateur à une ressource.
    /// </summary>
    public interface IResourceAuthorizationCacheService
    {
        /// <summary>
        /// Efface le cache des habilitations.
        /// </summary>
        void Clear();
    }
}