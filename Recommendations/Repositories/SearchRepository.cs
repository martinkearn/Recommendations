using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Recommendations.Dtos;
using Recommendations.Interfaces;
using Recommendations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Recommendations.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly AppSettings _appSettings;
        private readonly ICatalogRepository _catalogItemsRepository;

        public SearchRepository(IOptions<AppSettings> appSettings, ICatalogRepository catalogRepository)
        {
            _appSettings = appSettings.Value;
            _catalogItemsRepository = catalogRepository;
        }

        public async Task<IEnumerable<CatalogItem>> SearchCatalogItems(string query)
        {
            var catalogItems = new List<CatalogItem>();

            if (string.IsNullOrEmpty(query))
            {
                return catalogItems;
            }

            //construct API parameters
            var parameters = new Dictionary<string, string> {
                { "search", query }
            };

            //construct full API endpoint uri
            var apiUri = QueryHelpers.AddQueryString(_appSettings.SearchApiBaseUrl, parameters);

            //get item to item recommendations
            var responseContent = string.Empty;
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(_appSettings.SearchApiBaseUrl);
                httpClient.DefaultRequestHeaders.Add("api-key", _appSettings.SearchApiQueryKey);

                //make request
                var response = await httpClient.GetAsync(apiUri);

                //read response and parse to object
                responseContent = await response.Content.ReadAsStringAsync();
            }

            catalogItems = CastResponseToCatalogItems(responseContent);

            return catalogItems;
        }

        private List<CatalogItem> CastResponseToCatalogItems(string responseContent)
        {
            var catalogItems = new List<CatalogItem>();

            if (string.IsNullOrEmpty(responseContent))
            {
                return catalogItems;
            }

            var searchresults = SearchResults.FromJson(responseContent);

            //cast to CatalogItems
            foreach (var searchResult in searchresults.Value)
            {
                var catalogItem = _catalogItemsRepository.GetCatalogItemById(searchResult.ID);
                catalogItems.Add(catalogItem);
            }

            return catalogItems.ToList();
        }
    }
}
