using Microsoft.EntityFrameworkCore.Migrations;

namespace TestProject.Migrations
{
    public partial class addComposition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Composition_Authors_AuthorID",
                table: "Composition");

            migrationBuilder.DropForeignKey(
                name: "FK_Composition_Books_BookID",
                table: "Composition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Composition",
                table: "Composition");

            migrationBuilder.RenameTable(
                name: "Composition",
                newName: "Compositions");

            migrationBuilder.RenameIndex(
                name: "IX_Composition_BookID",
                table: "Compositions",
                newName: "IX_Compositions_BookID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Compositions",
                table: "Compositions",
                columns: new[] { "AuthorID", "BookID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Compositions_Authors_AuthorID",
                table: "Compositions",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "AuthorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Compositions_Books_BookID",
                table: "Compositions",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compositions_Authors_AuthorID",
                table: "Compositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Compositions_Books_BookID",
                table: "Compositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Compositions",
                table: "Compositions");

            migrationBuilder.RenameTable(
                name: "Compositions",
                newName: "Composition");

            migrationBuilder.RenameIndex(
                name: "IX_Compositions_BookID",
                table: "Composition",
                newName: "IX_Composition_BookID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Composition",
                table: "Composition",
                columns: new[] { "AuthorID", "BookID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Composition_Authors_AuthorID",
                table: "Composition",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "AuthorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Composition_Books_BookID",
                table: "Composition",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
