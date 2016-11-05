using System.Text.RegularExpressions;


namespace FullContactTest
{
    public static class StringExtensions
    {
        public static bool IsEmail(this string s)
        {
            bool isEmail = 
                Regex.IsMatch(s, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase);
            return isEmail;
        }
    }
}
