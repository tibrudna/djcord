using System;

namespace tibrudna.djcort.src.Exceptions
{
    public class DuplicateSongException : Exception
    {
        public DuplicateSongException(string message = "") : base(message)
        {

        }
    }
}