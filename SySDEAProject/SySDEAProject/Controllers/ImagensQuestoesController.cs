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
    public class ImagensQuestoesController : Controller
    {
        private SySDEAContext db = new SySDEAContext();

        // GET: ImagensQuestoes
        public ActionResult Index()
        {
            return View(db.ImagemQuestao.ToList());
        }

        // GET: ImagensQuestoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImagemQuestao imagemQuestao = db.ImagemQuestao.Find(id);
            if (imagemQuestao == null)
            {
                return HttpNotFound();
            }
            return View(imagemQuestao);
        }

        // GET: ImagensQuestoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ImagensQuestoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPedacoQuestao,idQuestao,numeroOrdem,imagem")] ImagemQuestao imagemQuestao)
        {
            if (ModelState.IsValid)
            {
                db.PedacoQuestao.Add(imagemQuestao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(imagemQuestao);
        }

        // GET: ImagensQuestoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImagemQuestao imagemQuestao = db.ImagemQuestao.Find(id);
            if (imagemQuestao == null)
            {
                return HttpNotFound();
            }
            return View(imagemQuestao);
        }

        // POST: ImagensQuestoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPedacoQuestao,idQuestao,numeroOrdem,imagem")] ImagemQuestao imagemQuestao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(imagemQuestao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(imagemQuestao);
        }

        // GET: ImagensQuestoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImagemQuestao imagemQuestao = db.ImagemQuestao.Find(id);
            if (imagemQuestao == null)
            {
                return HttpNotFound();
            }
            return View(imagemQuestao);
        }

        // POST: ImagensQuestoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ImagemQuestao imagemQuestao = db.ImagemQuestao.Find(id);
            db.PedacoQuestao.Remove(imagemQuestao);
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
