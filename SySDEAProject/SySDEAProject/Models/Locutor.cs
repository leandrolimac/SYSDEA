namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOCUTORES")]
    public partial class Locutor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Locutor()
        {
            LocutorXSotaque = new HashSet<LocutorXSotaque>();
        }

        [Key]
        [Column(name: "ID_LOCUTOR")]
        public int idLocutor { get; set; }

        [Required]
        [StringLength(70)]
        [Column(name: "NM_NOME")]
        public string nome { get; set; }


        [Column(name: "TP_OCUPACAO")]
        public int ocupacao { get; set; }

        [StringLength(90)]

        [Column(name: "TX_EMAIL")]
        public string email { get; set; }

        [Column(name: "ID_PESSOA")]
        public int idPessoa { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocutorXSotaque> LocutorXSotaque { get; set; }
    }
}
