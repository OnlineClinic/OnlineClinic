using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyOnlineClinic.Web.Helper
{
    public class AdsRotator
    {
        public string NavigateUrl
        {
            get;
            set;
        }

        public string Target
        {
            get;
            set;
        }

        public object LinkAttributes
        {
            get;
            set;
        }

        public string ImageUrl
        {
            get;
            set;
        }

        public string AlternateText
        {
            get;
            set;
        }

        public object ImageAttributes
        {
            get;
            set;
        }

        public string Keyword
        {
            get;
            set;
        }

        public int Impressions
        {
            get;
            set;
        }

        public static string Rotate(string keywordFilter, params AdsRotator[] ads)
        {
            AdsRotator ad = PickAd(keywordFilter, ads);

            string html = (ad == null) ? string.Empty : GenerateHtml(ad);

            return html;
        }

        public static string Rotate(params AdsRotator[] ads)
        {
            return Rotate(null, ads);
        }

        private static AdsRotator PickAd(string keywordFilter, params AdsRotator[] ads)
        {
            AdsRotator targetAd = null;

            IList<AdsRotator> matchedAds = ads.Where(ad => string.Compare(ad.Keyword, keywordFilter, StringComparison.InvariantCultureIgnoreCase) == 0)
                                      .OrderBy(ad => ad.Impressions)
                                      .ToList();

            if (matchedAds.Count > 0)
            {
                int max = matchedAds.Sum(ad => ad.Impressions);
                int random = new Random().Next(max + 1);
                int runningTotal = 0;

                foreach (AdsRotator ad in matchedAds)
                {
                    runningTotal += ad.Impressions;

                    if (random <= runningTotal)
                    {
                        targetAd = ad;
                        break;
                    }
                }

                if (targetAd == null)
                {
                    targetAd = matchedAds.Last();
                }
            }

            return targetAd;
        }

        private static string GenerateHtml(AdsRotator ad)
        {
            Action<TagBuilder, object> merge = (builder, values) =>
            {
                if (values != null)
                {
                    builder.MergeAttributes(new RouteValueDictionary(values));
                }
            };

            Action<TagBuilder, string, string> mergeIfNotBlank = (builder, name, value) =>
            {
                if (!string.IsNullOrEmpty(value))
                {
                    builder.MergeAttribute(name, value, true);
                }
            };

            TagBuilder imageBuilder = new TagBuilder("img");

            merge(imageBuilder, ad.ImageAttributes);
            mergeIfNotBlank(imageBuilder, "src", ad.ImageUrl);
            mergeIfNotBlank(imageBuilder, "alt", ad.AlternateText);

            if (!imageBuilder.Attributes.ContainsKey("alt"))
            {
                imageBuilder.Attributes.Add("alt", string.Empty);
            }

            TagBuilder linkBuilder = new TagBuilder("a");

            merge(linkBuilder, ad.LinkAttributes);
            mergeIfNotBlank(linkBuilder, "href", ad.NavigateUrl);
            mergeIfNotBlank(linkBuilder, "target", ad.Target);

            linkBuilder.InnerHtml = imageBuilder.ToString(TagRenderMode.SelfClosing);

            return linkBuilder.ToString();
        }
    }
}