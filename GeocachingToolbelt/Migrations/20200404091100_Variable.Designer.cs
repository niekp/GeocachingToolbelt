﻿// <auto-generated />
using GeocachingToolbelt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeocachingToolbelt.Migrations
{
    [DbContext(typeof(ToolbeltContext))]
    [Migration("20200404091100_Variable")]
    partial class Variable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("GeocachingToolbelt.Models.Multi", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Multi");
                });

            modelBuilder.Entity("GeocachingToolbelt.Models.Variable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Letter")
                        .HasColumnType("TEXT");

                    b.Property<int>("MultiId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MultiId");

                    b.ToTable("Variable");
                });

            modelBuilder.Entity("GeocachingToolbelt.Models.Waypoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Coordinate")
                        .HasColumnType("TEXT");

                    b.Property<int>("MultiId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MultiId");

                    b.ToTable("Waypoint");
                });

            modelBuilder.Entity("GeocachingToolbelt.Models.Variable", b =>
                {
                    b.HasOne("GeocachingToolbelt.Models.Multi", "Multi")
                        .WithMany()
                        .HasForeignKey("MultiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GeocachingToolbelt.Models.Waypoint", b =>
                {
                    b.HasOne("GeocachingToolbelt.Models.Multi", "Multi")
                        .WithMany("Waypoints")
                        .HasForeignKey("MultiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
