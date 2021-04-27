namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PARTES_PROVAS")]
    public partial class ParteProva
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ParteProva()
        {
            Questao = new HashSet<Questao>();
            VersaoProva = new HashSet<VersaoProva>();
        }

        [Key]
        [Column(name: "ID_PARTE_PROVA")]
        public int idParteProva { get; set; }

        [Column(name: "NR_NUMERO_PARTE")]
        public int? numeroParte { get; set; }

        [Column(name: "TX_TEXTO")]
        public string texto { get; set; }

        [Column(name: "TX_TEXTO_COMPLEMENTAR")]
        public string textoComp { get; set; }

        [Column(name: "TX_TEXTO_COMPLEMENTAR2")]
        public string textoComp2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Questao> Questao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VersaoProva> VersaoProva { get; set; }
    }
}

