namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UsuarioLogin")]
    public partial class UsuarioLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idUsuario { get; set; }

        [Required]
        [StringLength(30)]
        public string senha { get; set; }

        [StringLength(255)]
        public string perguntaSegura { get; set; }

        [StringLength(255)]
        public string respostaSegura { get; set; }

        public int nivelAcesso { get; set; }

        [StringLength(30)]
        public string nomeUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
