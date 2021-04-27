using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SySDEAProject.Models
{
 
    public partial class EmailModelo
    {
        public int Id{ get; set; }
        
        public string texto { get; set; }

        public string assunto { get; set; }

        public string descricao { get; set; }

        public virtual LocalEntidade LocalEntidade{ get; set; }
    }
}