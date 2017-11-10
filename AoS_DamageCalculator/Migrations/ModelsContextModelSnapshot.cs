using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AoS_DamageCalculator.Models;

namespace AoS_DamageCalculator.Migrations
{
    [DbContext(typeof(ModelsContext))]
    partial class ModelsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("AoS_DamageCalculator.Models.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MaxUnitModelCount");

                    b.Property<int>("MinUnitModelCount");

                    b.Property<string>("ModelName");

                    b.HasKey("Id");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("AoS_DamageCalculator.Models.Weapon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Damage");

                    b.Property<int>("HitMortalWoundDamage");

                    b.Property<int>("HitMortalWoundNumber");

                    b.Property<int>("ModelId");

                    b.Property<int>("NumberOfAttacks");

                    b.Property<int>("Rend");

                    b.Property<int>("SpecialDamageDamage");

                    b.Property<int>("SpecialDamageNumber");

                    b.Property<int>("ToHitNumber");

                    b.Property<int>("ToWoundNumber");

                    b.Property<string>("WeaponName");

                    b.Property<int>("WoundMortalWoundDamage");

                    b.Property<int>("WoundMortalWoundNumber");

                    b.HasKey("Id");

                    b.HasIndex("ModelId");

                    b.ToTable("Weapons");
                });

            modelBuilder.Entity("AoS_DamageCalculator.Models.Weapon", b =>
                {
                    b.HasOne("AoS_DamageCalculator.Models.Model", "Model")
                        .WithMany("Weapons")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
