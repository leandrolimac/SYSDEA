using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SySDEAProject.Models
{
    [Table("SOLICITACOES_HORARIOS")]
    public class SolicitacaoHorario
    {
        [Key]
        [Column(name: "ID_SOLICITACAO")]
        public int idSolicitacao { get; set; }

        [ForeignKey("Piloto")]
        [Column(name: "ID_PILOTO")]
        public int idPiloto { get; set; }
        [ForeignKey("LocalEntidade")]
        [Column(name: "ID_LOCALENTIDADE")]
        public int idLocalEntidade { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        [Column(name: "DT_HORA1_SOLICITACAO", TypeName = "datetime2")]
        public DateTime hora1 { get; set; }

        [Column(name: "DT_HORA2_SOLICITACAO", TypeName = "datetime2")]
        public DateTime? hora2 { get; set; }

        [Column(name: "DT_HORA3_SOLICITACAO", TypeName = "datetime2")]
        public DateTime? hora3 { get; set; }



        [DataType(DataType.MultilineText)]
        [Column(name: "DS_DESCRICAO_SOLICITACAO")]
        public string observacoes { get; set; }

        [Column(name: "SN_ABERTA_SOLICITACAO")]
        public bool aberta { get; set; }

        [Column(name: "SN_ACEITA_SOLICITACAO")]
        public bool? aceita { get; set; }

        public virtual Piloto Piloto { get; set; }

        public virtual LocalEntidade LocalEntidade { get; set; }
    }
}