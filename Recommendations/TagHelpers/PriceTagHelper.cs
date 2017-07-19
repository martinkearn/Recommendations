using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendations.TagHelpers
{
    public class PriceTagHelper : TagHelper
    {
        public decimal Sell { get; set; }
        public decimal Rrp { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            //Get child content (the content within the tags)
            var childContentRaw = (await output.GetChildContentAsync()).GetContent();

            if (Sell == Rrp)
            {
                //not discounted, display sell price on its own
                output.Content.SetContent(Sell.ToString("c"));
            }
            else
            {
                //is discounted, display was and now
                var sellPercentageOfRrp = (Sell/Rrp) * 100;
                var saving = 100 - sellPercentageOfRrp;
                var savingRounded = Math.Round(saving, 0);

                output.TagName = "span";
                output.Content.SetHtmlContent($@"<span style=""text-decoration: line-through;"">Was {Rrp.ToString("c")}</span>, Now {Sell.ToString("c")}. Save {savingRounded}%");
                output.TagMode = TagMode.StartTagAndEndTag;
            }


        }
    }
}
