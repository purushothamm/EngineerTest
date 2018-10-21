using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Html;

namespace EngineerTest.Class
{
    public static class PagingHelper
    {
        public static IHtmlContent PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            string anchorInnerHtml = "";
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                anchorInnerHtml = AnchorInnerHtml(i, pagingInfo);

                if (anchorInnerHtml == "..")
                    tag.MergeAttribute("href", "#");
                else
                    tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml.Equals(anchorInnerHtml);
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("active");
                }
                tag.AddCssClass("paging");
                if (anchorInnerHtml != "")
                    result.Append(tag.ToString());
            }
            //return mvcHtmlContent.Create(result.ToString());
            return new HtmlString(result.ToString());
        }
        public static string AnchorInnerHtml(int i, PagingInfo pagingInfo)
        {
            string anchorInnerHtml = "";
            if (pagingInfo.TotalPages <= 10)
                anchorInnerHtml = i.ToString();
            else
            {
                if (pagingInfo.CurrentPage <= 5)
                {
                    if ((i <= 8) || (i == pagingInfo.TotalPages))
                        anchorInnerHtml = i.ToString();
                    else if (i == pagingInfo.TotalPages - 1)
                        anchorInnerHtml = "..";
                }
                else if ((pagingInfo.CurrentPage > 5) && (pagingInfo.TotalPages - pagingInfo.CurrentPage >= 5))
                {
                    if ((i == 1) || (i == pagingInfo.TotalPages) || ((pagingInfo.CurrentPage - i >= -3) && (pagingInfo.CurrentPage - i <= 3)))
                        anchorInnerHtml = i.ToString();
                    else if ((i == pagingInfo.CurrentPage - 4) || (i == pagingInfo.CurrentPage + 4))
                        anchorInnerHtml = "..";
                }
                else if (pagingInfo.TotalPages - pagingInfo.CurrentPage < 5)
                {
                    if ((i == 1) || (pagingInfo.TotalPages - i <= 7))
                        anchorInnerHtml = i.ToString();
                    else if (pagingInfo.TotalPages - i == 8)
                        anchorInnerHtml = "..";
                }
            }
            return anchorInnerHtml;
        }
    }
}