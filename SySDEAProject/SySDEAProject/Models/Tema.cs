namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TEMA")]
    public partial class Tema
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tema()
        {
            Questao = new HashSet<Questao>();
        }

        [Key]
        [Column(name: "ID_TEMA")]
        public int idTema { get; set; }

        [StringLength(255)]
        [Column(name: "TX_TITULO_TEMA")]
        public string titulo { get; set; }

        [Column(name: "DS_DESCRICAO_TEMA")]
        public string descricao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Questao> Questao { get; set; }
    }
}
