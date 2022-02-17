using Microsoft.AspNetCore.Mvc;
using ParkyWebMVC.Models;
using ParkyWebMVC.Repository.IRepository;
using System.IO;
using System.Threading.Tasks;

namespace ParkyWebMVC.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;

        public NationalParksController(INationalParkRepository nationalParkRepository)
        {
            _nationalParkRepository = nationalParkRepository;
        }

        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark obj = new NationalPark();

            // for create
            if (id == null)
            {
                return View(obj);
            }

            // for update
            obj = await _nationalParkRepository.GetAsync(StaticDetails.NationalParkAPIPath, id.GetValueOrDefault());

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark obj)
        {
            if (ModelState.IsValid)
            {
                var file = HttpContext.Request.Form.Files;
                if (file.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = file[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }

                    obj.Picture = p1;
                }
                else
                {
                    var objFromDb = await _nationalParkRepository.GetAsync(StaticDetails.NationalParkAPIPath, obj.Id);
                    obj.Picture = objFromDb.Picture;
                }

                if (obj.Id == 0)
                {
                    await _nationalParkRepository.CreateAsync(StaticDetails.NationalParkAPIPath, obj);
                }
                else
                {
                    await _nationalParkRepository.UpdateAsync(StaticDetails.NationalParkAPIPath + obj.Id, obj);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new { data = await _nationalParkRepository.GetAllAsync(StaticDetails.NationalParkAPIPath) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _nationalParkRepository.DeleteAsync(StaticDetails.NationalParkAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = true, message = "Delete Not Successful" });
        }
    }
}
