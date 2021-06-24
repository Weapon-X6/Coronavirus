using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Coronavirus.Helpers
{
    public interface IHttpClientHelper
    {
        Task<String> GetAsync(string uri, IDictionary<string, string> headers = null);
    }
}