using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SySDEAProject.Models;

namespace SySDEAProject.Controllers
{
    public class SotaquesController : Controller
    {
        private SySDEAContext db = new SySDEAContext();

        // GET: Sotaques
        public ActionResult Index()
        {
            return View(db.Sotaque.ToList());
        }

        // GET: Sotaques/Details/5
        public ActionResult Detalhes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sotaque sotaque = db.Sotaque.Find(id);
            if (sotaque == null)
            {
                return HttpNotFound();
            }
            return View(sotaque);
        }

        // GET: Sotaques/Create
        public ActionResult Criar()
        {
            return View();
        }

        // POST: Sotaques/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Criar([Bind(Include = "idSotaque,titulo,descricao")] Sotaque sotaque, string tipoA1)
        {
            if (ModelState.IsValid)
            {
                if(tipoA1 == "A1")
                {
                    sotaque.tipoA1 = true;
                }
                db.Sotaque.Add(sotaque);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sotaque);
        }

        // GET: Sotaques/Edit/5
        public ActionResult Alterar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sotaque sotaque = db.Sotaque.Find(id);
            if (sotaque == null)
            {
                return HttpNotFound();
            }
            return View(sotaque);
        }

        // POST: Sotaques/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alterar([Bind(Include = "idSotaque,titulo,descricao")] Sotaque sotaque, string tipoA1)
        {
            if (ModelState.IsValid)
            {
                if (tipoA1 == "A1")
                {
                    sotaque.tipoA1 = true;
                }
                db.Entry(sotaque).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sotaque);
        }

        // GET: Sotaques/Delete/5
        public ActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sotaque sotaque = db.Sotaque.Find(id);
            if (sotaque == null)
            {
                return HttpNotFound();
            }
            return View(sotaque);
        }

        // POST: Sotaques/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sotaque sotaque = db.Sotaque.Find(id);
            db.Sotaque.Remove(sotaque);
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
