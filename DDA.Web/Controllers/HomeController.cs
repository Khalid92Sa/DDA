using DDA.Business.Interfaces;
using DDA.Resources;
using DDA.ViewModels.Campaigns;
using DDA.Web.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;

namespace DDA.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ICampaignService _campaignService;

        public HomeController(IUserService userService, ICampaignService campaignService)
        {
            _userService = userService;
            _campaignService = campaignService;
        }

        // ---------- Upload tab ----------

        public ActionResult Index()
        {
            ViewBag.Title = Resource.Upload;

            var model = new UploadPageViewModel
            {
                Campaigns = _campaignService.GetActiveCampaignsLookup(),
                HasResult = TempData["UploadMessage"] != null,
                ResultSuccess = TempData["UploadSuccess"] != null && (bool)TempData["UploadSuccess"],
                ResultMessage = TempData["UploadMessage"] as string
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadExcel(int? campaignId, HttpPostedFileBase file)
        {
            if (!campaignId.HasValue)
                return Fail(Resource.Upload_SelectCampaign);

            if (file == null || file.ContentLength == 0)
                return Fail(Resource.Upload_ChooseFile);

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return Fail(Resource.Upload_OnlyXlsx);

            try
            {
                int skipped;
                var rows = ExcelCampaignReader.Read(file.InputStream, out skipped);
                var result = _campaignService.ImportEntries(campaignId.Value, rows, skipped, CurrentUserId());

                if (!result.Success)
                    return Fail(ResourceText.Get(result.MessageKey));

                var message = string.Format(Resource.Upload_SuccessMessage, result.CampaignName, result.InsertedCount);
                if (result.SkippedCount > 0)
                    message += " " + string.Format(Resource.Upload_SkippedMessage, result.SkippedCount);

                TempData["UploadSuccess"] = true;
                TempData["UploadMessage"] = message;
            }
            catch (Exception ex)
            {
                return Fail(string.Format(Resource.Upload_ReadError, ex.Message));
            }

            return RedirectToAction("Index");
        }

        // ---------- Spin tab ----------

        public ActionResult Spin()
        {
            ViewBag.Title = Resource.Spin;
            return View();
        }

        [HttpGet]
        public JsonResult Campaigns()
        {
            var list = _campaignService.GetActiveCampaignsLookup()
                .Select(c => new { Value = c.Value, Text = c.Text });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult WheelState(int campaignId)
        {
            return Json(_campaignService.GetWheelState(campaignId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DoSpin(int campaignId)
        {
            var result = _campaignService.Spin(campaignId);
            if (!result.Success) result.Message = ResourceText.Get(result.Message);
            return Json(result);
        }

        // ---------- helpers ----------

        private ActionResult Fail(string message)
        {
            TempData["UploadSuccess"] = false;
            TempData["UploadMessage"] = message;
            return RedirectToAction("Index");
        }

        private int CurrentUserId()
        {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = _userService.GetUserByUsername(User.Identity.Name.Split(',')[0]);
                if (user != null) return user.Id;
            }
            return 0;
        }
    }
}