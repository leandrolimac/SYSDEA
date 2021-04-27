using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SySDEAProject.Models;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace SySDEAProject.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        SySDEAContext db = new SySDEAContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        public ActionResult Index()
        {
            
                return RedirectToAction("PainelUsuario");
            
          
        }
        [Authorize]
        public ActionResult PainelUsuario(ManageMessageId? message, string StatusMessage)
        {

             StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Sua senha foi alterada com sucesso."                
                : message == ManageMessageId.Error ? "Ocorreu um erro."
                
                : StatusMessage;
            Usuario usuario = db.Users.Find(User.Identity.GetUserId<int>());
            ViewBag.StatusMessage = StatusMessage;
            if(User.IsInRole("Piloto_Incompleto"))
            {
                return RedirectToAction("CompletarCadastro", "Pilotos");
            }
            
            return View(usuario.Roles.ToList());
            
        }

        //GET: /Manage/ContatarEntidade
        public ActionResult ContatarEntidade()
        {
            return View();
        }

        public ActionResult verAgendamentoAtivo()
        {
            int pilotoid = User.Identity.GetUserId<int>();
            var horarios = from h in db.Horario where h.idPiloto == pilotoid select h;
            Horario horario = horarios.Last();
            return View(horario);
        }
        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(int.Parse(User.Identity.GetUserId()), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(int.Parse(User.Identity.GetUserId()));
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

      

    



        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(int.Parse(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(int.Parse(User.Identity.GetUserId()));
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("PainelUsuario", "Manage", new { StatusMessage = "Sua senha foi alterada com sucesso!" });//ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //


        //


        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
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

        private bool HasPassword()
        {
            var user = UserManager.FindById(int.Parse(User.Identity.GetUserId()));
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(int.Parse(User.Identity.GetUserId()));
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }
        [Authorize(Roles ="Fiscal, Admin, Avaliador")]
        public ActionResult CadastroPiloto()
        {
            int idUsuario = User.Identity.GetUserId<int>();
            UserPessoa userpessoa = db.UserPessoa.Find(idUsuario);
            var pilotos = from p in db.InfoAeronauta where p.email == userpessoa.Email select p;
            if(pilotos.Count() == 0)
            {
                return RedirectToAction("PainelUsuario", new { StatusMessage ="E-mail não existente em nosso banco de dados"});
            }else if (pilotos.Count() == 1)
            {
                Piloto piloto = new Piloto(pilotos.First(), idUsuario,userpessoa);
                ViewBag.idEmpresa = new SelectList(db.Empresa, "idEmpresa", "razaoSocial");
                return View(piloto);
            }
            else
            {
                return RedirectToAction("PainelUsuario", new { StatusMessage = "Mais de um piloto encontrado no banco, contate a ANAC" });
            }
            
        }
        [HttpPost]
        public async Task<ActionResult> CadastroPiloto([Bind(Include = "CANACPiloto, IdEmpresa")] Piloto piloto, string tContato)
        {
            if (tContato != null && tContato != "")
            {
                tContato = Regex.Replace(tContato, "[^0-9]+", string.Empty);
                piloto.telContato = Int64.Parse(tContato);
            }
            if (ModelState.IsValid)
            {
                piloto.Id = User.Identity.GetUserId<int>();
                piloto.UserPessoa = db.UserPessoa.Find(piloto.Id);
                //piloto.UserPessoa.Discriminator.Add(db.Discriminator.Find(1));
                await UserManager.AddToRoleAsync(piloto.Id, "Piloto");
                db.Entry(piloto.UserPessoa).State = EntityState.Modified;
                db.Piloto.Add(piloto);
                db.SaveChanges();
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login","Account", new { StatusMessage = "Faça o login novamente" });
            }
            return RedirectToAction("painelUsuario");
        }

        [Authorize(Roles = "Entidade")]
        public ActionResult configuracoesEntidade()
        {
            int idEntidade = User.Identity.GetUserId<int>();
            
            Entidade entidade = db.Entidade.Find(idEntidade);
            if (entidade.LocalEntidade.Count() > 1)
            {
                return RedirectToAction("configLocalEntidade");
            }
            return View(entidade.LocalEntidade.First());
        }


        [HttpPost]
        public ActionResult configuracoesEntidade([Bind(Include="idLocalEntidade,idEntidade, aceitaSolicitacoes,emailContato,precoAvaliacao,numeroSalas,titulo, tel1, tel2")] LocalEntidade localEntidade)
        {
            if (ModelState.IsValid)
            {                
                db.Entry(localEntidade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PainelUsuario", new { StatusMessage = "Configurações alteradas com sucesso" });                
            }
            return RedirectToAction("PainelEntidade");
        }
        public ActionResult configLocalEntidade()
        {
            int idEntidade = User.Identity.GetUserId<int>();
            Entidade entidade = db.Entidade.Find(idEntidade);
            ViewBag.idLocalEntidade = new SelectList(entidade.LocalEntidade, "idLocalEntidade", "titulo");
            return View();
        }
        [HttpPost]
        public ActionResult configLocalEntidade(int idLocalEntidade)
        {
            LocalEntidade localEntidade = db.LocalEntidade.Find(idLocalEntidade);
            return View("configuracoesEntidade", localEntidade);
        }
        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}