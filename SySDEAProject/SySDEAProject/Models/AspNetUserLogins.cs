namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOGIN")]
    public partial class CustomUserLogin
    {
        [Key]
        [Column(name:"TX_PROVEDOR_LOGIN",Order = 0)]
        public override string LoginProvider { get; set; }

        [Key]
        [Column(name:"TX_CHAVE_PROVEDOR", Order = 1)]
        public override string ProviderKey { get; set; }

        [Key, ForeignKey("Usuario")]
        [Column(name:"ID_USUARIO", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int UserId { get; set; }

        public virtual Usuario Usuario { get; set;}
    }
}
