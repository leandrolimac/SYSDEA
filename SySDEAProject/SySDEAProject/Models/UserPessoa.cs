namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PESSOA_SYSDEA")]
    public partial class UserPessoa : Usuario
    {
        SySDEAContext db = new SySDEAContext();
        public UserPessoa()
        {
        }

        public UserPessoa(InfoAeronauta infoAeronauta)
        {
            this.nome = infoAeronauta.nome;
            this.UserName = infoAeronauta.email;
            this.Email = infoAeronauta.email;
                        
        }
        [Column(name:"ID_USUARIO")]
        [Key]
        public override int Id { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(70)]
        [Column(name: "NM_NOME_PESSOA")]
        public string nome { get; set; }

        [Required, Index(IsUnique =true)]
        [Column(name: "NR_CPF_PESSOA")]
        public long cpf { get; set; }      

        [Column(name: "ID_PESSOA")]        
        public int? idPessoaCorporativo { get; set; }
        

        [Column(name: "DT_NASCIMENTO_PESSOA", TypeName = "datetime")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dataNascimento { get; set; }

        public virtual ICollection<RegistroAtividade> RegistroAtividade { get; set; }

        public virtual Avaliador Avaliador { get; set; }

        public virtual Fiscal Fiscal { get; set; }

        public virtual Piloto Piloto { get; set; }
        
        public virtual Endereco Endereco { get; set; }

        //[NotMapped]
        //public virtual Usuario Usuario { get; set; }

    }
}
