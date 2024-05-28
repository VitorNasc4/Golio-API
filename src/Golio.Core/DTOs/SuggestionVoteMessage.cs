using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Golio.Core.DTOs
{
    public class SuggestionVoteMessage
    {
        public int SuggestionId { get; set; }
        public bool IsValid { get; set; }
    }
}