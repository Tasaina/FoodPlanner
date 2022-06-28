using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FoodPlanner.DataLayer;
using FoodPlanner.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FoodPlanner.Recommendation
{
    public class RecommendationViewModel : ReactiveObject
    {
        [Reactive] public Food Food { get; private set; }
        private Food _lastFood;
        [Reactive] public ObservableCollection<Tag> TopTags { get; set; }
        [Reactive] public ObservableCollection<Tag> TopMinorTags { get; set; }

        private readonly ITrackerService _trackerService;

        public RecommendationViewModel()
        {
            _trackerService = FoodServiceProvider.GetService<ITrackerService>();
        }

        public async Task Initialize()
        {
            await GenerateRecommendation(true);
            _lastFood = Food;
        }

        private async Task GenerateRecommendation(bool first)
        {
            var recommendationGenerator = new RecommendationGenerator(100, 50);
            var foods = await recommendationGenerator.Generate();
            Food = first ? foods.First() : foods.Random();
            TopTags = new ObservableCollection<Tag>(Food.Tags.Select(t => t.Tag).Where(t => t.IsMajor).Take(5));
            TopMinorTags = new ObservableCollection<Tag>(Food.Tags.Select(t => t.Tag).Where(t => !t.IsMajor).Take(5));
        }

        public async Task UpdateFood()
        {
            const int maxTries = 100;

            var tries = 0;
            _lastFood = Food;
            while (Food.Name == _lastFood.Name)
            {
                await GenerateRecommendation(false);
                if (tries++ > maxTries) break;
            }
        }
    }
}