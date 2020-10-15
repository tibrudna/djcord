using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tibrudna.djcort.src.Models
{
    /// <summary>Holds all Information about a song.</summary>
    public class Song
    {
        /// <summary>The identifier for the song. Is the same as the one used by Youtube.</summary>
        [Column("id", TypeName="varchar(30)")]
        [Required]
        [Key]
        public string ID { get; set; }

        /// <summary>The title of the song.</summary>
        [Column("title", TypeName="varchar(100)")]
        [Required]
        public string Title { get; set; }

        /// <summary>The artist of the song. Can be empty if the artist is not known.</summary>
        [Column("artist", TypeName="varchar(100)")]
        public string Artist { get; set; }

        /// <summary>The Youtube link to the song.</summary>
        [NotMapped]
        public string Url { get { return $"https://www.youtube.com/watch?v={ID}"; } }

        /// <summary>The Url to the hd thumbnail.</summary>
        [NotMapped]
        public string ThumbnailUrl { get { return $"https://img.youtube.com/vi/{ID}/hqdefault.jpg"; } }

        /// <summary>String represenation of this instance.</summary>
        /// <returns>The string representation of this instance.</returns>
        public override string ToString()
        {
            return $"{ID}\t\t{Title} - {Artist}";
        }

        /// <summary>Hash for this isntance.</summary>
        /// <returns>The hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        /// <summary>Compares an object to this instance.</summary>
        /// <param name="obj">The object which is compared to this instance.</param>
        /// <returns>Wether the object is equal to this instance.</returns>
        public override bool Equals(object obj)
        {
            var song = obj as Song;
            if (song == null) return false;

            if (!this.ID.Equals(song.ID)) return false;
            return true;
        }
    }
}