﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230711000013_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.8");

            modelBuilder.Entity("Domain.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyDescription")
                        .HasMaxLength(5000)
                        .HasColumnType("TEXT");

                    b.Property<int>("CompanyMemberCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CompanyRegistryCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Domain.EventInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("EventDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventLocation")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EventInfos");
                });

            modelBuilder.Entity("Domain.EventParticipator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EventInfoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParticipatorId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EventInfoId");

                    b.HasIndex("ParticipatorId");

                    b.ToTable("EventParticipators");
                });

            modelBuilder.Entity("Domain.Participator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PaymentType")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PersonId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("PersonId");

                    b.ToTable("Participators");
                });

            modelBuilder.Entity("Domain.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("PersonDescription")
                        .HasMaxLength(1500)
                        .HasColumnType("TEXT");

                    b.Property<string>("PersonFirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PersonIdCode")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("TEXT");

                    b.Property<string>("PersonLastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Domain.EventParticipator", b =>
                {
                    b.HasOne("Domain.EventInfo", "EventInfo")
                        .WithMany()
                        .HasForeignKey("EventInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Participator", "Participator")
                        .WithMany()
                        .HasForeignKey("ParticipatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventInfo");

                    b.Navigation("Participator");
                });

            modelBuilder.Entity("Domain.Participator", b =>
                {
                    b.HasOne("Domain.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("Domain.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId");

                    b.Navigation("Company");

                    b.Navigation("Person");
                });
#pragma warning restore 612, 618
        }
    }
}
