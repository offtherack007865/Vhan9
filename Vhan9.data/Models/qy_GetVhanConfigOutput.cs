using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vhan9.data.Models
{
    public class qy_GetVhanConfigOutput
    {
        public qy_GetVhanConfigOutput()
        {
            IsOk = true;
            ErrorMessage = string.Empty;
            qy_GetVhanConfigOutputColumnsList =
                new List<qy_GetVhanConfigOutputColumns>();
        }
        public bool IsOk { get; set; }
        public string ErrorMessage { get; set; }
        public List<qy_GetVhanConfigOutputColumns>
            qy_GetVhanConfigOutputColumnsList
            { get; set; }
    }
}
