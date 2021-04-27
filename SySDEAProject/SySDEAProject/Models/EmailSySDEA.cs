namespace SySDEAProject.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Net.Mail;
    using Microsoft.AspNet.Identity;

    [Table("EMAIL_SYSDEA")]
    public partial class EmailSySDEA 
    {

        public EmailSySDEA(MailMessage message) 
        {
            this.horaEnvio = DateTime.Now;
            this.textoEmail = message.Body;
            this.assunto = message.Subject;
            this.destino = message.To.ToString(); 
        }
        public EmailSySDEA(IdentityMessage message)
        {
            this.horaEnvio = DateTime.Now;
            this.textoEmail = message.Body;
            this.assunto = message.Subject;
            this.destino = message.Destination;
            
        }

        public EmailSySDEA()
        {
            this.horaEnvio = DateTime.Now;
        }

      
        public int idEmail { get; set; }
        
        public int? idHorario { get; set; }

        public string textoEmail { get; set; }

        public string assunto { get; set; }

        public string destino { get; set; }

        public string remetente { get; set; }

        public string tipoOrigem { get; set; }
        public DateTime horaEnvio { get; set; }
        public virtual Horario Horario { get; set; }
    }
}
