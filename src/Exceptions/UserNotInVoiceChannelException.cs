using System;

namespace tibrudna.djcort.src.Exceptions
{
    /// <summary>Exception, whenn a User is not in a VoiceChannel, while he should have benn.</summary>
    public class UserNotInVoiceChannelException : Exception
    {
        
        /// <summary>Initializes a new instance of the UserNotInVoiceChannelException.</summary>
        /// <param name="message">Error message.</param>
        public UserNotInVoiceChannelException(string message = "") : base(message)
        {

        }
    }
}