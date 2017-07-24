# Recommendations Sample
This is a simple application showing how to call the [Cortana Intelligence Gallery Recommendations Solution](https://gallery.cortanaintelligence.com/Tutorial/Recommendations-Solution) in ASP.Net Core 1.1.

## What is the 'Cortana Intelligence Gallery Recommendations Solution'
The Cortana Intelligence Gallery Recommendations Solution is based on the same capabilities as the [Microsoft Cognitive Services Recommendations API](https://azure.microsoft.com/en-us/services/cognitive-services/recommendations/) but instead depliys the various components required into your Azure subscription. You can deploy teh solution [here](https://start.cortanaintelligence.com/Deployments/new/recommendationswebapp). You will require an Azure subscription to use the solution. 

## What does the application do? 
This is a simple web application which presents a very basic e-commerce store experience using a sample dataset which has already been configured with recommendations solution. 

The data set is based around sportswear and has over 65,000 catalog items (skus).

The web application uses the exact same data file as the 'catalog' file for the recommendations solution and so is easily interchangable for different catalogs. You can get sample data based around a collection of books or the Microsoft store from http://aka.ms/RecoSampleData (copies of both catalogs are included in the wwwroot folder).

The store provides several examples of how the recommendation solution can be used in real world scenarios:
* Catalog Item recommendations. See http://sportswearshop.azurewebsites.net/Home/CatalogItem/244265 as an example
    * 'Complete the outfit': Recommended items of different outfit section to make-up a complete outfit
    * 'You may also like': Recommended items based
    * 'Like this but cheaper': Recommended items of eth same type which are at least 20% cheaper
* Cart Recommendations. Add several items to the cart to see these. labelled as 'You may also like'
* Personalised Recommendations. Based on a user's priori shopping history. Login as one of teh sampel user IDs to see these

The application is deployed at http://sportswearshop.azurewebsites.net/
