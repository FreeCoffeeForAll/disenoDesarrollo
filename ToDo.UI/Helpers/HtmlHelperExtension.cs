using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDo.UI.Helpers
{
    public static class HtmlHelperExtension
    {
        public static IHtmlContent YesNo(this IHtmlHelper htmlHelper, bool yesNo)
        {
            var result = yesNo ? "Yes" : "No";
            return new HtmlString(result);
        }
    }
}
