using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
namespace MyWeb.Core.Exceptions
{
    internal class NotFoundException: Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }

   

}
