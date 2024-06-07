using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class MessageResponse
    {
        public string Message { get; set; }

        public MessageResponse(string message)
        {
            Message = message;
        }
    }
}
