using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Golio.Core.Entities;

namespace Golio.Core.Repositories
{
    public interface ISuggestionRepository
    {
        Task AddSuggestionAsync(Suggestion suggestion);
        Task<Suggestion> GetSuggestionByIdAsync(int suggestionsId);
        Task<bool> CheckSuggestionExistsAsync(Suggestion suggestion);
        Task SaveChangesAsync();
    }
}
