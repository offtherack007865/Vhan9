using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vhan9.ConsoleApp
{
    public class ReadInConfigOptions
    {
        public ReadInConfigOptions(Microsoft.Extensions.Configuration.IConfiguration myConfig)
        {
            MyConfig = myConfig;
        }

        public Microsoft.Extensions.Configuration.IConfiguration MyConfig { get; set; }

        public Vhan9.data.Models.ConfigOptions ReadIn()
        {
            Vhan9.data.Models.ConfigOptions
                returnConfigOptions =
                new Vhan9.data.Models.ConfigOptions();

            returnConfigOptions.BaseWebUrl =
                MyConfig.GetValue<string>(Vhan9.data.MyConstants.BaseWebUrl);
            return returnConfigOptions;

        }
    }
}
