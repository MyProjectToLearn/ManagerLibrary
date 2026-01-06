// Library.Domain/Entities/User.cs
namespace Library.Domain.Entities
{
    public class User : BaseEntity
    {
        public long UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string Role { get; set; } = "member";
        public string Status { get; set; } = "active";

        // Navigation properties
        public ICollection<BorrowingRecord> BorrowedRecords { get; set; } = new List<BorrowingRecord>(); // Sách người dùng mượn
        public ICollection<BorrowingRecord> IssuedBy { get; set; } = new List<BorrowingRecord>();      // Sách do thủ thư cấp (mới thêm)

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Fine> Fines { get; set; } = new List<Fine>();
        public ICollection<BookReview> Reviews { get; set; } = new List<BookReview>();
    }
}