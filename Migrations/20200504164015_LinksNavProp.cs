using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstellationWebApp.Migrations
{
    public partial class LinksNavProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactLink_User_UsersUserID",
                table: "ContactLink");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectLink_Project_ProjectsProjectID",
                table: "ProjectLink");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectLink",
                table: "ProjectLink");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactLink",
                table: "ContactLink");

            migrationBuilder.RenameTable(
                name: "ProjectLink",
                newName: "ProjectLinks");

            migrationBuilder.RenameTable(
                name: "ContactLink",
                newName: "ContactLinks");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectLink_ProjectsProjectID",
                table: "ProjectLinks",
                newName: "IX_ProjectLinks_ProjectsProjectID");

            migrationBuilder.RenameIndex(
                name: "IX_ContactLink_UsersUserID",
                table: "ContactLinks",
                newName: "IX_ContactLinks_UsersUserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectLinks",
                table: "ProjectLinks",
                column: "ProjectLinkID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactLinks",
                table: "ContactLinks",
                column: "ContactLinkID");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactLinks_User_UsersUserID",
                table: "ContactLinks",
                column: "UsersUserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectLinks_Project_ProjectsProjectID",
                table: "ProjectLinks",
                column: "ProjectsProjectID",
                principalTable: "Project",
                principalColumn: "ProjectID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactLinks_User_UsersUserID",
                table: "ContactLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectLinks_Project_ProjectsProjectID",
                table: "ProjectLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectLinks",
                table: "ProjectLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactLinks",
                table: "ContactLinks");

            migrationBuilder.RenameTable(
                name: "ProjectLinks",
                newName: "ProjectLink");

            migrationBuilder.RenameTable(
                name: "ContactLinks",
                newName: "ContactLink");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectLinks_ProjectsProjectID",
                table: "ProjectLink",
                newName: "IX_ProjectLink_ProjectsProjectID");

            migrationBuilder.RenameIndex(
                name: "IX_ContactLinks_UsersUserID",
                table: "ContactLink",
                newName: "IX_ContactLink_UsersUserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectLink",
                table: "ProjectLink",
                column: "ProjectLinkID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactLink",
                table: "ContactLink",
                column: "ContactLinkID");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactLink_User_UsersUserID",
                table: "ContactLink",
                column: "UsersUserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectLink_Project_ProjectsProjectID",
                table: "ProjectLink",
                column: "ProjectsProjectID",
                principalTable: "Project",
                principalColumn: "ProjectID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
