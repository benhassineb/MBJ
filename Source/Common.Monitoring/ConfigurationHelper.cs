using System;
using System.Configuration;
using System.Globalization;
using Common.Resources;

namespace Common.Monitoring
{
    /// <summary>
    /// Définit un ensemble de méthodes pour gérer les paramètres de configuration.
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Obtient la chaîne de connexion dont le nom est spécifié ou génère une exception précise.
        /// </summary>
        /// <param name="keyName">Le nom de la clé.</param>
        /// <exception cref="ApplicationConfigurationException">Exception si la chaîne de connexion est vide.</exception>
        /// <returns>La chaîne de connexion.</returns>
        public static string GetConnectionString(string keyName)
        {
            if (keyName == null) throw new ArgumentNullException(nameof(keyName));
            var connectionString = ConfigurationManager.ConnectionStrings[keyName]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ApplicationConfigurationException(ErrorCodes.Application.InvalidConfiguration, string.Format(CultureInfo.CurrentCulture, InternalMessages.ApplicationConfigurationMissingConnectionString, keyName));
            return connectionString;
        }

        /// <summary>
        /// Obtient la données de configuration obligatoire dont le nom est spécifié ou génère une exception précise.
        /// </summary>
        /// <param name="keyName">Le nom de la clé.</param>
        /// <exception cref="ApplicationConfigurationException">Exception si la donnée de configuration est vide.</exception>
        /// <returns>La donnée de configuration.</returns>
        public static T GetApplicationSetting<T>(string keyName)
        {
            if (keyName == null) throw new ArgumentNullException(nameof(keyName));
            string stringValue = ConfigurationManager.AppSettings[keyName];
            if (stringValue == null) throw new ApplicationConfigurationException(ErrorCodes.Application.InvalidConfiguration, string.Format(CultureInfo.CurrentCulture, InternalMessages.ApplicationConfigurationMissingSetting, keyName));
            try
            {
                return (T)Convert.ChangeType(stringValue, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                if (ex is InvalidCastException || ex is FormatException || ex is OverflowException)
                    throw new ApplicationConfigurationException(ErrorCodes.Application.InvalidConfiguration, string.Format(CultureInfo.CurrentCulture, InternalMessages.ApplicationConfigurationBadValue, keyName, stringValue), innerException: ex);
                throw;
            }
        }

        /// <summary>
        /// Obtient la données de configuration optionnelle dont le nom est spécifié ou la valeur par défaut spécifiée sinon.
        /// </summary>
        /// <param name="keyName">Le nom de la clé.</param>
        /// <param name="defaultValue">La valeur par défaut.</param>
        /// <returns>La donnée de configuration.</returns>
        public static T GetOptionalApplicationSetting<T>(string keyName, T defaultValue = default(T))
        {
            if (keyName == null) throw new ArgumentNullException(nameof(keyName));
            string stringValue = ConfigurationManager.AppSettings[keyName];
            if (stringValue == null) return defaultValue;
            try
            {
                return (T)Convert.ChangeType(stringValue, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                if (ex is InvalidCastException || ex is FormatException || ex is OverflowException)
                    throw new ApplicationConfigurationException(ErrorCodes.Application.InvalidConfiguration, string.Format(CultureInfo.CurrentCulture, InternalMessages.ApplicationConfigurationBadValue, keyName, stringValue), innerException: ex);
                throw;
            }
        }

    }
}
