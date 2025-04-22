using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_and_Supply_Management.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    department_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contact_person_email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DisposedItems",
                columns: table => new
                {
                    disposal_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    item_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisposalMethod = table.Column<int>(type: "int", nullable: false),
                    DisposalDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisposedItems", x => x.disposal_id);
                });

            migrationBuilder.CreateTable(
                name: "DisposedMedications",
                columns: table => new
                {
                    disposal_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MedicationType = table.Column<int>(type: "int", nullable: false),
                    medicineDisposalType = table.Column<int>(type: "int", nullable: false),
                    NotifiedByEmail = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisposedMedications", x => x.disposal_id);
                });

            migrationBuilder.CreateTable(
                name: "ExpiredMedicines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicationType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    disposalMethod = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpiredMedicines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmergencyMedications",
                columns: table => new
                {
                    drug_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicationType = table.Column<int>(type: "int", nullable: false),
                    department_id = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmergencyMedications", x => x.drug_id);
                    table.ForeignKey(
                        name: "FK_EmergencyMedications_Departments_department_id",
                        column: x => x.department_id,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    asset_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    purchase_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    maintenance_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedTo = table.Column<int>(type: "int", nullable: true),
                    User = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LastMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.id);
                    table.ForeignKey(
                        name: "FK_Items_Departments_AssignedTo",
                        column: x => x.AssignedTo,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceItems",
                columns: table => new
                {
                    maintenance_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsNotified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceItems", x => x.maintenance_Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceItems_Items_item_id",
                        column: x => x.item_id,
                        principalTable: "Items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "contact_person_email", "department_name" },
                values: new object[,]
                {
                    { 1, "", "Administration & Management Department" },
                    { 2, "", "Patient Services Department" },
                    { 3, "", "Medical & Clinical Department" },
                    { 4, "", "Pharmacy & Medication Management Department" },
                    { 5, "", "Laboratory & Diagnostics Department" },
                    { 6, "", "Front Desk & Customer Relations Department" },
                    { 7, "", "Finance & Accounting Department" },
                    { 8, "", "Human Resources Department" },
                    { 9, "", "Information Technology (IT) Department" },
                    { 10, "", "Engineering & Maintenance Department" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyMedications_department_id",
                table: "EmergencyMedications",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_Items_AssignedTo",
                table: "Items",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceItems_item_id",
                table: "MaintenanceItems",
                column: "item_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisposedItems");

            migrationBuilder.DropTable(
                name: "DisposedMedications");

            migrationBuilder.DropTable(
                name: "EmergencyMedications");

            migrationBuilder.DropTable(
                name: "ExpiredMedicines");

            migrationBuilder.DropTable(
                name: "MaintenanceItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
