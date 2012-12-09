using ConsoleApplication1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLine
{
    class ExportParts : BaseExporter
    {
        string MAIN_QUERY = 
            "SELECT "+
	        "  NAME as name, DESCR AS description, DRAWNO AS drawing_name, IDENT AS part_name, INFO1 as material, INFO2 as dimensions, INFO3 as info3, "+
	        "  PREIS1 as price1, PREIS2 as priece2, PREIS3 as price3, PREIS4 as price4, PREIS5 as price5, PREIS6 as price6, MENGE1 as amount1, MENGE2 as amount2, MENGE3 as amount3, MENGE4 amount4, MENGE5 as amount5, MENGE6 as amount6, DRAWIND as version, "+
	        "  CDATE as created_ad, CHDATE as changed_at "+
            "FROM OR_ORDER "+
            "WHERE STATUS <> 2";

        public ExportParts(SqlConnection conn, JsonWriter jsonWriter)
            : base(conn, jsonWriter)
        {                       
        }
        
        public override void Export()
        {
            Query(MAIN_QUERY, delegate(SqlDataReader reader)
            {
                StartObj();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Property(reader.GetName(i), reader, i);
                }
                EndObj();
            });
        }
    }
}
