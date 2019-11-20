using SportsLabs.DAL;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SportsLabs.Models;

namespace SportsLabs.Controllers
{
    public class TeamController : Controller
    {
        // GET: Teams
        private TeamContext db = new TeamContext();
      
        public ActionResult Index(int? id, string searchTerm=null)
        {
            if (id != 1)
            {
                Session["Notification"] = string.Empty;
            }
            var model = from t in db.Teams
                        select t;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model = model.Where(x => x.Name.Contains(searchTerm.Trim())
                                       || x.Country.Contains(searchTerm.Trim()));
            }
            model = model.Where(x => x.Id > 0);
            return View(model);
        }

        // GET: Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Team model)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Teams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }


            var model = new Team
            {
                Id = team.Id,
                Name = team.Name,
                Country = team.Country,
                Eliminated = team.Eliminated,
                Countries = GetCountrieList()
            };
            return View(model);
        }

        // POST: Teams/Edit/5       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Team model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                Session["notification"] = $"{model.Name} was successfully updated!";
                return RedirectToAction("Index", new { id = 1 });
            }
            return View(model);
        }

        // GET: Teams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.Teams.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var model = db.Teams.Find(id);
            db.Teams.Remove(model);
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

        public IEnumerable<string> GetCountrieList()
        {
            var countries = from c in db.Teams
                            orderby c.Country
                            select c.Country;

            return countries.ToList().Distinct();
        }
    }
}