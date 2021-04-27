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
    public class QuestoesController : Controller
    {
        private SySDEAContext db = new SySDEAContext();

        // GET: Questoes
        public ActionResult Index()
        {
            var questao = db.Questao.Include(q => q.ParteOriginal).Include(q => q.Tema);
            return View(questao.ToList());
        }

        // GET: Questoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questao questao = db.Questao.Find(id);
            if (questao == null)
            {
                return HttpNotFound();
            }
            return View(questao);
        }

        // GET: Questoes/Create
        public ActionResult Create()
        {
            ViewBag.idParteOriginal = new SelectList(db.ParteOriginal, "idParteOriginal", "idParteOriginal");
            ViewBag.idTema = new SelectList(db.Tema, "idTema", "titulo");
            return View();
        }

        // POST: Questoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idQuestao,idTema,idParteOriginal,dificuldade,peso,suspensa,vezesEscolhida,conteudo,diretriz,numeroParte")] Questao questao)
        {
            if (ModelState.IsValid)
            {
                db.Questao.Add(questao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idParteOriginal = new SelectList(db.ParteOriginal, "idParteOriginal", "idParteOriginal", questao.idParteOriginal);
            ViewBag.idTema = new SelectList(db.Tema, "idTema", "titulo", questao.idTema);
            return View(questao);
        }

        // GET: Questoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questao questao = db.Questao.Find(id);
            if (questao == null)
            {
                return HttpNotFound();
            }
            ViewBag.idParteOriginal = new SelectList(db.ParteOriginal, "idParteOriginal", "idParteOriginal", questao.idParteOriginal);
            ViewBag.idTema = new SelectList(db.Tema, "idTema", "titulo", questao.idTema);
            return View(questao);
        }

        // POST: Questoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idQuestao,idTema,idParteOriginal,dificuldade,peso,suspensa,vezesEscolhida,conteudo,diretriz,numeroParte")] Questao questao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idParteOriginal = new SelectList(db.ParteOriginal, "idParteOriginal", "idParteOriginal", questao.idParteOriginal);
            ViewBag.idTema = new SelectList(db.Tema, "idTema", "titulo", questao.idTema);
            return View(questao);
        }

        // GET: Questoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questao questao = db.Questao.Find(id);
            if (questao == null)
            {
                return HttpNotFound();
            }
            return View(questao);
        }

        // POST: Questoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Questao questao = db.Questao.Find(id);
            db.Questao.Remove(questao);
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
