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
                var price = await _priceRepository.GetPriceByIdAsync(suggestion.PriceId);

                if (price.Suggestions is null)
                {
                    price.Suggestions = new List<Suggestion>();
                }

                price.Suggestions.Add(suggestion);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao adicionar sugestão");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("Erro ao consultar sugestão pelo ID");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("Erro ao salvar a sugestão");
                Console.WriteLine(ex.Message);
            }
        }
    }
}