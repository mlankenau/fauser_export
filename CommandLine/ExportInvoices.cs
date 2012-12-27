using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLine
{
    class ExportInvoices : ExportPaper
    {

        public ExportInvoices(string conn, JsonWriter jsonWriter)
            : base(conn, jsonWriter)
        {                       
        }

        public override string GetPaperName()
        {
            return "Rechnung";
        }

        public override string GetPrefix()
        {
            return "invoice";
        }
    }
}
