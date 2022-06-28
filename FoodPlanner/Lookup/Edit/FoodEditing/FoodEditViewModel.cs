using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FoodPlanner.DataLayer;
using FoodPlanner.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FoodPlanner.Lookup.Edit.FoodEditing
{
    public class FoodEditViewModel : ReactiveObject
    {
        [Reactive] public Food Food { get; set; }
        [Reactive] public ObservableCollection<FoodTag> Tags { get; set; }
        public bool IsEdit { get; }
        private readonly IFoodService _foodService;
        private readonly ITagService _tagService;

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

        public FoodEditViewModel(Food food, bool isEdit)
        {
            Food = food;
            IsEdit = isEdit;
            _foodService = FoodServiceProvider.GetService<IFoodService>();
            _tagService = FoodServiceProvider.GetService<ITagService>();
            Tags = (new ObservableCollection<FoodTag>(Food.Tags)).Order();
        }

        public async Task SaveFoodChanges()
        {
            Food.Tags = Tags.ToList();
            if (IsEdit) await _foodService.Update(Food);
            else await _foodService.Add(Food);
        }

        public async Task AddTag(string text, bool? isChecked)
        {
            var tag = await _tagService.GetFromLibrary(text) ?? new Tag()
                {Text = text, IsMajor = isChecked ?? false};
            if (Tags.Select(t => t.Tag.Text)
                .Any(t => string.Equals(t, text, StringComparison.InvariantCultureIgnoreCase))) return;
            Tags.Add(tag.AsFoodTag());
            Tags = Tags.Order();
        }

        public void RemoveTag(FoodTag tag)
        {
            Tags.Remove(tag);
            Tags = Tags.Order();
        }

        public void Cancel()
        {
            _foodService.ClearChanges();
        }

        public void DeleteFood()
        {
            _foodService.Delete(Food);
        }
    }
}