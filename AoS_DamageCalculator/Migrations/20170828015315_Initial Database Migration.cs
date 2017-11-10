using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AoS_DamageCalculator.Migrations
{
    public partial class InitialDatabaseMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaxUnitModelCount = table.Column<int>(nullable: false),
                    MinUnitModelCount = table.Column<int>(nullable: false),
                    ModelName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Damage = table.Column<string>(nullable: true),
                    HitMortalWoundDamage = table.Column<int>(nullable: false),
                    HitMortalWoundNumber = table.Column<int>(nullable: false),
                    ModelId = table.Column<int>(nullable: false),
                    NumberOfAttacks = table.Column<int>(nullable: false),
                    Rend = table.Column<int>(nullable: false),
                    SpecialDamageDamage = table.Column<int>(nullable: false),
                    SpecialDamageNumber = table.Column<int>(nullable: false),
                    ToHitNumber = table.Column<int>(nullable: false),
                    ToWoundNumber = table.Column<int>(nullable: false),
                    WeaponName = table.Column<string>(nullable: true),
                    WoundMortalWoundDamage = table.Column<int>(nullable: false),
                    WoundMortalWoundNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weapons_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_ModelId",
                table: "Weapons",
                column: "ModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "Models");
        }
    }
}
