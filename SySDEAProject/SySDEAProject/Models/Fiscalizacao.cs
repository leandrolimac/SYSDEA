namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Fiscalizacao
    {
        public int idFiscalizacao { get; set; }

        public int idAvaliacao { get; set; }

        public int idFiscal { get; set; }

        public int idRecurso { get; set; }

        public int? codigoTipoFisc { get; set; }

        public DateTime? dthrInicio { get; set; }

        public DateTime? dthrFim { get; set; }

        public string observacoes { get; set; }

       
        public bool? parteDeRecurso { get; set; }


        public virtual Avaliacao Avaliacao { get; set; }

        public virtual Fiscal Fiscal { get; set; }

        public virtual Recurso Recurso { get; set; }


    }
}
