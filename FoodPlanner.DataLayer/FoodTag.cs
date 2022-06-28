using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodPlanner.DataLayer
{
    public class FoodTag
    {
        public Guid Id { get; set; }
        public virtual Tag Tag { get; set; }

        public override string ToString()
        {
            return $"{Tag.Text}";
        }
    }
}