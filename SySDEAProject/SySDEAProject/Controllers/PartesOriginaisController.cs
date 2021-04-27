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
    public class PartesOriginaisController : Controller
    {
        private SySDEAContext db = new SySDEAContext();

        // GET: PartesOriginais
        public ActionResult Index()
        {
            var parteOriginal = db.ParteOriginal.Include(p => p.VersaoOriginal);
            return View(parteOriginal.ToList());
        }

        // GET: PartesOriginais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParteOriginal parteOriginal = db.ParteOriginal.Find(id);
            if (parteOriginal == null)
            {
                return HttpNotFound();
            }
            return View(parteOriginal);
        }

        // GET: PartesOriginais/Create
        public ActionResult Create()
        {
            ViewBag.idVersaoOriginal = new SelectList(db.VersaoOriginal, "idVersaoOriginal", "idVersaoOriginal");
            return View();
        }

        // POST: PartesOriginais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idParteOriginal,idVersaoOriginal,numeroParte")] ParteOriginal parteOriginal)
        {
            if (ModelState.IsValid)
            {
                db.ParteOriginal.Add(parteOriginal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idVersaoOriginal = new SelectList(db.VersaoOriginal, "idVersaoOriginal", "idVersaoOriginal", parteOriginal.idVersaoOriginal);
            return View(parteOriginal);
        }

        // GET: PartesOriginais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParteOriginal parteOriginal = db.ParteOriginal.Find(id);
            if (parteOriginal == null)
            {
                return HttpNotFound();
            }
            ViewBag.idVersaoOriginal = new SelectList(db.VersaoOriginal, "idVersaoOriginal", "idVersaoOriginal", parteOriginal.idVersaoOriginal);
            return View(parteOriginal);
        }

        // POST: PartesOriginais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idParteOriginal,idVersaoOriginal,numeroParte")] ParteOriginal parteOriginal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parteOriginal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idVersaoOriginal = new SelectList(db.VersaoOriginal, "idVersaoOriginal", "idVersaoOriginal", parteOriginal.idVersaoOriginal);
            return View(parteOriginal);
        }

        // GET: PartesOriginais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParteOriginal parteOriginal = db.ParteOriginal.Find(id);
            if (parteOriginal == null)
            {
                return HttpNotFound();
            }
            return View(parteOriginal);
        }

        // POST: PartesOriginais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParteOriginal parteOriginal = db.ParteOriginal.Find(id);
            db.ParteOriginal.Remove(parteOriginal);
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
