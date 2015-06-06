using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static SqlConnection connection = new SqlConnection(  @"Data Source=(localdb)\Projects;
                                                                Initial Catalog=Database1;
                                                                Integrated Security=True;
                                                                Connect Timeout=30;Encrypt=False;
                                                                TrustServerCertificate=False");
        static void Main(string[] args)
        {
            connection.Open();

            SqlCommand command = new SqlCommand
            {
                Connection = connection,
            };

            //using (var transaction = connection.BeginTransaction("Data init"))
            //{
            //    command.Transaction = transaction;
            //    var mergeCommandText = File.ReadAllText(@"..\..\..\Database1\Reference Data\Teams.sql").Replace("GO", "");
            //    command.CommandText = mergeCommandText;
            //    Console.WriteLine(command.ExecuteNonQuery() + " rows marged");
            //    command.CommandText = @"DELETE FROM [dbo].[Teams] WHERE [Name] = N'Team E';";
            //    var rowsDeleted = command.ExecuteNonQuery();
            //    Console.WriteLine(rowsDeleted + " rows with Team E, deleted");
            //    if (rowsDeleted > 0)
            //        transaction.Commit();
            //    else
            //        transaction.Rollback();
            //    command.Transaction = null;
            //}
            command.CommandText = "[dbo].[InitData]";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@delete", SqlDbType.Bit).Value = true;
            SqlParameter parametr;
            parametr = command.Parameters.Add(new SqlParameter("@deleted", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output,
                });
            command.ExecuteNonQuery();
            Console.WriteLine(parametr.Value + " rows with Team E deleted");
            command.CommandType = CommandType.Text;

            ReadData();

            command.CommandText =
                @"INSERT INTO [dbo].[Teams] ([Name], [Wins], [Losses], [Draws], [PictureUrl])
                  VALUES (@teamName, @wins, @losses, @draws, @url);";
            parametr = command.Parameters.AddWithValue("@teamName", "Team E");
            parametr = command.Parameters.AddWithValue("@wins", 7);
            parametr = command.Parameters.AddWithValue("@losses", 4);
            parametr = command.Parameters.AddWithValue("@draws", 6);
            parametr = command.Parameters.AddWithValue("@url", "Another URL");
/*            command.CommandText =
                @"INSERT INTO [dbo].[Teams] ([Id], [Name], [Wins], [Losses], [Draws], [PictureUrl])
                  VALUES (N'7E452A9A-A2D3-48DB-BC62-E9954D04F210', N'Team E', 7, 4, 6, N'Another URL')"; */
            Console.WriteLine(command.ExecuteNonQuery().ToString() + " rows inserted");
            command.Parameters.Clear();

            command.CommandText =
                @"UPDATE [dbo].[Teams]
                  SET [PictureUrl] = @url
                  WHERE [Id] = @id;";
            command.Parameters.AddWithValue("@url", "45463485L");
            command.Parameters.AddWithValue("@id", "da36bc84-b984-4554-aaaf-71a2a5c7058f");
/*            command.CommandText =
                @"UPDATE [dbo].[Teams]
                  SET [PictureUrl] = N'Updated URL'
                  WHERE [Id] = N'da36bc84-b984-4554-aaaf-71a2a5c7058f';";  */
            Console.WriteLine(command.ExecuteNonQuery().ToString() + " rows updated");
            command.Parameters.Clear();

            command.CommandText =
                @"DELETE FROM [dbo].[Teams]
                  WHERE [Id] = @id";
            command.Parameters.AddWithValue("@id", "31114623-5dd7-467f-926d-b4d56f07077f");
/*            command.CommandText =
                @"DELETE FROM [dbo].[Teams]
                  WHERE [Id] = N'31114623-5dd7-467f-926d-b4d56f07077f';";*/
            Console.WriteLine(command.ExecuteNonQuery().ToString() + " rows deleted");

            ReadData();
            Console.ReadKey();
        }

        private static void ReadData()
        {
            SqlCommand selectCommand = new SqlCommand("SELECT * FROM Teams", connection);
            using (SqlDataReader reader = selectCommand.ExecuteReader())
            {
                Console.WriteLine();
                while (reader.Read())
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Console.Write(reader[i]);
                        Console.Write(' ');
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}
