namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

   
    public partial class AudioQuestao : PedacoQuestao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AudioQuestao()
        {
            Interacao = new HashSet<Interacao>();
        }
        
        public byte[] audio { get; set; }

        
        public DateTime dataGravacao { get; set; }

        
        public int idLocutor { get; set; }

        
        public int idSotaque { get; set; }

        
        public int? numeroInteracoes { get; set; }

        public virtual LocutorXSotaque LocutorXSotaque { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Interacao> Interacao { get; set; }
    }
}
