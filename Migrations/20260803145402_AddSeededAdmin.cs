using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CogStayMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddSeededAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "StaffId", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "PhoneNumber", "Role" },
                values: new object[] { 99, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), "admin@cogstay.in", "Admin", true, "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=", "+91 9999999999", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 99);
        }
    }
}
