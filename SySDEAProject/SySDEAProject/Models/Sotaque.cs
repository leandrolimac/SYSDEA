namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SOTAQUES")]
    public partial class Sotaque
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sotaque()
        {
            LocutorXSotaque = new HashSet<LocutorXSotaque>();
        }

        [Key]
        [Column(name: "ID_SOTAQUE")]
        public int idSotaque { get; set; }

        [Column(name: "SN_A1_SOTAQUE")]
        public bool tipoA1 { get; set; }

        [StringLength(70)]
        [Column(name: "TX_TITULO_SOTAQUE")]
        public string titulo { get; set; }

        [Column(name: "DS_DESCRICAO_SOTAQUE")]
        public string descricao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocutorXSotaque> LocutorXSotaque { get; set; }
    }
}
