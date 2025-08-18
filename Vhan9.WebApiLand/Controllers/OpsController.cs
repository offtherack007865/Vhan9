using log4net;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Vhan9.data.Models;

namespace Vhan9.WebApiLand.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OpsController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OpsController));

        public OpsController(SimplifyVbcAdt8Context inputNrcContext)
        {
            MyContext = inputNrcContext;

            log.Info($"Start of OpsController Connection String:  {MyContext.MyConnectionString}");

        }
        public SimplifyVbcAdt8Context MyContext { get; set; }


        // GET /api/Ops/qy_GetVhanConfig
        [HttpGet]
        public qy_GetVhanConfigOutput
                    qy_GetVhanConfig
                    ()
        {
            qy_GetVhanConfigOutput
                returnOutput =
                    new qy_GetVhanConfigOutput();

            string sql = $"vhan.qy_GetVhanConfig";

            List<SqlParameter> parms = new List<SqlParameter>();


            try
            {
                returnOutput.qy_GetVhanConfigOutputColumnsList =
                    MyContext
                    .qy_GetVhanConfigOutputColumnsList
                    .FromSqlRaw<qy_GetVhanConfigOutputColumns>
                    (
                          sql
                        , parms.ToArray()
                    )
                    .ToList();
            }
            catch (Exception ex)
            {
                returnOutput.IsOk = false;

                string myErrorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    myErrorMessage = $"{myErrorMessage}.  InnerException:  {ex.InnerException.Message}";
                }
                returnOutput.ErrorMessage = myErrorMessage;
                return returnOutput;
            }
            return returnOutput;
        }
    }
}
