using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Common.WebApi
{
    /// <summary>
    ///     Implémente un ensemble d'extensions à <see cref="HttpResponse"/>.
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Envoie une réponse http d'erreur de Web API conforme à la RFC https://tools.ietf.org/html/rfc7807.
        /// </summary>
        /// <param name="httpResponse">L'instance <see cref="HttpResponse"/>.</param>
        /// <param name="statusCode">Le code http retourné.</param>
        /// <param name="detail">Le message de détail.</param>
        /// <returns></returns>
        public static Task WriteProblemAsync(this HttpResponse httpResponse, HttpStatusCode statusCode, string detail)
        {
            if (httpResponse == null) throw new ArgumentNullException(nameof(httpResponse));
            var problem = new ProblemDetails
            {
                Status = (int) statusCode,
                Title = InternalMessages.WebApiProblemTitle,
                Detail = detail
            };
            httpResponse.StatusCode = (int)statusCode;
            httpResponse.ContentType = "application/json; charset=utf-8";
            return httpResponse.WriteAsync(JsonConvert.SerializeObject(problem, Formatting.Indented), Encoding.UTF8);
        }
    }
}
