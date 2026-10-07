using DDA.Business.Interfaces;
using DDA.Resources;
using DDA.ViewModels.Campaigns;
using DDA.Web.Helpers;
using System.Resources;
using System.Web.Mvc;

namespace DDA.Web.Controllers
{
    public class CampaignsController : BaseController
    {
        private readonly ICampaignService _campaignService;
        private readonly IUserService _userService;

        public CampaignsController(ICampaignService campaignService, IUserService userService)
        {
            _campaignService = campaignService;
            _userService = userService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = Resource.Campaigns;
            return View(_campaignService.GetAll());
        }

        public ActionResult Add()
        {
            ViewBag.Title = Resource.Campaign_Add;
            return View("Edit", new CampaignViewModel { IsActive = true });
        }

        public ActionResult Edit(int id)
        {
            var model = _campaignService.GetById(id);
            if (model == null) return HttpNotFound();
            ViewBag.Title = Resource.Campaign_Edit;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CampaignViewModel model)
        {
            ViewBag.Title = model.Id == 0 ? Resource.Campaign_Add : Resource.Campaign_Edit;

            var errorKey = _campaignService.Save(model, CurrentUserId());
            if (errorKey != null)
            {
                ModelState.AddModelError("Name", ResourceText.Get(errorKey));
                return View("Edit", model);
            }

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