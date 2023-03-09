using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly string Url_Base = "http://webservices.oorsprong.org/websamples.countryinfo/CountryInfoService.wso/CountryName";
        [HttpPost("api/test")]
        public async Task<IActionResult> Test(IsoContry Code)
        {

            var Client = new HttpClient();
            var parameters = new Dictionary<string, string>();
            foreach (PropertyInfo item in new List<PropertyInfo>(Code.GetType().GetProperties()))
            {
                parameters.Add(item.Name, item.GetValue(Code).ToString());
            }
            var response = await Client.PostAsync(Url_Base, new FormUrlEncodedContent(parameters));
            var result = await response.Content.ReadAsStringAsync();
            var doc = new XmlDocument();
            doc.LoadXml(result);
            if (response.IsSuccessStatusCode)
                return Ok(new ResponseC() { contryName = doc.InnerText });
            else
                return Ok(new Error() { messageError = doc.InnerText });
        }
    }
}


public class IsoContry
{
    public string sCountryISOCode { get; set; }
}
public class ResponseC
{
    public string contryName { get; set; }
}

public class Error
{
    public string messageError { get; set; }
}