using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection( @"Data Source=(localdb)\Projects;
                                                            Initial Catalog=Database1;
                                                            Integrated Security=True;
                                                            Connect Timeout=30;Encrypt=False;
                                                            TrustServerCertificate=False");
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Teams", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < 6; i++)
                {
                    Console.Write(reader[i]);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
