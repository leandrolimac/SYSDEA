namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NOTAS_AVALIADORES")]
    public partial class NotaAvaliador : Nota
    {
        

        [Column(name: "ID_AVALIADOR")]
        [ForeignKey("Avaliador")]
        public int idAvaliador { get; set; }

        [Column(name: "AR_RELATORIO_INDIVIDUAL")]
        public byte[] relIndividual { get; set; }

        [Column(name: "ID_AVALIACAO")]
        [ForeignKey("Avaliacao")]
        public int IdAvaliacao { get; set; }

        public virtual Avaliacao Avaliacao { get; set; }

        public virtual Avaliador Avaliador { get; set; }

    }
}
