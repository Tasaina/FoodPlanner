using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodPlanner.DataLayer;
using FoodPlanner.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FoodPlanner.Services
{
    public interface ITrackerService
    {
        Task<IEnumerable<FoodPlanEntry>> DecreaseEntryAmount(FoodPlanEntry entry);
        Task<IEnumerable<FoodPlanEntry>> IncreaseEntryAmount(FoodPlanEntry entry);
        FoodPlan GetPlan(DateTime date);
        FoodPlan GetNextPlan(FoodPlan plan);
        FoodPlan GetPreviousPlan(FoodPlan plan);
        Task<IEnumerable<FoodPlanEntry>> IncreaseEntryBaseAmount(FoodPlanEntry entry);
        Task<IEnumerable<FoodPlanEntry>> DecreaseEntryBaseAmount(FoodPlanEntry entry);
        Task NewEntry(FoodPlan plan);
        void DeleteEntry(FoodPlanEntry entry);
        Task EditEntryName(FoodPlanEntry entry, string name);
    }

    public class TrackerService : ITrackerService
    {
        private readonly FoodContext _context;
        private int _maxEntries=999;

        public TrackerService(FoodContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FoodPlanEntry>> DecreaseEntryAmount(FoodPlanEntry entry)
        {
            if (entry.AmountLeft > 0) entry.AmountLeft--;
            await _context.SaveChangesAsync();
            var relatedPlan = await _context.FoodPlans.FirstOrDefaultAsync(fp => fp.Entries.Contains(entry));
            return relatedPlan.Entries;
        }

        public async Task<IEnumerable<FoodPlanEntry>> IncreaseEntryAmount(FoodPlanEntry entry)
        {
            if (entry.AmountLeft < 999) entry.AmountLeft++;
            await _context.SaveChangesAsync();
            var relatedPlan = await _context.FoodPlans.FirstOrDefaultAsync(fp => fp.Entries.Contains(entry));
            return relatedPlan.Entries;
        }


        public FoodPlan GetPlan(DateTime date)
        {
            var startOfWeek = date.StartOfWeek();
            var plan = _context.FoodPlans.FirstOrDefault(fp => fp.Start == startOfWeek);
            if (plan != null) return plan;

            plan = new FoodPlan()
            {
                Start = startOfWeek,
                End = startOfWeek.AddDays(7)
            };

            var previousPlan = _context.FoodPlans.OrderBy(fp => fp.Start).LastOrDefault();
            if (previousPlan != null)
            {
                foreach (var entry in previousPlan.Entries)
                {
                    plan.Entries.Add(
                        new FoodPlanEntry()
                        {
                            Name = entry.Name,
                            BaseAmount = entry.BaseAmount,
                            AmountLeft = 0
                        }
                    );
                }
            }

            _context.FoodPlans.Add(plan);
            _context.SaveChanges();

            return plan;
        }

        public FoodPlan GetNextPlan(FoodPlan plan)
        {
            return _context.FoodPlans.OrderBy(fp => fp.Start).GetNext(plan);
        }

        public FoodPlan GetPreviousPlan(FoodPlan plan)
        {
            return _context.FoodPlans.OrderBy(fp => fp.Start).GetPrevious(plan);
        }

        public async Task<IEnumerable<FoodPlanEntry>> IncreaseEntryBaseAmount(FoodPlanEntry entry)
        {
            if (entry.BaseAmount < 999) entry.BaseAmount++;
            await _context.SaveChangesAsync();
            var relatedPlan = await _context.FoodPlans.FirstOrDefaultAsync(fp => fp.Entries.Contains(entry));
            return relatedPlan.Entries;
        }

        public async Task<IEnumerable<FoodPlanEntry>> DecreaseEntryBaseAmount(FoodPlanEntry entry)
        {
            if (entry.BaseAmount > 0) entry.BaseAmount--;
            await _context.SaveChangesAsync();
            var relatedPlan = await _context.FoodPlans.FirstOrDefaultAsync(fp => fp.Entries.Contains(entry));
            return relatedPlan.Entries;
        }

        public async Task NewEntry(FoodPlan plan)
        {
            if (plan.Entries.Count > _maxEntries) return;
            plan.Entries.Add(new FoodPlanEntry() {BaseAmount = 1, AmountLeft = 0, Name = "New Entry"});
            await _context.SaveChangesAsync();
        }

        public void DeleteEntry(FoodPlanEntry entry)
        {
            _context.FoodPlanEntries.Remove(entry);
            _context.SaveChanges();
        }

        public async Task EditEntryName(FoodPlanEntry entry, string name)
        {
            entry.Name = name;
            await _context.SaveChangesAsync();
        }
    }
}