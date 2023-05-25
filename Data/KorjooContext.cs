using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using KorJoo.Models.korjoo;

namespace KorJoo.Data
{
    public partial class korjooContext : DbContext
    {
        public korjooContext()
        {
        }

        public korjooContext(DbContextOptions<korjooContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<KorJoo.Models.korjoo.ApplicantCoursesTest>()
              .HasOne(i => i.Applicant)
              .WithMany(i => i.ApplicantCoursesTests)
              .HasForeignKey(i => i.ApplicantId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<KorJoo.Models.korjoo.ApplicantEducationHistory>()
              .HasOne(i => i.Applicant)
              .WithMany(i => i.ApplicantEducationHistories)
              .HasForeignKey(i => i.ApplicantId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<KorJoo.Models.korjoo.ApplicantPortofolio>()
              .HasOne(i => i.Applicant)
              .WithMany(i => i.ApplicantPortofolios)
              .HasForeignKey(i => i.ApplicantId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<KorJoo.Models.korjoo.ApplicantSkill>()
              .HasOne(i => i.Applicant)
              .WithMany(i => i.ApplicantSkills)
              .HasForeignKey(i => i.ApplicantId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<KorJoo.Models.korjoo.ApplicantSkill>()
              .HasOne(i => i.Skill)
              .WithMany(i => i.ApplicantSkills)
              .HasForeignKey(i => i.SkillId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<KorJoo.Models.korjoo.ApplicantWorkHistory>()
              .HasOne(i => i.Applicant)
              .WithMany(i => i.ApplicantWorkHistories)
              .HasForeignKey(i => i.ApplicantId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<KorJoo.Models.korjoo.JobLanguage>()
              .HasOne(i => i.Job)
              .WithMany(i => i.JobLanguages)
              .HasForeignKey(i => i.JobId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<KorJoo.Models.korjoo.JobSkill>()
              .HasOne(i => i.Job)
              .WithMany(i => i.JobSkills)
              .HasForeignKey(i => i.JobId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<KorJoo.Models.korjoo.JobSkill>()
              .HasOne(i => i.Skill)
              .WithMany(i => i.JobSkills)
              .HasForeignKey(i => i.SkillId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<KorJoo.Models.korjoo.Specialization>()
              .HasOne(i => i.SpecCategory)
              .WithMany(i => i.Specializations)
              .HasForeignKey(i => i.SpecCategoryId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<KorJoo.Models.korjoo.ApplicantCoursesTest>()
              .Property(p => p.IssueDate)
              .HasColumnType("datetime");

            builder.Entity<KorJoo.Models.korjoo.ApplicantEducationHistory>()
              .Property(p => p.StartDate)
              .HasColumnType("datetime");

            builder.Entity<KorJoo.Models.korjoo.ApplicantEducationHistory>()
              .Property(p => p.EndDate)
              .HasColumnType("datetime");

            builder.Entity<KorJoo.Models.korjoo.ApplicantSkill>()
              .Property(p => p.CreatedAt)
              .HasColumnType("datetime");

            builder.Entity<KorJoo.Models.korjoo.ApplicantWorkHistory>()
              .Property(p => p.StartDate)
              .HasColumnType("datetime");

            builder.Entity<KorJoo.Models.korjoo.ApplicantWorkHistory>()
              .Property(p => p.EndDate)
              .HasColumnType("datetime");

            builder.Entity<KorJoo.Models.korjoo.Applicant>()
              .Property(p => p.Birthday)
              .HasColumnType("datetime");
            this.OnModelBuilding(builder);
        }

        public DbSet<KorJoo.Models.korjoo.ApplicantCoursesTest> ApplicantCoursesTests { get; set; }

        public DbSet<KorJoo.Models.korjoo.ApplicantEducationHistory> ApplicantEducationHistories { get; set; }

        public DbSet<KorJoo.Models.korjoo.ApplicantLanguage> ApplicantLanguages { get; set; }

        public DbSet<KorJoo.Models.korjoo.ApplicantPortofolio> ApplicantPortofolios { get; set; }

        public DbSet<KorJoo.Models.korjoo.ApplicantSkill> ApplicantSkills { get; set; }

        public DbSet<KorJoo.Models.korjoo.ApplicantWorkHistory> ApplicantWorkHistories { get; set; }

        public DbSet<KorJoo.Models.korjoo.Applicant> Applicants { get; set; }

        public DbSet<KorJoo.Models.korjoo.Citiie> Citiies { get; set; }

        public DbSet<KorJoo.Models.korjoo.Company> Companies { get; set; }

        public DbSet<KorJoo.Models.korjoo.JobLanguage> JobLanguages { get; set; }

        public DbSet<KorJoo.Models.korjoo.JobSkill> JobSkills { get; set; }

        public DbSet<KorJoo.Models.korjoo.Job> Jobs { get; set; }

        public DbSet<KorJoo.Models.korjoo.Skill> Skills { get; set; }

        public DbSet<KorJoo.Models.korjoo.SpecCategory> SpecCategories { get; set; }

        public DbSet<KorJoo.Models.korjoo.Specialization> Specializations { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}