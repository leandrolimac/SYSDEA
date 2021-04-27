namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class AudioProva
    {
        public int idAudioProva { get; set; }
        public int idAvaliacao { get; set; }
        public DateTime? dthrGravacao { get; set; }
        public virtual Avaliacao Avaliacao { get; set; }
    }
}
