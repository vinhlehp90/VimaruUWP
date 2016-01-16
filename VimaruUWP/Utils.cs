using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net;
//using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace VimaruUWP
{
    public class Utils
    {
        internal static async Task ShowMessage(string content, string title = "")
        {
            var msg = new MessageDialog(content, title);
            await msg.ShowAsync();
        }
        internal static HttpClient GetHttpClient()
        {
            var filter = new HttpBaseProtocolFilter() { AllowAutoRedirect = false, AutomaticDecompression = true };
            var client = new HttpClient(filter);
            
            return client;
        }
        internal static async Task<string> DownloadString(string url, Dictionary<string, string> headers = null)
        {
            var client = GetHttpClient();

            if (headers != null && headers.Count > 0)
            {
                foreach (var pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Key, pair.Value);
                }
            }
            var response = await client.GetAsync(new Uri(url));

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        internal static async Task<string> UploadString(string url,  Dictionary<string, string> headers,IHttpContent content, string method = "post")
        {
            var client = GetHttpClient();
            if (headers != null && headers.Count > 0)
            {
                foreach (var pair in headers)
                {
                    client.DefaultRequestHeaders.Add(pair.Key, pair.Value);
                }
            }
            HttpResponseMessage response;
            switch (method.ToLower())
            {
                case "put":
                    response = await client.PutAsync(new Uri(url), content);
                    break;
                case "delete":
                    response = await client.DeleteAsync(new Uri(url));
                    break;
                default:
                    response = await client.PostAsync(new Uri(url), content);
                    break;

            }
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
