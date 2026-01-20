using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateAPI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationSetupModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Currency_CurrencyId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Branches_Code",
                table: "Branches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.RenameTable(
                name: "Currency",
                newName: "Currencies");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId1",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Branches",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Branches",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Branches",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Branches",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Currencies",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "DecimalPlaces",
                table: "Currencies",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Currencies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "Currencies",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CostCenters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ParentCostCenterId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsMandatoryForExpenses = table.Column<bool>(type: "bit", nullable: false),
                    IsMandatoryForProcurement = table.Column<bool>(type: "bit", nullable: false),
                    IsMandatoryForJournalEntries = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostCenters_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostCenters_CostCenters_ParentCostCenterId",
                        column: x => x.ParentCostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ManagerId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ParentDepartmentId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    ToCurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinancialPeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PeriodNumber = table.Column<int>(type: "int", nullable: false),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ClosedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReopenedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ReopenedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReopenReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialPeriods_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CurrencyId1",
                table: "Companies",
                column: "CurrencyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CompanyId_Code",
                table: "Branches",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_IsActive",
                table: "Branches",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Code",
                table: "Currencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_IsActive",
                table: "Currencies",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_CompanyId_Code",
                table: "CostCenters",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_CompanyId_Name",
                table: "CostCenters",
                columns: new[] { "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_IsActive",
                table: "CostCenters",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_ParentCostCenterId",
                table: "CostCenters",
                column: "ParentCostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyId_Code",
                table: "Departments",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyId_Name",
                table: "Departments",
                columns: new[] { "CompanyId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_IsActive",
                table: "Departments",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentDepartmentId",
                table: "Departments",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CompanyId_CurrencyId_EffectiveDate_IsActive",
                table: "ExchangeRates",
                columns: new[] { "CompanyId", "CurrencyId", "EffectiveDate", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CurrencyId_EffectiveDate",
                table: "ExchangeRates",
                columns: new[] { "CurrencyId", "EffectiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialPeriods_CompanyId_FiscalYear_PeriodNumber",
                table: "FinancialPeriods",
                columns: new[] { "CompanyId", "FiscalYear", "PeriodNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialPeriods_CompanyId_StartDate_EndDate",
                table: "FinancialPeriods",
                columns: new[] { "CompanyId", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialPeriods_CompanyId_Status",
                table: "FinancialPeriods",
                columns: new[] { "CompanyId", "Status" });

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Currencies_CurrencyId",
                table: "Companies",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Currencies_CurrencyId1",
                table: "Companies",
                column: "CurrencyId1",
                principalTable: "Currencies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Currencies_CurrencyId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Currencies_CurrencyId1",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "CostCenters");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropTable(
                name: "FinancialPeriods");

            migrationBuilder.DropIndex(
                name: "IX_Companies_CurrencyId1",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Branches_CompanyId_Code",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_IsActive",
                table: "Branches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_Code",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_IsActive",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "CurrencyId1",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "DecimalPlaces",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "Currencies");

            migrationBuilder.RenameTable(
                name: "Currencies",
                newName: "Currency");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Currency",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Code",
                table: "Branches",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Currency_CurrencyId",
                table: "Companies",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
