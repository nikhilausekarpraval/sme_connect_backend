using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoDotNetCoreApplication.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentitySetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_on_dt = table.Column<DateOnly>(type: "date", nullable: true),
                    created_by = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    designation = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    mobile_no = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    position = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__employee__3213E83FB92F5291", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "task",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    assigned_on_dt = table.Column<DateOnly>(type: "date", nullable: true),
                    created_on_dt = table.Column<DateOnly>(type: "date", nullable: true),
                    employee_id = table.Column<int>(type: "int", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    created_by = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    status = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__task__3213E83F05BE6225", x => x.id);
                    table.ForeignKey(
                        name: "FKmeqi2abtbehx871tag4op3hag",
                        column: x => x.employee_id,
                        principalTable: "employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_task_employee_id",
                table: "task",
                column: "employee_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "task");

            migrationBuilder.DropTable(
                name: "employee");
        }
    }
}
