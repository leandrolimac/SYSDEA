namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FiscEntrevista : Fiscalizacao
    {
        public int? usoDaLinguagem { get; set; }

        public int? construcaoRapport { get; set; }

        public int? conducaoEntrevista { get; set; }

        public bool? speechRate { get; set; }

        public bool? ingles { get; set; }

        public bool? parafrases { get; set; }

        public bool? energia { get; set; }

        public bool? etiqueta { get; set; }

        public bool? atitude { get; set; }

        public bool? clarification { get; set; }

        public bool? procedimentosBasicos { get; set; }

        public bool? controleTempo { get; set; }

        public bool? interpretacaoScript { get; set; }

    }
}
