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
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;
using System.Text;
using System.Data.Entity.Infrastructure;

namespace SySDEAProject.Controllers
{
    //[Authorize(Roles = "Admin")]

    public class FiscaisController : Controller
    {
        public FiscaisController() { }
        public FiscaisController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        // GET: Fiscais
        public ActionResult Index()
        {
            var fiscaisAtivos = from f in db.Fiscal where f.ativo == true select f;
            return View(fiscaisAtivos.ToList());
        }

        // GET: Fiscais/Details/5
        public ActionResult Detalhes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fiscal fiscal = db.Fiscal.Find(id);
            if (fiscal == null)
            {
                return HttpNotFound();
            }
            return View(fiscal);
        }
        [Authorize(Roles ="Admin")]
        public ActionResult verificaCPF()
        {
            return View();
        }
        [HttpPost]
        public ActionResult verificaCPF([Bind(Include = "cpf, dataNascimento")] string cpf, UserPessoa userPessoa)
        {
            db = new SySDEAContext();
            userPessoa.cpf = Int64.Parse(cpf);
            if (db.UserPessoa.Any(p => p.cpf == userPessoa.cpf) == false)// O pedaço de código abaixo é executado caso o cpf informado não exista no banco
            {
                return RedirectToAction("Criar", "Fiscais", new { cpf = userPessoa.cpf, dataNascimento = userPessoa.dataNascimento });
            }
            if (db.UserPessoa.Any(p => p.cpf == userPessoa.cpf && p.dataNascimento == userPessoa.dataNascimento) == true)//Verifica CPF com data de nascimento da pessoa 
            {
                userPessoa.Id = db.UserPessoa.First(u => u.cpf == userPessoa.cpf).Id;
                return RedirectToAction("CadastrarFiscalExistente", new { id = userPessoa.Id });
            }
            else
            {
                ViewBag.StatusMessage = "Data de nascimento não correspondente ao CPF informado, verifique as informações";
                return View();
            }

        }
        // GET: Fiscais/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Criar(long? cpf, DateTime? dataNascimento)
        {
            if (cpf != null)
            {
                ViewBag.cpf = cpf;
            }
            if (dataNascimento != null)
            {
                ViewBag.dataNascimento = dataNascimento;
            }
            else
            {
                ViewBag.dataNascimento = DateTime.Now;
            }
            return View();
        }

