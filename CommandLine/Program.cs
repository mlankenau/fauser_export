using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.IO;
using CommandLine;

namespace CommandLine
{
    class Program
    {               
        static void Main(string[] args)
        {
            String connectionString =
                //"user id=mes;" +
                //"password=mes;" +
                "Trusted_Connection=True;" +                
                "server=marcus-pc\\SQLEXPRESS;" +
                "database=isdata; " +
                "connection timeout=5";

            Console.WriteLine("start exporting");
            try
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                JsonWriter writer = new JsonTextWriter(sw);
                writer.Formatting = Formatting.Indented;
                BaseExporter export = new ExportCustomer(connectionString, writer);
                //export.Export();
                export = new ExportParts(connectionString, writer);
                //export.Export();
                export = new ExportOfferings(connectionString, writer);
                export.Export();
                    
                writer.Close();
                sw.Close();
                Console.WriteLine(sb.ToString());

            }
            catch (Exception e)
            {
                Console.WriteLine("ohh shit: " + e.ToString());
            }
        }
    }



}
