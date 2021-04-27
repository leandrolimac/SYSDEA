using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SySDEAProject.Models;
using Microsoft.AspNet.Identity;

namespace SySDEAProject.Controllers
{
    [Authorize(Roles = "Admin,Fiscal,Entidade")]
    public class LocaisEntidadesController : Controller
    {
        private SySDEAContext db = new SySDEAContext();

        // GET: LocaisEntidades
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {                        
            var localEntidade = db.LocalEntidade.Include(l => l.Entidade);
            return View(localEntidade.ToList());

        }
        [Authorize(Roles = "Admin, Fiscal")]
        public ActionResult ListaFiliais(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var locaisEntidades = from l in db.LocalEntidade where l.idEntidade == id select l;
            var localEntidade = locaisEntidades.Include(l => l.Entidade);
            return View(localEntidade.ToList());
        }

        [Authorize(Roles = "Entidade")]
        public ActionResult Listar()
        {
            Entidade entidade = db.Entidade.Find(User.Identity.GetUserId<int>());
            var entidades = from e in db.Entidade where e.Id == entidade.Id && e.ativa == true select e.LocalEntidade;
            return View(entidades.ToList());
        }


        // GET: LocaisEntidades/Detalhes/5
        [Authorize(Roles = "Admin, Fiscal,Entidade")]
        public ActionResult Detalhes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocalEntidade localEntidade = db.LocalEntidade.Find(id);
            if (localEntidade == null)
            {
                return HttpNotFound();
            }
            return View(localEntidade);
        }

        // GET: LocaisEntidades/Create
        [Authorize(Roles = "Admin, Fiscal")]
        public ActionResult Criar()
        {
            
            return View();
        }

        // POST: LocaisEntidades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Fiscal")]
        public ActionResult Criar([Bind(Include = "idEntidade, tel1, tel2, emailContato, endereco.rua, endereco.numero, endereco.complemento, endereco.bairro, endereco.municipio, endereco.uf, endereco.numeroSalas, endereco.titulo")] LocalEntidade localEntidade, Endereco Endereco)
        {
            if (ModelState.IsValid)
            {
                localEntidade.Endereco = Endereco;
                localEntidade.idLocalEntidade = User.Identity.GetUserId<int>();
                localEntidade.Entidade = db.Entidade.Find(localEntidade.idLocalEntidade);
                localEntidade.suspensa = true;
                db.LocalEntidade.Add(localEntidade);
                db.SaveChanges();
                return RedirectToAction("Listar");
            }

            ViewBag.idEntidade = new SelectList(db.Entidade, "idEntidade", "nome", localEntidade.idEntidade);
            return View(localEntidade);
        }

        // GET: LocaisEntidades/Edit/5
        [Authorize(Roles = "Admin, Fiscal")]
        public ActionResult Alterar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocalEntidade localEntidade = db.LocalEntidade.Find(id);
            if (localEntidade == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEntidade = new SelectList(db.Entidade, "Id", "nome", localEntidade.idEntidade);
            return View(localEntidade);
        }

        // POST: LocaisEntidades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Fiscal")]
        public ActionResult Alterar([Bind(Include = "idLocalEntidade,idEntidade,tel1, tel2 ,emailContato,rua,numero,complemento,bairro,municipio,uf,numeroSalas,titulo,suspensa")] LocalEntidade localEntidade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(localEntidade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEntidade = new SelectList(db.Entidade, "idEntidade", "nome", localEntidade.idEntidade);
            return View(localEntidade);
        }

        // GET: LocaisEntidades/Delete/5
        public ActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocalEntidade localEntidade = db.LocalEntidade.Find(id);
            if (localEntidade == null)
            {
                return HttpNotFound();
            }
            return View(localEntidade);
        }

        // POST: LocaisEntidades/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LocalEntidade localEntidade = db.LocalEntidade.Find(id);
            //db.EnderecoLocalEntidade.Remove(localEntidade.EnderecoLocalEntidade);
            //db.LocalEntidade.Remove(localEntidade);
            localEntidade.ativa = false;
            db.Entry(localEntidade).State = EntityState.Modified;
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
