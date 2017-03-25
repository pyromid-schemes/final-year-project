using System;

/*
 * @author Daniel Burnley
 */
namespace AI.States.Exceptions
{
    public class StateDoesNotExistException : Exception
    {
        public StateDoesNotExistException(string message) : base(message)
        {
        }
    }
}
