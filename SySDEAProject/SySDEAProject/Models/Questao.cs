namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QUESTOES")]
    public partial class Questao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Questao()
        {
            PedacoQuestao = new HashSet<PedacoQuestao>();
            Historico = new HashSet<Historico>();
            VersaoProva = new HashSet<VersaoProva>();
            //ParteProva = new HashSet<ParteProva>();
        }

        [Key]
        [Column(name: "ID_QUESTAO")]
        public int idQuestao { get; set; }

        [Column(name: "TX_MATERIAL_INSPIRACAO")]
        public string materialInspiracao { get; set; }

        [Column(name: "TX_PARTICIPANTES_CONSTRUCAO")]
        public string participantesConstrucao { get; set; }

        [Column(name: "ID_TEMA")]
        public int idTema { get; set; }

        [Column(name: "ID_PARTEORIGINAL")]
        public int? idParteOriginal { get; set; }

        [Column(name: "NR_NIVEL_DIFICULDADE")]
        public int? dificuldade { get; set; }

        [Column(name: "NR_PESO_QUESTAO")]
        public decimal? peso { get; set; }

        [Column(name: "SN_SUSPENSA_QUESTAO")]
        public bool? suspensa { get; set; }

        [Column(name: "QT_VEZES_ESCOLHIDA")]
        public int? vezesEscolhida { get; set; }

        [StringLength(70)]
        [Column(name: "TX_CONTEUDO")]
        public string conteudo { get; set; }

        [StringLength(70)]
        [Column(name: "TX_DIRETRIZ")]
        public string diretriz { get; set; }

        [Column(name: "NR_NUMEROPARTE")]
        public int? numeroParte { get; set; }


        public virtual ParteOriginal ParteOriginal { get; set; }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PedacoQuestao> PedacoQuestao { get; set; }

        public virtual Tema Tema { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Historico> Historico { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VersaoProva> VersaoProva { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParteProva> ParteProva { get; set; }
    }
}
