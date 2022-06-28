using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodPlanner.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace FoodPlanner.Services
{
    public class FoodSearchQuery
    {
        private readonly FoodSearchRequest _foodSearchRequest;
        private IQueryable<Food> Query { get; set; }
        private IEnumerable<Food> Results { get; set; }
        private int PageSize;

        public FoodSearchQuery(FoodSearchRequest foodSearchRequest, int pageSize)
        {
            _foodSearchRequest = foodSearchRequest;
            PageSize = pageSize;
            foodSearchRequest.MakeSearchTermsLowercase();
        }

        public async Task<IEnumerable<Food>> Build(DbSet<Food> contextFoods)
        {
            Query = contextFoods.AsQueryable();
            FilterMealType();
            FilterTemperature();
            FilterFlavor();
            FilterSize();

            Results = await Query.ToListAsync();
            IncludeTerms();
            ExcludeTerms();
            GetPage();

            return Results.OrderBy(f => f.Name);
        }

        private void GetPage()
        {
            Results = Results.Skip(PageSize * (_foodSearchRequest.Page - 1)).Take(PageSize);
        }

        private void IncludeTerms()
        {
            if (_foodSearchRequest.IncludeTerms.Length == 0) return;
            Results = Results.Where(f =>
                _foodSearchRequest.IncludeTerms.All(
                    term => f.Name.ToLowerInvariant().Contains(term) ||
                            f.Tags.Select(t => t.Tag.Text.ToLowerInvariant()).Contains(term)));
        }

        private void ExcludeTerms()
        {
            if (_foodSearchRequest.ExcludeTerms.Length == 0) return;
                  Results = Results.Where(f =>
                !_foodSearchRequest.ExcludeTerms.All(
                    term => f.Name.ToLowerInvariant().Contains(term) ||
                            f.Tags.Select(t => t.Tag.Text.ToLowerInvariant()).Contains(term)));
        }

        private void FilterSize()
        {
            if (!Enum.IsDefined(_foodSearchRequest.MealSize)) return;
            Query = Query.Where(f => f.MealSize == _foodSearchRequest.MealSize);
        }

        private void FilterFlavor()
        {
            if (!Enum.IsDefined(_foodSearchRequest.FlavorProfile)) return;
            Query = Query.Where(f => f.FlavorProfile == _foodSearchRequest.FlavorProfile);
        }

        private void FilterTemperature()
        {
            if (!Enum.IsDefined(_foodSearchRequest.Temperature)) return;
            Query = Query.Where(f => f.Temperature == _foodSearchRequest.Temperature);
        }

        private void FilterMealType()
        {
            if (!Enum.IsDefined(_foodSearchRequest.MealType)) return;
            Query = Query.Where(f => f.Type == _foodSearchRequest.MealType);
        }
    }
}