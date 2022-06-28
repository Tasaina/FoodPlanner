using System.Collections.Generic;
using System.Linq;

namespace FoodPlanner.DataLayer
{
    public class FoodSearchRequest
    {
        public static FoodSearchRequest Empty => new FoodSearchRequest()
        {
            ExcludeTerms = new string[] { },
            IncludeTerms = new string[] { },
            MealSize = (MealSize) (-1),
            MealType = (MealType) (-1),
            Temperature = (Temperature) (-1),
            FlavorProfile = (FlavorProfile) (-1),
            Page = 1
        };

        public int Page { get; set; }
        public MealType MealType { get; set; }
        public MealSize MealSize { get; set; }
        public FlavorProfile FlavorProfile { get; set; }
        public Temperature Temperature { get; set; }
        public string[] IncludeTerms { get; set; } = { };
        public string[] ExcludeTerms { get; set; } = { };


        public void MakeSearchTermsLowercase()
        {
            if (IncludeTerms.Length > 0) IncludeTerms = IncludeTerms.Select(t => t.ToLowerInvariant()).ToArray();
            if (ExcludeTerms.Length > 0) ExcludeTerms = ExcludeTerms.Select(t => t.ToLowerInvariant()).ToArray();
        }

        public void RemoveEmptySearchTerms()
        {
            var nonEmptyIncludeTerms = new List<string>();
            foreach (var term in IncludeTerms)
            {
                if (!string.IsNullOrWhiteSpace(term)) nonEmptyIncludeTerms.Add(term);
            }

            var nonEmptyExcludeTerms = new List<string>();
            foreach (var term in ExcludeTerms)
            {
                if (!string.IsNullOrWhiteSpace(term)) nonEmptyExcludeTerms.Add(term);
            }

            IncludeTerms = nonEmptyIncludeTerms.ToArray();
            ExcludeTerms = nonEmptyExcludeTerms.ToArray();
        }
    }
}