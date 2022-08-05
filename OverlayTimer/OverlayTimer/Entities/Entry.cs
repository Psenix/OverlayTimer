using System;

namespace OverlayTimer.Entities
{
    public class Entry
    {
        public Guid Guid { get; set; }

        public Guid ID { get; set; }

        public string Username { get; set; }

        public string Category { get; set; }

        public TimeSpan TimeScore { get; set; }

        public DateTime SubmitDate { get; set; }

        public string VideoLink { get; set; }

        public Guid Key { get; set; }


    }
}
