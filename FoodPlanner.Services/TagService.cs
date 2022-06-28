using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodPlanner.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace FoodPlanner.Services
{
    public interface ITagService
    {
        Task<Tag> GetFromLibrary(string text);
        IEnumerable<Tag> GetMatching(string text);
        Task Save();
        void ClearChanges();
        Task Delete(Tag tag);
        void Add(Tag tag);
    }

    public class TagService : ITagService
    {
        private readonly FoodContext _context;

        public TagService(FoodContext context)
        {
            _context = context;
        }

        public async Task<Tag> GetFromLibrary(string text)
        {
            return await _context.TagLibrary.FirstOrDefaultAsync(t =>
                t.Text == text);
        }

        public IEnumerable<Tag> GetMatching(string text)
        {
            return _context.TagLibrary.Where(t => t.Text.Contains(text));
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void ClearChanges()
        {
            _context.ChangeTracker.Clear();
        }

        public async Task Delete(Tag tag)
        {
            var relatedTags = _context.TagsOnFood.Where(t => t.Tag == tag);
            _context.RemoveRange(relatedTags);
            _context.Remove(tag);
            await _context.SaveChangesAsync();
        }

        public void Add(Tag tag)
        {
            if (_context.TagLibrary.Any(t => t.Text == tag.Text)) return;
            _context.TagLibrary.Add(tag);
        }
    }
}