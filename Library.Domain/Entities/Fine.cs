using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public partial class Fine
    {
        public long FineId { get; set; }

        public long UserId { get; set; }

        public long? RecordId { get; set; }

        public decimal Amount { get; set; }

        public string? Reason { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? PaidAt { get; set; }

        public virtual BorrowingRecord? Record { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
