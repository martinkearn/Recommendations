using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Recommendations.Interfaces;
using Recommendations.Dtos;
using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Recommendations.Repositories
{
    public class RecommendationsRepository : IRecommendationsRepository
    {
        private readonly AppSettings _appSettings;
        private string _baseItemToItemApiUrl;
        private readonly ICatalogRepository _catalogItemsRepository;

        public RecommendationsRepository(ICatalogRepository catalogItemsRepository, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            _catalogItemsRepository = catalogItemsRepository;

            _baseItemToItemApiUrl = _appSettings.RecommendationsApiBaseUrl.Replace("MODELID", _appSettings.RecommendationsApiModelId);
        }

        /// <summary>
        /// Helper function to call the Cognitive Recommendations API with an Item-to-Item build
        /// </summary>
        /// <param name="ids">Comma seperated list of ItemId's to seed recommendations on</param>
        /// <param name="numberOfResults">How many results to return</param>
        /// <param name="minimalScore">Minimal score for results to be included</param>
        /// <returns>catalogItems object - a list of CatalogItems representing recommended items for the passed in Ids</returns>
        public async Task<IEnumerable<CatalogItem>> GetRecommendations(string ids, string numberOfResults, string minimalScore)
        {
            var catalogItems = new List<CatalogItem>();

            if (string.IsNullOrEmpty(ids))
            {
                return catalogItems;
            }

            var responseContent = await CallRecommendationsApi(ids, numberOfResults, minimalScore, _appSettings.RecommendationsApiBuildId);
            var recomendedItems = JsonConvert.DeserializeObject<RecommendedItems>(responseContent);

            //cast to CatalogItems
            foreach (var rootRecommendedItem in recomendedItems.recommendedItems)
            {
                foreach (var recommendedItem in rootRecommendedItem.items)
                {
                    var catalogItem = _catalogItemsRepository.GetCatalogItemById(recommendedItem.id);
                    catalogItem.RecommendationRating = rootRecommendedItem.rating;
                    catalogItems.Add(catalogItem);
                }
            }

            return catalogItems;
        }

        private async Task<string> CallRecommendationsApi(string ids, string numberOfResults, string minimalScore, string buildId)
        {
            //construct API parameters
            var parameters = new Dictionary<string, string> {
                { "itemIds", ids},
                { "numberOfResults", numberOfResults },
                { "minimalScore", minimalScore },
            };

            //Only add build ID if it is not empty. buildId is an optional field and API defaults to active build if it is ommited
            if (!String.IsNullOrEmpty(buildId))
            {
                parameters.Add("buildId", buildId);
            }

            //construct full API endpoint uri
            var apiUri = QueryHelpers.AddQueryString(_baseItemToItemApiUrl, parameters);

            //get item to item recommendations
            var responseContent = string.Empty;
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_baseItemToItemApiUrl);
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.RecommendationsApiKey);

                //make request
                var response = await httpClient.GetAsync(apiUri);

                //read response and parse to object
                responseContent = await response.Content.ReadAsStringAsync();
            }

            return responseContent;
        }
    }
}
