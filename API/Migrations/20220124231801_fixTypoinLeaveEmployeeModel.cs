using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class fixTypoinLeaveEmployeeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "tb_tr_leaveemployees",
                newName: "Attachment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Attachment",
                table: "tb_tr_leaveemployees",
                newName: "Description");
        }
    }
}
