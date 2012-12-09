using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLine
{
    class ExportOfferings : BaseExporter
    {
        string MAIN_QUERY =
            "SELECT VORGNO,AUSGNO,TXTNUMMER as name, ANLAGEZEIT as offering_date,PRINTTIM1 as print_date, CU_COMP.NAME as company_name " +
            "FROM PA_PAPER " +
            "JOIN CU_COMP on CU_COMP.CONO = PA_PAPER.ADDRNO " +
            "WHERE  " +
            "	TXTIDENT='Angebot' " +
            "	AND PA_PAPER.STATUS > 0";


        string SUB_QUERY =
            "SELECT POSTEXT as text, EPREIS as part_price, GPREIS as total_price, MENGE as amount, POSTNAME as partname, POSTART as lager_id, CDATE as created_at, CHDATE as changed_at "+
            "FROM PA_POSIT " +
            "WHERE VORGNO=$1 "+
            "AND AUSGNO=$2";

        public ExportOfferings(string conn, JsonWriter jsonWriter) : base(conn, jsonWriter)
        {                       
        }

        public override void Export()
        {
            Query(MAIN_QUERY, delegate(SqlDataReader reader)
            {                
                StartObj();
                int[] columns = new int[] {2, 3, 4, 5};
                foreach (int i in columns)
                {
                    Property(reader.GetName(i), reader, i);
                }
                

                jsonWriter.WritePropertyName("positions");
                StartArray();
                // todo, use prepared statement
                string query = SUB_QUERY.Replace("$1", reader.GetDecimal(0).ToString()).Replace("$2", reader.GetDecimal(1).ToString());
                
                
                Query(query, delegate(SqlDataReader subReader)
                {
                    StartObj();
                    for (int i = 0; i < subReader.FieldCount; i++)
                    {
                        Property(subReader.GetName(i), subReader, i);
                    }
                    EndObj();
                });

                EndArray();

                EndObj();
            });
        }
    }
}
