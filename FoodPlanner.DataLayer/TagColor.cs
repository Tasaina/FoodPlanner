using System.Drawing;

namespace FoodPlanner.DataLayer
{
    public class TagColor
    {
        public int Id { get; set; }
        public string HexCode { get; set; }

        public TagColor()
        {
        }

        public TagColor(string hexCode)
        {
            HexCode = hexCode;
        }

        public Color AsColor()
        {
            return ColorTranslator.FromHtml($"#{HexCode}");
        }
    }
}