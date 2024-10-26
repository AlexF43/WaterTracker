﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WaterTracker;

#nullable disable

namespace WaterTracker.Migrations
{
    [DbContext(typeof(WaterTrackerDbContext))]
    partial class WaterTrackerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("WaterTracker.Model.User", b =>
                {
                    b.Property<string>("userId")
                        .HasColumnType("TEXT");

                    b.Property<double>("dailyGoal")
                        .HasColumnType("REAL");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("userPwd")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("weeklyGoal")
                        .HasColumnType("REAL");

                    b.HasKey("userId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WaterTracker.Model.WaterAmount", b =>
                {
                    b.Property<string>("usageType")
                        .HasColumnType("TEXT");

                    b.Property<double>("usageLiterPerSec")
                        .HasColumnType("REAL");

                    b.HasKey("usageType");

                    b.ToTable("WaterAmounts");
                });

            modelBuilder.Entity("WaterTracker.Model.WaterUsage", b =>
                {
                    b.Property<string>("usageId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("date")
                        .HasColumnType("TEXT");

                    b.Property<double>("totalUsage")
                        .HasColumnType("REAL");

                    b.Property<string>("usageName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("usageType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("usedSec")
                        .HasColumnType("INTEGER");

                    b.Property<string>("userId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("usageId");

                    b.ToTable("WaterUsages");
                });
#pragma warning restore 612, 618
        }
    }
}
