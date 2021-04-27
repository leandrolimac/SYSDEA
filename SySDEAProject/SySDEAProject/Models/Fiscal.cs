namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Fiscal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Fiscal()
        {
            //UserPessoa = new UserPessoa();
            Fiscalizacao = new HashSet<Fiscalizacao>();
            Recurso = new HashSet<Recurso>();
        }

        public int Id { get; set; }


        public int? siape { get; set; }


        public bool administrador { get; set; }


        public bool ativo { get; set; }

        

        public virtual UserPessoa UserPessoa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fiscalizacao> Fiscalizacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recurso> Recurso { get; set; }
    }
}
