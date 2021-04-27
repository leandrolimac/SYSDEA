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
    public class PedacosQuestaoController : Controller
    {
        private SySDEAContext db = new SySDEAContext();

        // GET: PedacosQuestao
        public ActionResult Index()
        {
            return View(db.PedacoQuestao.ToList());
        }

        // GET: PedacosQuestao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedacoQuestao pedacoQuestao = db.PedacoQuestao.Find(id);
            if (pedacoQuestao == null)
            {
                return HttpNotFound();
            }
            return View(pedacoQuestao);
        }

        // GET: PedacosQuestao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PedacosQuestao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPedacoQuestao,idQuestao,numeroOrdem")] PedacoQuestao pedacoQuestao)
        {
            if (ModelState.IsValid)
            {
                db.PedacoQuestao.Add(pedacoQuestao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pedacoQuestao);
        }

        // GET: PedacosQuestao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedacoQuestao pedacoQuestao = db.PedacoQuestao.Find(id);
            if (pedacoQuestao == null)
            {
                return HttpNotFound();
            }
            return View(pedacoQuestao);
        }

        // POST: PedacosQuestao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPedacoQuestao,idQuestao,numeroOrdem")] PedacoQuestao pedacoQuestao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedacoQuestao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pedacoQuestao);
        }

        // GET: PedacosQuestao/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedacoQuestao pedacoQuestao = db.PedacoQuestao.Find(id);
            if (pedacoQuestao == null)
            {
                return HttpNotFound();
            }
            return View(pedacoQuestao);
        }

        // POST: PedacosQuestao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PedacoQuestao pedacoQuestao = db.PedacoQuestao.Find(id);
            db.PedacoQuestao.Remove(pedacoQuestao);
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
