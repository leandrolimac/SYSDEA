namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOCUTORES_SOTAQUES")]
    public partial class LocutorXSotaque
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LocutorXSotaque()
        {
            AudioQuestao = new HashSet<AudioQuestao>();
        }

        [Key, ForeignKey("Locutor")]
        [Column(name: "ID_LOCUTOR",Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idLocutor { get; set; }

        [Key,ForeignKey("Sotaque")]
        [Column(name: "ID_SOTAQUE",Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idSotaque { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AudioQuestao> AudioQuestao { get; set; }

        public virtual Locutor Locutor { get; set; }

        public virtual Sotaque Sotaque { get; set; }
    }
}
