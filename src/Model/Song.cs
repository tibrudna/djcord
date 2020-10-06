using System.Text.RegularExpressions;
using VideoLibrary;

namespace tibrudna.djcort.src.Models
{
    public class Song
    {
        public string ID { get; private set; }
        public string Url { get; private set; }
        public string StreamUrl { get; private set; }
        public string Title { get; private set; }
        public string Artist { get; private set; }
        public string ThumbnailUrl { get { return $"https://img.youtube.com/vi/{ID}/hqdefault.jpg"; } }

        private Song(string ID, string Url, string StreamUrl, string Title, string Artist)
        {
            this.ID = ID;
            this.Url = Url;
            this.StreamUrl = StreamUrl;
            this.Title = Title;
            this.Artist = Artist;
        }

        public static Song NewSong(string url, Video video)
        {
            //Gets Artist and Title
            var parts = video.Title.Split('-', 2);
            
            return new Song(ParseId(url), url, video.Uri, parts[1], parts[0]);
        }

        public static string ParseId(string url)
        {
            var pattern = "v=\\w*";
            var match = Regex.Match(url, pattern);
            return match.Value.Remove(0, 2);
        }
    }
}