using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using VideoLibrary;

namespace tibrudna.djcort.src.Models
{
    public class Song
    {
        [Column("id")]
        public string ID { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("artist")]
        public string Artist { get; set; }
        [NotMapped]
        public string Url { get { return $"https://www.youtube.com/watch?v={ID}"; } }
        [NotMapped]
        public string ThumbnailUrl { get { return $"https://img.youtube.com/vi/{ID}/hqdefault.jpg"; } }
        [NotMapped]
        public string StreamUrl { get; set; }
    }
}