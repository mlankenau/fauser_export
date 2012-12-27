using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLine
{
    class ExportOrders : ExportPaper
    {
        

        public ExportOrders(string conn, JsonWriter jsonWriter) : base(conn, jsonWriter)
        {                       
        }

        public override string GetPaperName()
        {
            return "Auftragsbestätigung";
        }

        public override string GetPrefix()
        {
            return "order";
        }


    }
}
