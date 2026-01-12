using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_SubDepartments_SubDepartmentId",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "SubDepartmentId",
                table: "Employees",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_SubDepartments_SubDepartmentId",
                table: "Employees",
                column: "SubDepartmentId",
                principalTable: "SubDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_SubDepartments_SubDepartmentId",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "SubDepartmentId",
                table: "Employees",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_SubDepartments_SubDepartmentId",
                table: "Employees",
                column: "SubDepartmentId",
                principalTable: "SubDepartments",
                principalColumn: "Id");
        }
    }
}
