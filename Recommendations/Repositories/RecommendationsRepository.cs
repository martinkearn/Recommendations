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

            catalogItems = CastResponseToCatalogItems(responseContent, new List<CatalogItem>());

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

            catalogItems = CastResponseToCatalogItems(responseContent, new List<CatalogItem>());

            return catalogItems;
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
            //For testing >>>> var bodyJson = @"[{""itemId"":""264343""},{""itemId"":""264293""}]";

            //get personalized recommendations
            var responseContent = string.Empty;
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_baseItemToItemApiUrl);
                httpClient.DefaultRequestHeaders.Add("x-api-key", _appSettings.RecommendationsApiKey);

                //make request
                var response = await httpClient.PostAsync(apiUri, new StringContent(bodyJson, Encoding.UTF8, "application/json"));

                //read response and parse to object
                responseContent = await response.Content.ReadAsStringAsync();
            }

            catalogItems = CastResponseToCatalogItems(responseContent, seedItems);

            return catalogItems;
        }

        private List<CatalogItem> CastResponseToCatalogItems(string responseContent, IEnumerable<CatalogItem> excludedItems)
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
                //check that this recommendation is not in the exclusions list
                if (excludedItems.Where(o => o.Id == recommendedItem.RecommendedItemId).Count() == 0)
                {
                    var catalogItem = _catalogItemsRepository.GetCatalogItemById(recommendedItem.RecommendedItemId);
                    catalogItem.RecommendationRating = recommendedItem.Score;
                    catalogItems.Add(catalogItem);
                }
            }

            return catalogItems.ToList();
        }
    }
}
