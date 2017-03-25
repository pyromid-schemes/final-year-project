using System;

/*
 * @author Daniel Burnley
 */
namespace AI.States.Exceptions
{
    public class InvalidStateTransitionException : Exception
    {
        public InvalidStateTransitionException(string message) : base(message)
        {

        }
    }
}
