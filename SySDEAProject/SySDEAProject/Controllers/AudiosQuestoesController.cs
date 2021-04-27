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
    public class AudiosQuestoesController : Controller
    {
        private SySDEAContext db = new SySDEAContext();

        // GET: AudiosQuestoes
        public ActionResult Index()
        {
            var audioQuestao = db.AudioQuestao.Include(a => a.LocutorXSotaque);
            return View(audioQuestao.ToList());
        }

        // GET: AudiosQuestoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudioQuestao audioQuestao = db.AudioQuestao.Find(id);
            if (audioQuestao == null)
            {
                return HttpNotFound();
            }
            return View(audioQuestao);
        }

        // GET: AudiosQuestoes/Create
        public ActionResult Create()
        {
            ViewBag.idLocutor = new SelectList(db.LocutorXSotaque, "idLocutor", "idLocutor");
            return View();
        }

        // POST: AudiosQuestoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPedacoQuestao,idQuestao,numeroOrdem,audio,dataGravacao,idLocutor,idSotaque,numeroInteracoes")] AudioQuestao audioQuestao)
        {
            if (ModelState.IsValid)
            {
                db.PedacoQuestao.Add(audioQuestao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idLocutor = new SelectList(db.LocutorXSotaque, "idLocutor", "idLocutor", audioQuestao.idLocutor);
            return View(audioQuestao);
        }

        // GET: AudiosQuestoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudioQuestao audioQuestao = db.AudioQuestao.Find(id);
            if (audioQuestao == null)
            {
                return HttpNotFound();
            }
            ViewBag.idLocutor = new SelectList(db.LocutorXSotaque, "idLocutor", "idLocutor", audioQuestao.idLocutor);
            return View(audioQuestao);
        }

        // POST: AudiosQuestoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPedacoQuestao,idQuestao,numeroOrdem,audio,dataGravacao,idLocutor,idSotaque,numeroInteracoes")] AudioQuestao audioQuestao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(audioQuestao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idLocutor = new SelectList(db.LocutorXSotaque, "idLocutor", "idLocutor", audioQuestao.idLocutor);
            return View(audioQuestao);
        }

        // GET: AudiosQuestoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudioQuestao audioQuestao = db.AudioQuestao.Find(id);
            if (audioQuestao == null)
            {
                return HttpNotFound();
            }
            return View(audioQuestao);
        }

        // POST: AudiosQuestoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AudioQuestao audioQuestao = db.AudioQuestao.Find(id);
            db.PedacoQuestao.Remove(audioQuestao);
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
