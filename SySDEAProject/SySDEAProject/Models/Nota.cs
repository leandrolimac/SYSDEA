namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NOTAS")]
    public partial class Nota
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Nota()
        {
           // NotaFisc1 = new HashSet<NotaFisc>();
        }

        [Key]
        [Column(name:"ID_NOTA")]
        public int idNota { get; set; }

        [Column(name:"DT_ATRIBUICAO_NOTA",TypeName = "datetime2")]
        public DateTime? dthrAtribuicao { get; set; }

        [Column(name: "TP_CONTEXTO_NOTA")]
        public int? codigoContexto { get; set; }

        [Column(name: "NR_NIVEL_PRONUNCIA")]
        public int? pronuncia { get; set; }

        [Column(name: "NR_NIVEL_ESTRUTURA")]
        public int? estrutura { get; set; }

        [Column(name: "NR_NIVEL_VOCABULARIO")]
        public int? vocabulario { get; set; }

        [Column(name: "NR_NIVEL_FLUENCIA")]
        public int? fluencia { get; set; }

        [Column(name: "NR_NIVEL_COMPREENSAO")]
        public int? compreensao { get; set; }

        [Column(name: "NR_NIVEL_INTERACAO")]
        public int? interacao { get; set; }

        [Column(name: "NR_NIVEL_FINAL")]
        public int? final { get; set; }
                
    }
}
