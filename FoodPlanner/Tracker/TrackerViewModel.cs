using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using FoodPlanner.DataLayer;
using FoodPlanner.Services;
using FoodPlanner.Services.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FoodPlanner.Tracker
{
    public class TrackerViewModel : ReactiveObject
    {
        private readonly ITrackerService _trackerService;

        private FoodPlan _plan;

        public FoodPlan Plan
        {
            get => _plan;
            set
            {
                _plan = value;
                PlanText = $"Week of {value.Start:d}";
            }
        }

        [Reactive] public ObservableCollection<FoodPlanEntry> Entries { get; set; }
        [Reactive] public string PlanText { get; set; }

        public TrackerViewModel()
        {
            _trackerService = FoodServiceProvider.GetService<ITrackerService>();
            Plan = _trackerService.GetPlan(DateTime.Now);
            Entries = new ObservableCollection<FoodPlanEntry>(Plan.Entries);
        }

        public async Task DecreaseEntry(FoodPlanEntry entry)
        {
            Entries = new ObservableCollection<FoodPlanEntry>(await _trackerService.DecreaseEntryAmount(entry));
        }

        public async Task IncreaseEntry(FoodPlanEntry entry)
        {
            Entries = new ObservableCollection<FoodPlanEntry>(await _trackerService.IncreaseEntryAmount(entry));
        }

        public void NextPlan()
        {
            Plan = _trackerService.GetNextPlan(Plan);
            Entries = new ObservableCollection<FoodPlanEntry>(Plan.Entries);
        }

        public void PreviousPlan()
        {
            Plan = _trackerService.GetPreviousPlan(Plan);
            Entries = new ObservableCollection<FoodPlanEntry>(Plan.Entries);
        }

        public void Refresh()
        {
            Plan = _trackerService.GetPlan(DateTime.Now);
            Entries = new ObservableCollection<FoodPlanEntry>(Plan.Entries);
        }
    }
}