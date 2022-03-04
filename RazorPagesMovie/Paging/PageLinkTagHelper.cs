using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Dynamic;

namespace RazorPages.Paging
{
  [HtmlTargetElement("div", Attributes = "page-model")]
  public class PageLinkTagHelper : TagHelper 
  {
    private IUrlHelperFactory urlHelperFactory;
    
    public PageLinkTagHelper(IUrlHelperFactory helperFactory)
    {
      urlHelperFactory = helperFactory;
    }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = new ViewContext();

    public PagingInfo PageModel { get; set; } = new PagingInfo();

    public string PageName { get; set; } = string.Empty;

    /*Accepts all attributes that are page-other-* like page-other-category="@Model.allTotal" page-other-some="@Model.allTotal"*/
    [HtmlAttributeName(DictionaryAttributePrefix = "page-other-")]
    public Dictionary<string, object> PageOtherValues { get; set; } = new Dictionary<string, object>();

    public bool PageClassesEnabled { get; set; } = false;

    public string PageClass { get; set; } = string.Empty;

    public string PageClassSelected { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
      TagBuilder result = new TagBuilder("div");
      string anchorInnerHtml = "";

      for (int i = 1; i <= PageModel.TotalPages; i++)
      {
          TagBuilder tag = new TagBuilder("a");
          anchorInnerHtml = AnchorInnerHtml(i, PageModel);

          if (anchorInnerHtml == "..")
              tag.Attributes["href"] = "#";
          else if (PageOtherValues.Keys.Count != 0)
              tag.Attributes["href"] = urlHelper.Page(PageName, AddDictionaryToQueryString(i));
          else
              tag.Attributes["href"] = urlHelper.Page(PageName, new { id = i });

          if (PageClassesEnabled)
          {
              tag.AddCssClass(PageClass);
              tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : "");
          }
          tag.InnerHtml.Append(anchorInnerHtml);
          if (anchorInnerHtml != "")
              result.InnerHtml.AppendHtml(tag);
      }
      output.Content.AppendHtml(result.InnerHtml);
    }

    public IDictionary<string, object?> AddDictionaryToQueryString(int i)
    {
        //object routeValues = new RouteValueDictionary();
        var dict = new RouteValueDictionary();
        dict.Add("id", i);
        foreach (string key in PageOtherValues.Keys)
        {
            dict.Add(key, PageOtherValues[key]);
        }

        var expandoObject = new ExpandoObject();
        var expandoDictionary = (IDictionary<string, object?>)expandoObject;
        foreach (var keyValuePair in dict)
        {
            expandoDictionary.Add(keyValuePair);
        }

        return expandoDictionary;
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