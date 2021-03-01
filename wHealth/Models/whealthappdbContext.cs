using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace wHealth
{
    public partial class whealthappdbContext : DbContext
    {
        public whealthappdbContext()
        {
        }

        public whealthappdbContext(DbContextOptions<whealthappdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<General> Generals { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:wHealthCS");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("appointment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClinicId).HasColumnName("clinic_id");

                entity.Property(e => e.Complain).HasColumnName("complain");

                entity.Property(e => e.DetailFromPatient).HasColumnName("detail_from_patient");

                entity.Property(e => e.Diagnose)
                    .HasMaxLength(50)
                    .HasColumnName("diagnose");

                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");

                entity.Property(e => e.PatientId).HasColumnName("patient_id");

                entity.Property(e => e.Prescription).HasColumnName("prescription");

                entity.Property(e => e.Remarks).HasColumnName("remarks");

                entity.Property(e => e.Slot)
                    .HasColumnType("datetime")
                    .HasColumnName("slot");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<Clinic>(entity =>
            {
                entity.ToTable("clinic");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(50)
                    .HasColumnName("phone_no");

                entity.Property(e => e.RegistrationNo)
                    .HasMaxLength(50)
                    .HasColumnName("registration_no");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("doctor");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Experience)
                    .HasMaxLength(50)
                    .HasColumnName("experience");

                entity.Property(e => e.LicenseNo)
                    .HasMaxLength(50)
                    .HasColumnName("license_no");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(50)
                    .HasColumnName("phone_no");

                entity.Property(e => e.ProfilePic).HasColumnName("profile_pic");

                entity.Property(e => e.Qualification)
                    .HasMaxLength(50)
                    .HasColumnName("qualification");
            });

            modelBuilder.Entity<Doctorclinic>(entity =>
            {
                entity.ToTable("doctorclinic");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClinicId).HasColumnName("clinic_id");

                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<General>(entity =>
            {
                entity.ToTable("general");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("patient");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(50)
                    .HasColumnName("phone_no");

                entity.Property(e => e.ProfilePic).HasColumnName("profile_pic");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClinicId).HasColumnName("clinic_id");

                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.PatientId).HasColumnName("patient_id");

                entity.Property(e => e.RegisteredDate)
                    .HasColumnType("datetime")
                    .HasColumnName("registered_date");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
