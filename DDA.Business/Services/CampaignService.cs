using DDA.Business.Interfaces;
using DDA.Business.Repositories;
using DDA.DAL;
using DDA.ViewModels.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;

namespace DDA.Business.Services
{
    public class CampaignService : ICampaignService
    {
        private const int BalancePerChance = 1000;
        private const int WheelSegments = 24;

        private readonly ICampaignRepository _repo;

        public CampaignService(ICampaignRepository repo)
        {
            _repo = repo;
        }

        public List<CampaignViewModel> GetAll()
        {
            var counts = _repo.GetEntriesCounts();
            return _repo.GetAll().Select(c => new CampaignViewModel
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive,
                EntriesCount = counts.ContainsKey(c.Id) ? counts[c.Id] : 0
            }).ToList();
        }

        public CampaignViewModel GetById(int id)
        {
            var c = _repo.GetById(id);
            if (c == null) return null;
            return new CampaignViewModel { Id = c.Id, Name = c.Name, IsActive = c.IsActive };
        }

        public List<SelectListItem> GetActiveCampaignsLookup()
        {
            return _repo.GetAll()
                .Where(c => c.IsActive)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();
        }

        public string Save(CampaignViewModel model, int userId)
        {
            var name = (model.Name ?? string.Empty).Trim();
            if (name.Length == 0) return "Campaign_NameRequired";
            if (_repo.NameExists(name, model.Id)) return "Campaign_NameExists";

            if (model.Id == 0)
            {
                _repo.Add(new Campaign
                {
                    Name = name,
                    IsActive = true,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now
                });
            }
            else
            {
                var c = _repo.GetById(model.Id);
                if (c == null) return "Campaign_NotFound";
                c.Name = name;
                c.ModifyBy = userId;
                c.ModifyOn = DateTime.Now;
                _repo.Update(c);
            }
            return null;
        }

        public ImportResultViewModel ImportEntries(int campaignId, List<CampaignExcelRow> rows, int skipped, int userId)
        {
            var campaign = _repo.GetById(campaignId);
            if (campaign == null)
                return new ImportResultViewModel { Success = false, MessageKey = "Campaign_NotFound" };
            if (rows == null || rows.Count == 0)
                return new ImportResultViewModel { Success = false, MessageKey = "Upload_NoValidRows", CampaignName = campaign.Name, SkippedCount = skipped };

            var now = DateTime.Now;
            var entries = new List<CampaignEntry>();

            foreach (var r in rows)
            {
                var chances = (int)Math.Floor(r.Balance / BalancePerChance); // 3601 => 3, 800 => 0
                for (var i = 0; i < chances; i++)
                {
                    entries.Add(new CampaignEntry
                    {
                        CampaignId = campaignId,
                        CIF = r.Cif,
                        Balance = r.Balance,
                        IsWithdrawn = false,
                        CreatedBy = userId,
                        CreatedOn = now
                    });
                }
            }

            if (entries.Count == 0)
                return new ImportResultViewModel { Success = false, MessageKey = "Upload_NoValidRows", CampaignName = campaign.Name, SkippedCount = skipped };

            var inserted = _repo.InsertEntries(entries);
            return new ImportResultViewModel
            {
                Success = true,
                CampaignName = campaign.Name,
                InsertedCount = inserted,
                SkippedCount = skipped
            };
        }

        public WheelStateViewModel GetWheelState(int campaignId)
        {
            var remaining = _repo.GetRemaining(campaignId);
            return new WheelStateViewModel
            {
                Success = true,
                RemainingCount = remaining.Count,
                WheelIds = Sample(remaining, WheelSegments)
            };
        }

        public WheelStateViewModel Spin(int campaignId)
        {
            for (var attempt = 0; attempt < 5; attempt++)
            {
                var remaining = _repo.GetRemaining(campaignId);
                if (remaining.Count == 0)
                    return new WheelStateViewModel { Success = false, Message = "Spin_AllWithdrawn", RemainingCount = 0, WheelIds = new List<int>() };

                var winnerId = remaining[(int)NextLong(remaining.Count)];   // every ID equally likely
                if (!_repo.TryWithdraw(winnerId)) continue;

                var others = remaining.Where(id => id != winnerId).ToList();
                var wheel = Sample(others, WheelSegments - 1);
                wheel.Add(winnerId);
                Shuffle(wheel);

                return new WheelStateViewModel
                {
                    Success = true,
                    WinnerId = winnerId,
                    RemainingCount = remaining.Count - 1,
                    WheelIds = wheel
                };
            }
            return new WheelStateViewModel { Success = false, Message = "Spin_Failed" };
        }

        private static List<int> Sample(List<int> ids, int count)
        {
            var copy = new List<int>(ids);
            Shuffle(copy);
            return copy.Take(count).ToList();
        }

        private static void Shuffle<T>(IList<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = (int)NextLong(i + 1);
                var tmp = list[i]; list[i] = list[j]; list[j] = tmp;
            }
        }

        // Uniform random in [0, max) using the crypto RNG (rejection sampling, no modulo bias).
        private static long NextLong(long max)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var buf = new byte[8];
                var limit = ulong.MaxValue - (ulong.MaxValue % (ulong)max);
                ulong v;
                do
                {
                    rng.GetBytes(buf);
                    v = BitConverter.ToUInt64(buf, 0);
                } while (v >= limit);
                return (long)(v % (ulong)max);
            }
        }
    }
}