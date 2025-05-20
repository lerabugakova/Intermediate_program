using ProfileSample.DAL;
using ProfileSample.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            using (var context = new ProfileSampleEntities())
            {
                var model = await context.ImgSources
                                         .Take(20)
                                         .Select(x => new ImageModel
                                         {
                                             Name = x.Name,
                                             Data = x.Data
                                         })
                                         .ToListAsync();

                return View(model);
            }
        }

        public async Task<ActionResult> Convert()
        {
            var imageDir = Server.MapPath("~/Content/Img");

            if (!Directory.Exists(imageDir))
                return RedirectToAction("Index");

            var files = Directory.GetFiles(imageDir, "*.jpg");

            using (var context = new ProfileSampleEntities())
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    byte[] fileData;

                    using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (var memory = new MemoryStream())
                    {
                        await stream.CopyToAsync(memory).ConfigureAwait(false);
                        fileData = memory.ToArray();
                    }

                    var entity = new ImgSource
                    {
                        Name = fileName,
                        Data = fileData
                    };

                    context.ImgSources.Add(entity);
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
