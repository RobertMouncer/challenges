﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using challenges.Models;

namespace challenges.Migrations
{
    [DbContext(typeof(challengesContext))]
    partial class challengesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("challenges.Models.Activity", b =>
                {
                    b.Property<int>("ActivityId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivityName")
                        .IsRequired();

                    b.Property<string>("GoalMetric")
                        .IsRequired();

                    b.HasKey("ActivityId");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("challenges.Models.Challenge", b =>
                {
                    b.Property<int>("ChallengeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActivityId");

                    b.Property<DateTime>("EndDateTime");

                    b.Property<int>("Goal");

                    b.Property<int>("PercentageComplete");

                    b.Property<bool>("Repeat");

                    b.Property<DateTime>("StartDateTime");

                    b.Property<int>("UserGroupId");

                    b.HasKey("ChallengeId");

                    b.HasIndex("ActivityId");

                    b.HasIndex("UserGroupId");

                    b.ToTable("Challenge");
                });

            modelBuilder.Entity("challenges.Models.UserGroup", b =>
                {
                    b.Property<int>("UserGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GroupId");

                    b.Property<string>("UserId");

                    b.Property<bool>("isGroup");

                    b.HasKey("UserGroupId");

                    b.ToTable("UserGroup");
                });

            modelBuilder.Entity("challenges.Models.Challenge", b =>
                {
                    b.HasOne("challenges.Models.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("challenges.Models.UserGroup", "UserGroup")
                        .WithMany()
                        .HasForeignKey("UserGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
