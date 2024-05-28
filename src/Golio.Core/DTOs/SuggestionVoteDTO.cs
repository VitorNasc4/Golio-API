using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Golio.Core.DTOs
{
    public class SuggestionVoteDTO
    {
        public int SuggestionId { get; set; }
        public int PriceId { get; set; }
        public double Value { get; set; }
        public bool IsValid { get; set; }
    }
}