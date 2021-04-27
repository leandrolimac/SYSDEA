using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SySDEAProject.Models
{
    public class EmailModel 
    {
        public string destino { get; set; }
        public string assunto { get; set; }
        public string corpo { get; set; }
    }
}