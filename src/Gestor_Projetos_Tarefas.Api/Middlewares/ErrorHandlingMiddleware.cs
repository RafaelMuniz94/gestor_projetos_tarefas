using Serilog;
using Serilog.Context;
using Gestor_Projetos_Tarefas.Domain.Exceptions;
using System.Net;
using Newtonsoft.Json;
using System;

namespace Gestor_Projetos_Tarefas.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate nextStep;

        public ErrorHandlingMiddleware(RequestDelegate _nextStep)
        {
            this.nextStep = _nextStep;
        }

        public async Task Invoke(HttpContext appContext)
        {
            using(LogContext.PushProperty("RequestId",Guid.NewGuid()))
            {
                try
                {
                    Log.Information("Iniciando Processo!");
                    await nextStep(appContext);
                    Log.Information("Processo Finalizado com sucesso!");
                } catch (DomainException domainException)
                {
                    Log.Error($"Processamento nao foi finalizado devido a Exception: {domainException.Message}");
                    var errorMessage = JsonConvert.SerializeObject(new { error = $"Necessario validar o erro a seguir: {domainException.Message}!" });
                    appContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    appContext.Response.ContentType = "application/json";

                    await appContext.Response.WriteAsync(errorMessage);
                }
                catch (Exception exception)
                {
                    Log.Error($"Processamento nao foi finalizado devido a Exception: {exception.Message}");
                    var errorMessage = JsonConvert.SerializeObject(new { error = "Nao foi possivel processar requisicao!" });
                    appContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    appContext.Response.ContentType = "application/json";

                    await appContext.Response.WriteAsync(errorMessage);
                }
            }
        }
    }
}
