using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodPlanner.DataLayer;

namespace FoodPlanner.Services
{
    public static class TagExtensions
    {
        public static FoodTag AsFoodTag(this Tag tag)
        {
            return new FoodTag() {Tag = tag};
        }
    }
}