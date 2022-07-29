using OverlayTimer.Entities;
using System;
using System.Collections.Generic;

namespace OverlayTimer.Models
{
    internal class DataModel
    {
        public List<DataEntity> Data { get; set; } = new List<DataEntity>();

        public class DataEntity
        {
            public DataEntity(Entry entry)
            {
                Entry = entry;
                Name = entry.Username;
                VideoLink = entry.VideoLink;
                Time = FormatTime(entry.TimeScore);
                PreviewLink = FormatURL(entry.VideoLink);
                Date = entry.SubmitDate.ToLocalTime().ToShortDateString();
            }

            public Entry Entry { get; set; }
            public string Name { get; set; }
            public string Time { get; set; }
            public string Date { get; set; }
            public string PreviewLink { get; set; }
            public string VideoLink { get; set; }
        }

        private static string FormatTime(TimeSpan time)
        {
            var timeStr = time.ToString("hh'h 'mm'm 'ss's 'fff' ms'");
            while (timeStr.StartsWith("00"))
            {
                timeStr = timeStr.Substring(4);
            }
            return timeStr;
        }

        private static string FormatURL(string previewLink)
        {
            if (!string.IsNullOrWhiteSpace(previewLink))
            {
                if (previewLink.StartsWith("http://"))
                {
                    previewLink = previewLink.Remove(0, 7);
                }
                else if (previewLink.StartsWith("https://"))
                {
                    previewLink = previewLink.Remove(0, 8);
                }
            }
            else
            {
                return "Not specified";
            }
            return previewLink;
        }
    }
}
