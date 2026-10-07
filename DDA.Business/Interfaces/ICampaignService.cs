using DDA.ViewModels.Campaigns;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DDA.Business.Interfaces
{
    public interface ICampaignService
    {
        List<CampaignViewModel> GetAll();
        CampaignViewModel GetById(int id);

        /// <summary>Active campaigns ready to bind to a dropdown.</summary>
        List<SelectListItem> GetActiveCampaignsLookup();

        /// <summary>Creates (Id = 0) or updates a campaign. Returns a resource key on error, or null on success.</summary>
        string Save(CampaignViewModel model, int userId);

        ImportResultViewModel ImportEntries(int campaignId, List<CampaignExcelRow> rows, int skipped, int userId);

        WheelStateViewModel GetWheelState(int campaignId);
        WheelStateViewModel Spin(int campaignId);
    }
}