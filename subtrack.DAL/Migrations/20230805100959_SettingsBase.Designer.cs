﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using subtrack.DAL;

#nullable disable

namespace subtrack.DAL.Migrations
{
    [DbContext(typeof(SubtrackDbContext))]
    [Migration("20230805100959_SettingsBase")]
    partial class SettingsBase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.18");

            modelBuilder.Entity("subtrack.DAL.Entities.SettingsBase", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("settings_type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Settings");

                    b.HasDiscriminator<string>("settings_type").HasValue("SettingsBase");
                });

            modelBuilder.Entity("subtrack.DAL.Entities.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Cost")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAutoPaid")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastPayment")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("subtrack.DAL.Entities.DateTimeSetting", b =>
                {
                    b.HasBaseType("subtrack.DAL.Entities.SettingsBase");

                    b.Property<DateTime?>("Value")
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("DateTimeSetting");
                });
#pragma warning restore 612, 618
        }
    }
}
