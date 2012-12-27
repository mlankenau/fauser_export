using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLine
{
    class ExportOfferings: ExportPaper
    {
        public ExportOfferings(string conn, JsonWriter jsonWriter)
            : base(conn, jsonWriter)
        {                       
        }

        public override string GetPaperName()
        {
            return "Angebot";
        }
        public override string GetPrefix()
        {
            return "offering";
        }
    }
}
