using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
    public partial class ActivityLog
    {
        public long LogId { get; set; }

        public long? UserId { get; set; }

        public string Action { get; set; } = null!;

        public string? EntityType { get; set; }

        public long? EntityId { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        public DateTime? CreatedAt { get; set; }
    }

}
