using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using FoodPlanner.DataLayer;

namespace FoodPlanner.Services
{
    public class FoodScorer
    {
        private readonly Bonus _matchingMealTypeBonus = new() {Value = 1.2, IsAdditive = false};
        private readonly Bonus _matchingTagBonus = new() {Value = 50, IsAdditive = true};
        private readonly Bonus _matchingNameBonus = new() {Value = 30, IsAdditive = true};
        private readonly Bonus _amountDifferenceBonus = new() {Value = 80, IsAdditive = true};
        private readonly Bonus _amountHigherThanMaxBonus = new() {Value = -10, IsAdditive = true};
        private readonly Bonus _amountIs0Bonus = new() {Value = 40, IsAdditive = true};
        private readonly Food _food;
        private readonly FoodSearchRequest _recommendedSearchRequest;
        private readonly FoodPlanEntry _entry;

        public FoodScorer(Food food, FoodSearchRequest recommendedSearchRequest, FoodPlanEntry entry)
        {
            _food = food;
            _recommendedSearchRequest = recommendedSearchRequest;
            _recommendedSearchRequest.MakeSearchTermsLowercase();
            _entry = entry;
        }

        public int Score()
        {
            var score = 0.0;
            var searchTerm = _recommendedSearchRequest.IncludeTerms.First();
            if (_entry.AmountDifference < 0)
            {
                for (var i = 0; i < (-1) * _entry.AmountDifference; i++)
                {
                    score = _amountHigherThanMaxBonus.Apply(score);
                }
            }

            if (_food.Name.ToLowerInvariant().Contains(searchTerm))
                score = _matchingNameBonus.Apply(score);
            if (_entry.AmountLeft == 0)
                score = _amountIs0Bonus.Apply(score);

            foreach (var tag in _food.Tags)
            {
                if (tag.Tag.Text.ToLowerInvariant().Contains(searchTerm))
                    score = _matchingTagBonus.Apply(score);
            }

            if (_recommendedSearchRequest.MealType == _food.Type)
                score = (int) _matchingMealTypeBonus.Apply(score);

            for (var i = 0; i < _entry.AmountDifference; i++)
            {
                score = _amountDifferenceBonus.Apply(score);
            }


            return (int) score;
        }
    }

    internal class Bonus
    {
        public double Value { get; init; }
        public bool IsAdditive { get; init; }

        public double Apply(double number)
        {
            return (IsAdditive) ? number + Value : number * Value;
        }
    }
}