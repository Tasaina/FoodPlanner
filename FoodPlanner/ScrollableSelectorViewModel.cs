using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FoodPlanner
{
    public class ScrollableSelectorViewModel : ReactiveObject
    {
        [Reactive] public IEnumerable<object> Items { get; set; }

        public ScrollableSelectorViewModel(IEnumerable<object> items)
        {
            Items = items;
        }

        public object? GetSelection(TextBlock sender)
        {
            if (sender == null) return null;
            return Items.FirstOrDefault(i=>i.ToString()==sender.Text);
        }
    }
}