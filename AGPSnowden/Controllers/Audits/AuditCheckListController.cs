using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AGPSnowden.Controllers.Audits
{
    public class AuditCheckListController : Controller
    {
        // GET: AuditCheckListController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AuditCheckListController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuditCheckListController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuditCheckListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuditCheckListController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuditCheckListController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuditCheckListController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuditCheckListController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
