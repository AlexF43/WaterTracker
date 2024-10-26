using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaterTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddedGoalUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "WaterUsages",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "totalUsage",
                table: "WaterUsages",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "usageName",
                table: "WaterUsages",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "usageType",
                table: "WaterUsages",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "usedSec",
                table: "WaterUsages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "WaterUsages",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "usageLiterPerSec",
                table: "WaterAmounts",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "dailyGoal",
                table: "Users",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "weeklyGoal",
                table: "Users",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date",
                table: "WaterUsages");

            migrationBuilder.DropColumn(
                name: "totalUsage",
                table: "WaterUsages");

            migrationBuilder.DropColumn(
                name: "usageName",
                table: "WaterUsages");

            migrationBuilder.DropColumn(
                name: "usageType",
                table: "WaterUsages");

            migrationBuilder.DropColumn(
                name: "usedSec",
                table: "WaterUsages");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "WaterUsages");

            migrationBuilder.DropColumn(
                name: "usageLiterPerSec",
                table: "WaterAmounts");

            migrationBuilder.DropColumn(
                name: "dailyGoal",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "weeklyGoal",
                table: "Users");
        }
    }
}
