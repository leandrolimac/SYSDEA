using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SySDEAProject.Models
{
    [Table("ENDERECO")]
    public class Endereco
    {
        [Key]
        //[ForeignKey("UserPessoa")]
        [Column(name: "ID_ENDERECO", Order =0)]
        public int Id { get; set; }

        
        [Column(name:"NR_CEP_ENDERECO")]
        public long cep { get; set; }

        [StringLength(70)]
        [Column(name: "TX_RUA_ENDERECO")]
        public string rua { get; set; }

        [StringLength(10)]
        [Column(name: "TX_NUMERO_ENDERECO")]
        public string numero { get; set; }

        [StringLength(255)]
        [Column(name: "TX_COMPLEMENTO_ENDERECO")]
        public string complemento { get; set; }

        [StringLength(255)]
        [Column(name: "TX_BAIRRO")]
        public string bairro { get; set; }

        [StringLength(255)]
        [Column(name: "TX_MUNICIPIO")]
        public string municipio { get; set; }

        [StringLength(2)]
        [Column(name: "TX_UF")]
        public string uf { get; set; }

        
        public virtual ICollection<UserPessoa> UserPessoa { get; set; }

        public virtual ICollection<LocalEntidade> LocalEntidade { get; set; }
    }
}