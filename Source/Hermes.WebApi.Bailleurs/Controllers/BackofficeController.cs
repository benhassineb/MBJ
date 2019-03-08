using Hermes.DataAccess.Commun;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Reflection;
using Common.WebApi.Authorization;
using Hermes.Resources;

namespace Hermes.WebApi.Bailleurs.Controllers
{
    /// <summary>
    /// Implémente les routes de la Web API pour la partie backoffice du module "Salariés".
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [Route("backoffice/{version:apiVersion}")]
    public class BackofficeController : ControllerBase
    {
        private readonly ICommunRepository _repository;
        private readonly IResourceAuthorizationCacheService _authorizationCacheService;

        /// <summary>
        /// Crée une instance de <see cref="BackofficeController"/>.
        /// </summary>
        /// <param name="repository">Le repository d'accès aux données "Communes".</param>
        /// <param name="authorizationCacheService">Le service de purge du cache des habilitations.</param>
        public BackofficeController(ICommunRepository repository, IResourceAuthorizationCacheService authorizationCacheService)
        {
            _repository = repository;
            _authorizationCacheService = authorizationCacheService;
        }

        /// <summary>
        /// Implémente la route '/ping' qui permet de valider le bon fonctionnement de la Web API vis à vis de l'accès aux données.
        /// </summary>
        /// <returns>Un objet json contenant des informations de temps et de version.</returns>
        [Route("ping")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Ping()
        {
            return Ok(new
            {
                Time = new
                {
                    WebApi = DateTime.UtcNow,
                    Database = _repository.GetUtcNow()
                },
                Version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion
            });
        }

        /// <summary>
        /// Implémente la route '/cache/autorisations' qui permet de vider le cache des autorisations sur la Web API.
        /// </summary>
        /// <returns>L'heure UTC sur le serveur.</returns>
        [Route("cache/autorisations")]
        [HttpDelete]
        [ResourceAuthorize(ResourceName.CacheAutorisations, ResourceGrant.Delete)]
        public IActionResult DeleteCacheAutorisations()
        {
            _authorizationCacheService.Clear();
            return Ok(new { Timestamp = DateTime.UtcNow });
        }
    }
}
