using System;
using System.ComponentModel.DataAnnotations;

namespace FoodPlanner.DataLayer
{
    public class Tag
    {
        [Key] public Guid Id { get; set; }
        [Required] public string Text { get; set; }
        public string ImageLink { get; set; }
        public virtual TagColor Color { get; set; }
        public bool IsMajor { get; set; }

        public Tag()
        {
        }

        public override string ToString()
        {
            return $"{Text}";
        }
    }
}