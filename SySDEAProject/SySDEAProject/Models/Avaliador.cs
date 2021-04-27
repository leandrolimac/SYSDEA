namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class Avaliador 
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Avaliador()
        {            
            NotaAvaliador = new HashSet<NotaAvaliador>();
            Avaliador_Entidade = new HashSet<AvaliadorXEntidade>();
            //Horario = new HashSet<Horario>();
            
        }

        public int Id { get; set; }        
       
        public DateTime dtInicioAtividade { get; set; }
        
        public bool emAtividade { get; set; }

        
        public int maxProvasDiarias { get; set; }

        
        public DateTime dataDeValidade { get; set; }


        public string tipoAvaliador { get; set; }


        public DateTime dtConclusaoCurso { get; set; }


        public bool suspenso { get; set; }

        public bool ativo { get; set; }

        public virtual UserPessoa UserPessoa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotaAvaliador> NotaAvaliador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AvaliadorXEntidade> Avaliador_Entidade { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Horario> Horario { get; set; }
    }
}
