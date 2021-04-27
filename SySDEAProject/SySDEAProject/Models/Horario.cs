namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class Horario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Horario()
        {           
            
            
        }

        public Horario(CriarHorarioViewModel horario2)
        {
            this.sala = horario2.sala;
            this.idHorario = horario2.idHorario;
            this.idLocalEntidade = horario2.idLocalEntidade;
            this.data = horario2.data+horario2.hora;            
            //this.hora = horario2.hora;            
            //this.data = this.data.Value.Add(hora.Value);
            
        }


        public int idHorario { get; set; }

        public double preco { get; set; }
        
        public int? idSme { get; set; }

        public int? idEle { get; set; }

        public DateTime? data { get; set; }


        public int? status { get; set; }


        public int? sala { get; set; }

        public int? idPiloto { get; set; }

        public int idLocalEntidade { get; set; }       
        
        public  virtual Avaliacao Avaliacao { get; set; }

        public virtual LocalEntidade LocalEntidade { get; set; }

        public virtual Piloto Piloto { get; set; }
        
        public virtual Avaliador ele { get; set; }

        public virtual Avaliador sme { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmailSySDEA> EmailSySDEA { get; set; }
    }
}
