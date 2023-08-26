using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TestClass",
                columns: new[] { "Id", "Address" },
                values: new object[,]
                {
                    { new Guid("0d4f7211-ed1b-4957-afa9-34134cacd1a2"), "456, Wall Street" },
                    { new Guid("8b4216be-c6fe-4178-8da1-59341179dd75"), "323, Wyoming street" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TestClass",
                keyColumn: "Id",
                keyValue: new Guid("0d4f7211-ed1b-4957-afa9-34134cacd1a2"));

            migrationBuilder.DeleteData(
                table: "TestClass",
                keyColumn: "Id",
                keyValue: new Guid("8b4216be-c6fe-4178-8da1-59341179dd75"));
        }
    }
}
