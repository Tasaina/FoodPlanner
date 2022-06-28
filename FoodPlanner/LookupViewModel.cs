using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodPlanner.DataLayer;
using ReactiveUI;

namespace FoodPlanner
{
    public class LookupViewModel : ReactiveObject
    {
        public IEnumerable<MealType> MealTypes =>
            Enum.GetValues(typeof(MealType))
                .Cast<MealType>();

        public IEnumerable<MealSize> MealSizes =>
            Enum.GetValues(typeof(MealSize))
                .Cast<MealSize>();

        public IEnumerable<FlavorProfile> FlavorProfiles =>
            Enum.GetValues(typeof(FlavorProfile))
                .Cast<FlavorProfile>();

        public IEnumerable<Temperature> Temperatures =>
            Enum.GetValues(typeof(Temperature))
                .Cast<Temperature>();
    }
}