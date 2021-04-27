using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SySDEAProject.Models;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Validation;
using System.Collections;
using System.Text;
using Microsoft.Owin.Security;

namespace SySDEAProject.Controllers
{
    public class PilotosController : Controller

    {

      

        public PilotosController()
        {
        }

        public PilotosController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

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

        // GET: Pilotoes
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            var piloto = db.Piloto.Include(p => p.Empresa).Include(p => p.Historico).Include(p => p.ImagemPiloto);
            return View(piloto.ToList());
        }
        [Authorize(Roles ="Piloto_Incompleto")]
        public ActionResult CompletarCadastro()
        {
            int idPiloto = User.Identity.GetUserId<int>();
            Piloto piloto = db.Piloto.Find(idPiloto);
            ViewBag.idEmpresa = new SelectList(db.Empresa, "idEmpresa", "nome");
            List<NivelSDEA> niveis = new List<NivelSDEA>()
            {
                new NivelSDEA() {nivelAtual = 0 ,descricao ="Sem nível" },
                new NivelSDEA() {nivelAtual = 1 ,descricao ="Nível 1" },
                new NivelSDEA() {nivelAtual = 2 ,descricao ="Nível 2" },
                new NivelSDEA() {nivelAtual = 3 ,descricao ="Nível 3" },
                new NivelSDEA() {nivelAtual = 4 ,descricao ="Nível 4" },
                new NivelSDEA() {nivelAtual = 5 ,descricao ="Nível 5" },
                new NivelSDEA() {nivelAtual = 6 ,descricao ="Nível 6" }
            };
            ViewBag.nivelAtual = new SelectList(niveis, "nivelAtual", "descricao");
            return View(piloto);
        }

