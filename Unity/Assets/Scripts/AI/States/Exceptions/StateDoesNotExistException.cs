using System;

namespace AI.States.Exceptions
{
    public class StateDoesNotExistException : Exception
    {
        public StateDoesNotExistException(string message) : base(message)
        {
        }
    }
}
