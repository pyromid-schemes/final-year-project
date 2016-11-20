using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.States.Exceptions
{
    public class InvalidStateTransitionException : Exception
    {
        public InvalidStateTransitionException(string message) : base(message)
        {

        }
    }
}
