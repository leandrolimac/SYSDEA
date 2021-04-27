namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NOTAS_FISCALIZACOES")]
    public partial class NotaFisc : Nota
    {


        [Column(name:"DS_OBSERVACOES")]        
        public string observacoes { get; set; }

        [ForeignKey("FiscParecer")]
        [Column(name: "ID_FISCALIZACAO")]
        public int idFiscalizacao { get; set; }

        public virtual FiscParecer FiscParecer { get; set; }

    }
}
