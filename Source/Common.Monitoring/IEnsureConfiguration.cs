namespace Common.Monitoring
{
    public interface IEnsureConfiguration
    {
        /// <summary>
        /// Obtient la chaîne de connexion dont le nom est spécifié ou génère une exception <see cref="ApplicationConfigurationException"/>.
        /// </summary>
        /// <param name="keyname">Le nom de la clé.</param>
        /// <exception cref="ApplicationConfigurationException">Exception si la chaîne de connexion est vide.</exception>
        /// <returns>La chaîne de connexion.</returns>
        string GetConnectionString(string keyname);

        /// <summary>
        /// Obtient la données de configuration obligatoire dont le nom est spécifié ou génère une exception <see cref="ApplicationConfigurationException"/>.
        /// </summary>
        /// <param name="keyname">Le nom de la clé.</param>
        /// <exception cref="ApplicationConfigurationException">Exception si la donnée de configuration est vide.</exception>
        /// <returns>La donnée de configuration.</returns>
        T GetApplicationSetting<T>(string keyname);

        /// <summary>
        /// Obtient la données de configuration optionnelle dont le nom est spécifié ou la valeur par défaut spécifiée sinon.
        /// </summary>
        /// <param name="keyname">Le nom de la clé.</param>
        /// <param name="defaultValue">La valeur par défaut.</param>
        /// <returns>La donnée de configuration.</returns>
        T GetOptionalApplicationSetting<T>(string keyname, T defaultValue = default(T));
    }
}
