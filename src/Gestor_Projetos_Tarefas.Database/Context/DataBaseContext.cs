﻿using Gestor_Projetos_Tarefas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;


namespace Gestor_Projetos_Tarefas.Database.Context
{
    [ExcludeFromCodeCoverage]
    public class DataBaseContext: DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskUpdateHistory> History { get; set; }

        private IConfiguration configuration;

        public DataBaseContext(IConfiguration config, DbContextOptions dbOptions) : base(dbOptions)
        {
            configuration = config;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(configuration.GetConnectionString("Database"));
            }
        }
    }
}
