using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using FoodPlanner.DataLayer;
using FoodPlanner.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FoodPlanner.Tracker.Settings
{
    public class TrackerSettingsViewModel : ReactiveObject
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

        public TrackerSettingsViewModel()
        {
            _trackerService = FoodServiceProvider.GetService<ITrackerService>();
        }

        public void InitializeValues()
        {
            Plan = _trackerService.GetPlan(DateTime.Now);
            Entries = new ObservableCollection<FoodPlanEntry>(Plan.Entries);
        }

        public async Task DecreaseEntry(FoodPlanEntry entry)
        {
            Entries = new ObservableCollection<FoodPlanEntry>(await _trackerService.DecreaseEntryBaseAmount(entry));
        }

        public async Task IncreaseEntry(FoodPlanEntry entry)
        {
            Entries = new ObservableCollection<FoodPlanEntry>(await _trackerService.IncreaseEntryBaseAmount(entry));
        }

        public void NewEntry()
        {
            _trackerService.NewEntry(Plan);
            Entries = new ObservableCollection<FoodPlanEntry>(Plan.Entries);
        }

        public void Delete(FoodPlanEntry entry)
        {
            _trackerService.DeleteEntry(entry);
            Plan.Entries.Remove(entry);
            Entries = new ObservableCollection<FoodPlanEntry>(Plan.Entries);
        }

        public async Task Edit(FoodPlanEntry entry)
        {
            var input = new InputBox();
            input.ShowDialog();
            if (input.Canceled) return;
            await _trackerService.EditEntryName(entry, input.Text);
            Plan = _trackerService.GetPlan(DateTime.Now);
            Entries = new ObservableCollection<FoodPlanEntry>(Plan.Entries);
        }
    }
}