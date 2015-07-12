using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\ProjectsV12;Initial Catalog=Database2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        static SqlCommand selectCommand = new SqlCommand("SELECT * FROM Teams", connection);

        static void Main(string[] args)
        {
            connection.Open();
            SqlCommand command = new SqlCommand
            {
                Connection = connection,
            };

            //using (var transaction = connection.BeginTransaction("Data initialization"))
            //{
            //    command.Transaction = transaction;
            //    command.CommandText = File.ReadAllText(@"..\..\..\Database2\Reference Data\Teams.sql").Replace("GO", "");
            //    Console.WriteLine(command.ExecuteNonQuery() + " rows merged");
            //    command.CommandText =
            //        @"DELETE FROM [dbo].[Teams]
            //      WHERE [Name] = N'Team E';";
            //    var rowsDeleted = command.ExecuteNonQuery();
            //    Console.WriteLine(rowsDeleted + " rows with Team E deleted");
            //    if (rowsDeleted > 0)
            //        transaction.Commit();

            //    else
            //        transaction.Rollback();
            //    command.Transaction = null;
            //}
            command.CommandText = "[dbo].[InitData]";
            command.CommandType = CommandType.StoredProcedure;
            //command.Parameters.Add("@delete", SqlDbType.Bit).Value = true;
            SqlParameter parameter;
            parameter = command.Parameters.Add(new SqlParameter("@deleted", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            });
            command.ExecuteNonQuery();
            //Console.WriteLine(parameter.Value + " rows with Team E deleted");

            Console.WriteLine();
            ReadData();
            Console.WriteLine();

            command.CommandType = CommandType.Text;
            command.CommandText =
                @"INSERT INTO [dbo].[Teams] ([Name], [Wins], [Losses], [Draws], [PictureUrl])
                  VALUES (@teamName, @wins, @Losses, @draws, @url);";
            parameter = command.Parameters.AddWithValue("@teamName", "Team E");
            parameter = command.Parameters.AddWithValue("@wins", 7);
            parameter = command.Parameters.AddWithValue("@losses", 4);
            parameter = command.Parameters.AddWithValue("@draws", 6);
            parameter = command.Parameters.AddWithValue("@url", "Another URL");
            Console.WriteLine(command.ExecuteNonQuery() + " rows inserted");
            command.Parameters.Clear();

            command.CommandText =
                @"UPDATE [dbo].[Teams]
                  SET [PictureUrl] = @url
                  WHERE [Id] = @id;";
            parameter = command.Parameters.AddWithValue("@id", "578595bc-f18c-4ba3-9fbc-10e8b34075cf");
            parameter = command.Parameters.AddWithValue("@url", "Updated URL");
            Console.WriteLine(command.ExecuteNonQuery() + " rows updated");
            command.Parameters.Clear();

            command.CommandText =
                @"DELETE FROM [dbo].[Teams]
                  WHERE [Id] = @id;";
            parameter = new SqlParameter("@id", "e75d4311-9479-408e-84d3-38d601765e4c");
            command.Parameters.Add(parameter);
            Console.WriteLine(command.ExecuteNonQuery() + " rows deleted");
            command.Parameters.Clear();
            Console.WriteLine();
            ReadData();
            Console.ReadKey();
        }

        private static void ReadData()
        {
            using (SqlDataReader reader = selectCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Console.Write(reader[i]);
                        Console.Write(' ');
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
