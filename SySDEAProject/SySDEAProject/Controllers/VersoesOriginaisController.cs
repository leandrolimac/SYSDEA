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
    public class VersoesOriginaisController : Controller
    {
        private SySDEAContext db = new SySDEAContext();

        // GET: VersoesOriginais
        public ActionResult Index()
        {
            return View(db.VersaoOriginal.ToList());
        }

        // GET: VersoesOriginais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VersaoOriginal versaoOriginal = db.VersaoOriginal.Find(id);
            if (versaoOriginal == null)
            {
                return HttpNotFound();
            }
            return View(versaoOriginal);
        }

        // GET: VersoesOriginais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VersoesOriginais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idVersaoOriginal,dataCadastro,numero")] VersaoOriginal versaoOriginal)
        {
            if (ModelState.IsValid)
            {
                db.VersaoOriginal.Add(versaoOriginal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(versaoOriginal);
        }

        // GET: VersoesOriginais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VersaoOriginal versaoOriginal = db.VersaoOriginal.Find(id);
            if (versaoOriginal == null)
            {
                return HttpNotFound();
            }
            return View(versaoOriginal);
        }

        // POST: VersoesOriginais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idVersaoOriginal,dataCadastro,numero")] VersaoOriginal versaoOriginal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(versaoOriginal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(versaoOriginal);
        }

        // GET: VersoesOriginais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VersaoOriginal versaoOriginal = db.VersaoOriginal.Find(id);
            if (versaoOriginal == null)
            {
                return HttpNotFound();
            }
            return View(versaoOriginal);
        }

        // POST: VersoesOriginais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VersaoOriginal versaoOriginal = db.VersaoOriginal.Find(id);
            db.VersaoOriginal.Remove(versaoOriginal);
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
