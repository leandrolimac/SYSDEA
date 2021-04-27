namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TEXTO_QUESTAO")]
    public partial class TextoQuestao : PedacoQuestao
    {
       

        [StringLength(511)]
        [Column(name: "TX_TEXTO_QUESTAO")]
        public string texto { get; set; }

        [Column(name: "TP_TIPO_QUESTAO")]
        public int? tipoTexto { get; set; }

    }
}
