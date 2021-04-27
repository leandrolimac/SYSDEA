namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LocalEntidade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LocalEntidade()
        {
            Horario = new HashSet<Horario>();
            Avaliacao = new HashSet<Avaliacao>();
            ativa = true;
            suspensa = false;
        }



        public int idLocalEntidade { get; set; }


        public int idEntidade { get; set; }
        
        
        public string emailContato { get; set; }

        
        public bool aceitaSolicitacoes { get; set; }

        
        public double precoAvaliacao { get; set; }

        public bool ativa { get; set; }

       
        public int? numeroSalas { get; set; }


        public string titulo { get; set; }

  
        public bool suspensa { get; set; }

        public virtual Endereco Endereco { get; set; }

        public virtual Entidade Entidade { get; set; }

        public virtual EmailModelo EmailModelo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Horario> Horario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Avaliacao> Avaliacao { get; set; }
    }
}
