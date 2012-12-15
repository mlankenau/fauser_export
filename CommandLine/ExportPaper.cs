using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLine
{
    abstract class ExportPaper : BaseExporter
    {
        string MAIN_QUERY =
            "SELECT VORGNO,AUSGNO,TXTNUMMER as name, ANLAGEZEIT as offering_date,PRINTTIM1 as print_date, CU_COMP.NAME as company_name, PA_PAPER.CDATE as created_at, PA_PAPER.CHDATE as updated_at " +
            "FROM PA_PAPER " +
            "JOIN CU_COMP on CU_COMP.CONO = PA_PAPER.ADDRNO " +
            "WHERE  " +
            "	TXTIDENT='#PAPER_NAME#' " +
            "	AND PA_PAPER.STATUS > 0";


        string SUB_QUERY =
            "SELECT POSTEXT as text, EPREIS as part_price, GPREIS as total_price, MENGE as amount, POSTNAME as partname, POSTART as lager_id, CDATE as created_at, CHDATE as updated_at "+
            "FROM PA_POSIT " +
            "WHERE VORGNO=$1 "+
            "AND AUSGNO=$2";

        public ExportPaper(string conn, JsonWriter jsonWriter) : base(conn, jsonWriter)
        {                       
        }

        public abstract string GetPaperName();

        protected string GetMainQuery() 
        {
            return MAIN_QUERY.Replace("#PAPER_NAME#", GetPaperName());
        }

        public override void Export()
        {
            StartArray();
            Query(GetMainQuery(), delegate(SqlDataReader reader)
            {                
                StartObj();
                int[] columns = new int[] {2, 3, 4, 5, 6, 7};
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
            EndArray();
        }
    }
}
