﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLine
{
    abstract class BaseExporter
    {
        //protected SqlConnection conn;
        //protected SqlConnection conn2; 
        protected string connectionString;
        protected JsonWriter jsonWriter;

        public delegate void Process(SqlDataReader reader);

        public BaseExporter(string connectionString, JsonWriter jsonWriter)
        {
            this.connectionString = connectionString;
            this.jsonWriter = jsonWriter;
        }


        protected void Query(string sql, Process process)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand myCommand = new SqlCommand(sql, conn);
                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {
                    while (myReader.Read())
                    {
                        process(myReader);
                    }
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
            jsonWriter.WriteWhitespace("\r\n");
        }

        protected void StartArray()
        {
            jsonWriter.WriteStartArray();
        }

        protected void EndArray()
        {
            jsonWriter.WriteEndArray();
        }

        protected void Property(string key, string value)
        {
            jsonWriter.WritePropertyName(key);
            jsonWriter.WriteValue(value);
        }

        public void Property(string key, SqlDataReader reader, int idx)
        {
            jsonWriter.WritePropertyName(key);
            string typeName = reader.GetDataTypeName(idx);
            if (reader.IsDBNull(idx))
                jsonWriter.WriteNull();
            else if (typeName == "decimal")
                jsonWriter.WriteValue(reader.GetDecimal(idx));            
            else if (typeName == "datetime")
                jsonWriter.WriteValue(reader.GetDateTime(idx));
            else if (typeName == "varchar" || typeName == "nvarchar")
                jsonWriter.WriteValue(reader.GetString(idx));
            else
                throw new Exception("Unknown data type " + typeName);
        }

        public abstract void Export();
    }
}
