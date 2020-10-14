using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using VideoLibrary;

namespace tibrudna.djcort.src.Models
{
    public class Song
    {
        [Column("id", TypeName="varchar(30)")]
        [Required]
        [Key]
        public string ID { get; set; }

        [Column("title", TypeName="varchar(100)")]
        [Required]
        public string Title { get; set; }

        [Column("artist", TypeName="varchar(100)")]
        public string Artist { get; set; }

        [NotMapped]
        public string Url { get { return $"https://www.youtube.com/watch?v={ID}"; } }

        [NotMapped]
        public string ThumbnailUrl { get { return $"https://img.youtube.com/vi/{ID}/hqdefault.jpg"; } }


        public override string ToString()
        {
            return $"{ID}\t\t{Title} - {Artist}";
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var song = obj as Song;
            if (song == null) return false;

            if (!this.ID.Equals(song.ID)) return false;
            return true;
        }
    }
}