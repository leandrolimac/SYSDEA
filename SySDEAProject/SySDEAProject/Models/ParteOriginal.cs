namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PARTES_ORIGINAIS")]
    public partial class ParteOriginal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParteOriginal()
        {
            Questao = new HashSet<Questao>();
        }

        [Key]
        [Column(name: "ID_PARTE_ORIGINAL")]
        public int idParteOriginal { get; set; }

        [Column(name: "ID_VERSAO_ORIGINAL")]
        public int idVersaoOriginal { get; set; }

        [Column(name: "NR_NUMERO_PARTE")]
        public int? numeroParte { get; set; }

        
        public virtual VersaoOriginal VersaoOriginal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Questao> Questao { get; set; }
    }
}
