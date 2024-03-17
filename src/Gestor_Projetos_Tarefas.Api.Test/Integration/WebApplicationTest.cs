using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gestor_Projetos_Tarefas.Database.Context;
using System.Diagnostics.CodeAnalysis;

namespace Gestor_Projetos_Tarefas.Api.Test.Integration
{
    // Criando um Factory customizado, dessa forma, sera possivel trocar o tipo de banco que utilizo para testes
    // Evitando que o banco esteja sujo quando o teste de integracao executar.
    // Essa classe ira utilizar reflecions para obter as configuracoes feitas na startup da api (Gestor_Projetos_Tarefas.Api/Program.cs)
    [ExcludeFromCodeCoverage]
    public class WebApplicationTest<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.ConfigureServices(services =>
            {

                // As linhas abaixo irao limpar o contexto da startup e a conexao caso ela exista
                var databaseContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataBaseContext>));
                services.Remove(databaseContext);

                var dbConnection = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

                services.Remove(dbConnection);

                // Criando a conexao que sera utilizada apenas durante os testes de integracao
                services.AddSingleton<DbConnection>(appContainer =>
                {
                    // Criando conexao em memoria
                    var connection = new SqliteConnection("mode=memory;cache=shared");

                    return connection;
                });

                // Inserindo conexao criada anteriormente no contexto
                services.AddDbContext<DataBaseContext>((appContainer, options) => {
                    var connection = appContainer.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });


            });
            builder.UseEnvironment("Development");
        }
    }
}

