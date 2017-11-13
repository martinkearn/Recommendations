using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Dtos
{
    public class SearchResponseDto
    {
        public string odatacontext { get; set; }
        public List<SearchResponseItemDto> value { get; set; }
        public string odatanextLink { get; set; }
    }
}
