namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Entidade : Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Entidade()
        {
            this.LocalEntidade = new HashSet<LocalEntidade>();
            Avaliador_Entidade = new HashSet<AvaliadorXEntidade>();            

        }
        public override int Id { get; set; }
        public string nome { get; set; }
        public string EmailEntidade { get; set; }

        public string emailELEs { get; set; }

        public long? cnpj { get; set; }

        public bool suspensa { get; set; }


        public bool ativa { get; set; } // campo para exclusão lógica


        public int tipoEntidade { get; set; } //Diferencia se é uma empresa aérea, escola de inglês ou centro de treinamento

        [NotMapped]
        public virtual Usuario Usuario { get; set; }
        

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocalEntidade> LocalEntidade { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvaliadorXEntidade> Avaliador_Entidade { get; set; }

    }
}
