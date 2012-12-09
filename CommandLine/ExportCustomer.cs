using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class ExportCustomer : BaseExporter
    {
        string MAIN_QUERY = 
            "SELECT CONO, NAME, ADDITION, STREET, BOXNO, POSTCODE, PLACE, PHONE, MODEM, FAX, COTYPNO, [STATUS], CUSTNO, LIEFERB, ZAHLUNGB, VATIDNO, SUPPLIER, CDATE, CHDATE " +
            "FROM CU_COMP";



        public ExportCustomer(SqlConnection conn, JsonWriter jsonWriter) : base(conn, jsonWriter)
        {
                       
        }

        public void Export()
        {
            Query(MAIN_QUERY, delegate(SqlDataReader reader)
            {
                StartObj();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string name = reader.GetName(i);
                    string value = reader.GetValue(i).ToString() ;                    
                    Property(name, value);
                }
                EndObj();
            });
        }
    }
}
