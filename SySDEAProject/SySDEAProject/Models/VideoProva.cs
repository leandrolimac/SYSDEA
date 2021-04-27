namespace SySDEAProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VIDEOS_PROVAS")]
    public partial class VideoProva
    {
        [Key]
        [Column(name: "ID_VIDEO_PROVA")]
        public int idVideoProva { get; set; }

        [ForeignKey("Avaliacao")]
        [Column(name: "ID_AVALIACAO")]
        public int idAvaliacao { get; set; }

        [Column(name: "ID_GUID_VIDEO_PROVA")]
        public Guid id { get; set; }

        [Column(name: "AR_VIDEO_PROVA")]
        public Byte[] arquivo { get; set; }

        public virtual Avaliacao Avaliacao { get; set; }
    }
}
