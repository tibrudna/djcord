using System;

namespace tibrudna.djcort.src.Exceptions
{
    /// <summary>Exception, when a song that is already in a database is added again.</summary>
    public class DuplicateSongException : Exception
    {
        /// <summary>Initializes a new instance of the DuplicateSongException.</summary>
        /// <param name="message">Error message.</param>
        public DuplicateSongException(string message = "") : base(message)
        {

        }
    }
}