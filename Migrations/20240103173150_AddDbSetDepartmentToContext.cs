using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Labb4IndividuelltDatabasprojekt.Migrations
{
    /// <inheritdoc />
    public partial class AddDbSetDepartmentToContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personnel_Department_FkDepartmentId",
                table: "Personnel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Department",
                table: "Department");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "Departments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personnel_Departments_FkDepartmentId",
                table: "Personnel",
                column: "FkDepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personnel_Departments_FkDepartmentId",
                table: "Personnel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Department");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Department",
                table: "Department",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personnel_Department_FkDepartmentId",
                table: "Personnel",
                column: "FkDepartmentId",
                principalTable: "Department",
                principalColumn: "DepartmentId");
        }
    }
}