        /***************************************************************************************************************************************/
        /*                                                                                                                                     */
        /* MÉTODO USADO QUANDO O PILOTO ACESSA O SISTEMA PELA PRIMEIRA VEZ E PRECISA COMPLETAR SEUS DADOS PESSOAIS ANTES DE AGENDAR UM HORÁRIO */
        /*                                                                                                                                     */
        /***************************************************************************************************************************************/
        [HttpPost]
        public async Task<ActionResult> CompletarCadastro([Bind(Include = "Id, UserPessoa.nome, userPessoa.cpf, userPessoa.cep, userPessoa.rua, userPessoa.numero, userPessoa.complemento, userPessoa.bairro, userPessoa.municipio, userPessoa.uf, userPessoa.tel1, userPessoa.tel2, telContato, CANACPiloto, idEmpresa, nivelAtual, observacoes,telContato")]Piloto piloto, UserPessoa userPessoa, HttpPostedFileBase arquivo, string tel1, string tel2, DateTime dataNascimento)
        {
            
            piloto.UserPessoa = db.UserPessoa.Find(piloto.Id);
            piloto.UserPessoa.nome = userPessoa.nome;
            piloto.UserPessoa.cpf = userPessoa.cpf;
            piloto.UserPessoa.Endereco.cep = userPessoa.Endereco.cep;
            piloto.UserPessoa.Endereco.rua = userPessoa.Endereco.rua;
            piloto.UserPessoa.Endereco.numero = userPessoa.Endereco.numero;
            piloto.UserPessoa.Endereco.complemento = userPessoa.Endereco.complemento;
            piloto.UserPessoa.Endereco.bairro = userPessoa.Endereco.bairro;
            piloto.UserPessoa.Endereco.municipio = userPessoa.Endereco.municipio;
            piloto.UserPessoa.Endereco.uf = userPessoa.Endereco.uf;
            piloto.UserPessoa.dataNascimento = dataNascimento;
            piloto.UserPessoa.PhoneNumber = userPessoa.PhoneNumber;
            
            if (arquivo != null && arquivo.ContentLength > 0)
            {
                piloto.ImagemPiloto = new ImagemPiloto();
                using (var reader = new System.IO.BinaryReader(arquivo.InputStream))
                {
                    piloto.ImagemPiloto.arquivo = reader.ReadBytes(arquivo.ContentLength);
                }
            }

            if (ModelState.IsValid)
            {
                if (db.UserPessoa.Any(p => p.cpf == piloto.UserPessoa.cpf && p.Id != piloto.Id) == false) //SE CPF NÃO EXISTE NO BANCO DE DADOS
                {

                    CustomUserRole rolePiloto = new CustomUserRole()
                    {
                        RoleId = 1,
                        CustomRole = db.Roles.Find(1),
                        Usuario = piloto.UserPessoa,
                        UserId = piloto.Id
                    };
                    piloto.UserPessoa.Roles.Add(rolePiloto);

                    piloto.UserPessoa.Roles.Remove((from r in piloto.UserPessoa.Roles where r.RoleId == 1004 select r).First());
                    

                    db.Entry(piloto.UserPessoa).State = EntityState.Modified;

                    db.SaveChanges();

                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    SignInManager.SignIn(piloto.UserPessoa, false, false);
                    return RedirectToAction("PainelUsuario", "Manage");
                }
                else //SE O CPF JÁ EXISTE NO BANCO DE DADOS
                {
                    var usuarios = from u in db.UserPessoa where u.cpf == piloto.UserPessoa.cpf && u.Id != piloto.Id select u;
                    UserPessoa usuarioExistente = usuarios.First();
                    if (piloto.UserPessoa.dataNascimento == usuarioExistente.dataNascimento)
                    {
                        piloto.UserPessoa.Id = usuarios.First().Id;
                        db.Piloto.Remove(piloto);
                        piloto.Id = piloto.UserPessoa.Id;
                        //piloto.UserPessoa.Discriminator.Add(db.Discriminator.Find(1));
                        await UserManager.AddToRoleAsync(piloto.UserPessoa.Id, "Piloto");
                        await UserManager.RemoveFromRoleAsync(piloto.UserPessoa.Id, "Piloto_Incompleto");
                        db.Entry(piloto.UserPessoa).State = EntityState.Modified;
                        db.Piloto.Add(piloto);

                        db.SaveChanges();

                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        SignInManager.SignIn(piloto.UserPessoa, false, false);
                        return RedirectToAction("PainelUsuario", "Manage");
                    }
                }
            }

            ViewBag.idEmpresa = new SelectList(db.Empresa, "idEmpresa", "nome", piloto.idEmpresa);
            ViewBag.Id = new SelectList(db.Historico, "idPiloto", "observacoes", piloto.Id);
            ViewBag.Id = new SelectList(db.ImagemPiloto, "idPiloto", "idPiloto", piloto.Id);
            List<NivelSDEA> niveis = new List<NivelSDEA>()
            {
                new NivelSDEA() {nivelAtual = 0 ,descricao ="Sem nível" },
                new NivelSDEA() {nivelAtual = 1 ,descricao ="Nível 1" },
                new NivelSDEA() {nivelAtual = 2 ,descricao ="Nível 2" },
                new NivelSDEA() {nivelAtual = 3 ,descricao ="Nível 3" },
                new NivelSDEA() {nivelAtual = 4 ,descricao ="Nível 4" },
                new NivelSDEA() {nivelAtual = 5 ,descricao ="Nível 5" },
                new NivelSDEA() {nivelAtual = 6 ,descricao ="Nível 6" }
            };
            ViewBag.nivelAtual = new SelectList(niveis, "nivelAtual", "descricao");
            return View(piloto);

        }
        // GET: Pilotoes/Details/5
        public ActionResult Detalhes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Piloto piloto = db.Piloto.Find(id);
            if (piloto == null)
            {
                return HttpNotFound();
            }
            return View(piloto);
        }

        // GET: Pilotoes/Create
        public ActionResult Criar()
        {
            ViewBag.idEmpresa = new SelectList(db.Empresa, "idEmpresa", "nome");
            ViewBag.Id = new SelectList(db.Historico, "idPiloto", "observacoes");
            ViewBag.Id = new SelectList(db.ImagemPiloto, "idPiloto", "idPiloto");
            List<NivelSDEA> niveis = new List<NivelSDEA>()
            {
                new NivelSDEA() {nivelAtual = 0 ,descricao ="Sem nível" },
                new NivelSDEA() {nivelAtual = 1 ,descricao ="Nível 1" },
                new NivelSDEA() {nivelAtual = 2 ,descricao ="Nível 2" },
                new NivelSDEA() {nivelAtual = 3 ,descricao ="Nível 3" },
                new NivelSDEA() {nivelAtual = 4 ,descricao ="Nível 4" },
                new NivelSDEA() {nivelAtual = 5 ,descricao ="Nível 5" },
                new NivelSDEA() {nivelAtual = 6 ,descricao ="Nível 6" }
            };

            ViewBag.nivelAtual = new SelectList(niveis, "nivelAtual", "descricao");

            return View();

        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Criar([Bind(Include = "Id, UserPessoa.nome,UserPessoa.cpf,UserPessoa.cep,UserPessoa.rua,UserPessoa.numero,UserPessoa.complemento,UserPessoa.bairro,UserPessoa.municipio,UserPessoa.uf,UserPessoa.tel1,UserPessoa.tel2,UserPessoa.dataNascimento,UserPessoa.Email,UserPessoa.PhoneNumber,UserPessoa.UserName,CANACPiloto,idEmpresa,nivelAtual,observacoes,telContato")]string t1, string t2, string tContato, Piloto piloto, UserPessoa userPessoa, HttpPostedFileBase arquivo)
        {
            if (ModelState.IsValid)
            {
                piloto.UserPessoa = userPessoa;
                Random rnd = new Random();
                string pilotopassword = "123456"; //rnd.Next(100000, 999999).ToString();
                //piloto.UserPessoa.Discriminator.Add(db.Discriminator.Find(1));
                piloto.UserPessoa.UserName = piloto.UserPessoa.Email;
               

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");


                if (t1 != null && t1 != "")
                {
                    t1 = Regex.Replace(t1, "[^0-9]+", string.Empty);
                    piloto.UserPessoa.PhoneNumber = t1;
                }
                if (tContato != null && tContato != "")
                {
                    tContato = Regex.Replace(tContato, "[^0-9]+", string.Empty);
                    piloto.telContato = Int64.Parse(tContato);
                }
                if (arquivo != null && arquivo.ContentLength > 0)
                {
                    piloto.ImagemPiloto = new ImagemPiloto();
                    using (var reader = new System.IO.BinaryReader(arquivo.InputStream))
                    {
                        piloto.ImagemPiloto.arquivo = reader.ReadBytes(arquivo.ContentLength);
                    }
                }

                if (db.UserPessoa.Any(p => p.cpf == piloto.UserPessoa.cpf) == false) //CPF NÃO ENCONTRADO NO BANCO
                {

                    //piloto.UserPessoa.Discriminator.Add(db.Discriminator.Find(1));
                    piloto.UserPessoa.UserName = piloto.UserPessoa.Email;
                    var result = await UserManager.CreateAsync(piloto.UserPessoa, pilotopassword);
                    AddErrors(result);

                }
                else
                {
                    var usuarios = from p in db.UserPessoa where p.cpf == piloto.UserPessoa.cpf select p;
                    UserPessoa usuario = usuarios.First();
                    //piloto.UserPessoa.Discriminator.Add(db.Discriminator.Find(1));
                    piloto.Id = usuario.Id;
                    usuario.Piloto = piloto;
                    usuario.Piloto.idEmpresa = piloto.idEmpresa;
                    db.Piloto.Add(piloto);
                    db.Entry(usuario).State = EntityState.Modified;
                    
                        db.SaveChanges();
                    
                }
                 
                return RedirectToAction("Index");
                }
                
                ViewBag.idEmpresa = new SelectList(db.Empresa, "idEmpresa", "nome", piloto.idEmpresa);
                ViewBag.IdHistorico = new SelectList(db.Historico, "idPiloto", "observacoes", piloto.Id);
                ViewBag.Id = new SelectList(db.ImagemPiloto, "idPiloto", "idPiloto", piloto.Id);
                return View(piloto);
            }
        

        // GET: Pilotoes/Edit/5
        public ActionResult Alterar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Piloto piloto = db.Piloto.Find(id);
            if (piloto == null)
            {
                return HttpNotFound();
            }
            //Piloto pilotoAtual = new Piloto (piloto);
            List<NivelSDEA> niveis = new List<NivelSDEA>()
            {
                new NivelSDEA() {nivelAtual = 0 ,descricao ="Sem nível" },
                new NivelSDEA() {nivelAtual = 1 ,descricao ="Nível 1" },
                new NivelSDEA() {nivelAtual = 2 ,descricao ="Nível 2" },
                new NivelSDEA() {nivelAtual = 3 ,descricao ="Nível 3" },
                new NivelSDEA() {nivelAtual = 4 ,descricao ="Nível 4" },
                new NivelSDEA() {nivelAtual = 5 ,descricao ="Nível 5" },
                new NivelSDEA() {nivelAtual = 6 ,descricao ="Nível 6" }
            };
            ViewBag.nivelAtual = new SelectList(niveis, "nivelAtual", "descricao");
            ViewBag.pilotoAtual = piloto;
            ViewBag.idEmpresa = new SelectList(db.Empresa, "idEmpresa", "nome", piloto.idEmpresa);
            ViewBag.Id = new SelectList(db.Historico, "idPiloto", "observacoes", piloto.Id);
            ViewBag.Id = new SelectList(db.ImagemPiloto, "idPiloto", "idPiloto", piloto.Id);
            return View(piloto);
        }

        // POST: Pilotoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alterar([Bind(Include = "Id,nome,cpf,cep,rua,numero,complemento,bairro,municipio,uf,tel1,tel2,dataNascimento,Email,CANACPiloto,idEmpresa,nivelAtual,observacoes")]string t1, string t2, string tContato, Piloto piloto, HttpPostedFileBase arquivo)
        {
            

            if (arquivo != null && arquivo.ContentLength > 0)
            {
                piloto.ImagemPiloto = new ImagemPiloto();
                using (var reader = new System.IO.BinaryReader(arquivo.InputStream))
                {
                    piloto.ImagemPiloto.arquivo = reader.ReadBytes(arquivo.ContentLength);
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(piloto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEmpresa = new SelectList(db.Empresa, "idEmpresa", "nome", piloto.idEmpresa);
            ViewBag.Id = new SelectList(db.Historico, "idPiloto", "observacoes", piloto.Id);
            ViewBag.Id = new SelectList(db.ImagemPiloto, "idPiloto", "idPiloto", piloto.Id);
            return View(piloto);
        }

        // GET: Pilotoes/Delete/5
        public ActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Piloto piloto = db.Piloto.Find(id);
            if (piloto == null)
            {
                return HttpNotFound();
            }
            return View(piloto);
        }

        // POST: Pilotoes/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Piloto piloto = db.Piloto.Find(id);
            if (piloto.ImagemPiloto != null)
            {
                db.ImagemPiloto.Remove(piloto.ImagemPiloto);
            }
            //piloto.UserPessoa.Discriminator.Remove(db.Discriminator.Find(1));
            db.Entry(piloto.UserPessoa).State = EntityState.Modified;
            UserManager.RemoveFromRoleAsync(piloto.Id, "Piloto");
            db.Piloto.Remove(piloto);
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
