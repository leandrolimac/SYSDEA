namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RECURSOS")]
    public partial class Recurso
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recurso()
        {
            Fiscalizacao = new HashSet<Fiscalizacao>();
        }

        [Key]
        [Column(name: "ID_RECURSO")]
        public int idRecurso { get; set; }

        [ForeignKey("Avaliacao")]
        [Column(name: "ID_AVALIACAO")]
        public int idAvaliacao { get; set; }

        [ForeignKey("Fiscal")]
        [Column(name: "ID_FISCAL")]
        public int idFiscal { get; set; }

        [Column(name: "DT_DIVULGACAO_RECURSO",TypeName = "date")]        
        public DateTime? dtDivulgRecurso { get; set; }

        [Column(name:"DT_ENTRADA_RECURSO", TypeName = "date")]
        public DateTime? dtEntradaRecurso { get; set; }

        [Column(name:"DT_COMUNICACAO_RECURSO", TypeName = "date")]
        public DateTime? dtComRec { get; set; }

        [Column(name:"DT_DISTRIBUICAO_RECURSO", TypeName = "datetime2")]
        public DateTime? dthrDistribuicao { get; set; }

        [Column(name:"DT_ADMISSAO_RECURSO",TypeName = "date")]        
        public DateTime? dtAdmissaoRec { get; set; }

        [Column(name:"DT_CADASTRO_RECURSO",TypeName = "date")]
        public DateTime? dtCadastro { get; set; }       

        public virtual Avaliacao Avaliacao { get; set; }

        public virtual Fiscal Fiscal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fiscalizacao> Fiscalizacao { get; set; }
    }
}
