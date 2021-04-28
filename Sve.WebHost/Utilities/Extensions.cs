using JxNet.Core.Extensions;
using JxNet.Extensions.WebHost.Constants;
using System;

namespace Sve.WebHost
{
    public static class Extensions
    {
        public static string FixUrl(this string url)
        {
            return url?.Replace(@"\", "/") ?? "";
        }

        public static string FixUrl(this string applicationUrl, string itemUrl)
        {
            var imagePath = itemUrl.IsNullOrEmpty() ? ImageConstants.NoProductImagePath : itemUrl.Trim();
            return $"{applicationUrl}/{imagePath.FixUrl()}";
        }

        public static decimal ToRound(this decimal amount, int decimals = 2)
        {
            return Math.Round(amount, decimals);
        }

        public static string ToRoundString(this decimal amount, int decimals = 2)
        {
            return Math.Round(amount, decimals).ToString();
        }
    }
}
