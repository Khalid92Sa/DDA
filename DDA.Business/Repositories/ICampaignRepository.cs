using DDA.DAL;
using SSSFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDA.Business.Repositories
{
    public interface ICampaignRepository : IGenericRepository
    {
        List<Campaign> GetAll();
        Campaign GetById(int id);
        bool NameExists(string name, int excludeId);
        Dictionary<int, int> GetEntriesCounts();
        void Add(Campaign campaign);
        void Update(Campaign campaign);
        int InsertEntries(List<CampaignEntry> entries);

        /// <summary>Ids + chances of entries that are still not withdrawn (Chances &gt; 0).</summary>
        List<int> GetRemaining(int campaignId);   // ids of entries not yet withdrawn

        /// <summary>Atomically marks the entry as withdrawn. Returns false if it was already withdrawn.</summary>
        bool TryWithdraw(int entryId);
    }
}
