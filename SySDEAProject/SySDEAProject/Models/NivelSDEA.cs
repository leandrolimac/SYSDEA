using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SySDEAProject.Models
{
    public class NivelSDEA
    {
        [Key]
        [Column(name: "NR_NIVELATUAL")]
        public int nivelAtual { get; set; }

        [Column(name: "TX_DESCRICAO")]
        public string descricao { get; set; }
    }
}