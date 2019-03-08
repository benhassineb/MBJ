using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Monitoring;
using Common.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Common.WebApi
{
    /// <summary>
    /// Implémente un middleware ASP.NET Core qui monitore et corrèle les performances.
    /// </summary>
    public class MonitoringMiddleware
    {
        private const string SessionIdHeader = "CustomSessionId";

        private readonly RequestDelegate _next;
        private readonly IFrameworkTracer _tracer;

        /// <summary>
        /// Crée une instance de <see cref="MonitoringMiddleware"/> en spécifiant le traceur du monitoring <see cref="IFrameworkTracer"/>.
        /// </summary>
        /// <param name="next">Le prochain middleware dans le pipeline.</param>
        /// <param name="tracer">Le traceur du monitoring <see cref="IFrameworkTracer"/>.</param>
        public MonitoringMiddleware(RequestDelegate next, IFrameworkTracer tracer)
        {
            _next = next;
            _tracer = tracer;
        }

        /// <summary>
        /// Exécute le code du <see cref="MonitoringMiddleware"/>.
        /// </summary>
        /// <param name="context">Le contexte Http.</param>
        /// <returns>Le code d'appel au middleware suivant.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            HttpRequest request = context.Request;
            // définit le session id au niveau du monitoring s'il a été fourni en tant que header http
            string sessionId = request?.Headers?[SessionIdHeader].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(sessionId)) _tracer.SetSessionId(sessionId);
            // définit le principal id au niveau du monitoring
            string principalId = context.User?.GetPreferredUserName();
            if (!string.IsNullOrWhiteSpace(principalId)) _tracer.SetPrincipalId(principalId);
            // débute une corrélation au niveau du monitoring
            _tracer.SetCorrelationId(Guid.NewGuid());
            try
            {
                using (_tracer.TracePerformance(traceEventType: TraceEventType.HttpResponse,
                    traceEventName: request.GetEncodedPathAndQuery(),
                    context: CreateContextForApiRequest(request)))
                {
                    await _next(context);
                }
            }
            catch (SqlException sqlException) when (sqlException.Number == 60000)
            {
                _tracer.TraceWarning(InternalMessages.UnexpectedUserAction, ErrorCodes.Application.UnexpectedUserAction, context: CreateContextForApiRequest(request));
                await context.Response.WriteProblemAsync(HttpStatusCode.NotFound, InternalMessages.NotFoundResource);
            }
            catch (Exception exception)
            {
                _tracer.TraceException(exception, context: CreateContextForApiRequest(request));
                await context.Response.WriteProblemAsync(HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Crée un contexte pour ajouter des informations de la requête dans les traces
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private TraceContext CreateContextForApiRequest(HttpRequest request)
        {
            string origin = request?.Headers?[Constants.TraceParameter.Origin].FirstOrDefault();
            TraceContext traceContext = TraceContext
                .Create(Constants.TraceParameter.Method, request.Method)
                .Add(Constants.TraceParameter.Uri, request.GetEncodedPathAndQuery())
                .Add(Constants.TraceParameter.Origin, origin);
            return traceContext;
        }
    }
}
