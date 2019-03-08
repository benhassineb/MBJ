using System;

namespace Common.WebApi.Authorization
{
    /// <summary>
    /// Définit les autorisations d'accès à une ressource.
    /// </summary>
    [Flags]
    public enum ResourceGrant
    {
        /// <summary>
        /// Aucune autorisation requise.
        /// </summary>
        None = 0,
        /// <summary>
        /// Autorisation de lecture requise.
        /// </summary>
        Read = 1,
        /// <summary>
        /// Autorisation de modification requise.
        /// </summary>
        Update = 2,
        /// <summary>
        /// Autorisation de suppression requise.
        /// </summary>
        Delete = 4,
        /// <summary>
        /// Autorisation de création requise.
        /// </summary>
        Create = 8
    }
}
