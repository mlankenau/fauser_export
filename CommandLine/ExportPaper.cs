using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommandLine
{
    abstract class ExportPaper : BaseExporter
    {
        string MAIN_QUERY =
            "SELECT VORGNO,AUSGNO,TXTNUMMER as visual_id, ANLAGEZEIT as PREFIX_date,PRINTTIM1 as print_date, CU_COMP.NAME as customer, PA_PAPER.CDATE as created_at, PA_PAPER.CHDATE as updated_at, ORDERNO as customer_mark " +
            "FROM PA_PAPER " +
            "JOIN CU_COMP on CU_COMP.CONO = PA_PAPER.ADDRNO " +
            "WHERE  " +
            "	TXTIDENT='#PAPER_NAME#' " +
            "	AND PA_PAPER.STATUS > 0";


        string SUB_QUERY =
            "SELECT POSTEXT as text, EPREIS as part_price, GPREIS as total_price, MENGE as amount, POSTNAME as partname, POSTART as lager_id, POSDAT as delivery_date, CDATE as created_at, CHDATE as updated_at, POSLIEF0 as other_delivered, POSINF " +
            "FROM PA_POSIT " +
            "WHERE VORGNO=$1 "+
            "AND AUSGNO=$2";

        public ExportPaper(string conn, JsonWriter jsonWriter) : base(conn, jsonWriter)
        {                       
        }

        public abstract string GetPaperName();
        public abstract string GetPrefix();

        protected string GetMainQuery() 
        {
            return MAIN_QUERY.Replace("#PAPER_NAME#", GetPaperName()).Replace("PREFIX", GetPrefix());
        }

        public override void Export()
        {
            StartArray();
            Query(GetMainQuery(), delegate(SqlDataReader reader)
            {                
                StartObj();
                int[] columns = new int[] {2, 3, 4, 5, 6, 7, 8};
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
                        if (subReader.GetName(i) == "POSINF")
                        {
                            if (subReader.GetString(i) != "")
                            
                            {
                                //                                              Bestell-Nr. 4500096548
                                // Regex.Match(subReader.GetString(i), @"Bestell-Nr\. ([0-9]+)", RegexOptions.IgnoreCase)
                                Match m = Regex.Match(subReader.GetString(i), @"Bestell-Nr\. ([0-9]*)", RegexOptions.IgnoreCase);
                                if (m.Success)
                                {
                                    Property("customer_mark", m.Groups[1].Value);
                                }
                            }
                        }
                        else
                        {
                            Property(subReader.GetName(i), subReader, i);
                        }
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
