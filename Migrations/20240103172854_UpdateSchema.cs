using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Labb4IndividuelltDatabasprojekt.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FkDepartmentId",
                table: "Personnel",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "Personnel",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_FkDepartmentId",
                table: "Personnel",
                column: "FkDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personnel_Department_FkDepartmentId",
                table: "Personnel",
                column: "FkDepartmentId",
                principalTable: "Department",
                principalColumn: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personnel_Department_FkDepartmentId",
                table: "Personnel");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Personnel_FkDepartmentId",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "FkDepartmentId",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Personnel");
        }
    }
}
