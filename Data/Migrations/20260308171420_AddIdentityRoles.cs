using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HuongDanLamDep.Data.Migrations
{
	public partial class AddIdentityRoles : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// ====== AspNetUserTokens: PK(UserId, LoginProvider, Name) ======
			migrationBuilder.DropPrimaryKey(
				name: "PK_AspNetUserTokens",
				table: "AspNetUserTokens");

			// Alter 2 cột thuộc PK
			migrationBuilder.AlterColumn<string>(
				name: "LoginProvider",
				table: "AspNetUserTokens",
				type: "nvarchar(128)",
				maxLength: 128,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(450)");

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "AspNetUserTokens",
				type: "nvarchar(128)",
				maxLength: 128,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(450)");

			// Add lại PK
			migrationBuilder.AddPrimaryKey(
				name: "PK_AspNetUserTokens",
				table: "AspNetUserTokens",
				columns: new[] { "UserId", "LoginProvider", "Name" });


			// ====== AspNetUserLogins: PK(LoginProvider, ProviderKey) ======
			migrationBuilder.DropPrimaryKey(
				name: "PK_AspNetUserLogins",
				table: "AspNetUserLogins");

			// Alter 2 cột thuộc PK
			migrationBuilder.AlterColumn<string>(
				name: "LoginProvider",
				table: "AspNetUserLogins",
				type: "nvarchar(128)",
				maxLength: 128,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(450)");

			migrationBuilder.AlterColumn<string>(
				name: "ProviderKey",
				table: "AspNetUserLogins",
				type: "nvarchar(128)",
				maxLength: 128,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(450)");

			// Add lại PK
			migrationBuilder.AddPrimaryKey(
				name: "PK_AspNetUserLogins",
				table: "AspNetUserLogins",
				columns: new[] { "LoginProvider", "ProviderKey" });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// ====== AspNetUserLogins revert ======
			migrationBuilder.DropPrimaryKey(
				name: "PK_AspNetUserLogins",
				table: "AspNetUserLogins");

			migrationBuilder.AlterColumn<string>(
				name: "ProviderKey",
				table: "AspNetUserLogins",
				type: "nvarchar(450)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(128)",
				oldMaxLength: 128);

			migrationBuilder.AlterColumn<string>(
				name: "LoginProvider",
				table: "AspNetUserLogins",
				type: "nvarchar(450)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(128)",
				oldMaxLength: 128);

			migrationBuilder.AddPrimaryKey(
				name: "PK_AspNetUserLogins",
				table: "AspNetUserLogins",
				columns: new[] { "LoginProvider", "ProviderKey" });


			// ====== AspNetUserTokens revert ======
			migrationBuilder.DropPrimaryKey(
				name: "PK_AspNetUserTokens",
				table: "AspNetUserTokens");

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "AspNetUserTokens",
				type: "nvarchar(450)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(128)",
				oldMaxLength: 128);

			migrationBuilder.AlterColumn<string>(
				name: "LoginProvider",
				table: "AspNetUserTokens",
				type: "nvarchar(450)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(128)",
				oldMaxLength: 128);

			migrationBuilder.AddPrimaryKey(
				name: "PK_AspNetUserTokens",
				table: "AspNetUserTokens",
				columns: new[] { "UserId", "LoginProvider", "Name" });
		}
	}
}