using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orders.Migrations
{
    /// <inheritdoc />
    public partial class AddingProductSnapshotsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "ProductSnapshotId",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSnapshots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductSnapshotId",
                table: "Orders",
                column: "ProductSnapshotId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ProductSnapshots_ProductSnapshotId",
                table: "Orders",
                column: "ProductSnapshotId",
                principalTable: "ProductSnapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ProductSnapshots_ProductSnapshotId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ProductSnapshots");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProductSnapshotId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductSnapshotId",
                table: "Orders");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
