﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using challenges.Data;

namespace challenges.Migrations
{
    [DbContext(typeof(challengesContext))]
    partial class ChallengesContextModelSnapshot : ModelSnapshot
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

                    b.Property<string>("ActivityName");

                    b.Property<int>("DbActivityId");

                    b.HasKey("ActivityId");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("challenges.Models.Challenge", b =>
                {
                    b.Property<int>("ChallengeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActivityId");

                    b.Property<DateTime>("EndDateTime");

                    b.Property<double>("Goal");

                    b.Property<int>("GoalMetricId");

                    b.Property<string>("Groupid");

                    b.Property<bool>("IsGroupChallenge");

                    b.Property<DateTime>("StartDateTime");

                    b.HasKey("ChallengeId");

                    b.HasIndex("ActivityId");

                    b.HasIndex("GoalMetricId");

                    b.ToTable("Challenge");
                });

            modelBuilder.Entity("challenges.Models.GoalMetric", b =>
                {
                    b.Property<int>("GoalMetricId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GoalMetricDbName");

                    b.Property<string>("GoalMetricDisplay");

                    b.HasKey("GoalMetricId");

                    b.ToTable("GoalMetric");
                });

            modelBuilder.Entity("challenges.Models.UserChallenge", b =>
                {
                    b.Property<int>("UserChallengeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChallengeId");

                    b.Property<bool>("EmailSent");

                    b.Property<int>("PercentageComplete");

                    b.Property<string>("UserId");

                    b.HasKey("UserChallengeId");

                    b.HasIndex("ChallengeId");

                    b.ToTable("UserChallenge");
                });

            modelBuilder.Entity("challenges.Models.Challenge", b =>
                {
                    b.HasOne("challenges.Models.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("challenges.Models.GoalMetric", "GoalMetric")
                        .WithMany()
                        .HasForeignKey("GoalMetricId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("challenges.Models.UserChallenge", b =>
                {
                    b.HasOne("challenges.Models.Challenge", "Challenge")
                        .WithMany()
                        .HasForeignKey("ChallengeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
