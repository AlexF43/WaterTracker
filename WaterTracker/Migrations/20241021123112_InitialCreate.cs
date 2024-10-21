using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<string>(type: "TEXT", nullable: false),
                    userPwd = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "WaterAmounts",
                columns: table => new
                {
                    usageType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterAmounts", x => x.usageType);
                });

            migrationBuilder.CreateTable(
                name: "WaterUsages",
                columns: table => new
                {
                    usageId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterUsages", x => x.usageId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WaterAmounts");

            migrationBuilder.DropTable(
                name: "WaterUsages");
        }
    }
}
