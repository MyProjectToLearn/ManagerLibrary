// Library.Domain/Entities/BorrowingRecord.cs
namespace Library.Domain.Entities
{
    public partial class BorrowingRecord
    {
        public long RecordId { get; set; }
        public long UserId { get; set; }
        public long CopyId { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? Status { get; set; }
        public long? LibrarianId { get; set; }
        public string? Notes { get; set; }

        public virtual BookCopy Copy { get; set; } = null!;
        public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();
        public virtual User? Librarian { get; set; }        // Thủ thư cấp sách
        public virtual User User { get; set; } = null!;     // Người mượn
    }
}