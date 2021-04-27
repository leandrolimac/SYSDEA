namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VERSAO_ORIGINAL")]
    public partial class VersaoOriginal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VersaoOriginal()
        {
            ParteOriginal = new HashSet<ParteOriginal>();
        }

        [Key]
        [Column(name:"ID_VERSAO_ORIGINAL")]
        public int idVersaoOriginal { get; set; }

        [Column(name: "DT_CADASTRO_VERSAO_ORIGINAL", TypeName = "date")]
        public DateTime? dataCadastro { get; set; }

        [Column(name: "NR_VERSAO_ORIGINAL")]
        public int? numero { get; set; }

        

        [Column(name: "DT_CONSTRUCAO_VERSAO_ORIGINAL")]
        public DateTime? dataConstrucao{ get; set; }

        [Column(name:"DT_INICIO_USO_VERSAO")]
        public DateTime? dataInicioUso { get; set; }

        [Column(name: "SN_ATIVA_VERSAO_ORIGINAL")]
        public bool ativa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParteOriginal> ParteOriginal { get; set; }

        public virtual ICollection<Historico> Historico { get; set; }
    }
}
