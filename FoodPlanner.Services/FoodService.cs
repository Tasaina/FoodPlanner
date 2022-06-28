using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodPlanner.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace FoodPlanner.Services
{
    public class FoodService : IFoodService
    {
        private readonly FoodContext _context;

        public FoodService(FoodContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Food>> Lookup(FoodSearchRequest foodSearchRequest)
        {
            return await LookupCore(foodSearchRequest, 100);
        }

        public async Task<IEnumerable<Food>> Lookup(FoodSearchRequest foodSearchRequest, int pageSize)
        {
            return await LookupCore(foodSearchRequest, pageSize);
        }

        public IEnumerable<Food> LookupSynchronous(FoodSearchRequest foodSearchRequest, int pageSize)
        {
            return LookupCore(foodSearchRequest, pageSize).Result;
        }

        public async Task<IEnumerable<Food>> LookupCore(FoodSearchRequest foodSearchRequest, int pageSize)
        {
            var foodSearchQuery = new FoodSearchQuery(foodSearchRequest, pageSize);
            return await foodSearchQuery.Build(_context.Foods);
        }

        public async Task<Food> GetRandom(FoodSearchRequest recommendedSearchRequest)
        {
            var results = (await Lookup(recommendedSearchRequest, 100)).ToList();
            if (results.Count == 0) return null;
            var result = results.Random();
            return result;
        }

        public async Task Update(Food food)
        {
            _context.Foods.Update(food);
            await _context.SaveChangesAsync();
        }

        public async Task Add(Food food)
        {
            _context.Foods.Add(food);
            await _context.SaveChangesAsync();
        }

        public async Task<Food> Find(Guid id)
        {
            return await _context.Foods.FindAsync(id);
        }

        public void ClearChanges()
        {
            _context.ChangeTracker.Clear();
        }

        public void Delete(Food food)
        {
            _context.Foods.Remove(food);
            _context.SaveChanges();
        }
    }

    public interface IFoodService
    {
        Task<IEnumerable<Food>> Lookup(FoodSearchRequest foodSearchRequest);
        Task<IEnumerable<Food>> Lookup(FoodSearchRequest foodSearchRequest, int pageSize);
        IEnumerable<Food> LookupSynchronous(FoodSearchRequest foodSearchRequest, int pageSize);
        Task<Food> GetRandom(FoodSearchRequest recommendedSearchRequest);
        Task Update(Food food);
        Task Add(Food food);
        Task<Food> Find(Guid foodId);
        void ClearChanges();
        void Delete(Food food);
    }
}