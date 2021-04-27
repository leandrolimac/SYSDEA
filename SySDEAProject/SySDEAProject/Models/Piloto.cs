namespace SySDEAProject.Models
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Security.Claims;
    using System.Threading.Tasks;
    [Table("PILOTOS")]
    public partial class Piloto
    {
       public Piloto(InfoAeronauta infoaeronauta, int id, UserPessoa userpessoa)
        {

            this.UserPessoa = userpessoa;
            this.Id = id;
            this.UserPessoa.Email = infoaeronauta.email;
            this.UserPessoa.nome = infoaeronauta.nome;
            this.CANACPiloto = int.Parse(infoaeronauta.canacpiloto.ToString());
        }
       public Piloto(InfoAeronauta infoaeronauta)
        {
            //UserPessoa = new UserPessoa();
            this.UserPessoa.Email = infoaeronauta.email;
            this.UserPessoa.nome = infoaeronauta.nome;
            this.CANACPiloto = int.Parse(infoaeronauta.canacpiloto.ToString());
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Piloto()
        {
            
            Horario = new HashSet<Horario>();
           
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key, ForeignKey("UserPessoa")]
        public int Id { get; set; }

        [Column(name:"NR_CANAC")]
        public int CANACPiloto { get; set; }

        [Column(name: "ID_EMPRESA")]
        public int idEmpresa { get; set; }

        [Column(name: "TP_NIVEL_ATUAL")]
        public int? nivelAtual { get; set; }

        [Column(name: "DS_OBSERVACOES")]
        public string observacoes { get; set; }

        [Required]
        [Column(name: "NR_TELCONTATO")]
        public long telContato { get; set; }

        [Column(name: "SN_AGENDAMENTO_ATIVO")]
        public bool agendamentoAtivo { get; set; }

        public virtual Empresa Empresa { get; set; }

        public virtual Historico Historico { get; set; }

        

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Horario> Horario { get; set; }

        public virtual ImagemPiloto ImagemPiloto { get; set; }

        public virtual UserPessoa UserPessoa { get; set; }

    }
}
