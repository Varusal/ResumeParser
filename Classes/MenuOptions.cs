using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeParser.Classes
{
    internal class MenuOptions
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public Action Method { get; set; }

        public MenuOptions()
        {
            Name = string.Empty;
            Message = string.Empty;
            Method = () => { };
        }
    }
}
