using DDA.DAL;
using SSSFramework;
using SSSFramework.Repository;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DDA.Business.Repositories
{
    public class CampaignRepository : GenericRepository<DDAEntities>, ICampaignRepository
    {
        public CampaignRepository(IUnitOfWork<DDAEntities> unitOfWork)
            : base(unitOfWork)
        {
        }

        public List<Campaign> GetAll()
        {
            return _context.Campaigns.AsNoTracking().OrderBy(c => c.Name).ToList();
        }

        public Campaign GetById(int id)
        {
            return _context.Campaigns.FirstOrDefault(c => c.Id == id);
        }

        public bool NameExists(string name, int excludeId)
        {
            return _context.Campaigns.Any(c => c.Name == name && c.Id != excludeId);
        }

        public Dictionary<int, int> GetEntriesCounts()
        {
            return _context.CampaignEntries
                .GroupBy(e => e.CampaignId)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionary(x => x.Key, x => x.Count);
        }

        public void Add(Campaign campaign)
        {
            _context.Campaigns.Add(campaign);
            _context.SaveChanges();
        }

        public void Update(Campaign campaign)
        {
            _context.SaveChanges();
        }

        public int InsertEntries(List<CampaignEntry> entries)
        {
            var oldDetect = _context.Configuration.AutoDetectChangesEnabled;
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;
                _context.CampaignEntries.AddRange(entries);
                _context.SaveChanges(); // single implicit transaction: all or nothing
                return entries.Count;
            }
            finally
            {
                _context.Configuration.AutoDetectChangesEnabled = oldDetect;
            }
        }

        public List<KeyValuePair<int, int>> GetRemaining(int campaignId)
        {
            return _context.CampaignEntries
                .Where(e => e.CampaignId == campaignId && !e.IsWithdrawn && e.Chances > 0)
                .Select(e => new { e.Id, e.Chances })
                .ToList()
                .Select(x => new KeyValuePair<int, int>(x.Id, x.Chances))
                .ToList();
        }

        public bool TryWithdraw(int entryId)
        {
            var rows = _context.Database.ExecuteSqlCommand(
                "UPDATE dbo.CampaignEntries SET IsWithdrawn = 1, WithdrawnOn = GETDATE() WHERE Id = @p0 AND IsWithdrawn = 0",
                entryId);
            return rows == 1;
        }
    }
}