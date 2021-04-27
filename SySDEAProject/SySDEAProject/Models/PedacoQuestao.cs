namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PEDACOS_QUESTAO")]
    public partial class PedacoQuestao
    {
        [Key]
        [Column(name: "ID_PEDACO_QUESTAO")]
        public int idPedacoQuestao { get; set; }

        [Column(name: "ID_QUESTAO")]
        public int idQuestao { get; set; }

        [Column(name: "NR_NUMERO_ORDEM")]
        public int? numeroOrdem { get; set; }

        [NotMapped]
        public virtual AudioQuestao AudioQuestao { get; set; }
        [NotMapped]
        public virtual ImagemQuestao ImagemQuestao { get; set; }
        [NotMapped]
        public virtual Questao Questao { get; set; }
        [NotMapped]
        public virtual TextoQuestao TextoQuestao { get; set; }
    }
}
