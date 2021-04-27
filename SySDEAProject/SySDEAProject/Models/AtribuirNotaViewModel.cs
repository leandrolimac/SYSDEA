using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SySDEAProject.Models
{
    public class AtribuirNotaViewModel
    {
        [Required]
        public int canacpiloto { get; set; }

        [Required]
        public string empresaPiloto {get; set;}

        [Required]
        public string emailPiloto { get; set; }

        [Required]
        public string nome { get; set; }

        [Required]
        public string entidade { get; set; }
        
        public string versão { get; set; }

        [Required]
        public DateTime dataProva { get; set; }


        public string numeroCandidato { get; set; }

        public string ele { get; set; }                

        public string sme { get; set; }

        public bool terceiraEscuta { get; set; }

        [Required]
        public int? pronuncia { get; set; }
       
        [Required]
        public int? estrutura { get; set; }

        [Required]
        public int? vocabulario { get; set; }

        [Required]
        public int? fluencia { get; set; }

        [Required]
        public int? compreensao { get; set; }

        [Required]
        public int? interacao { get; set; }

        [Required]
        public int? geral { get; set; }

        public bool? pronuncia5m { get; set; }

        public bool? estrutura5m { get; set; }

        public bool? vocabulario5m { get; set; }

        public bool? fluencia5m { get; set; }

        public bool? compreensao5m { get; set; }

        public bool? interacao5m { get; set; }

        public bool? geral5m { get; set; }
        
    }
}