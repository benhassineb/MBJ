using System.Collections.Generic;
using System.Linq;
using Common.Resources;


namespace Common.WebApi
{
    /// <summary>
    /// Définit la remontée des erreurs de validations des modèles de la Web API à l'appelant.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Crée une instance de <see cref="ValidationError"/> avec un message par défaut.
        /// </summary>
        public ValidationError()
        {
        }

        /// <summary>
        /// Crée une instance de <see cref="ValidationError"/> avec la collection d'erreurs spécifiées.
        /// </summary>
        /// <param name="messages"></param>
        public ValidationError(IEnumerable<string> messages)
        {
            Errors = new List<string>(messages);
        }

        /// <summary>
        /// Message global pour l'utilisateur.
        /// </summary>
        public string Message { get; } = InternalMessages.ValidationError;

        /// <summary>
        /// Liste des erreurs de validation.
        /// </summary>
        public IEnumerable<string> Errors { get; } = Enumerable.Empty<string>();
    }
}
