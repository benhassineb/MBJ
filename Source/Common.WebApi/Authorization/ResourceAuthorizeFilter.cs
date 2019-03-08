using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.WebApi.Authorization
{
    /// <summary>
    /// Implémente un <see cref="IAuthorizationFilter"/> qui vérifie que l'utilisateur authentifié
    /// dispose du <see cref="ResourceGrant"/> suffisant sur la resource spécifée.
    /// </summary>
    public class ResourceAuthorizeFilter : IAuthorizationFilter
    {
        private readonly string _resourceName;
        private readonly ResourceGrant _requiredGrants;
        private readonly IResourceAuthorizationService _resourceAuthorizationService;

        /// <summary>
        /// Crée une instance de <see cref="ResourceAuthorizeFilter"/> pour la resource et les autorisations spécifiées.
        /// </summary>
        /// <param name="resourceName">Le nom de la ressource.</param>
        /// <param name="requiredGrants">Les autorisations nécessaires sur la ressource.</param>
        /// <param name="resourceAuthorizationService">Le service permettant d'effectuer la vérification.</param>
        public ResourceAuthorizeFilter(string resourceName, ResourceGrant requiredGrants, IResourceAuthorizationService resourceAuthorizationService)
        {
            _resourceName = resourceName;
            _requiredGrants = requiredGrants;
            _resourceAuthorizationService = resourceAuthorizationService;
        }

        /// <summary>
        /// Appelle le code de vérification d'autorisations.
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string userId = context.HttpContext.User?.GetEmail();
            // interrogation de la base des habilitations pour obtenir l'ensemble des droits de l'utilisateur sur la ressource en question
            ResourceGrant effectiveGrants = _resourceAuthorizationService.GetGrantsForResource(userId, _resourceName);
            // vérification des droits  de l'utilisateur (provenant de la base habilitations) avec le droit requis pour la ressource  en question
            if (effectiveGrants == ResourceGrant.None || !effectiveGrants.HasFlag(_requiredGrants))
                context.Result = new ForbidResult();
        }

    }

}
