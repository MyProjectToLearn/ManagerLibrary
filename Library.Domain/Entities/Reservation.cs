using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public partial class Reservation
    {
        public long ReservationId { get; set; }

        public long UserId { get; set; }

        public long BookId { get; set; }

        public DateTime? ReservationDate { get; set; }

        public string? Status { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public virtual Book Book { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
