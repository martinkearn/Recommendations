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
using System.Text;
using System.Net.Http.Headers;

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
        public async Task<IEnumerable<CatalogItem>> GetITIRecommendations(string seedItemId, string numberOfResults, string minimalScore)
        {
            var catalogItems = new List<CatalogItem>();

            if (string.IsNullOrEmpty(seedItemId))
            {
                return catalogItems;
            }

            //construct API parameters
            var parameters = new Dictionary<string, string> {
                { "recommendationCount", numberOfResults }
            };
            if (!string.IsNullOrEmpty(seedItemId)) parameters.Add("itemId", seedItemId);

            //construct full API endpoint uri
            var apiUri = QueryHelpers.AddQueryString(_baseItemToItemApiUrl, parameters);

            //get item to item recommendations
            var responseContent = string.Empty;
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_baseItemToItemApiUrl);
                httpClient.DefaultRequestHeaders.Add("x-api-key", _appSettings.RecommendationsApiKey);

                //make request
                var response = await httpClient.GetAsync(apiUri);

                //read response and parse to object
                responseContent = await response.Content.ReadAsStringAsync();
            }

            catalogItems = CastResponseToCatalogItems(responseContent);

            return catalogItems;
        }

        public async Task<IEnumerable<CatalogItem>> GetPersonalizedRecommendedItemsByUser(string userId, string numberOfResults)
        {
            var catalogItems = new List<CatalogItem>();

            //construct API parameters
            var parameters = new Dictionary<string, string> {
                { "recommendationCount", numberOfResults }
            };
            if (!string.IsNullOrEmpty(userId)) parameters.Add("userId", userId);

            //construct full API endpoint uri
            var apiUri = QueryHelpers.AddQueryString(_baseItemToItemApiUrl, parameters);

            //get personalized recommendations
            var responseContent = string.Empty;
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_baseItemToItemApiUrl);
                httpClient.DefaultRequestHeaders.Add("x-api-key", _appSettings.RecommendationsApiKey);

                //make request
                var response = await httpClient.PostAsync(apiUri, null);

                //read response and parse to object
                responseContent = await response.Content.ReadAsStringAsync();
            }

            catalogItems = CastResponseToCatalogItems(responseContent);

            return CastResponseToCatalogItems(responseContent);
        }


        public async Task<IEnumerable<CatalogItem>> GetPersonalizedRecommendedItemsByItems(IEnumerable<CatalogItem> seedItems, string numberOfResults)
        {
            var catalogItems = new List<CatalogItem>();

            //construct API parameters
            var parameters = new Dictionary<string, string> {
                { "recommendationCount", numberOfResults }
            };

            //construct full API endpoint uri
            var apiUri = QueryHelpers.AddQueryString(_baseItemToItemApiUrl, parameters);

            //construct body of ItemIds
            var bodyJson = string.Empty;
            if (seedItems.Count() > 0)
            {
                var seedItemsDto = new List<ItemsDto>();
                foreach (var seedItem in seedItems)
                {
                    seedItemsDto.Add(new ItemsDto() { itemId = seedItem.Id });
                }
                bodyJson = JsonConvert.SerializeObject(seedItemsDto);
            }
            var body = JsonConvert.SerializeObject(bodyJson);

            //get personalized recommendations
            var responseContent = string.Empty;
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_baseItemToItemApiUrl);
                httpClient.DefaultRequestHeaders.Add("x-api-key", _appSettings.RecommendationsApiKey);

                //make request
                var response = await httpClient.PostAsync(apiUri, new StringContent(body, Encoding.UTF8, "application/json"));

                //read response and parse to object
                responseContent = await response.Content.ReadAsStringAsync();
                //ISSUE: The above request always returns the default results which all have a recommendation rating of 0.0. This is the same result as when no body is passed in Postman, however if you pass the value of bodyJson in PostMan, you get proper results 
            }

            catalogItems = CastResponseToCatalogItems(responseContent);

            return CastResponseToCatalogItems(responseContent);
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
                if (recommendedItem.Score == 0)
                {
                    recommendedItem.Score = Convert.ToDecimal(0.000000000000000000000);
                }
                catalogItem.RecommendationRating = recommendedItem.Score;
                catalogItems.Add(catalogItem);
            }

            return catalogItems.OrderByDescending(o => o.RecommendationRating).ToList();
        }
    }
}
