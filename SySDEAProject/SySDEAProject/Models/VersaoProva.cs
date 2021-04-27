namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VERSOES_PROVA")]
    public partial class VersaoProva
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VersaoProva()
        {
            Avaliacao = new HashSet<Avaliacao>();
            Questao = new HashSet<Questao>();
            //ParteProva = new HashSet<ParteProva>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(name: "ID_VERSAO_PROVA")]
        public int idVersaoProva { get; set; }

        [Column(name: "QT_VEZES_APLICADA")]
        public int? vezesAplicada { get; set; }

        [Column(name:"DT_GERACAO", TypeName = "datetime")]
        public DateTime? dtGeracao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Avaliacao> Avaliacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Questao> Questao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         public virtual ICollection<ParteProva> ParteProva { get; set; }
    }
}
