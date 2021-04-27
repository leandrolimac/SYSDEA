namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HISTORICO")]
    public partial class Historico
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Historico()
        {
            Questao = new HashSet<Questao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("Piloto")]
        [Column(name:"ID_PILOTO")]
        public int idPiloto { get; set; }


        [Column(name: "DS_OBSERVACOES")]
        public string observacoes { get; set; }

        public virtual Piloto Piloto { get; set; }

        public virtual ICollection<VersaoOriginal> VersaoOriginal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Questao> Questao { get; set; }
    }
}
