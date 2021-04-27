using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using SySDEAProject.Models;
using System.Threading.Tasks;
using BotDetect.Web.Mvc;
using SySDEAProject.ConsultarAeronauta;

namespace SySDEAProject.Controllers
{
    public class HomeController : Controller
    {
        SySDEAContext db = new SySDEAContext();
        public ActionResult Index()
        {
            //TESTE DE CONSULTA AERONAUTA
            /* 

            var sr = new AeronautaServiceClient("BasicHttpBinding_IAeronautaService1");
            sr.ClientCredentials.UserName.UserName = "sistema.sysdea";
            sr.ClientCredentials.UserName.Password = "Anac10!";
            var param = new AeronautaParametro()
            {
                CdAnac = "250355",
                NomeSistemaAutenticacao = "sysdea",
                LoginSistema = "sistema.sysdea",
                NumeroCpf = "05236585406",
                IdAeronauta = 0,
                NomeAeronauta = "PAULO EDUARDO DE ALMEIDA SANTOS",
                NomeSistemaRequisitante = "sistema.sysdea"
            };
            var ans = sr.ConsultarAeronauta(param);
            ConsultarAeronauta.AeronautaType atype = ans.First();
            */
            return View();
        }
        public ActionResult Sent()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Página de descrição da aplicação.";

            return View();
        }

        public ActionResult Map()
        {
            ViewBag.Message = " Mapa do sistema";
            return View();
        }
        
        public ActionResult Contact()
        {
            ViewBag.Message = "Página de contato";

            return View();
        }


        public ActionResult Erro(int StatusMessage) // 1 = "Email inválido, contate a proficiência linguística da ANAC no telefone: 3501-5409"
                                                    // 2 = "Usuário já cadastrado como fiscal"
        {
            switch (StatusMessage)
            {
                case 1:
                    ViewBag.StatusMessage = "Email inválido, contate a proficiência linguística da ANAC no telefone: 3501-5409";
                    break;
                case 2:
                    ViewBag.StatusMessage = "Usuário já cadastrado como fiscal";
                    break;
            }           
            
            return View();
        }
        public ActionResult Sucesso()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "ContactCaptcha", "Incorrect CAPTCHA code!")]
        public async Task<ActionResult> Contact(EmailFormModel emailContato, string CaptchaCode)
        {                        
                if (!ModelState.IsValid)
                {
                return View();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var body = "<p>Email de: {0} ({1})</p><p>Mensagem:</p><p>{2}</p>";
                        var message = new MailMessage();
                        message.To.Add(new MailAddress("leandro.campos@anac.gov.br"/*"sistema.sysdea@anac.gov.br"*/));

                        message.Subject = "Contato SySDEA";
                        message.Body = string.Format(body, emailContato.FromName, emailContato.FromEmail, emailContato.Message);
                        message.IsBodyHtml = true;
                        EmailSySDEA emailSySDEA = new EmailSySDEA(message);
                        emailSySDEA.remetente = emailContato.FromName + "(" + emailContato.FromEmail + ")";
                        emailSySDEA.tipoOrigem = "Página de Contato";
                        db.EmailSySDEA.Add(emailSySDEA);
                        db.SaveChanges();
                    using (var smtp = new SmtpClient()) 
                        {
                        smtp.UseDefaultCredentials = false;                                                
                        var credential = new NetworkCredential
                        {
                            UserName = "sistema.sysdea@anac.gov.br",  // Email usado para envio da mensagem
                            Password = "qiuEUvZBW7vGAZm9PseK"  // senha para autenticação no servidor de emails
                        };
                        smtp.Credentials = credential;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            
                            await smtp.SendMailAsync(message);
                            return RedirectToAction("Index");
                        }
                    }
                    return View(emailContato);
                }
        }
    }
}