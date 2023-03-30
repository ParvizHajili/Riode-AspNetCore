using Riode.WebUI.Models.Entities;
using System.Text;

namespace Riode.WebUI.AppCode.Extensions
{
    public static partial class Extension
    {
        public static string GetCategoriesRaw(this List<Category> categories)
        {
            if (categories == null || !categories.Any())
                return "";

            StringBuilder builder = new();

            builder.Append("<ul class='widget-body filter-items search-ul'>");
            foreach (var category in categories)
            {
                FillCategoriesRaw(category);
            }
            builder.Append("</ul>");

            return builder.ToString();

            void FillCategoriesRaw(Category category)
            {
                builder.Append($"<li><a href='#'>{category.Name}</a>");

                if (category.Children != null && category.Children.Any())
                {
                    builder.Append("<ul>");

                    foreach (var item in category.Children)
                        FillCategoriesRaw(item);

                    builder.Append("</ul>");
                }

                builder.Append("</li>");
            }
        }
    }
}
