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
using System.Linq;

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
        /// Helper function to call the Recommendations API for recommendations on a list of items
        /// </summary>
        /// <param name="ids">Comma seperated list of ItemId's to seed recommendations on</param>
        /// <param name="numberOfResults">How many results to return</param>
        /// <param name="minimalScore">Minimal score for results to be included</param>
        /// <returns>A list of CatalogItems representing recommended items</returns>
        public async Task<IEnumerable<CatalogItem>> GetRecommendations(List<CatalogItem> seedItems, string numberOfResults, string minimalScore)
        {
            var catalogItems = new List<CatalogItem>();

            if (seedItems.Count == 0)
            {
                return catalogItems;
            }

            //get seed item IDs into a comma seperated string
            var seedIds = string.Join(",", seedItems.Select(o => o.Id).ToList());

            var responseContent = await CallRecommendationsApi(seedIds, null, numberOfResults);

            return CastResponseToCatalogItems(responseContent);
        }

        /// <summary>
        /// Helper function to call the Recommendations API for recommendations for a user id
        /// </summary>
        /// <param name="userId">ID of the user to get recommendatiosn for</param>
        /// <param name="numberOfResults">How many results to return</param>
        /// <returns>A list of CatalogItems representing recommended items</returns>
        public async Task<IEnumerable<CatalogItem>> GetPersonalizedRecommendedItems(string userId, string numberOfResults)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new List<CatalogItem>();
            }

            var responseContent = await CallRecommendationsApi(null, userId, numberOfResults);

            return CastResponseToCatalogItems(responseContent);
        }

        private async Task<string> CallRecommendationsApi(string itemIds, string userId, string numberOfResults)
        {
            //construct API parameters
            var parameters = new Dictionary<string, string> {
                { "recommendationCount", numberOfResults }
            };
            if (!string.IsNullOrEmpty(itemIds)) parameters.Add("itemId", itemIds);
            if (!string.IsNullOrEmpty(userId)) parameters.Add("userId", userId);

            //construct full API endpoint uri
            var apiUri = QueryHelpers.AddQueryString(_baseItemToItemApiUrl, parameters);

            //get item to item recommendations
            var responseContent = string.Empty;
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_baseItemToItemApiUrl);
                httpClient.DefaultRequestHeaders.Add("x-api-key", _appSettings.RecommendationsApiKey);

                //make request. If we are using itemId, we need to do a GET, if we are using userId, we need a POST
                var response = (itemIds != null) ?
                    await httpClient.GetAsync(apiUri) :
                    await httpClient.PostAsync(apiUri, null);

                //read response and parse to object
                responseContent = await response.Content.ReadAsStringAsync();
            }

            return responseContent;
        }

        private List<CatalogItem> CastResponseToCatalogItems(string responseContent)
        {
            var catalogItems = new List<CatalogItem>();

            if (string.IsNullOrEmpty(responseContent))
            {
                return catalogItems;
            }

            var recommendedItems = JsonConvert.DeserializeObject<List<RecommendedItemDTO>>(responseContent);

            //cast to CatalogItems
            foreach (var recommendedItem in recommendedItems)
            {
                var catalogItem = _catalogItemsRepository.GetCatalogItemById(recommendedItem.RecommendedItemId);
                catalogItem.RecommendationRating = recommendedItem.Score;
                catalogItems.Add(catalogItem);
            }

            return catalogItems;
        }
    }
}
