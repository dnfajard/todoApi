using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
//using TodoApi.ErrorHandlingMiddleware;


namespace TodoApi.Tests
{
    public class ErrorHandlingMiddlewareTests
    {
        [Fact]
        public async Task ErrorHandlingMiddleware_DoesNotChangeStatusCode_WhenNoException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            context.Response.StatusCode = 201; 

            RequestDelegate next = ctx =>
            {
                // normal processing, no exception
                return Task.CompletedTask;
            };

            var middleware = new ErrorHandlingMiddleware(next, loggerMock.Object);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(201, context.Response.StatusCode); 
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Equal(string.Empty, body); 
        }

        [Fact]
        public async Task ErrorHandlingMiddleware_Returns500_WhenExceptionThrown()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            RequestDelegate next = (ctx) =>
            {
                throw new Exception("Test Internal Server Error");
            };

            var middleware = new ErrorHandlingMiddleware(next, loggerMock.Object);
            await middleware.InvokeAsync(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();


            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            Assert.Contains("Test Internal Server Error", responseBody);
        }

        [Fact]
        public async Task ErrorHandlingMiddleware_LogsError_WhenExceptionThrown()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            RequestDelegate next = (ctx) =>
            {
                throw new Exception("Test Internal Server Error - Logging");
            };

            var middleware = new ErrorHandlingMiddleware(next, loggerMock.Object);
            await middleware.InvokeAsync(context);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while processing the request.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once
            );
        }
        
    }
}
