using Common.Resources;
using System;
using System.Configuration;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace Common.Monitoring
{
    /// <summary>
    /// Implémente un ensemble de méthodes pour obtenir et contrôler les paramètres de configuration.
    /// </summary>
    public class EnsureConfiguration : IEnsureConfiguration
    {
        private readonly Func<string, string> _getConnectionStringFunc;
        private readonly Func<string, string> _getApplicationSettingFunc;

        /// <summary>
        /// Crée une instance de <see cref="EnsureConfiguration"/> avec une dépendance à <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="configuration">La configuration ASP.NET Core.</param>
        public EnsureConfiguration(IConfiguration configuration)
        {
            if (configuration != null)
            {
                _getConnectionStringFunc = configuration.GetConnectionString;
                _getApplicationSettingFunc = keyname => configuration[keyname];
            }
            else
            {
                _getConnectionStringFunc = keyname => ConfigurationManager.ConnectionStrings[keyname]?.ConnectionString;
                _getApplicationSettingFunc = keyname => ConfigurationManager.AppSettings[keyname];
            }
        }

        public EnsureConfiguration(): this(null) { }

        /// <summary>
        /// Obtient la chaîne de connexion dont le nom est spécifié ou génère une exception précise.
        /// </summary>
        /// <param name="keyname">Le nom de la clé.</param>
        /// <exception cref="ApplicationConfigurationException">Exception si la chaîne de connexion est vide.</exception>
        /// <returns>La chaîne de connexion.</returns>
        public string GetConnectionString(string keyname)
        {
            if (keyname == null) throw new ArgumentNullException(nameof(keyname));
            var connectionString = _getConnectionStringFunc(keyname);
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ApplicationConfigurationException(ErrorCodes.Application.InvalidConfiguration, string.Format(CultureInfo.CurrentCulture, InternalMessages.ApplicationConfigurationMissingConnectionString, keyname));
            return connectionString;
        }

        /// <summary>
        /// Obtient la données de configuration obligatoire dont le nom est spécifié ou génère une exception précise.
        /// </summary>
        /// <param name="keyname">Le nom de la clé.</param>
        /// <exception cref="ApplicationConfigurationException">Exception si la donnée de configuration est vide.</exception>
        /// <returns>La donnée de configuration.</returns>
        public T GetApplicationSetting<T>(string keyname)
        {
            if (keyname == null) throw new ArgumentNullException(nameof(keyname));
            string stringValue = _getApplicationSettingFunc(keyname);
            if (stringValue == null) throw new ApplicationConfigurationException(ErrorCodes.Application.InvalidConfiguration, string.Format(CultureInfo.CurrentCulture, InternalMessages.ApplicationConfigurationMissingSetting, keyname));
            try
            {
                return (T)Convert.ChangeType(stringValue, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                if (ex is InvalidCastException || ex is FormatException || ex is OverflowException)
                    throw new ApplicationConfigurationException(ErrorCodes.Application.InvalidConfiguration, string.Format(CultureInfo.CurrentCulture, InternalMessages.ApplicationConfigurationBadValue, keyname, stringValue), innerException: ex);
                throw;
            }
        }

        /// <summary>
        /// Obtient la données de configuration optionnelle dont le nom est spécifié ou la valeur par défaut spécifiée sinon.
        /// </summary>
        /// <param name="keyname">Le nom de la clé.</param>
        /// <param name="defaultValue">La valeur par défaut.</param>
        /// <returns>La donnée de configuration.</returns>
        public T GetOptionalApplicationSetting<T>(string keyname, T defaultValue = default(T))
        {
            if (keyname == null) throw new ArgumentNullException(nameof(keyname));
            string stringValue = _getApplicationSettingFunc(keyname);
            if (stringValue == null) return defaultValue;
            try
            {
                return (T)Convert.ChangeType(stringValue, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                if (ex is InvalidCastException || ex is FormatException || ex is OverflowException)
                    throw new ApplicationConfigurationException(ErrorCodes.Application.InvalidConfiguration, string.Format(CultureInfo.CurrentCulture, InternalMessages.ApplicationConfigurationBadValue, keyname, stringValue), innerException: ex);
                throw;
            }
        }

    }

}
