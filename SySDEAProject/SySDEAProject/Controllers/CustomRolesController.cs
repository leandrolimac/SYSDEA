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
    [Authorize(Roles ="Admin")]
    public class CustomRolesController : Controller
    {
        private SySDEAContext db = new SySDEAContext();

        // GET: CustomRoles
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }

        // GET: CustomRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomRole customRole = db.Roles.Find(id);
            if (customRole == null)
            {
                return HttpNotFound();
            }
            return View(customRole);
        }

        // GET: CustomRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] CustomRole customRole)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(customRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customRole);
        }

        // GET: CustomRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomRole customRole = db.Roles.Find(id);
            if (customRole == null)
            {
                return HttpNotFound();
            }
            return View(customRole);
        }

        // POST: CustomRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] CustomRole customRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customRole);
        }

        // GET: CustomRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomRole customRole = db.Roles.Find(id);
            if (customRole == null)
            {
                return HttpNotFound();
            }
            return View(customRole);
        }

        // POST: CustomRoles/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomRole customRole = db.Roles.Find(id);
            db.Roles.Remove(customRole);
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
