using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Dtos
{
    public class RecommendedItemDTO
    {
        public string RecommendedItemId { get; set; }
        public double Score { get; set; }
    }
}
