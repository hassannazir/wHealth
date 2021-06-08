using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace wHealthApi.Models
{
    public partial class wHealthappDbContext : DbContext
    {
        public wHealthappDbContext()
        {
        }

        public wHealthappDbContext(DbContextOptions<wHealthappDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Appointmentdetail> Appointmentdetails { get; set; }
        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Doctorclinic> Doctorclinics { get; set; }
        public virtual DbSet<General> Generals { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=den1.mssql7.gear.host;Database=whealthappdb;User ID=whealthappdb;Password=Ts3DWoZk-!97;Trusted_Connection=False;");
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

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");

                

                entity.Property(e => e.PatientId).HasColumnName("patient_id");

                entity.Property(e => e.ScheduleId)
                    .IsUnicode(false)
                    .HasColumnName("scheduleId");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.EndTime).HasColumnName("end_time");
            });

            modelBuilder.Entity<Appointmentdetail>(entity =>
            {
                entity.ToTable("appointmentdetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");

                entity.Property(e => e.Complain).HasColumnName("complain");

                entity.Property(e => e.DetailFromPatient).HasColumnName("detail_from_patient");

                entity.Property(e => e.Diagnose)
                    .HasMaxLength(50)
                    .HasColumnName("diagnose");

                entity.Property(e => e.Prescription).HasColumnName("prescription");

                entity.Property(e => e.Remarks).HasColumnName("remarks");
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

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("schedule");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClinicId).HasColumnName("clinic_id");

                entity.Property(e => e.Day)
                    .HasMaxLength(50)
                    .HasColumnName("day");

                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.EndTime).HasColumnName("end_time");

                entity.Property(e => e.Recurring).HasColumnName("recurring");

                entity.Property(e => e.SlotLength).HasColumnName("slot_length");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.StartTime).HasColumnName("start_time");
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

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("schedule");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClinicId).HasColumnName("clinic_id");

                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");

                entity.Property(e => e.EndTime)
                    .HasColumnType("time")
                    .HasColumnName("end_time");

                entity.Property(e => e.StartTime)
                    .HasColumnType("time")
                    .HasColumnName("start_time");

                entity.Property(e => e.StartDate)
                   .HasColumnType("datetime")
                   .HasColumnName("start_date");


                entity.Property(e => e.EndDate)
                   .HasColumnType("datetime")
                   .HasColumnName("end_date");

                entity.Property(e => e.Recurring)
                    .HasColumnName("recurring");

                entity.Property(e => e.Day)
                     .HasColumnName("day");

                entity.Property(e => e.SlotLength)
                     .HasColumnName("slot_length");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
