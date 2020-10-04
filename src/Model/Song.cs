using System.Text.RegularExpressions;
using VideoLibrary;

namespace tibrudna.djcort.src.Models
{
    public class Song
    {
        public string Url { get; }
        public Video Video { get; }
        public string Title { get; private set; }
        public string Artist { get; private set; }
        public string ThumbnailUrl { get; private set; }

        public Song(string Url, Video video)
        {
            this.Url = Url;
            this.Video = video;
            TitleParser();
            ThumbnailParser();
        }

        private void TitleParser()
        {
            var parts = this.Video.Title.Split('-', 2);
            this.Artist = parts[0].Trim();
            this.Title = parts[1].Trim();
        }

        private void ThumbnailParser()
        {
            var pattern = "v=\\w*";
            var match = Regex.Match(Url, pattern);
            var id = match.Value.Remove(0, 2);
            this.ThumbnailUrl = $"https://img.youtube.com/vi/{id}/hqdefault.jpg";
        }
    }
}