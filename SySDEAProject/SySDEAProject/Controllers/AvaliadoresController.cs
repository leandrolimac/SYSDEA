using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SySDEAProject.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SySDEAProject.Controllers
{
    
    public class AvaliadoresController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        private SySDEAContext db = new SySDEAContext();

        // GET: Avaliadores
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Avaliador.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AvaliadoresXEntidades()
        {
            return View(db.Avaliador.ToList());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult EntidadesDoAvaliador(int id)
        {
            var avaliadoresEntidades = from ae in db.Avaliadores_Entidades where ae.idAvaliador == id orderby ae.dataAdmissao select ae ;
            return View(avaliadoresEntidades);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdicionarEntidade(int? Id)
        {
            List<Entidade> entidades = db.Entidade.ToList();
            
            Avaliador avaliador = db.Avaliador.Find(Id);
            foreach (var item in avaliador.Avaliador_Entidade) {
                if (item.dataEncerramento == null)
                {
                    entidades.Remove(item.Entidade);
                }
            }
            ViewBag.idEntidade = new SelectList(entidades, "idEntidade", "nome");
            if(entidades.Count == 0){
                return RedirectToAction("PainelUsuario", "Manage", new { StatusMessage = "Avaliador já está cadastrado em todas entidades possíveis" });
            }
            return View(avaliador);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AdicionarEntidade([Bind(Include="Id")]int idEntidade, Avaliador avaliador, DateTime dataAdmissao)
        {
                AvaliadorXEntidade avaliadorXEntidade = new AvaliadorXEntidade(idEntidade, avaliador.Id, dataAdmissao);
                avaliador = db.Avaliador.Find(avaliador.Id);
            avaliadorXEntidade.idAvaliadorXEntidade = avaliador.Avaliador_Entidade.Count;
            avaliador.Avaliador_Entidade.Add(avaliadorXEntidade);
                
                db.Entry(avaliador).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
        }          


        [Authorize(Roles = "Admin")]
        public ActionResult RemoverEntidade(int? Id, int? idEntidade, int? idAvaliadorXEntidade)
        {
            AvaliadorXEntidade avaliadorXEntidade = db.Avaliadores_Entidades.Find(Id, idEntidade, idAvaliadorXEntidade);
            return View(avaliadorXEntidade);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult RemoverEntidade([Bind(Include="Id")] int idEntidade,int idAvaliadorXEntidade, Avaliador avaliador, DateTime dataEncerramento)
        {
            
            Entidade entidade = db.Entidade.Find(idEntidade);
            avaliador = db.Avaliador.Find(avaliador.Id);
            AvaliadorXEntidade avaliadorXEntidade = db.Avaliadores_Entidades.Find(avaliador.Id, idEntidade, idAvaliadorXEntidade);
            //avaliador.AvaliadorXEntidade.Remove(avaliadorXEntidade);
            avaliadorXEntidade.dataEncerramento = dataEncerramento;
            db.Entry(avaliadorXEntidade).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [Authorize(Roles ="Entidade")]
        public ActionResult AvaliadoresDaInstituicao()
        {
            int IdEntidade = User.Identity.GetUserId<int>();
            var avaliadorXEntidade = from ae in db.Avaliadores_Entidades where ae.idEntidade == IdEntidade && ae.dataEncerramento == null select ae;
            var avaliadores = from a in avaliadorXEntidade where a.Entidade.Id == IdEntidade select a.Avaliador;
            return View(avaliadores.ToList());
        }


        // GET: Avaliadores/Detalhes/5
        [Authorize(Roles = "Admin,Entidade")]
        public ActionResult Detalhes(int? id)
        {
            if (User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Avaliador avaliador = db.Avaliador.Find(id);
                if (avaliador == null)
                {
                    return HttpNotFound();
                }
                return View(avaliador);
            }

            else if (User.IsInRole("Entidade"))
            {
                int idEntidade = User.Identity.GetUserId<int>();
                Avaliador avaliador = db.Avaliador.Find(id);
                if (avaliador.Avaliador_Entidade.Any(x => x.idEntidade == idEntidade))
                {
                    return View(avaliador);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
        }

        // GET: Avaliadores/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Criar()
        {
            return View();
        }

        // POST: Avaliadores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Criar([Bind(Include = "Id,nome,Email,tel1,tel2,UserName,dtInicioAtividade,emAtividade,dataDeValidade,dtConclusaoCurso")] Avaliador avaliador, UserPessoa userPessoa, int tipoAvaliador, string password)
        {
            string tipoAvaliadorString ="";
            switch (tipoAvaliador)
            {

                case 01:
                    tipoAvaliadorString = "01";
                    break;
                case 02:
                    tipoAvaliadorString = "02";
                    break;
                case 03:
                    tipoAvaliadorString = "03";
                    break;
                default:
                    tipoAvaliadorString = "00";
                    break;
            }         
            avaliador.tipoAvaliador = tipoAvaliadorString;
            userPessoa.Avaliador = avaliador;
            userPessoa.UserName = userPessoa.Email;
            if (ModelState.IsValid)
            {                
                var result = await UserManager.CreateAsync(userPessoa, userPessoa.Password);
                //await UserManager.AddToRoleAsync(userPessoa.Id, "Avaliador");
                
                if (result.Succeeded)
                {
                    var idRoleAvaliador = (from i in db.Roles where i.Name == "Avaliador" select i.Id).First();
                    CustomUserRole roleAvaliador = new CustomUserRole()
                    {
                        RoleId = idRoleAvaliador,
                        CustomRole = db.Roles.Find(idRoleAvaliador),
                        UserId = userPessoa.Id,
                        Usuario = db.Users.Find(userPessoa.Id)
                    };
                    avaliador.UserPessoa.Roles.Add(roleAvaliador);
                    db.UserPessoa.Attach(userPessoa);
                    //avaliador.UserPessoa.Discriminator.Add(db.Discriminator.Find(3));
                    avaliador.ativo = true;
                    RegistroAtividade registroAtividade = new RegistroAtividade { idUserPessoa = avaliador.Id, inicioAtividade = DateTime.Now, tipo = 2 };
                    avaliador.UserPessoa.RegistroAtividade.Add(registroAtividade);
                    db.Entry(avaliador.UserPessoa).State = EntityState.Modified;
                }
                AddErrors(result);
                return RedirectToAction("Index");
            }

            return View(avaliador);
        }

        // GET: Avaliadores/Alterar/
        [Authorize(Roles = "Admin")]
        public ActionResult Alterar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliador avaliador = db.Avaliador.Find(id);
            if (avaliador == null)
            {
                return HttpNotFound();
            }
            return View(avaliador);
        }

        // POST: Avaliadores/Alterar/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Alterar([Bind(Include = "Id,nome,discriminator,Password,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,dtInicioAtividade,emAtividade,dataDeValidade,tipoAvaliador,dtConclusaoCurso")] Avaliador avaliador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(avaliador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(avaliador);
        }

        // GET: Avaliadores/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avaliador avaliador = db.Avaliador.Find(id);
            if (avaliador == null)
            {
                return HttpNotFound();
            }
            return View(avaliador);
        }

        // POST: Avaliadores/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirConfirmado(int id)
        {
            Avaliador avaliador = db.Avaliador.Find(id);
            avaliador.UserPessoa.RegistroAtividade.Last().fimAtividade = DateTime.Now;
            avaliador.ativo = false;
            db.Entry(avaliador).State = EntityState.Modified;

            //db.Users.Remove(avaliador.UserPessoa);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
