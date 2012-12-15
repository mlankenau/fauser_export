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
            "	OR_ORDER.NAME as name, DESCR AS description, DRAWNO AS drawing_name, IDENT AS part_name, CU_COMP.NAME as customer, OR_ORDER.INFO1 as material, OR_ORDER.INFO2 as dimensions, INFO3 as info3, "+
            "	PREIS1 as price1, PREIS2 as price2, PREIS3 as price3, PREIS4 as price4, PREIS5 as price5, PREIS6 as price6, MENGE1 as amount1, MENGE2 as amount2, MENGE3 as amount3, MENGE4 amount4, MENGE5 as amount5, MENGE6 as amount6, DRAWIND as version,"+
            "	OR_ORDER.CDATE as created_ad, OR_ORDER.CHDATE as updated_at "+
            "FROM OR_ORDER "+
            "LEFT JOIN CU_COMP ON OR_ORDER.KCONO = CU_COMP.CONO "+
            "WHERE OR_ORDER.STATUS <> 2";


        public ExportParts(String connectionString, JsonWriter jsonWriter)
            : base(connectionString, jsonWriter)
        {                       
        }
        
        public override void Export()
        {
            StartArray();
            Query(MAIN_QUERY, delegate(SqlDataReader reader)
            {
                StartObj();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Property(reader.GetName(i), reader, i);
                }
                EndObj();
            });
            EndArray();
        }
    }
}
