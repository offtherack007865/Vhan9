using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vhan9.data.Models
{
    public class qy_GetVhanConfigOutputColumns
    {
        public string ReadDirectory {  get; set; }
        public string ArchiveInputFileThisRunDirectory { get; set; }
        public string ArchiveInputFileDirectory { get; set; }
        public string ArchiveOutputFileDirectory { get; set; }
        public string ToArcadiaSftpDirectory { get; set; }
        public string MyBaseWebApiUrl { get; set; }
        public string EmailBaseWebApiUrl { get; set; }
        public string EmailSubject { get; set; }
        public string EmailFromAddress { get; set; }
        public string EmaileesString { get; set; }

    }
}
