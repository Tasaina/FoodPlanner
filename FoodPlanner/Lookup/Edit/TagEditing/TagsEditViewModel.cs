using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using FoodPlanner.DataLayer;
using FoodPlanner.Services;
using ReactiveUI.Fody.Helpers;

namespace FoodPlanner.Lookup.Edit.TagEditing
{
    public class TagsEditViewModel
    {
        [Reactive] public Tag Tag { get; set; }
        public bool IsEdit { get; }
        private readonly ITagService _tagService;

        public TagsEditViewModel(Tag tag, bool isEdit)
        {
            Tag = tag;
            IsEdit = isEdit;
            _tagService = FoodServiceProvider.GetService<ITagService>();
        }

        public void DeleteTag()
        {
            _tagService.Delete(Tag);
        }

        public void Cancel()
        {
            _tagService.ClearChanges();
        }

        public async Task SaveTagChanges()
        {
            if (!IsEdit) _tagService.Add(Tag);
            await _tagService.Save();
        }
    }
}