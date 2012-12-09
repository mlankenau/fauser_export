using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class BaseExporter
    {
        SqlConnection conn; 
        JsonWriter jsonWriter;

        public delegate void Process(SqlDataReader reader);

        public BaseExporter(SqlConnection conn, JsonWriter jsonWriter)
        {
            this.conn = conn;
            this.jsonWriter = jsonWriter;
        }


        protected void Query(string sql, Process process)
        {
            SqlCommand myCommand = new SqlCommand(sql, conn);
            using (SqlDataReader myReader = myCommand.ExecuteReader())
            {
                while (myReader.Read())
                {
                    process(myReader);
                }
            }
        }

        protected void StartObj()
        {
            jsonWriter.WriteStartObject();
        }

        protected void EndObj()
        {
            jsonWriter.WriteEndObject();
        }

        protected void Property(string key, string value)
        {
            jsonWriter.WritePropertyName(key);
            jsonWriter.WriteValue(value);
        }
    }
}
