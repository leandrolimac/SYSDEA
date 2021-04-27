 namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PERMISSAO")]
    public partial class CustomUserRole
    {
        [Key]
        [Column(name:"ID_USUARIO",Order = 0), ForeignKey("Usuario")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int UserId { get; set; }

        [Key]
        [Column(name: "ID_ROLE", Order = 1),ForeignKey("CustomRole")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int RoleId { get; set; }

        
        public virtual CustomRole CustomRole { get; set; }
        public virtual Usuario Usuario { get; set; }
    }

    
}
