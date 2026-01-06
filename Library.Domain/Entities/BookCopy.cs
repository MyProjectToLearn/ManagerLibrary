using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public partial class BookCopy
    {
        public long CopyId { get; set; }

        public long BookId { get; set; }

        public string Barcode { get; set; } = null!;

        public string? Status { get; set; }

        public string? Location { get; set; }

        public DateOnly? AcquisitionDate { get; set; }

        public string? ConditionStatus { get; set; }

        public virtual Book Book { get; set; } = null!;

        public virtual ICollection<BorrowingRecord> BorrowingRecords { get; set; } = new List<BorrowingRecord>();
    }
}
