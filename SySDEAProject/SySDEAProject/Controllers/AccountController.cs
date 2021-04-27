using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SySDEAProject.Models;
using System.Net.Mail;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;
using System.Text.RegularExpressions;
using BotDetect.Web.Mvc;
using SySDEAProject.ConsultarAeronauta;
using System.Web.UI;

namespace SySDEAProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private SySDEAContext db = new SySDEAContext();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //GET: /Account/AcessoNegado
        //********************************************************************************************//
        // EXIBE A PÁGINA DE ACESSO NEGADO CASO UM USUÁRIO TENTE EXECUTAR UMA AÇÃO A QUAL NÃO TENHA   //
        // ACESSO                                                                                     //
        //                                                                                            //
        //********************************************************************************************//
        public ActionResult AcessoNegado()
        {
            return View();
        }


        //
        // GET: /Account/Login
        //********************************************************************************************//
        // REALIZA A AUTENTICAÇÃO DE UM USUARIO NO SISTEMA                                            //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//

        [AllowAnonymous]

        public ActionResult Login(Uri returnUrl)
        {

            if (Request.IsAuthenticated)
            {
                if (returnUrl == null)
                {
                    return View("sessaoativa");
                }
                else
                {
                    return View("AcessoNegado");
                }
            }
            if (returnUrl != null)
            {
                ViewBag.ReturnUrl = returnUrl.ToString();
            }
            return View();
        }

        //********************************************************************************************//
        //   GERA USUÁRIO E SENHA PARA PRIMEIRO ACESSO DE PILOTO AO SYSDEA BASEADO EM EMAIL E CANAC   //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        [AllowAnonymous]
        public ActionResult AcessoPilotos(string codErro)
        {
            ViewBag.codErro = codErro;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [CaptchaValidation("CaptchaCode", "AcessoPilotoCaptcha", "Incorrect CAPTCHA code!")]
        public async Task<ActionResult> AcessoPilotos(string canacstring, string cpfaeronauta, string CaptchaCode)
        {
            if (!ModelState.IsValid)
            {

                return View();
            }

            else {

                var sr = new AeronautaServiceClient("BasicHttpBinding_IAeronautaService1");
                sr.ClientCredentials.UserName.UserName = "sistema.sysdea";
                sr.ClientCredentials.UserName.Password = "Anac10!";
                var param = new AeronautaParametro()
                {
                    CdAnac = canacstring,
                    NomeSistemaAutenticacao = "sysdea",
                    LoginSistema = "sistema.sysdea",
                    NumeroCpf = cpfaeronauta,
                    IdAeronauta = 0,
                    NomeAeronauta = "",
                    NomeSistemaRequisitante = "sistema.sysdea"
                };
                var ans = sr.ConsultarAeronauta(param);
                ConsultarAeronauta.AeronautaType aeronauta = ans.First();
                string email = aeronauta.EnderecosEletronico.First().ToString();

                // SE O EMAIL ESTIVER CADASTRADO PARA MAIS DE UM PILOTO NO BANCO, REDIRECIONA A UMA PÁGINA DE ERRO
                /*
                if (emailCounter > 1)
                {
                    return RedirectToAction("Erro", "Home", new { StatusMessage = 1 });
                }
                */
                //SE O CANAC JÁ ESTIVER CADASTRADO NA TABELA DE PILOTOS, EXIBE MENSAGEM INFORMANDO QUE PILOTO JÁ ESTÁ CADASTRADO
                int canac = int.Parse(canacstring);
                if (db.Piloto.Any(p => p.CANACPiloto == canac))
                {
                    return RedirectToAction("AcessoPilotos", new { codErro = "Piloto já cadastrado" });
                }
                //SE O EMAIL JÁ ESTIVER CADASTRADO NA TABELA DE USUARIO, EXIBE MENSAGEM INFORMANDO QUE PILOTO JÁ ESTÁ CADASTRADO

                if (db.UserPessoa.Any(p => p.Email == email))
                {
                    return RedirectToAction("AcessoPilotos", new { codErro = "Piloto já cadastrado" });
                }

                //SE O CANAC NÃO EXISTE NA BASE DE DADOS
              /*  if (db.InfoAeronauta.Count(i => i.canacpiloto == canac) == 0)
                {
                    return RedirectToAction("AcessoPilotos", new { codErro = "Canac Inválido, verifique e tente novamente ou contate a ANAC" });
                }
                

                //CRIA UM NOVO OBJETO DE INFOAERONAUTA BUSCANDO PELO CANAC
                InfoAeronauta infoAeronauta = (from i in db.InfoAeronauta where i.canacpiloto == canac select i).SingleOrDefault();

                //SE O EMAIL INSERIDO NÃO CORRESPONDE AO CANAC, RETORNA UMA MENSAGEM DE ERRO
                if (infoAeronauta.email.Equals(email, StringComparison.InvariantCultureIgnoreCase) == false)
                {
                    return RedirectToAction("AcessoPilotos", new { codErro = "Email inválido" });
                }
                */

                //GERA UMA SENHA ALEATÓRIA DE 6 DÍGITOS
                Random rnd = new Random();
                string password = rnd.Next(100000, 999999).ToString();


                //INICIA UM NOVO OBJETO DE PILOTO BASEADO NOS DADOS DE INFOAERONAUTA
                Piloto piloto = new Piloto();
                if (db.Empresa.Count() == 0)
                {
                    Empresa semEmpresa = new Empresa();
                    semEmpresa.idEmpresa = 1;
                    semEmpresa.nome = "sem empresa";
                    db.Empresa.Add(semEmpresa);
                    db.SaveChanges();
                }
                piloto.idEmpresa = 1; //CÓDIGO DE 'SEM EMPRESA' NO BANCO DE DADOS É 1
                piloto.UserPessoa = new UserPessoa();

                if (aeronauta.Telefones.First() != null)
                {
                    piloto.UserPessoa.PhoneNumber = aeronauta.Telefones.First().ToString();
                    piloto.telContato = Int64.Parse(piloto.UserPessoa.PhoneNumber);
                }
                else
                {                    
                    piloto.telContato = 0;


                    piloto.UserPessoa.nome = aeronauta.NomeAeronauta;
                    piloto.UserPessoa.UserName = email;
               /* piloto.UserPessoa.Endereco = new Endereco();
                if (infoAeronauta.cep != null)
                {
                    piloto.UserPessoa.Endereco.cep = Int64.Parse(infoAeronauta.cep);
                }*/
                piloto.UserPessoa.Email = email;
                /*piloto.UserPessoa.Endereco.rua = infoAeronauta.rua;
                piloto.UserPessoa.Endereco.numero = infoAeronauta.numero;
                piloto.UserPessoa.Endereco.complemento = infoAeronauta.complemento;
                piloto.UserPessoa.Endereco.bairro = infoAeronauta.bairro;
                piloto.UserPessoa.Endereco.municipio = infoAeronauta.cidade;
                piloto.UserPessoa.Endereco.uf = infoAeronauta.uf;*/
                if (aeronauta.NumeroCpf != null)
                {
                    aeronauta.NumeroCpf = Regex.Replace(aeronauta.NumeroCpf, "[^0-9]+", string.Empty);
                    piloto.UserPessoa.cpf = Int64.Parse(aeronauta.NumeroCpf);
                }
                piloto.UserPessoa.dataNascimento = aeronauta.Nascimento;

                //REPETIÇÃO: SE O CPF FOR IGUAL A UM CPF JÁ EXISTENTE NO BANCO, CRIA UM CPF ALEATÓRIO(SOLUÇÃO TEMPORÁRIA)
                /*while (db.UserPessoa.Any(u => u.cpf == piloto.UserPessoa.cpf))
                {
                    piloto.UserPessoa.cpf = rnd.Next(100000, 999999);
                }*/


                //SE O EMAIL JÁ EXISTE NO BANCO, RETORNA MENSAGEM DE ERRO
                if (db.UserPessoa.Any(p => p.UserName == piloto.UserPessoa.UserName || p.Email == piloto.UserPessoa.Email))
                {
                    RedirectToAction("PessoaExistente");

                }


                //
                // try
                //{
                var result = await UserManager.CreateAsync(piloto.UserPessoa, password); //CRIA UM NOVO USUÁRIO DO SISTEMA NO BANCO
                if (result.Succeeded)
                {
                    db.UserPessoa.Attach(piloto.UserPessoa);

                    CustomUserRole rolePiloto = new CustomUserRole()  //ADICIONA PERMISSÕES DE PILOTO INCOMPLETO AO USUÁIO
                    {
                        RoleId = 1004,
                        CustomRole = db.Roles.Find(1004),
                        UserId = piloto.Id,
                        Usuario = piloto.UserPessoa
                    };
                    piloto.UserPessoa.Roles.Add(rolePiloto);
                    db.Entry(piloto.UserPessoa).State = EntityState.Modified;
                    piloto.Id = piloto.UserPessoa.Id;
                    piloto.CANACPiloto = canac;                    
                    piloto.idEmpresa = 1; //CÓDIGO DE 'SEM EMPRESA' NO BANCO DE DADOS É 1
                    db.Piloto.Add(piloto);

                    //ENVIA UM EMAIL PARA O PILOTO CADASTRADO COM SUA SENHA
                    EmailSySDEA emailsysdea = new EmailSySDEA();
                    emailsysdea.assunto = "Primeiro acesso de piloto";
                    emailsysdea.textoEmail = "Bem vindo ao sysdea, " + piloto.UserPessoa.nome + "<br> Sua senha de acesso é: " + password;
                    emailsysdea.destino = piloto.UserPessoa.Email;
                    emailsysdea.tipoOrigem = "Primeiro acesso de piloto";
                    var body = "<p>SySDEA </p><p>{0}</p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(piloto.UserPessoa.Email));
                    message.Subject = emailsysdea.assunto;
                    message.Body = string.Format(body, emailsysdea.textoEmail);
                    message.IsBodyHtml = true;

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
                        
                    }

                    db.EmailSySDEA.Add(emailsysdea); //GRAVA O EMAIL ENVIADO NO BANCO

                }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var failure in ex.EntityValidationErrors)
                        {
                            sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                            foreach (var error in failure.ValidationErrors)
                            {
                                sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                sb.AppendLine();
                            }
                        }
                        return RedirectToAction("Erro", "Home");
                    }
                }
                return RedirectToAction("Sucesso", "Home");
            }
        }
        public ActionResult PessoaExistente()
        {
            return View();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "LoginCaptcha", "Incorrect CAPTCHA code!")]
        public async Task<ActionResult> Login(LoginViewModel model, Uri returnUrl, string CaptchaCode)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int canacLogin;
            int.TryParse(model.UserName, out canacLogin);
            //canacLogin = int.Parse(model.UserName);
            if (db.Piloto.Any(u => u.CANACPiloto == canacLogin))
            {
                model.UserName = (from u in db.Piloto where u.CANACPiloto == canacLogin select u.UserPessoa.UserName).SingleOrDefault().ToString();
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    if (returnUrl == null)
                    {
                        return RedirectToAction("PainelUsuario", "Manage");
                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Login Inválido.");
                    return View(model);
            }
        }

        //



        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(int.Parse(userId), code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        //********************************************************************************************//
        // RECUPERA A SENHA DE UM USUÁRIO QUE TENHA ESQUECIDO                                         //
        //                                                                                            //
        //                                                                                            //
        //********************************************************************************************//
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    ViewBag.StatusMessage = "Email não cadastrado!";
                    return View(model);
                }
                if (user != null)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Reset Password", "Por favor, troque sua senha clicando \"" + callbackUrl + "\">aqui");
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //

        //





        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(Uri returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl.ToString()))
            {
                return Redirect(returnUrl.ToString());
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(Uri provider, Uri redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(Uri provider, Uri redirectUri, string userId)
            {
                LoginProvider = provider.ToString();
                RedirectUri = redirectUri.ToString();
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }

}