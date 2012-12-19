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
                using (FileStream fs = new FileStream("\\\\VBOXSVR\\Shared\\export.json", FileMode.Create, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    JsonWriter writer = new JsonTextWriter(sw);
                    writer.Formatting = Formatting.Indented;

                    writer.WriteStartObject();

                    writer.WritePropertyName("customers");
                    BaseExporter export = new ExportCustomer(connectionString, writer);
                    export.Export();


                    writer.WritePropertyName("parts");
                    export = new ExportParts(connectionString, writer);
                    export.Export();

                    writer.WritePropertyName("offerings");
                    export = new ExportOfferings(connectionString, writer);
                    export.Export();

                    writer.WritePropertyName("orders");
                    export = new ExportOrders(connectionString, writer);
                    export.Export();

                    writer.WritePropertyName("deliveries");
                    export = new ExportDeliveries(connectionString, writer);
                    export.Export();
                    writer.WritePropertyName("invoices");
                    export = new ExportInvoices(connectionString, writer);
                    export.Export();

                    writer.WriteEndObject();

                    writer.Close();
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ohh shit: " + e.ToString());
            }
        }
    }



}
