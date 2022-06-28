using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace FoodPlanner.DataLayer
{
    public class Food
    {
        public Guid Id { get; set; }
        [Required] public string Name { get; set; }
        public Temperature Temperature { get; set; }
        public FlavorProfile FlavorProfile { get; set; }
        public virtual List<FoodTag> Tags { get; set; } = new();
        public Cost Cost { get; set; }
        public MealType Type { get; set; }
        public MealSize MealSize { get; set; }

        public Food()
        {
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }

    public enum Temperature
    {
        Warm,
        Cold,
        Room
    }

    public enum FlavorProfile
    {
        Sweet,
        Savory
    }

    public enum Cost
    {
        Cheap,
        Affordable,
        Expensive
    }

    public enum MealSize
    {
        SinglePerson,
        MultiplePeople,
        Family
    }

    public enum MealType
    {
        Breakfast,
        Lunch,
        Dinner,
        Snack,
        Desert
    }
}