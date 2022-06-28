using System;
using System.Collections.Generic;

namespace FoodPlanner.DataLayer
{
    public class FoodPlan
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual List<FoodPlanEntry> Entries { get; set; } = new List<FoodPlanEntry>();
    }
}