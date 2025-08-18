using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vhan9.data.Models;

namespace Vhan9.ConsoleApp
{
    public class GetSourceFileList
    {
        public GetSourceFileList(qy_GetVhanConfigOutputColumns inputConfigOptions)
        {
            MyConfigOptions = inputConfigOptions;
        }
        public qy_GetVhanConfigOutputColumns MyConfigOptions { get; set; }

        public List<string> MySourceFileList { get; set; }

        public void DoIt()
        {
            MySourceFileList =
                Directory.EnumerateFiles(MyConfigOptions.ReadDirectory)
                .Where(f => f.EndsWith(".csv")).ToList();
        }
    }
}
