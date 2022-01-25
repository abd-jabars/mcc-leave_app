using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class add_AprrovalStatus_Attibute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "tb_tr_leaveemployees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "tb_tr_leaveemployees");
        }
    }
}
