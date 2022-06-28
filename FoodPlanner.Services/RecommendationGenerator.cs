using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodPlanner.DataLayer;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace FoodPlanner.Services
{
    public class RecommendationGenerator
    {
        private readonly int _maxRuns;
        private readonly int _strictRuns;
        private int _breakfastThreshold = 12;
        private int _lunchThreshold = 16;
        private int _dinnerThreshold = 23;
        private int _snackThreshold = 5;
        private List<FoodPlanEntry> _triedEntries;
        private readonly ITrackerService _trackerService;
        private IFoodService _foodService;
        public FoodSearchRequest RecommendedSearchRequest { get; private set; }
        private List<ScoredFood> PossibleFoods;

        private List<Food> _defaultFood = new List<Food>()
        {
            new Food()
            {
                Name = "Couldn't find a recommendation :("
            }
        };

        private readonly List<FoodPlanEntry> _entries;
        private FoodPlanEntry _entryToUse;

        public RecommendationGenerator(int maxRuns, int strictRuns)
        {
            RecommendedSearchRequest = FoodSearchRequest.Empty;
            _trackerService = FoodServiceProvider.GetService<ITrackerService>();
            _foodService = FoodServiceProvider.GetService<IFoodService>();
            _entries = _trackerService.GetPlan(DateTime.Now).Entries;
            _maxRuns = maxRuns;
            _strictRuns = strictRuns;
        }

        public async Task<IEnumerable<Food>> Generate()
        {
            PossibleFoods = new List<ScoredFood>();
            _triedEntries = new List<FoodPlanEntry>();
            for (var i = 0; i < _maxRuns; i++)
            {
                CalculateRecommendation(i < _strictRuns);
                var result = await _foodService.GetRandom(RecommendedSearchRequest);
                if (result != null) PossibleFoods.Add(ScoreFood(result, RecommendedSearchRequest));
            }


            if (PossibleFoods.Count == 0) return _defaultFood;
            return GenerateFoodsAdjustedForScore();
        }

        private IEnumerable<Food> GenerateFoodsAdjustedForScore()
        {
            var topFoods = PossibleFoods.RemoveDuplicates().OrderByDescending(pf => pf.Score).Take(25);
            var scoreAdjustedList = new List<Food>();

            foreach (var food in topFoods)
                for (var i = 0; i < food.Score / 10; i++)
                    scoreAdjustedList.Add(food.Food);

            return scoreAdjustedList;
        }

        private ScoredFood ScoreFood(Food food, FoodSearchRequest recommendedSearchRequest)
        {
            var scorer = new FoodScorer(food, recommendedSearchRequest, _entryToUse);
            return new ScoredFood() {Score = scorer.Score(), Food = food};
        }

        private void CalculateRecommendation(bool strict)
        {
            CalculateMealType(strict);
            CalculateTags();
        }

        private void CalculateTags()
        {
            _entryToUse = new FoodPlanEntry() {BaseAmount = 0, AmountLeft = int.MinValue};
            if (_entries.Count == _triedEntries.Count) _triedEntries.Clear();
            foreach (var entry in _entries.Where(e => !_triedEntries.Exists(te => te == e)))
            {
                if (_entryToUse.AmountDifference < entry.AmountDifference) _entryToUse = entry;
            }

            if (_entryToUse.Name != null) RecommendedSearchRequest.IncludeTerms = new[] {_entryToUse.Name};

            _triedEntries.Add(_entryToUse);
        }

        private void CalculateMealType(bool strict)
        {
            if (!strict)
            {
                RecommendedSearchRequest.MealType = (MealType) (-1);
                return;
            }

            RecommendedSearchRequest.MealType = MealType.Snack;

            if (DateTime.Now.Hour > _snackThreshold && DateTime.Now.Hour < _breakfastThreshold)
                RecommendedSearchRequest.MealType = MealType.Breakfast;
            else if (DateTime.Now.Hour > _breakfastThreshold && DateTime.Now.Hour < _lunchThreshold)
                RecommendedSearchRequest.MealType = MealType.Lunch;
            else if (DateTime.Now.Hour >= _lunchThreshold && DateTime.Now.Hour < _dinnerThreshold)
                RecommendedSearchRequest.MealType = MealType.Dinner;
        }
    }
}