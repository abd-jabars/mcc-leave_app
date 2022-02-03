using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_m_leaves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_leaves", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_accountroles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_accountroles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_tr_accountroles_tb_m_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tb_m_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_employees",
                columns: table => new
                {
                    NIK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_employees", x => x.NIK);
                    table.ForeignKey(
                        name: "FK_tb_m_employees_tb_m_employees_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "tb_m_employees",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_accounts",
                columns: table => new
                {
                    NIK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeaveQuota = table.Column<int>(type: "int", nullable: false),
                    PrevLeaveQuota = table.Column<int>(type: "int", nullable: false),
                    LeaveStatus = table.Column<bool>(type: "bit", nullable: false),
                    OTP = table.Column<int>(type: "int", nullable: true),
                    ExpiredToken = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_accounts", x => x.NIK);
                    table.ForeignKey(
                        name: "FK_tb_m_accounts_tb_m_employees_NIK",
                        column: x => x.NIK,
                        principalTable: "tb_m_employees",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_departments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_m_departments_tb_m_employees_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "tb_m_employees",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_leaveemployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    NIK = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LeaveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_leaveemployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_tr_leaveemployees_tb_m_employees_NIK",
                        column: x => x.NIK,
                        principalTable: "tb_m_employees",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_tr_leaveemployees_tb_m_leaves_LeaveId",
                        column: x => x.LeaveId,
                        principalTable: "tb_m_leaves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_departments_ManagerId",
                table: "tb_m_departments",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_DepartmentId",
                table: "tb_m_employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_ManagerId",
                table: "tb_m_employees",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_accountroles_AccountId",
                table: "tb_tr_accountroles",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_accountroles_RoleId",
                table: "tb_tr_accountroles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_leaveemployees_LeaveId",
                table: "tb_tr_leaveemployees",
                column: "LeaveId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_leaveemployees_NIK",
                table: "tb_tr_leaveemployees",
                column: "NIK");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_tr_accountroles_tb_m_accounts_AccountId",
                table: "tb_tr_accountroles",
                column: "AccountId",
                principalTable: "tb_m_accounts",
                principalColumn: "NIK",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_m_employees_tb_m_departments_DepartmentId",
                table: "tb_m_employees",
                column: "DepartmentId",
                principalTable: "tb_m_departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_m_departments_tb_m_employees_ManagerId",
                table: "tb_m_departments");

            migrationBuilder.DropTable(
                name: "tb_tr_accountroles");

            migrationBuilder.DropTable(
                name: "tb_tr_leaveemployees");

            migrationBuilder.DropTable(
                name: "tb_m_accounts");

            migrationBuilder.DropTable(
                name: "tb_m_roles");

            migrationBuilder.DropTable(
                name: "tb_m_leaves");

            migrationBuilder.DropTable(
                name: "tb_m_employees");

            migrationBuilder.DropTable(
                name: "tb_m_departments");
        }
    }
}
