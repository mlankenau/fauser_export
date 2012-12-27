using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLine
{
    class ExportDeliveries : ExportPaper
    {

        public ExportDeliveries(string conn, JsonWriter jsonWriter)
            : base(conn, jsonWriter)
        {                       
        }

        public override string GetPaperName()
        {
            return "Lieferschein";
        }

        public override string GetPrefix()
        {
            return "delivery";
        }
    }
}
