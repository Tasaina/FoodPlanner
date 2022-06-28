using System;

namespace FoodPlanner.DataLayer
{
    public class FoodPlanEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int BaseAmount { get; set; }
        public int AmountLeft { get; set; }
        public int AmountDifference => BaseAmount - AmountLeft;

        public FoodPlanEntry()
        {
        }
    }
}