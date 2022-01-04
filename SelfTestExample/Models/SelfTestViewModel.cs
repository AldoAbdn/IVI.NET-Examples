using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfTestExample.Models
{
    class SelfTestViewModel
    {
        public string Name { get; }
        public int Code { get; }
        public string Message { get; }
        public SelfTestViewModel(string name, int code, string message)
        {
            Name = name;
            Code = code;
            Message = message;
        }
    }
}
