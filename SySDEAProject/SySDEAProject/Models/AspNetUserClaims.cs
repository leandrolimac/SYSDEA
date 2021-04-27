namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomUserClaim
    {
        public override int Id { get; set; }

        public override int UserId { get; set; }

        public override string ClaimType { get; set; }

        public override string ClaimValue { get; set; }

        public virtual Usuario Usuario { get; set; }
    }

}
