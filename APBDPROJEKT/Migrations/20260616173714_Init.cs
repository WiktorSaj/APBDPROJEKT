using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APBDPROJEKT.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Software",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasePricePerYear = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Software", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyClients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Krs = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyClients_Clients_Id",
                        column: x => x.Id,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndividualClients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pesel = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualClients_Clients_Id",
                        column: x => x.Id,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoftwareVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BonusSupportYears = table.Column<int>(type: "int", nullable: false),
                    IsSigned = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    SoftwareId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contracts_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftwareId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discounts_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "Login", "Password", "RefreshToken", "RefreshTokenExpires", "Role" },
                values: new object[,]
                {
                    { 1, "appUser1", "$2a$12$1GWilMWEoqaPCXQ1kNCkf.o97C1kvE81lCw83mrP9qLFUn3t/Lkam", null, null, "Admin" },
                    { 2, "appUser2", "$2a$12$fhGCEtu08X2Px5C.gW8Yn.XgL5OMVRXiYk1GNQtgW2hvbdGXutLQ6", null, null, "User" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "Email", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "ul. Pierwsza 1, 00-001 Warszawa", "first.user@poczta.com", "123456789" },
                    { 2, "ul. Druga 2, 30-000 Kraków", "second.user@poczta.com", "987654321" },
                    { 3, "ul. Trzecia 3, 00-001 Warszawa", "third.user@poczta.com", "123654789" },
                    { 4, "ul. Czwarta 4, 30-000 Kraków", "fourth.user@poczta.com", "321456987" }
                });

            migrationBuilder.InsertData(
                table: "Software",
                columns: new[] { "Id", "BasePricePerYear", "Category", "Description", "Name", "Version" },
                values: new object[,]
                {
                    { 1, 2500.00m, "Education", "Software Description 1", "Software 1", "1.0.0" },
                    { 2, 5000.00m, "Finance", "Software Description 2", "Software 2", "2.0.0" }
                });

            migrationBuilder.InsertData(
                table: "CompanyClients",
                columns: new[] { "Id", "CompanyName", "Krs" },
                values: new object[,]
                {
                    { 3, "CorporationFirst", "0000123456" },
                    { 4, "CorporationSecond", "0000234567" }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "BonusSupportYears", "ClientId", "EndDate", "IsSigned", "Price", "SoftwareId", "SoftwareVersion", "StartDate" },
                values: new object[,]
                {
                    { 1, 0, 1, new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 4500.00m, 1, "1.0.0", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, 3, new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3000.00m, 2, "2.0.0", new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "EndDate", "Name", "SoftwareId", "StartDate", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Discount 1", 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.00m },
                    { 2, new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Discount 2", 2, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20.00m }
                });

            migrationBuilder.InsertData(
                table: "IndividualClients",
                columns: new[] { "Id", "FirstName", "IsDeleted", "LastName", "Pesel" },
                values: new object[,]
                {
                    { 1, "John", false, "Doe", "12345678901" },
                    { 2, "Adam", false, "Smith", "23456789012" }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "ContractId", "Date" },
                values: new object[,]
                {
                    { 1, 2000.00m, 1, new DateTime(2026, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2500.00m, 1, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Login",
                table: "AppUsers",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClients_Krs",
                table: "CompanyClients",
                column: "Krs",
                unique: true,
                filter: "[Krs] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ClientId",
                table: "Contracts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_SoftwareId",
                table: "Contracts",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_SoftwareId",
                table: "Discounts",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualClients_Pesel",
                table: "IndividualClients",
                column: "Pesel",
                unique: true,
                filter: "[Pesel] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ContractId",
                table: "Payments",
                column: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "CompanyClients");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "IndividualClients");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Software");
        }
    }
}
