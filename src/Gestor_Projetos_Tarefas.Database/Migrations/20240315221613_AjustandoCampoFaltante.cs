﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestor_Projetos_Tarefas.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class AjustandoCampoFaltante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationTime",
                table: "taskUpdateHistory",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModificationTime",
                table: "taskUpdateHistory");
        }
    }
}
