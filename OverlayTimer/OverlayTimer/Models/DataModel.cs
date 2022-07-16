using System.Collections.Generic;

namespace OverlayTimer.Models
{
    internal class DataModel
    {
        public List<DataEntity> Data { get; set; } = new List<DataEntity>();

        public class DataEntity
        {
            public DataEntity(string name, string time, string date, string previewLink, string videoLink)
            {
                Name = name;
                Time = time;
                Date = date;
                PreviewLink = previewLink;
                VideoLink = videoLink;
            }

            public string Name { get; set; }
            public string Time { get; set; }
            public string Date { get; set; }
            public string PreviewLink { get; set; }
            public string VideoLink { get; set; }
        }
    }
}
