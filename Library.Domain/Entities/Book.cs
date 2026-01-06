using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public partial class Book
    {
        public long BookId { get; set; }

        public string? Isbn { get; set; }

        public string Title { get; set; } = null!;

        public string? Subtitle { get; set; }

        public string Author { get; set; } = null!;

        public string? Publisher { get; set; }

        public int? PublicationYear { get; set; }

        public string? Language { get; set; }

        public int? Pages { get; set; }

        public int? CategoryId { get; set; }

        public string? Description { get; set; }

        public string? CoverImageUrl { get; set; }

        public int? TotalCopies { get; set; }

        public int? AvailableCopies { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();

        public virtual ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();

        public virtual Category? Category { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
