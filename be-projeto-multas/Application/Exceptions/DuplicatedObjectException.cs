using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class DuplicatedObjectException : Exception
    {
        public DuplicatedObjectException() : base() { }

        public DuplicatedObjectException(string message) : base(message) { }
    }
}
