using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoodPlanner.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace FoodPlanner.Services
{
    public static class IEnumerableExtensions
    {
        public static T Random<T>(this IEnumerable<T> collection)
        {
            var list = collection.ToList();
            return list[new Random().Next(list.Count)];
        }

        public static IEnumerable<ScoredFood> RemoveDuplicates(this IEnumerable<ScoredFood> collection)
        {
            var distinctList = new List<ScoredFood>();
            var scoredFoods = collection.ToList();
            foreach (var item in scoredFoods)
            {
                if (!distinctList.Exists(i => i.Food.Id == item.Food.Id))
                    distinctList.Add(item);
                else
                {
                    var foodToOverwrite = scoredFoods.First(sf => sf.Food.Id == item.Food.Id);
                    if (foodToOverwrite.Score < item.Score) foodToOverwrite.Score = item.Score;
                }
            }

            return distinctList;
        }
    }

    public static class FoodPlanCollectionExtensions
    {
        public static FoodPlan GetNext(this IList<FoodPlan> plans, FoodPlan current)
        {
            return plans.SkipWhile(x => x.Id != current.Id).Skip(1).DefaultIfEmpty(plans[0])
                .FirstOrDefault();
        }

        public static FoodPlan GetPrevious(this IList<FoodPlan> plans, FoodPlan current)
        {
            return plans.TakeWhile(x => x.Id != current.Id).DefaultIfEmpty(plans[^1]).LastOrDefault();
        }

        public static FoodPlan GetNext(this IEnumerable<FoodPlan> plans, FoodPlan current)
        {
            return plans.SkipWhile(x => x.Id != current.Id).Skip(1).DefaultIfEmpty(plans.First())
                .FirstOrDefault();
        }

        public static FoodPlan GetPrevious(this IEnumerable<FoodPlan> plans, FoodPlan current)
        {
            return plans.TakeWhile(x => x.Id != current.Id).DefaultIfEmpty(plans.Last()).LastOrDefault();
        }

        public static FoodPlan GetNext(this DbSet<FoodPlan> plans, FoodPlan current)
        {
            return plans.SkipWhile(x => x.Id != current.Id).Skip(1).DefaultIfEmpty(plans.First())
                .FirstOrDefault();
        }

        public static FoodPlan GetPrevious(this DbSet<FoodPlan> plans, FoodPlan current)
        {
            return plans.TakeWhile(x => x.Id != current.Id).DefaultIfEmpty(plans.Last()).LastOrDefault();
        }
    }

    public static class ObservableTagCollectionExtensions
    {
        public static ObservableCollection<FoodTag> Order(this ObservableCollection<FoodTag> tags)
        {
            return new ObservableCollection<FoodTag>(tags.OrderByDescending(t => t.Tag.IsMajor)
                .ThenBy(t => t.Tag.Text));
        }
    }
}