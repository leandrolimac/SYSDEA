namespace SySDEAProject.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ROLE")]
    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        [Column(name: "ID_ROLE")]
        public new int Id { get; set; }
        [Column(name: "NM_NOME")]
        public new string Name { get; set; }
        public CustomRole() : base() { }
        public CustomRole(int id, string name) { Id = id; Name = name; }
        [Column(name: "DS_DESCRICAO")]
        public string Description { get; set; }

        
        //public virtual ICollection<Usuario> Usuario { get; set; }
    }
}