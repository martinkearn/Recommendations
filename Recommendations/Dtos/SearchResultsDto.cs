using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.Dtos
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class SearchResults
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string OdataNextLink { get; set; }

        [JsonProperty("value")]
        public List<Value> Value { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("Brand")]
        public string Brand { get; set; }

        [JsonProperty("Colour")]
        public string Colour { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("ImageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("Rrp")]
        public long Rrp { get; set; }

        [JsonProperty("@search.score")]
        public double SearchScore { get; set; }

        [JsonProperty("Sell")]
        public long Sell { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }
    }

    public partial class SearchResults
    {
        public static SearchResults FromJson(string json) => JsonConvert.DeserializeObject<SearchResults>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this SearchResults self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
