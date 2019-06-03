using System;
using System.Collections.Generic;
using System.Text;

namespace AnonChat.BLL.Infrastructure
{
    public class OperationDetails
    {
        public OperationDetails(bool succeeded, string message, string prop)
        {
            Succeeded = succeeded;
            Message = message;
            Property = prop;
        }
        public bool Succeeded { get; private set; }
        public string Message { get; private set; }
        public string Property { get; private set; }
    }
}
