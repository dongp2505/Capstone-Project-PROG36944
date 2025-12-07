using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capstone_Project_PROG36944.Data.Migrations
{
    /// <inheritdoc />
    public partial class IntialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    TaskItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    AssignedEmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.TaskItemId);
                    table.ForeignKey(
                        name: "FK_TaskItems_Employees_AssignedEmployeeId",
                        column: x => x.AssignedEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_TaskItems_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_AssignedEmployeeId",
                table: "TaskItems",
                column: "AssignedEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_ProjectId",
                table: "TaskItems",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
