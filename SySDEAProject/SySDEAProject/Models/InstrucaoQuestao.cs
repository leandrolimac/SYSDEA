namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InstrucaoQuestao : PedacoQuestao
    {
        public string texto { get; set; }
        public int? nivelVocab { get; set; }
        public int? nivelStruct { get; set; }

    }
}