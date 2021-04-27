namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Interacao
    {

        public int idInteracao { get; set; }


        public string titulo { get; set; }

        public string descricao { get; set; }

        public int idPedacoQuestao { get; set; }

        public virtual AudioQuestao AudioQuestao { get; set; }
    }
}
