using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Gestor_Projetos_Tarefas.Api.Middlewares;
using Gestor_Projetos_Tarefas.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Xunit;
using Moq;

namespace YourNamespace.Tests.Middlewares
{
    public class ErrorHandlingMiddlewareTest
    {
        [Fact]
        public async Task Invoke_CatchesGenericExceptionFromNextStep_ReturnsInternalServerError()
        {
            // Arrange
            var nextStepMock = new Mock<RequestDelegate>(); // Proxima etapa a ser executada
            var middleware = new ErrorHandlingMiddleware(nextStepMock.Object);
            var context = new DefaultHttpContext(); //Contexto HTTP falso
            var responseStream = new MemoryStream();// Funciona como corpo da requisicao http
            context.Response.Body = responseStream;
            var exceptionMessage = "Exception generica";

            
            nextStepMock.Setup(step => step(context)).Throws(new Exception(exceptionMessage));

            // Act
            await middleware.Invoke(context);

            
            responseStream.Seek(0, SeekOrigin.Begin);// Visto que o corpo da msg e uma stream sera necessario retornar ao inicio para conseguir obter toda a msg

            // Assert
            var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);

            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            Assert.Equal("Nao foi possivel processar requisicao!", (string)responseObject.error);
        }

        [Fact]
        public async Task Invoke_CatchesDomainExceptionFromNextStep_ReturnsInternalServerError()
        {
            // Arrange
            var nextStepMock = new Mock<RequestDelegate>(); 
            var middleware = new ErrorHandlingMiddleware(nextStepMock.Object);
            var context = new DefaultHttpContext(); 
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;
            var exceptionMessage = "Exception de dominio";


            nextStepMock.Setup(step => step(context)).Throws(new DomainException(exceptionMessage));

            // Act
            await middleware.Invoke(context);


            responseStream.Seek(0, SeekOrigin.Begin);

            // Assert
            var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);

            Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
            Assert.Equal($"Necessario validar o erro a seguir: {exceptionMessage}!", (string)responseObject.error);
        }

    }
}
