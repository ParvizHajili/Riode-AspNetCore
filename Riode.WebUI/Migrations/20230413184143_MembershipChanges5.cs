using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Riode.WebUI.Migrations
{
    public partial class MembershipChanges5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Membership");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                schema: "Membersip",
                newName: "UserTokens",
                newSchema: "Membership");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "Membersip",
                newName: "Users",
                newSchema: "Membership");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "Membersip",
                newName: "UserRoles",
                newSchema: "Membership");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                schema: "Membersip",
                newName: "UserLogins",
                newSchema: "Membership");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "Membersip",
                newName: "UserClaims",
                newSchema: "Membership");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "Membersip",
                newName: "Roles",
                newSchema: "Membership");

            migrationBuilder.RenameTable(
                name: "RoleClaim",
                schema: "Membersip",
                newName: "RoleClaim",
                newSchema: "Membership");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Membersip");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                schema: "Membership",
                newName: "UserTokens",
                newSchema: "Membersip");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "Membership",
                newName: "Users",
                newSchema: "Membersip");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "Membership",
                newName: "UserRoles",
                newSchema: "Membersip");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                schema: "Membership",
                newName: "UserLogins",
                newSchema: "Membersip");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "Membership",
                newName: "UserClaims",
                newSchema: "Membersip");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "Membership",
                newName: "Roles",
                newSchema: "Membersip");

            migrationBuilder.RenameTable(
                name: "RoleClaim",
                schema: "Membership",
                newName: "RoleClaim",
                newSchema: "Membersip");
        }
    }
}
