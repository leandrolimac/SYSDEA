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
    public class TextosQuestoesController : Controller
    {
        private SySDEAContext db = new SySDEAContext();

        // GET: TextosQuestoes
        public ActionResult Index()
        {
            return View(db.PedacoQuestao.ToList());
        }

        // GET: TextosQuestoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextoQuestao textoQuestao = db.TextoQuestao.Find(id);
            if (textoQuestao == null)
            {
                return HttpNotFound();
            }
            return View(textoQuestao);
        }

        // GET: TextosQuestoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TextosQuestoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPedacoQuestao,idQuestao,numeroOrdem,texto,tipoTexto")] TextoQuestao textoQuestao)
        {
            if (ModelState.IsValid)
            {
                db.PedacoQuestao.Add(textoQuestao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(textoQuestao);
        }

        // GET: TextosQuestoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextoQuestao textoQuestao = db.TextoQuestao.Find(id);
            if (textoQuestao == null)
            {
                return HttpNotFound();
            }
            return View(textoQuestao);
        }

        // POST: TextosQuestoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPedacoQuestao,idQuestao,numeroOrdem,texto,tipoTexto")] TextoQuestao textoQuestao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(textoQuestao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(textoQuestao);
        }

        // GET: TextosQuestoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextoQuestao textoQuestao = db.TextoQuestao.Find(id);
            if (textoQuestao == null)
            {
                return HttpNotFound();
            }
            return View(textoQuestao);
        }

        // POST: TextosQuestoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TextoQuestao textoQuestao = db.TextoQuestao.Find(id);
            db.PedacoQuestao.Remove(textoQuestao);
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
