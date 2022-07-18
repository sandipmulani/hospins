using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace hospins.Repository.Data
{
    public partial class hospinsContext : DbContext
    {
        public hospinsContext(DbContextOptions<hospinsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AddressType> AddressTypes { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Designation> Designations { get; set; } = null!;
        public virtual DbSet<DocumentType> DocumentTypes { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<EmployeeAddress> EmployeeAddresses { get; set; } = null!;
        public virtual DbSet<EmployeeDocument> EmployeeDocuments { get; set; } = null!;
        public virtual DbSet<EmployeeHistory> EmployeeHistories { get; set; } = null!;
        public virtual DbSet<EmployeeSalarySetup> EmployeeSalarySetups { get; set; } = null!;
        public virtual DbSet<Logbook> Logbooks { get; set; } = null!;
        public virtual DbSet<Priority> Priorities { get; set; } = null!;
        public virtual DbSet<SalaryType> SalaryTypes { get; set; } = null!;
        public virtual DbSet<State> States { get; set; } = null!;
        public virtual DbSet<SubCategory> SubCategories { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=hospins;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressType>(entity =>
            {
                entity.ToTable("AddressType");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.Code).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(510);
            });

            modelBuilder.Entity<Designation>(entity =>
            {
                entity.ToTable("Designation");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.ToTable("DocumentType");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.AddressLine1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.AddressLine2)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.BloodGroup)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Picture)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmployeeAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId);

                entity.ToTable("EmployeeAddress");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmployeeDocument>(entity =>
            {
                entity.ToTable("EmployeeDocument");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FileName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmployeeHistory>(entity =>
            {
                entity.ToTable("EmployeeHistory");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.JoiningDate).HasColumnType("date");

                entity.Property(e => e.LastSalary)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.ReleasingDate).HasColumnType("date");
            });

            modelBuilder.Entity<EmployeeSalarySetup>(entity =>
            {
                entity.HasKey(e => e.SalarySetupId);

                entity.ToTable("EmployeeSalarySetup");

                entity.Property(e => e.Basis)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BenefitsType)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Ctc).HasColumnName("CTC");

                entity.Property(e => e.Health)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.SalaryBenefits)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Transport)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Logbook>(entity =>
            {
                entity.ToTable("Logbook");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Priority>(entity =>
            {
                entity.ToTable("Priority");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<SalaryType>(entity =>
            {
                entity.ToTable("SalaryType");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.ToTable("SubCategory");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
