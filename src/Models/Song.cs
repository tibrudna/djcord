using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VideoLibrary;

namespace tibrudna.djcort.src.Models
{
    ///<summary>Represents a Song.</summary>
    [Table("songs")]
    public class Song
    {
        ///<summary>The songs id.</summary>
        [Key]
        [Column("id")]
        public string Id { get; set; }

        ///<summary>The title of the song.</summary>
        [Required]
        [Column("title")]
        public string Title { get; set; }

        ///<summary>The uri to the videostream.</summary>
        [NotMapped]
        public string StreamUri { get; set; }

        ///<summary>The url to the song.</summary>
        [NotMapped]
        public string Url => $"https://www.youtube.com/watch?v={Id}";

        ///<summary>The url to the songs thumbnail.</summary>
        [NotMapped]
        public string ThumbnailUrl => $"https://img.youtube.com/vi/{Id}/hqdefault.jpg";

        ///<summary>Creates a new instance of a song.</summary>
        ///<param name="id">The songs id.</param>
        ///<param name="title">The songs title.</param>
        ///<param name="streamUri">The Uri to the stream.</param>
        ///<returns>A new isntance of a song.</returns>
        public Song(string id, string title, string streamUri)
        {
            this.Id = id;
            this.Title = title;
            this.StreamUri = streamUri;
        }

        ///<summary>Creates a new instance of a song. Only used for dependency injection.</summary>
        public Song()
        {

        }
    }
}