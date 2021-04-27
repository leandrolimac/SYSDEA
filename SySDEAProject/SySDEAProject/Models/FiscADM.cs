namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FiscADM : Fiscalizacao
    {
        public int? documentacao { get; set; }

        public int? gravacao { get; set; }

        public int? relatorios { get; set; }

       
    }
}
