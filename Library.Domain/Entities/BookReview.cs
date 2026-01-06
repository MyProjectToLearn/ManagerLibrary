using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public partial class BookReview
    {
        public long ReviewId { get; set; }

        public long BookId { get; set; }

        public long UserId { get; set; }

        public byte? Rating { get; set; }

        public string? ReviewText { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual Book Book { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
