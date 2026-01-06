using Microsoft.EntityFrameworkCore;
using Library.Domain.Entities;

namespace Library.Infrastructure.Data
{
    public class ManagerLibraryDemoContext : DbContext
    {
        public ManagerLibraryDemoContext(DbContextOptions<ManagerLibraryDemoContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<BorrowingRecord> BorrowingRecords { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<BookReview> BookReviews { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).ValueGeneratedOnAdd();
                entity.Property(e => e.Username).HasMaxLength(50).IsRequired().HasColumnType("varchar(50)");
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired().HasColumnType("varchar(100)");
                entity.Property(e => e.PasswordHash).HasMaxLength(255).IsRequired();
                entity.Property(e => e.FullName).HasMaxLength(100).IsRequired().HasColumnType("nvarchar(100)");
                entity.Property(e => e.Phone).HasMaxLength(20).HasColumnType("varchar(20)");
                entity.Property(e => e.Address).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Role).HasMaxLength(20).HasDefaultValue("member");
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("active");

                entity.HasIndex(e => e.Email).HasDatabaseName("idx_email");
                entity.HasIndex(e => e.Username).HasDatabaseName("idx_username");
                entity.HasIndex(e => e.Role).HasDatabaseName("idx_role");
            });

            // 2. Category (self-referencing)
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryName).HasMaxLength(100).IsRequired().HasColumnType("nvarchar(100)");
                entity.Property(e => e.Description).HasColumnType("nvarchar(max)");

                entity.HasOne(c => c.ParentCategory)
                      .WithMany(c => c.ChildCategories) 
                      .HasForeignKey(c => c.ParentCategoryId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // 3. Book
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("books");
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.Isbn).HasMaxLength(13).HasColumnType("varchar(13)");
                entity.Property(e => e.Title).HasMaxLength(255).IsRequired().HasColumnType("nvarchar(255)");
                entity.Property(e => e.Subtitle).HasMaxLength(255).HasColumnType("nvarchar(255)");
                entity.Property(e => e.Author).HasMaxLength(255).IsRequired().HasColumnType("nvarchar(255)");
                entity.Property(e => e.Publisher).HasMaxLength(100).HasColumnType("nvarchar(100)");
                entity.Property(e => e.PublicationYear).HasColumnType("int");
                entity.Property(e => e.Language).HasMaxLength(50).HasDefaultValue("Vietnamese").HasColumnType("nvarchar(50)");
                entity.Property(e => e.Pages).HasColumnType("int");
                entity.Property(e => e.Description).HasColumnType("nvarchar(max)");
                entity.Property(e => e.CoverImageUrl).HasMaxLength(500).HasColumnType("varchar(500)");
                entity.Property(e => e.TotalCopies).HasDefaultValue(1);
                entity.Property(e => e.AvailableCopies).HasDefaultValue(1);

                entity.HasIndex(e => e.Isbn).HasDatabaseName("idx_isbn");
                entity.HasIndex(e => e.Title).HasDatabaseName("idx_title");
                entity.HasIndex(e => e.Author).HasDatabaseName("idx_author");
                entity.HasIndex(e => e.CategoryId).HasDatabaseName("idx_category");

                // Mối quan hệ
                entity.HasOne(b => b.Category)
                      .WithMany(c => c.Books)
                      .HasForeignKey(b => b.CategoryId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // 4. BookCopy
            modelBuilder.Entity<BookCopy>(entity =>
            {
                entity.ToTable("book_copies");
                entity.HasKey(e => e.CopyId);
                entity.Property(e => e.BookId).IsRequired();
                entity.Property(e => e.Barcode).HasMaxLength(50).IsRequired().HasColumnType("varchar(50)");
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("available");
                entity.Property(e => e.Location).HasMaxLength(100).HasColumnType("nvarchar(100)");
                entity.Property(e => e.ConditionStatus).HasMaxLength(20).HasDefaultValue("good");

                entity.HasIndex(e => e.Barcode).HasDatabaseName("idx_barcode");
                entity.HasIndex(e => new { e.BookId, e.Status }).HasDatabaseName("idx_book_status");

                entity.HasOne(bc => bc.Book)
                      .WithMany(b => b.BookCopies)
                      .HasForeignKey(bc => bc.BookId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 5. BorrowingRecord
            modelBuilder.Entity<BorrowingRecord>(entity =>
            {
                entity.ToTable("borrowing_records");
                entity.HasKey(e => e.RecordId);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.CopyId).IsRequired();
                entity.Property(e => e.BorrowDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.DueDate).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("borrowed");
                entity.Property(e => e.LibrarianId).IsRequired(false);

                entity.HasIndex(e => e.UserId).HasDatabaseName("idx_user_status");
                entity.HasIndex(e => e.BorrowDate).HasDatabaseName("idx_borrow_date");
                entity.HasIndex(e => e.DueDate).HasDatabaseName("idx_due_date");
                entity.HasIndex(e => e.Status).HasDatabaseName("idx_status");

                // Mối quan hệ: Người mượn
                entity.HasOne(br => br.User)
                      .WithMany(u => u.BorrowedRecords)
                      .HasForeignKey(br => br.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Mối quan hệ: Thủ thư cấp sách (optional)
                entity.HasOne(br => br.Librarian)
                      .WithMany(u => u.IssuedBy)
                      .HasForeignKey(br => br.LibrarianId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .IsRequired(false);

                // Mối quan hệ: Bản sao sách
                entity.HasOne(br => br.Copy)
                      .WithMany(bc => bc.BorrowingRecords)
                      .HasForeignKey(br => br.CopyId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 6. Reservation
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("reservations");
                entity.HasKey(e => e.ReservationId);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.BookId).IsRequired();
                entity.Property(e => e.ReservationDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("pending");

                entity.HasIndex(e => new { e.UserId, e.Status }).HasDatabaseName("idx_user_status");
                entity.HasIndex(e => new { e.BookId, e.Status }).HasDatabaseName("idx_book_status");

                entity.HasOne(r => r.User)
                      .WithMany(u => u.Reservations)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Book)
                      .WithMany(b => b.Reservations)
                      .HasForeignKey(r => r.BookId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 7. Fine
            modelBuilder.Entity<Fine>(entity =>
            {
                entity.ToTable("fines");
                entity.HasKey(e => e.FineId);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Amount).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.Reason).HasMaxLength(255).HasColumnType("nvarchar(255)");
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("unpaid");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => new { e.UserId, e.Status }).HasDatabaseName("idx_user_status");

                entity.HasOne(f => f.User)
                      .WithMany(u => u.Fines)
                      .HasForeignKey(f => f.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.Record)
                      .WithMany(br => br.Fines)
                      .HasForeignKey(f => f.RecordId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .IsRequired(false);
            });

            // 8. BookReview
            modelBuilder.Entity<BookReview>(entity =>
            {
                entity.ToTable("book_reviews");
                entity.HasKey(e => e.ReviewId);
                entity.Property(e => e.BookId).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Rating).HasColumnType("tinyint").IsRequired();
                entity.Property(e => e.ReviewText).HasColumnType("nvarchar(max)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => new { e.BookId, e.Rating }).HasDatabaseName("idx_book_rating");

                entity.HasOne(br => br.Book)
                      .WithMany(b => b.BookReviews)
                      .HasForeignKey(br => br.BookId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(br => br.User)
                      .WithMany(u => u.Reviews)
                      .HasForeignKey(br => br.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 9. ActivityLog
            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.ToTable("activity_logs");
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.Action).HasMaxLength(50).IsRequired().HasColumnType("varchar(50)");
                entity.Property(e => e.EntityType).HasMaxLength(50).HasColumnType("varchar(50)");
                entity.Property(e => e.IpAddress).HasMaxLength(45).HasColumnType("varchar(45)");
                entity.Property(e => e.UserAgent).HasColumnType("nvarchar(max)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => new { e.UserId, e.CreatedAt }).HasDatabaseName("idx_user_date");
                entity.HasIndex(e => e.Action).HasDatabaseName("idx_action");
            });
        }
    }
}