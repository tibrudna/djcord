using System.Text.RegularExpressions;
using VideoLibrary;

namespace tibrudna.djcort.src.Models
{
    public class Song
    {
        public string ID { get; set; }
        public string StreamUrl { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Url { get { return $"https://www.youtube.com/watch?v={ID}"; } }
        public string ThumbnailUrl { get { return $"https://img.youtube.com/vi/{ID}/hqdefault.jpg"; } }

    }
}