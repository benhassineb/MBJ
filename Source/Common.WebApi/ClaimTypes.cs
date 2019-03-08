namespace Common.WebApi
{
    /// <summary>
    /// Décrit les types de claims utilisés dans les identités et id token.
    /// </summary>
    public static class ClaimTypes
    {
        /// <summary>
        /// Le claim PreferredUserName utilisé par Azure B2B contient l'email utilisé pour la connexion.
        /// Voir <a href="https://docs.microsoft.com/en-us/azure/active-directory/develop/id-tokens">ID tokens</a>
        /// </summary>
        /// 
        public const string PreferredUserName = "preferred_username";

        /// <summary>
        /// Le claim Emails utilisé par Azure B2C contient l'email utilisé pour la connexion.
        /// Voir <a href="https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-reference-tokens">ID tokens</a>
        /// </summary>
        /// 
        public const string Email = "emails";

    }
}
