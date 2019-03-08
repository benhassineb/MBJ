using Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Common.Monitoring;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;

namespace Common.WebApi.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MonitoringMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _requestDelegateMock = new Mock<RequestDelegate>();

        [TestInitialize]
        public void Startup()
        {
        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("WebApi")]
        [Description("MonitoringMiddleware trace les performances de l'appel de la Web API dans le monitoring.")]
        public void MonitoringMiddleware_Traces_Performance()
        {
            Mock<IFrameworkTracer> frameworkTracerMock = new Mock<IFrameworkTracer>();

            var systemUnderTest = new MonitoringMiddleware(_requestDelegateMock.Object, frameworkTracerMock.Object);
            systemUnderTest.InvokeAsync(GetHttpContextMock().Object).GetAwaiter().GetResult();

            frameworkTracerMock.Verify(t => t.TracePerformance(It.IsAny<string>(), It.IsAny<TraceEventType>(), It.IsAny<string>(), It.IsAny<TraceContext>()), Times.Once);
        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("WebApi")]
        [Description("MonitoringMiddleware génère une nouvelle corrélation dans le monitoring..")]
        public void MonitoringMiddleware_Generate_Correlation()
        {
            Mock<IFrameworkTracer> frameworkTracerMock = new Mock<IFrameworkTracer>();

            var systemUnderTest = new MonitoringMiddleware(_requestDelegateMock.Object, frameworkTracerMock.Object);
            systemUnderTest.InvokeAsync(GetHttpContextMock().Object).GetAwaiter().GetResult();

            frameworkTracerMock.Verify(t => t.SetCorrelationId(It.Is<Guid>(v => v != Guid.Empty)));
        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("WebApi")]
        [Description("MonitoringMiddleware propage le header CustomSessionId de la Web API dans le monitoring..")]
        public void MonitoringMiddleware_Propagate_CustomSessionId()
        {
            Mock<IFrameworkTracer> frameworkTracerMock = new Mock<IFrameworkTracer>();
            StringValues expectedCustomSessionId = "1234";
            HeaderDictionary headers = new HeaderDictionary { { "CustomSessionId", expectedCustomSessionId } };

            var systemUnderTest = new MonitoringMiddleware(_requestDelegateMock.Object, frameworkTracerMock.Object);
            systemUnderTest.InvokeAsync(GetHttpContextMock(headers).Object).GetAwaiter().GetResult();

            frameworkTracerMock.Verify(t => t.SetSessionId(It.Is<string>(v => v == expectedCustomSessionId)));
        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("WebApi")]
        [Description("MonitoringMiddleware trace les exceptions lors de l'appel de la Web API dans le monitoring..")]
        public void MonitoringMiddleware_Traces_Exception()
        {
            Mock<IFrameworkTracer> frameworkTracerMock = new Mock<IFrameworkTracer>();
            InvalidOperationException expectedException = new InvalidOperationException("TEST");
            _requestDelegateMock.Setup(rd => rd(It.IsAny<HttpContext>())).Throws(expectedException);
            var httpResponseMock = GetHttpResponseMock();
            var httpContextMock = GetHttpContextMock(responseMock: httpResponseMock);

            var systemUnderTest = new MonitoringMiddleware(_requestDelegateMock.Object, frameworkTracerMock.Object);
            systemUnderTest.InvokeAsync(httpContextMock.Object).GetAwaiter().GetResult();

            frameworkTracerMock.Verify(t => t.TraceException(It.Is<InvalidOperationException>(e => e == expectedException),
                    It.IsAny<TraceEventType>(), It.IsAny<int>(), It.IsAny<TraceContext>()), Times.Once);

        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("WebApi")]
        [Description("MonitoringMiddleware génère une réponse 500 - TraceDetail en Json en cas d'exception.")]
        public void MonitoringMiddleware_Returns_JsonDetail_And_500_On_Exception()
        {
            int expectedHttpCode = 500;
            Mock<IFrameworkTracer> frameworkTracerMock = new Mock<IFrameworkTracer>();
            InvalidOperationException expectedException = new InvalidOperationException("TEST");
            _requestDelegateMock.Setup(rd => rd(It.IsAny<HttpContext>())).Throws(expectedException);
            var httpResponseMock = GetHttpResponseMock();
            var httpContextMock = GetHttpContextMock(responseMock: httpResponseMock);

            var systemUnderTest = new MonitoringMiddleware(_requestDelegateMock.Object, frameworkTracerMock.Object);
            systemUnderTest.InvokeAsync(httpContextMock.Object).GetAwaiter().GetResult();

            httpResponseMock.VerifySet(t => t.StatusCode = expectedHttpCode);
        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("WebApi")]
        [Description("MonitoringMiddleware génère une réponse 500 - TraceDetail en Json en cas d'exception SQL quelconque.")]
        public void MonitoringMiddleware_Returns_JsonDetail_And_500_On_SqlException()
        {
            int expectedHttpCode = 500;
            Mock<IFrameworkTracer> frameworkTracerMock = new Mock<IFrameworkTracer>();
            SqlException expectedException = SqlExceptionCreator.NewSqlException(1024);
            _requestDelegateMock.Setup(rd => rd(It.IsAny<HttpContext>())).Throws(expectedException);
            var httpResponseMock = GetHttpResponseMock();
            var httpContextMock = GetHttpContextMock(responseMock: httpResponseMock);

            var systemUnderTest = new MonitoringMiddleware(_requestDelegateMock.Object, frameworkTracerMock.Object);
            systemUnderTest.InvokeAsync(httpContextMock.Object).GetAwaiter().GetResult();

            httpResponseMock.VerifySet(t => t.StatusCode = expectedHttpCode);
        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("WebApi")]
        [Description("MonitoringMiddleware génère une réponse 404 - TraceDetail en Json en cas d'exception SQL 60 000.")]
        public void MonitoringMiddleware_Returns_JsonDetail_And_404_On_Specific_SqlException()
        {
            int expectedHttpCode = 404;
            Mock<IFrameworkTracer> frameworkTracerMock = new Mock<IFrameworkTracer>();
            SqlException expectedException = SqlExceptionCreator.NewSqlException(60000);
            _requestDelegateMock.Setup(rd => rd(It.IsAny<HttpContext>())).Throws(expectedException);
            var httpResponseMock = GetHttpResponseMock();
            var httpContextMock = GetHttpContextMock(responseMock: httpResponseMock);

            var systemUnderTest = new MonitoringMiddleware(_requestDelegateMock.Object, frameworkTracerMock.Object);
            systemUnderTest.InvokeAsync(httpContextMock.Object).GetAwaiter().GetResult();

            httpResponseMock.VerifySet(t => t.StatusCode = expectedHttpCode);
        }

        private Mock<HttpContext> GetHttpContextMock(HeaderDictionary headers = null, Mock<HttpResponse> responseMock = null)
        {
            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(x => x.Scheme).Returns("https");
            requestMock.Setup(x => x.Host).Returns(new HostString("web.lvh.me"));
            requestMock.Setup(x => x.Path).Returns(new PathString("/test"));
            requestMock.Setup(x => x.PathBase).Returns(new PathString("/"));
            requestMock.Setup(x => x.Method).Returns("GET");
            requestMock.Setup(x => x.Body).Returns(new MemoryStream());
            requestMock.Setup(x => x.QueryString).Returns(new QueryString("?param1=2"));
            if (headers != null)
            {
                requestMock.Setup(x => x.Headers).Returns(headers);
            }
            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.Request).Returns(requestMock.Object);
            if (responseMock != null)
            {
                contextMock.Setup(x => x.Response).Returns(responseMock.Object);
            }
            return contextMock;
        }

        private Mock<HttpResponse> GetHttpResponseMock()
        {
            var responseMock = new Mock<HttpResponse>();
            responseMock.SetupSet<int>(h => h.StatusCode = It.IsAny<int>());
            responseMock.SetupSet<int>(h => h.ContentType = It.IsAny<string>());
            responseMock.Setup(h => h.Body).Returns(new MemoryStream());
            return responseMock;
        }
    }

}
