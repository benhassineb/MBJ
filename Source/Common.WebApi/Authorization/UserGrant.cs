namespace Common.WebApi.Authorization
{
    /// <summary>
    /// Le modèle correspondant à la table [HAB_DROITS].
    /// </summary>
    public class UserGrant
    {
#pragma warning disable CA1707 // Identifiers should not contain underscores
        public decimal I_PROFIL { get; set; }
        public decimal I_FONC { get; set; }
        public bool B_CONSULT { get; set; }
        public bool B_CREATE { get; set; }
        public bool B_UPDATE { get; set; }
        public bool B_DELETE { get; set; }
        public bool B_EXEC { get; set; }
        public string C_SPEC { get; set; }
#pragma warning restore CA1707 // Identifiers should not contain underscores

        /// <summary>
        /// Convertit le modèle <see cref="UserGrant"/> en une <see cref="ResourceGrant"/>.
        /// </summary>
        /// <returns></returns>
        public ResourceGrant ToResourceGrant()
        {
            ResourceGrant result = B_CONSULT ? ResourceGrant.Read : 0;
            result |= B_CREATE ? ResourceGrant.Create : 0;
            result |= B_UPDATE ? ResourceGrant.Update : 0;
            result |= B_DELETE ? ResourceGrant.Delete : 0;
            return result;
        }
    }
}