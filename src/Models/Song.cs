using VideoLibrary;

namespace tibrudna.djcort.src.Models
{
    ///<summary>Represents a Song.</summary>
    public class Song
    {
        private readonly Video video;

        ///<summary>The songs id.</summary>
        public string Id { get; }

        ///<summary>The uri to the videostream.</summary>
        public string StreamUri => video.Uri;

        ///<summary>The title of the song.</summary>
        public string Title => video.Title;

        ///<summary>The url to the song.</summary>
        public string Url => $"https://www.youtube.com/watch?v={Id}";

        ///<summary>The url to the songs thumbnail.</summary>
        public string ThumbnailUrl => $"https://img.youtube.com/vi/{Id}/hqdefault.jpg";

        ///<summary>Creates a new instance of a song.</summary>
        ///<param name="video">The video that is used as a song.</param>
        ///<param name="id">The songs id.</param>
        ///<returns>A new isntance of a song.</returns>
        public Song(Video video, string id)
        {
            this.video = video;
            this.Id = id;
        }
    }
}