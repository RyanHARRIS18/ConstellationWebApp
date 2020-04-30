using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstellationWebApp.Migrations
{
    public partial class LinkTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactLinkOne",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ContactLinkThree",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ContactLinkTwo",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ProjectLinkOne",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectLinkThree",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectLinkTwo",
                table: "Project");

            migrationBuilder.AddColumn<int>(
                name: "ProjectLinkID",
                table: "Project",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ContactLink",
                columns: table => new
                {
                    ContactLinkID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactLinkLabel = table.Column<string>(maxLength: 50, nullable: true),
                    ContactLinkUrl = table.Column<string>(nullable: true),
                    UsersUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactLink", x => x.ContactLinkID);
                    table.ForeignKey(
                        name: "FK_ContactLink_User_UsersUserID",
                        column: x => x.UsersUserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectLink",
                columns: table => new
                {
                    ProjectLinkID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectLinkLabel = table.Column<string>(maxLength: 50, nullable: true),
                    ProjectLinkUrl = table.Column<string>(nullable: true),
                    ProjectsProjectID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectLink", x => x.ProjectLinkID);
                    table.ForeignKey(
                        name: "FK_ProjectLink_Project_ProjectsProjectID",
                        column: x => x.ProjectsProjectID,
                        principalTable: "Project",
                        principalColumn: "ProjectID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactLink_UsersUserID",
                table: "ContactLink",
                column: "UsersUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectLink_ProjectsProjectID",
                table: "ProjectLink",
                column: "ProjectsProjectID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactLink");

            migrationBuilder.DropTable(
                name: "ProjectLink");

            migrationBuilder.DropColumn(
                name: "ProjectLinkID",
                table: "Project");

            migrationBuilder.AddColumn<string>(
                name: "ContactLinkOne",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactLinkThree",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactLinkTwo",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectLinkOne",
                table: "Project",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectLinkThree",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectLinkTwo",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
