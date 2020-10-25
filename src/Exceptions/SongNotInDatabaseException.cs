using System;

namespace tibrudna.djcort.src.Exceptions
{
    /// <summary>Exception, when an operation is performed with a song,
    /// that is not contained in the database.</summary>
    public class SongNotInDatabaseException : Exception
    {

        /// <summary>Initializes a new instance of the SongNotInDatabaseException class.</summary>
        /// <param name="message">Error message.</param>
        public SongNotInDatabaseException(string message = "") : base(message)
        {
        }
    }
}