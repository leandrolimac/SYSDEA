using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SySDEAProject.Models
{
    public class EmailFormModel
    {
        public string destino { get; set; }
        public string assunto { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Message { get; set; }
    }
}