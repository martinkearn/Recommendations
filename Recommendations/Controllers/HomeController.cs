using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recommendations.Interfaces;
using Recommendations.ViewModels;
using Recommendations.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Recommendations.Dtos;
using System.Net.Http;
using System.Text;

namespace Recommendations.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICatalogRepository _catalogItems;
        private readonly IRecommendationsRepository _recommendations;
        private readonly ICartRepository _cart;
        public int _pageSize { get; set; }

        public HomeController(IOptions<AppSettings> appSettings, ICatalogRepository catalogItemsRepository, IRecommendationsRepository recommendationsRepository, ICartRepository cart)
        {
            _catalogItems = catalogItemsRepository;
            _recommendations = recommendationsRepository;
            _cart = cart;

            _pageSize = Convert.ToInt16(appSettings.Value.ItemsPerPage);
        }

        public IActionResult Index(int? page)
        {
            var allCatalogItems = _catalogItems.GetCatalogItems();
     
            //get page of catalogItems
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var numberToSkip = pageNumber * _pageSize;

            var pageOfCatalogItems = (pageNumber ==1) ?
                allCatalogItems.Take(_pageSize) :
                allCatalogItems.Skip(numberToSkip).Take(_pageSize);

            //paging values
            var pagingVm = ConstructPagingVm(allCatalogItems.Count(), pageNumber);

            //construct main items view model
            var catalogItemGroupViewModel = new CatalogItemGroupViewModel()
            {
                GroupName = "Home",
                CatalogItems = pageOfCatalogItems,
                PagingPartialViewModel = pagingVm
            };

            //construct featured items view model
            var featuredCatalogItems = new List<CatalogItem>();
            featuredCatalogItems.Add(allCatalogItems.Where(i => i.Id == "244265").FirstOrDefault());
            featuredCatalogItems.Add(allCatalogItems.Where(i => i.Id == "250497").FirstOrDefault());
            featuredCatalogItems.Add(allCatalogItems.Where(i => i.Id == "245126").FirstOrDefault());
            featuredCatalogItems.Add(allCatalogItems.Where(i => i.Id == "264343").FirstOrDefault());
            featuredCatalogItems.Add(allCatalogItems.Where(i => i.Id == "87185").FirstOrDefault()); 

            //construct main view model
            var vm = new HomeViewModel()
            {
                FeaturedCatalogItems = featuredCatalogItems,
                CatalogItems = catalogItemGroupViewModel
            };

            //return view
            return View(vm);
        }

        public async Task<IActionResult> CatalogItem(string id)
        {
            //get this catalogItem
            var catalogItem = _catalogItems.GetCatalogItemById(id);

            //get recommendations
            var recommendations = await _recommendations.GetITIRecommendations(catalogItem.Id, "100", "1");

            //get cheaper recommendations
            var cheaperRecommendations = _catalogItems.GetLikeThisButCheaper(catalogItem, recommendations, 20);

            //get body section
            var bodySection = _catalogItems.GetOutfitSection(catalogItem);

            //get outfit
            var outfit = _catalogItems.GetOutfit(catalogItem, recommendations);

            //get accesory recommendations
            var accesories = _catalogItems.GetRelatedAccesories(new List<CatalogItem>() { catalogItem }, recommendations);

            //get online link
            var onlineLink = ($"https://www.jdsports.co.uk/product/{catalogItem.Colour}-{catalogItem.Title}/{catalogItem.Id}/").ToLower().Replace(" ", "-");

            //construct view model
            var vm = new CatalogItemViewModel()
            {
                CatalogItem = catalogItem,
                Recommendations = recommendations,
                OutfitSection = bodySection,
                Outfit = outfit,
                OnlineLink = onlineLink,
                CheaperRecommendations = cheaperRecommendations,
                AcessoryRecommendations = accesories
            };

            //return view
            return View(vm);
        }

        public IActionResult Categories()
        {
            var categories = _catalogItems.GetCategories();

            //construct view model
            var vm = new CategoriesViewModel()
            {
                Categories = categories
            };

            //return view
            return View(vm);
        }

        public IActionResult Category(string id, int? page)
        {
            var category = _catalogItems.GetCategories().Where(o => o.Title.ToLower() == id.ToLower()).FirstOrDefault();

            var allCatalogItems = _catalogItems.GetCatalogItems().Where(o => o.Type.ToLower() == id.ToLower());

            //get page of catalogItems
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var numberToSkip = pageNumber * _pageSize;

            var pageOfcatalogItems = (pageNumber == 1) ?
                allCatalogItems.Take(_pageSize) :
                allCatalogItems.Skip(numberToSkip).Take(_pageSize);

            //paging values
            var pagingVm = ConstructPagingVm(allCatalogItems.Count(), pageNumber);

            //construct view model
            var vm = new CategoryViewModel()
            {
                GroupName = id,
                CatalogItems = pageOfcatalogItems,
                PagingPartialViewModel = pagingVm,
                RelatedCategoryTitles = category.TopRelatedCategoryTitles,
                OutfitSection = category.OutfitSection,
                RelatedAccessoryTitles = category.AccessoryCategoryTitles
            };

            //return view
            return View(vm);
        }

        public IActionResult Brands()
        {
            var brands = _catalogItems.GetBrands();

            //construct view model
            var vm = new BrandsViewModel()
            {
                Brands = brands
            };

            //return view
            return View(vm);
        }

        public IActionResult Brand(string id, int? page)
        {
            var allCatalogItems = _catalogItems.GetCatalogItems().Where(o => o.Brand.ToLower() == id.ToLower());

            //get page of catalogItems
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var numberToSkip = pageNumber * _pageSize;

            var pageOfcatalogItems = (pageNumber == 1) ?
                allCatalogItems.Take(_pageSize) :
                allCatalogItems.Skip(numberToSkip).Take(_pageSize);

            //paging values
            var pagingVm = ConstructPagingVm(allCatalogItems.Count(), pageNumber);

            //construct view model
            var vm = new CatalogItemGroupViewModel()
            {
                GroupName = id,
                CatalogItems = pageOfcatalogItems,
                PagingPartialViewModel = pagingVm
            };

            //return view
            return View(vm);
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> Test()
        {
            var catalogItems = new List<CatalogItem>();

            //construct API parameters
            var parameters = new Dictionary<string, string> {
                { "recommendationCount", "10" }
            };

            //construct full API endpoint uri
            var apiBaseUri = "https://sportshopxmlgqkb7ew5gyws.azurewebsites.net";
            var apiUri = $"{apiBaseUri}/api/models/f7153430-0ef8-429d-9a1c-d28b43435634/recommend";
            var apiUriWithParams = QueryHelpers.AddQueryString(apiUri, parameters);

            //construct body of ItemIds
            var body = @"[{""itemId"":""264343""},{""itemId"":""264293""}]";

            //get personalized recommendations
            var responseContent = string.Empty;
            using (var httpClient = new HttpClient())
            {
                //setup HttpClient
                httpClient.BaseAddress = new Uri(apiBaseUri);
                httpClient.DefaultRequestHeaders.Add("x-api-key", "cWZ1bzI1cnJseTVsZw==");

                //make request
                var response = await httpClient.PostAsync(apiUri, new StringContent(body, Encoding.UTF8, "application/json"));

                //read response and parse to object
                responseContent = await response.Content.ReadAsStringAsync();
                //ISSUE: The above request always returns the default results which all have a recommendation rating of 0.0. This is the same result as when no body is passed in Postman, however if you pass the value of bodyJson in PostMan, you get proper results 
            }

            return View(responseContent);
        }

        private PagingPartialViewModel ConstructPagingVm(int catalogItemsCount, int currentPage)
        {
            return new PagingPartialViewModel()
            {
                TotalPages = catalogItemsCount / _pageSize,
                CurrentPage = currentPage,
                NextPage = currentPage + 1,
                PreviousPage = (currentPage == 1) ? 1 : currentPage - 1,
                TotalCatalogItems = catalogItemsCount
            };
        }
    }
}
