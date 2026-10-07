using System.Collections.Generic;
using System.Web.Mvc;

namespace DDA.ViewModels.Campaigns
{
    public class CampaignViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int EntriesCount { get; set; }
    }

    /// <summary>Model of the Upload page (Home/Index).</summary>
    public class UploadPageViewModel
    {
        public int? CampaignId { get; set; }
        public IEnumerable<SelectListItem> Campaigns { get; set; }

        // Result of the last upload (shown in the popup)
        public bool HasResult { get; set; }
        public bool ResultSuccess { get; set; }
        public string ResultMessage { get; set; }
    }

    /// <summary>Model of the Spin page (Home/Spin).</summary>
    public class SpinPageViewModel
    {
        public int? CampaignId { get; set; }
        public IEnumerable<SelectListItem> Campaigns { get; set; }
    }

    public class CampaignExcelRow
    {
        public string Cif { get; set; }
        public decimal Balance { get; set; }
    }

    public class ImportResultViewModel
    {
        public bool Success { get; set; }
        /// <summary>Resource key of the error (translated by the web layer).</summary>
        public string MessageKey { get; set; }
        public string CampaignName { get; set; }
        public int InsertedCount { get; set; }
        public int SkippedCount { get; set; }
    }

    public class WheelStateViewModel
    {
        public bool Success { get; set; }
        /// <summary>Service returns a resource key; the controller replaces it with the translated text.</summary>
        public string Message { get; set; }
        public int? WinnerId { get; set; }
        public int RemainingCount { get; set; }
        public List<int> WheelIds { get; set; }
    }
}