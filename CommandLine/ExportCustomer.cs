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
            "SELECT CONO as forsa_id, NAME as name, ADDITION as name2, STREET as street, BOXNO, POSTCODE as zip, PLACE as city, PHONE as phone, MODEM as web, FAX as fax, COTYPNO, CUSTNO customer_no, LIEFERB as delivery_conditions, ZAHLUNGB as payment_conditions, VATIDNO vat_id, SUPPLIER, CDATE as created_at, CHDATE as changed_at " +
            "FROM CU_COMP " +
            "WHERE STATUS = 0";



        public ExportCustomer(SqlConnection conn, JsonWriter jsonWriter) : base(conn, jsonWriter)
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
