namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FISCALIZACAO_PARECER")]
    public partial class FiscParecer : Fiscalizacao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FiscParecer()
        {
            NotaFisc = new HashSet<NotaFisc>();
        }
       
        public bool? notaAlterada { get; set; }

        
        //public virtual Fiscalizacao Fiscalizacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotaFisc> NotaFisc { get; set; }
    }
}
