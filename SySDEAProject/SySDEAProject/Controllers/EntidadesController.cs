using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SySDEAProject.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace SySDEAProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EntidadesController : Controller
    {
        public EntidadesController() { }
        // GET: Entidades/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Criar()
        {
            //ViewBag.idEntidade = new SelectList(db.EntidadeLogin, "Id", "nome");
            ViewBag.idEntidade = new SelectList(db.Entidade, "Id", "nome");
            return View();
        }


        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        

        public EntidadesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        private SySDEAContext db = new SySDEAContext();

        // GET: Entidades
        public ActionResult Index()
        {

            var entidade = db.Entidade;
            return View(entidade.ToList());
        }

        // GET: Entidades/Details/5
        public ActionResult Detalhes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entidade entidade = db.Entidade.Find(id);
            if (entidade == null)
            {
                return HttpNotFound();
            }
            return View(entidade);
        }
        // POST: Entidades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Criar([Bind(Include = "Id,nome,emailentidade,tel1,nome,emailRepAdm,emailELEs,cnpj,tel2,suspensa, titulo, numeroSalas")]string password, Entidade entidade, LocalEntidade localEntidade)
        {
            entidade.UserName = entidade.EmailEntidade;
            entidade.Email = entidade.EmailEntidade;
            if (ModelState.IsValid)
            {
                //EntidadeLogin entidadeLogin = new EntidadeLogin();
                
                
                //entidadeLogin.Email = entidade.EmailEntidade;
                //entidadeLogin.nome = entidade.nome;
                //entidadeLogin.UserName = entidade.EmailEntidade;
                localEntidade.suspensa = false;
                entidade.Email = entidade.EmailEntidade;
                localEntidade.idEntidade = entidade.Id;
                //entidade.LocalEntidade.Add(localEntidade);
                //entidade.EntidadeLogin = entidadeLogin;
                //entidadeLogin.Entidade = entidade;
                //entidade.EntidadeLogin = entidadeLogin;



                var result = await UserManager.CreateAsync(entidade, password);
                if (result.Succeeded)
                {
                    //db.Entidade.Add(entidade);
                    CustomUserRole entidadeRole = new CustomUserRole
                    {
                        CustomRole = db.Roles.Find(1002),
                        RoleId = 1002,
                        UserId = entidade.Id,
                        Usuario = entidade
                    };

                    //entidade.Roles.Add(entidadeRole);
                    db.AspNetUserRoles.Add(entidadeRole);
                    //db.EntidadeLogin.Attach(entidade.EntidadeLogin);
                    //await this.UserManager.AddToRoleAsync(entidadeLogin.Id, "Entidade");
                    //await this.UserManager.AddToRoleAsync(entidade.Id, "Entidade");
                    //entidadeLogin.Discriminator.Add(db.Discriminator.Find(4));
                    //entidade.Discriminator.Add(db.Discriminator.Find(4));
                    //db.Entry(entidade.EntidadeLogin).State = EntityState.Modified;
                    localEntidade.idEntidade = entidade.Id;
                    db.LocalEntidade.Add(localEntidade);
                    db.Entry(entidade).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                   
                }          
                                  
                AddErrors(result);
            }
            
            ViewBag.idEntidade = new SelectList(db.Users, "Id", "nome", entidade.Id);
            entidade.LocalEntidade = new List<LocalEntidade>();
            entidade.LocalEntidade.Add(localEntidade);
            return View(entidade);
        }

        // GET: Entidades/Edit/5
        public ActionResult Alterar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entidade entidade = db.Entidade.Find(id);
            if (entidade == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEntidade = new SelectList(db.Users, "Id", "nome", entidade.Id);
            return View(entidade);
        }

        // POST: Entidades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alterar([Bind(Include = "idEntidade,EmailEntidade, tel1,nome,emailRepAdm,emailELEs,cnpj,tel2,suspensa")] Entidade entidade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entidade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEntidade = new SelectList(db.Users, "Id", "nome", entidade.Id);
            return View(entidade);
        }

        // GET: Entidades/Delete/5
        public ActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entidade entidade = db.Entidade.Find(id);
            if (entidade == null)
            {
                return HttpNotFound();
            }
            return View(entidade);
        }

        // POST: Entidades/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirConfirmado(int id)
        {
            Entidade entidade = db.Entidade.Find(id);

            //int z = 0;
            //while (db.LocalEntidade.Count() > 0)
            //{


            //}
            //db.Users.Remove(entidade.EntidadeLogin);
            int i = 0;
            for (i = 0; i < entidade.LocalEntidade.Count(); i++){
                LocalEntidade localEntidade = entidade.LocalEntidade.ElementAt(i);
                //db.Endereco.Remove(localEntidade.Endereco);
                localEntidade.ativa = false;
                //db.LocalEntidade.Remove(localEntidade);
                //db.SaveChanges();
            }
            entidade.ativa = false;
            //db.Users.Remove(entidade);
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
