using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikuliStandard.sikuli_JSON
{
    public class json_GetText
    {
        public json_Pattern jPattern { get; set; }
        public bool highlight { get; set; }

        public json_GetText(json_Pattern pattrn, bool hghlght = false)
        {
            jPattern = pattrn;
            highlight = hghlght;
        }
    }
}
