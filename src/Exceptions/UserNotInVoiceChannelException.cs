using System;

namespace tibrudna.djcort.src.Exceptions
{
    public class UserNotInVoiceChannelException : Exception
    {
        public UserNotInVoiceChannelException(string message = "") : base(message)
        {

        }
    }
}