using log4net;
using Newtonsoft.Json;
using Vhan9.data.Models;

namespace Vhan9.CallWebApiLand
{
    public class CallWebApiLandClass
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CallWebApiLandClass));

        public CallWebApiLandClass
        (
            string inputBaseWebApiUrl
        )
        {
            MyBaseWebApiUrl = inputBaseWebApiUrl;
        }
        public string MyBaseWebApiUrl { get; set; }

        // GET /api/Ops/qy_GetConfig
        public qy_GetVhanConfigOutput
                    qy_GetVhanConfig
                    ()
        {
            qy_GetVhanConfigOutput
                returnOutput =
                    qy_GetVhanConfigAsync
                    ()
                    .Result;

            return returnOutput;
        }

        public async Task<qy_GetVhanConfigOutput>
                        qy_GetVhanConfigAsync
                        ()
        {
            log.Info($"In qy_GetVhanConfigAsync");
            qy_GetVhanConfigOutput
                returnOutput =
                    new qy_GetVhanConfigOutput();

            string myCompleteUrl = $"{MyBaseWebApiUrl}/api/Ops/qy_GetVhanConfig";
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromHours(1);

                    var result = await client.GetAsync(myCompleteUrl);
                    var response = await result.Content.ReadAsStringAsync();
                    returnOutput = JsonConvert.DeserializeObject<qy_GetVhanConfigOutput>(response);
                }
            }
            catch (Exception ex)
            {
                returnOutput.IsOk = false;
                string myErrorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    myErrorMessage = $"{myErrorMessage}.  Inner Exception:  {ex.InnerException.Message}";
                }
                return returnOutput;
            }

            return returnOutput;
        }
    }

}
