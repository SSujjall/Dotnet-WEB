using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WEB.Models.Common
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Data { get; set; }
    }
}