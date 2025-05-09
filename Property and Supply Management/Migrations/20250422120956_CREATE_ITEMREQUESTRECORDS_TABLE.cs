using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_and_Supply_Management.Migrations
{
    public partial class CREATE_ITEMREQUESTRECORDS_TABLE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "itemRequestRecords",
                columns: table => new
                {
                    request_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    department_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    itemUser = table.Column<int>(type: "int", nullable: false),
                    approvalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isRejected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemRequestRecords", x => x.request_id);
                    table.ForeignKey(
                        name: "FK_itemRequestRecords_Departments_department_id",
                        column: x => x.department_id,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_itemRequestRecords_Items_item_id",
                        column: x => x.item_id,
                        principalTable: "Items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_itemRequestRecords_department_id",
                table: "itemRequestRecords",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_itemRequestRecords_item_id",
                table: "itemRequestRecords",
                column: "item_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "itemRequestRecords");
        }
    }
}
