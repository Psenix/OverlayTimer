using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverlayTimer.Entities
{
    internal class FullEntry
    {
        public DateTime SubmitDate { get; set; }

        public Guid Guid { get; set; }

        public string Username { get; set; }

        public string Category { get; set; }

        public TimeSpan TimeScore { get; set; }

        public string VideoLink { get; set; }
    }
}
