using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LibraryManagementWpf.Models
{
    public partial class LibraryManageSystemContext : DbContext
    {
        public LibraryManageSystemContext()
        {
        }

        public LibraryManageSystemContext(DbContextOptions<LibraryManageSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<BorrowBook> BorrowBooks { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Fine> Fines { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Thesis> Theses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =localhost; database = LibraryManageSystem;uid=sa;pwd=123;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.AuthorName)
                    .HasMaxLength(255)
                    .HasColumnName("author_name");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .HasColumnName("image");

                entity.Property(e => e.PublicationDate)
                    .HasColumnType("date")
                    .HasColumnName("publication_date");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Books_Categories");

                entity.HasMany(d => d.Authors)
                    .WithMany(p => p.Books)
                    .UsingEntity<Dictionary<string, object>>(
                        "BooksAuthor",
                        l => l.HasOne<Author>().WithMany().HasForeignKey("AuthorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Books_Authors_Authors"),
                        r => r.HasOne<Book>().WithMany().HasForeignKey("BookId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Books_Authors_Books"),
                        j =>
                        {
                            j.HasKey("BookId", "AuthorId");

                            j.ToTable("Books_Authors");

                            j.IndexerProperty<int>("BookId").HasColumnName("book_id");

                            j.IndexerProperty<int>("AuthorId").HasColumnName("author_id");
                        });
            });

            modelBuilder.Entity<BorrowBook>(entity =>
            {
                entity.HasKey(e => e.BorrowId)
                    .HasName("PK__BorrowBo__262B57A0CD3880E6");

                entity.Property(e => e.BorrowId).HasColumnName("borrow_id");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.BorrowDate)
                    .HasColumnType("date")
                    .HasColumnName("borrow_date");

                entity.Property(e => e.DueDate)
                    .HasColumnType("date")
                    .HasColumnName("due_date");

                entity.Property(e => e.LibrarianInCharge)
                    .HasMaxLength(50)
                    .HasColumnName("librarian_in_charge");

                entity.Property(e => e.ReservationDate)
                    .HasColumnType("date")
                    .HasColumnName("reservation_date");

                entity.Property(e => e.ReturnDate)
                    .HasColumnType("date")
                    .HasColumnName("return_date");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BorrowBooks)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK_BorrowBooks_Books");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BorrowBooks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_BorrowBooks_Users");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .HasColumnName("category_name");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");
            });

            modelBuilder.Entity<Fine>(entity =>
            {
                entity.Property(e => e.FineId).HasColumnName("fine_id");

                entity.Property(e => e.BorrowId).HasColumnName("borrow_id");

                entity.Property(e => e.FineAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("fine_amount");

                entity.Property(e => e.FineType)
                    .HasMaxLength(50)
                    .HasColumnName("fine_type");

                entity.Property(e => e.PaidDate)
                    .HasColumnType("date")
                    .HasColumnName("paid_date");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Borrow)
                    .WithMany(p => p.Fines)
                    .HasForeignKey(d => d.BorrowId)
                    .HasConstraintName("FK_Fines_BorrowBooks");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.ReportId).HasColumnName("report_id");

                entity.Property(e => e.BorrowId).HasColumnName("borrow_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.FineId).HasColumnName("fine_id");

                entity.Property(e => e.ReportReason)
                    .HasMaxLength(200)
                    .HasColumnName("report_reason");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasColumnName("status");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Borrow)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.BorrowId)
                    .HasConstraintName("FK_Reports_BorrowedBooks");

                entity.HasOne(d => d.Fine)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.FineId)
                    .HasConstraintName("FK_Reports_Fines");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Reports_Users");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Thesis>(entity =>
            {
                entity.ToTable("Thesis");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Author)
                    .HasMaxLength(255)
                    .HasColumnName("author");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("create_at");

                entity.Property(e => e.FileDoc)
                    .HasColumnType("text")
                    .HasColumnName("file_doc");

                entity.Property(e => e.Title)
                    .HasMaxLength(150)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__AB6E616400D088D7")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("date")
                    .HasColumnName("create_at");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.ExpirationCode)
                    .HasColumnType("date")
                    .HasColumnName("expiration_code");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(50)
                    .HasColumnName("fullname");

                entity.Property(e => e.Password)
                    .HasMaxLength(80)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .HasColumnName("phone");

                entity.Property(e => e.Role)
                    .HasMaxLength(10)
                    .HasColumnName("role");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.StudentCode)
                    .HasMaxLength(10)
                    .HasColumnName("student_code");

                entity.Property(e => e.VerifyCode)
                    .HasMaxLength(6)
                    .HasColumnName("verify_code");

                entity.Property(e => e.Image)
                   .HasColumnName("image");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
