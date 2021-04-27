using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SySDEAProject.Models
{
    [Table("REGISTROS_ATIVIDADE")]
    public class RegistroAtividade
    {
        [Key]
        [Column("ID_REGISTROS_ATIVIDADE")]
        public int Id { get; set; }

        [ForeignKey("UserPessoa")]
        [Column(name: "ID_USUARIO")]
        public int idUserPessoa { get; set; }

        [Column(name: "DT_INICIO_ATIVIDADE")]
        public DateTime inicioAtividade { get; set; }
        [Column(name: "DT_FIM_ATIVIDADE")]
        public DateTime? fimAtividade { get; set; }

        [Column(name: "TP_TIPO_REGISTRO")]
        public int tipo { get; set; }//1- Avaliador ou 2- Fiscal

        public virtual UserPessoa UserPessoa{get;set;}

    }
}