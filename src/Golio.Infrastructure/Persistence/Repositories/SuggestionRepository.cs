using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Golio.Infrastructure.Persistence.Repositories
{
    public class SuggestionRepository : ISuggestionRepository
    {
        private readonly GolioDbContext _dbContext;
        private readonly IPriceRepository _priceRepository;

        public SuggestionRepository(GolioDbContext dbContext, IPriceRepository priceRepository)
        {
            _dbContext = dbContext;
            _priceRepository = priceRepository;
        }

        public async Task AddSuggestionAsync(Suggestion suggestion)
        {
            try
            {
                var existingSuggestion = await GetSuggestionByIdAsync(suggestion.Id);
                if (existingSuggestion != null)
                {
                    Console.WriteLine("A suggestion with this ID already exists.");
                    return;
                }

                var price = await _priceRepository.GetPriceByIdAsync(suggestion.PriceId);

                if (price.Suggestions is null)
                {
                    price.Suggestions = new List<Suggestion>();
                }

                _dbContext.Entry(suggestion).State = EntityState.Added;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving suggestion: {ex.Message}");
            }
        }

        public async Task<bool> CheckSuggestionExistsAsync(Suggestion suggestion)
        {
            return await _dbContext.Suggestions
                .AnyAsync(s =>
                    s.PriceId == suggestion.PriceId &&
                    s.Value == suggestion.Value
                );
        }

        public async Task DeleteSuggestionAsync(int suggestionsId)
        {
            var suggestion = await GetSuggestionByIdAsync(suggestionsId);
            if (suggestion != null)
            {
                try
                {
                    _dbContext.Suggestions.Remove(suggestion);
                    var entry = _dbContext.Entry(suggestion);
                    var entriesBefore = _dbContext.ChangeTracker.Entries().ToList();
                    await _dbContext.SaveChangesAsync();
                    var entriesAfter = _dbContext.ChangeTracker.Entries().ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public async Task<Suggestion> GetSuggestionByIdAsync(int suggestionsId)
        {
            try
            {
                var suggestion = await _dbContext.Suggestions
                    .SingleOrDefaultAsync(s => s.Id == suggestionsId);

                return suggestion;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching suggestion by ID {suggestionsId}: {ex.Message}");
                return null;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving suggestion: {ex.Message}");
            }
        }
    }
}