using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_and_Supply_Management.Migrations
{
    public partial class add_new_rules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationRequestHistory_Departments_department_id",
                table: "MedicationRequestHistory");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationRequestHistory_medication_id",
                table: "MedicationRequestHistory",
                column: "medication_id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationRequestHistory_Departments_department_id",
                table: "MedicationRequestHistory",
                column: "department_id",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationRequestHistory_EmergencyMedications_medication_id",
                table: "MedicationRequestHistory",
                column: "medication_id",
                principalTable: "EmergencyMedications",
                principalColumn: "drug_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicationRequestHistory_Departments_department_id",
                table: "MedicationRequestHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicationRequestHistory_EmergencyMedications_medication_id",
                table: "MedicationRequestHistory");

            migrationBuilder.DropIndex(
                name: "IX_MedicationRequestHistory_medication_id",
                table: "MedicationRequestHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicationRequestHistory_Departments_department_id",
                table: "MedicationRequestHistory",
                column: "department_id",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