        // POST: Fiscais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> Criar([Bind(Include = "Id,UserPessoa.nome,UserPessoa.discriminator, UserPessoa.Email,UserPessoa.cpf,UserPessoa.cep,UserPessoa.rua,UserPessoa.numero,UserPessoa.complemento,UserPessoa.bairro,UserPessoa.municipio,UserPessoa.uf,UserPessoa.dataNascimento, UserPessoa.t1,UserPessoa.tel2,siape")]Fiscal fiscal, UserPessoa userPessoa, string password, bool administrador)
        {
            if (userPessoa. cpf != 0)
            {
                ViewBag.cpf = userPessoa.cpf;
            }
            if (userPessoa.dataNascimento != null)
            {
                ViewBag.dataNascimento = userPessoa.dataNascimento;
            }
            else
            {
                ViewBag.dataNascimento = DateTime.Now;
            }           
            if (ModelState.IsValid)
            {
                fiscal.administrador = administrador;
                fiscal.UserPessoa = userPessoa;
                //fiscal.UserPessoa.Discriminator.Add(db.Discriminator.Find(3));
                fiscal.UserPessoa.UserName = fiscal.UserPessoa.Email;
                fiscal.UserPessoa.Fiscal = fiscal;
                var result = await UserManager.CreateAsync(fiscal.UserPessoa, fiscal.UserPessoa.Password);

                if (result.Succeeded)
                {
                    fiscal.Id = fiscal.UserPessoa.Id;




                    if (administrador == true)
                    {
                       /* if (fiscal.UserPessoa.Discriminator.Any(d => d.Id == 2) == false)
                        {
                            fiscal.UserPessoa.Discriminator.Add(db.Discriminator.Find(2));
                        }*/
                        await this.UserManager.AddToRoleAsync(fiscal.UserPessoa.Id, "Admin");
                    }
                    await this.UserManager.AddToRoleAsync(fiscal.Id, "Fiscais");
                    RegistroAtividade registroAtividade = new RegistroAtividade { idUserPessoa = fiscal.Id, inicioAtividade = DateTime.Now, tipo = 1 };
                    fiscal.UserPessoa.RegistroAtividade.Add(registroAtividade);
                    db.Entry(fiscal).State = EntityState.Modified;
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

                        throw new DbEntityValidationException(
                            "Entity Validation Failed - errors follow:\n" +
                            sb.ToString(), ex
                        ); // Add the original exception as the innerException
                    }
                }
                AddErrors(result);


                /* else// Caso o CPF já exista na base de dados
                 {
                     var usuarios = from p in db.UserPessoa where p.cpf == fiscal.UserPessoa.cpf select p;
                     UserPessoa usuario = usuarios.First();
                     fiscal.Id = usuario.Id;
                     usuario.Fiscal = fiscal;
                     fiscal.UserPessoa.Discriminator.Add(db.Discriminator.Find(2));                    
                     db.Entry(usuario).State = EntityState.Modified;
                 }*/
                return RedirectToAction("Index");
            }
            return View(fiscal);
        }

        // GET: Fiscais/CadastrarFiscalExistente/5
        public ActionResult CadastrarFiscalExistente(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPessoa userPessoa = db.UserPessoa.Find(id);
            if (userPessoa == null)
            {
                return HttpNotFound();
            }
            Fiscal fiscal = new Fiscal();
            if (userPessoa.Roles.Any(d=> d.RoleId ==3) == false) 
            {
                fiscal.UserPessoa = userPessoa;

                return View(fiscal);
            }
            return RedirectToAction("Erro", "Home", new { StatusMessage = 2 });
        }
        [HttpPost]
        public ActionResult CadastrarFiscalExistente([Bind(Include = "Id,UserPessoa.nome,UserPessoa.discriminator,UserPessoa.Password,UserPessoa.Email,UserPessoa.cpf,UserPessoa.cep,UserPessoa.rua,UserPessoa.numero,UserPessoa.complemento,UserPessoa.bairro,UserPessoa.municipio,UserPessoa.uf,UserPessoa.t1,UserPessoa.tel1,UserPessoa.tel2,siape")]Fiscal fiscal, UserPessoa userPessoa, DateTime dataNascimento, string t2, bool administrador)
        {
            fiscal.UserPessoa = db.UserPessoa.Find(fiscal.Id);
            
            fiscal.UserPessoa.nome = userPessoa.nome;
            fiscal.UserPessoa.cpf = userPessoa.cpf;
            fiscal.UserPessoa.Endereco = new Endereco();
            fiscal.UserPessoa.Endereco.cep = userPessoa.Endereco.cep;
            fiscal.UserPessoa.Endereco.rua = userPessoa.Endereco.rua;
            fiscal.UserPessoa.Endereco.numero = userPessoa.Endereco.numero;
            fiscal.UserPessoa.Endereco.complemento = userPessoa.Endereco.complemento;
            fiscal.UserPessoa.Endereco.bairro = userPessoa.Endereco.bairro;
            fiscal.UserPessoa.Endereco.municipio = userPessoa.Endereco.municipio;
            fiscal.UserPessoa.Endereco.uf = userPessoa.Endereco.uf;
            fiscal.UserPessoa.dataNascimento = dataNascimento;
            fiscal.UserPessoa.PhoneNumber = userPessoa.PhoneNumber;
            fiscal.UserPessoa.dataNascimento = dataNascimento;
            fiscal.administrador = administrador;
            if (ModelState.IsValid)
            {
                //userPessoa.Discriminator = db.UserPessoa.Find(fiscal.UserPessoa.Id).Discriminator;
                if (userPessoa.Roles.Any(d => d.RoleId == 3) == false)//PESSOA NÃO POSSUI ROLE DE FISCAL
                {
                    //userPessoa.Discriminator.Add(db.Discriminator.Find(3));
                    CustomUserRole fiscalRole = new CustomUserRole
                    {
                        CustomRole = db.Roles.Find(3),
                        RoleId = 3,
                        UserId = fiscal.Id,
                        Usuario = fiscal.UserPessoa
                    };

                    fiscal.UserPessoa.Roles.Add(fiscalRole);
                    //await this.UserManager.AddToRoleAsync(fiscal.Id, "Fiscal");

                }
                if (administrador == true)
                {

                    if (fiscal.UserPessoa.Roles.Any(d => d.RoleId == 2) == false)//(fiscal.UserPessoa.Discriminator.Any(d => d.Id == 2) == false)
                    {
                        //userPessoa.Discriminator.Add(db.Discriminator.Find(2));
                    }
                    //await this.UserManager.AddToRoleAsync(fiscal.UserPessoa.Id, "Admin");
                    CustomUserRole adminRole = new CustomUserRole
                    {
                        CustomRole = db.Roles.Find(2),
                        RoleId = 2,
                        UserId = fiscal.Id,
                        Usuario = fiscal.UserPessoa
                    };

                    fiscal.UserPessoa.Roles.Add(adminRole);
                }
                fiscal.ativo = true;
                db.Entry(fiscal.UserPessoa).State = EntityState.Modified;
                db.Fiscal.Add(fiscal);
                db.SaveChanges();

            }
            else
            {

            }
            SignInManager.SignIn(db.Users.Find(User.Identity.GetUserId<int>()), false, false);
            return RedirectToAction("Index", "Fiscais");
        }

        // GET: Fiscais/Alterar/5
        public ActionResult Alterar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fiscal fiscal = db.Fiscal.Find(id);
            if (fiscal == null)
            {
                return HttpNotFound();
            }
            return View(fiscal);
        }

        // POST: Fiscais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> Alterar([Bind(Include = "Id,UserPessoa.nome,UserPessoa.discriminator,UserPessoa.Password,UserPessoa.Email,UserPessoa.cpf,UserPessoa.cep,UserPessoa.rua,UserPessoa.numero,UserPessoa.complemento,UserPessoa.bairro,UserPessoa.municipio,UserPessoa.uf,UserPessoa.tel1,UserPessoa.tel2,siape")]Fiscal fiscal, UserPessoa userPessoa, DateTime dataNascimento, string t1, string t2, bool administrador)
        {
            if (ModelState.IsValid)
            {
                fiscal.UserPessoa = db.UserPessoa.Find(fiscal.Id);
                //fiscal.UserPessoa.Usuario = db.Users.Find(fiscal.Id);
                fiscal.UserPessoa.nome = userPessoa.nome;
                fiscal.UserPessoa.cpf = userPessoa.cpf;
                fiscal.UserPessoa.Endereco.cep = userPessoa.Endereco.cep;
                fiscal.UserPessoa.Endereco.rua = userPessoa.Endereco.rua;
                fiscal.UserPessoa.Endereco.numero = userPessoa.Endereco.numero;
                fiscal.UserPessoa.Endereco.complemento = userPessoa.Endereco.complemento;
                fiscal.UserPessoa.Endereco.bairro = userPessoa.Endereco.bairro;
                fiscal.UserPessoa.Endereco.municipio = userPessoa.Endereco.municipio;
                fiscal.UserPessoa.Endereco.uf = userPessoa.Endereco.uf;
                fiscal.UserPessoa.dataNascimento = dataNascimento;
                fiscal.UserPessoa.PhoneNumber = userPessoa.PhoneNumber;
                fiscal.UserPessoa.dataNascimento = dataNascimento;
                fiscal.administrador = administrador;

                //ADICIONA PERMISSÕES DE ADMINISTRADOR SE FISCAL FOR ALTERADO PARA ADMNISTRADOR
                if (fiscal.administrador)
                {
                    if (db.Fiscal.Find(fiscal.Id).administrador == false)
                    {
                        //fiscal.UserPessoa.Discriminator.Add(db.Discriminator.Find(3));
                        await this.UserManager.AddToRoleAsync(fiscal.Id, "Administradores");
                    }
                }

                //REMOVE PERMISSÕES DE ADMINISTRADOR SE USUÁRIO FOR ALTERADO PARA APENAS FISCAL
                if (!fiscal.administrador)
                {
                    await this.UserManager.RemoveFromRoleAsync(fiscal.Id, "Administradores");
                    if (db.Fiscal.Find(fiscal.Id).administrador == true)
                    {
                        // fiscal.UserPessoa.Discriminator.Remove(db.Discriminator.Find(3));
                    }
                }

                db.Entry(fiscal.UserPessoa).State = EntityState.Modified;
                db.Entry(fiscal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fiscal);
        }

        // GET: Fiscais/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fiscal fiscal = db.Fiscal.Find(id);
            if (fiscal == null)
            {
                return HttpNotFound();
            }
            return View(fiscal);
        }

        // POST: Fiscais/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExcluirConfirmado(int id)
        {
            Fiscal fiscal = db.Fiscal.Find(id);
            //fiscal.UserPessoa.Discriminator.Remove(db.Discriminator.Find(3)); // DELETA AS PERMISSÕES DE FISCAL DO USUÁRIO
            await this.UserManager.RemoveFromRoleAsync(fiscal.Id, "Fiscais");

            if (fiscal.administrador == true) // DELETA AS PERMISSÕES DE ADMINISTRADOR DO USUÁRIO CASO ELE TENHA SIDO UM ADMINISTRADOR
            {
                await this.UserManager.RemoveFromRoleAsync(fiscal.Id, "Administradores");
                //fiscal.UserPessoa.Discriminator.Remove(db.Discriminator.Find(2));
            }
            fiscal.UserPessoa.RegistroAtividade.Last().fimAtividade = DateTime.Now;
            db.Entry(fiscal).State = EntityState.Modified;

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
