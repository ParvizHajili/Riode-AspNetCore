using System.Text.RegularExpressions;

namespace Riode.WebUI.AppCode.Extensions
{
    public static partial class RegexExtensions
    {
        public static bool IsEmail(this string value)
        {
            return Regex.IsMatch(value, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
