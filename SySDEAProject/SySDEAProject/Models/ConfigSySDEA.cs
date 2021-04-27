using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SySDEAProject.Models
{

    public partial class ConfigSySDEA
    {
        public int Id { get; set; }
        public DateTime lastModified { get; set; }
    }
}