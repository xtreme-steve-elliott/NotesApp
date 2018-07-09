using System.Net.Http;
using System.Text;

namespace NotesApp.IntegrationTests.Extensions
{
    public static class StringExtensions
    {
        public static StringContent ToStringContent(this string body, string mediaType = "application/json", Encoding encoding = null)
        {
            return new StringContent(body, encoding ?? Encoding.UTF8, mediaType);
        }
    }
}