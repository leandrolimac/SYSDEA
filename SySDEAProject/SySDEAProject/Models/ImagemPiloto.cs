namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class ImagemPiloto
    {
        public int idPiloto { get; set; }

        
        public byte[] arquivo { get; set; }


        public virtual Piloto Piloto { get; set; }
    }
}
