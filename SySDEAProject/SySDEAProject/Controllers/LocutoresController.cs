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
    public class LocutoresController : Controller
    {
        private SySDEAContext db = new SySDEAContext();

        // GET: Locutors
        public ActionResult Index()
        {
            return View(db.Locutor.ToList());
        }

        // GET: Locutors/Details/5
        public ActionResult Detalhes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Locutor locutor = db.Locutor.Find(id);
            if (locutor == null)
            {
                return HttpNotFound();
            }
            return View(locutor);
        }

        // GET: Locutors/Create
        public ActionResult Criar()
        {
            return View();
        }

        // POST: Locutors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Criar([Bind(Include = "idLocutor,nome,ocupacao,email,tel1,tel2")] Locutor locutor)
        {
            if (ModelState.IsValid)
            {
                db.Locutor.Add(locutor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(locutor);
        }

        // GET: Locutors/Edit/5
        public ActionResult Alterar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Locutor locutor = db.Locutor.Find(id);
            if (locutor == null)
            {
                return HttpNotFound();
            }
            return View(locutor);
        }

        // POST: Locutors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alterar([Bind(Include = "idLocutor,nome,ocupacao,email,tel1,tel2")] Locutor locutor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(locutor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(locutor);
        }

        // GET: Locutors/Delete/5
        public ActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Locutor locutor = db.Locutor.Find(id);
            if (locutor == null)
            {
                return HttpNotFound();
            }
            return View(locutor);
        }

        // POST: Locutors/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Locutor locutor = db.Locutor.Find(id);
            db.Locutor.Remove(locutor);
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
