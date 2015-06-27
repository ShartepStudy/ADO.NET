using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SqlConnection connection;
        private readonly SqlCommand selectCommand;

        public MainWindow()
        {
            InitializeComponent();

            connection = new SqlConnection(@"Data Source=(localdb)\Projects;Initial Catalog=Database2;Integrated Security=True;Pooling=False;Connect Timeout=30");
            selectCommand = new SqlCommand("SELECT * FROM Teams", connection);
        }

        private async void ButtonClearClick(object sender, RoutedEventArgs e)
        {
            WriteInfo("Cleaning DB");
            SqlCommand command = null;
            SqlParameter parameter = null;

            var openTask = connection.OpenAsync();
            var compositTask = openTask.ContinueWith(t =>
            {
                command = new SqlCommand
                {
                    Connection = connection,
                };

                command.CommandText = "[dbo].[InitData]";
                command.CommandType = CommandType.StoredProcedure;

                parameter = command.Parameters.Add(new SqlParameter("@deleted", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                });
            });
            await compositTask;
            await command.ExecuteNonQueryAsync();

            resultTextBlock.Inlines.Add(new Run("Rows deleted " + parameter.Value));
            resultTextBlock.Inlines.Add(new LineBreak());

            connection.Close();
            WriteInfo("Cleared DB");
        }

        private async void ButtonLoadClick(object sender, RoutedEventArgs e)
        {
            WriteInfo("Before starting opening connection");
            Task openTask = connection.OpenAsync();
            WriteInfo("Connection opening");
            await openTask;
            WriteInfo("Connection open"); 
            SqlCommand command = new SqlCommand
            {
                Connection = connection,
            };

            await ReadDataAsync();

            connection.Close();
        }

        private async void ButtonChangeClick(object sender, RoutedEventArgs e)
        {
            await connection.OpenAsync();
            var command = new SqlCommand
            {
                Connection = connection,
            };

            WriteInfo("Inserting");
            command.CommandText =
                @"WAITFOR DELAY '0:0:03';
                  INSERT INTO [dbo].[Teams] ([Name], [Wins], [Losses], [Draws], [PictureUrl])
                  VALUES (@teamName, @wins, @Losses, @draws, @url);";
            SqlParameter parameter;
            parameter = command.Parameters.AddWithValue("@teamName", "Team E");
            parameter = command.Parameters.AddWithValue("@wins", 7);
            parameter = command.Parameters.AddWithValue("@losses", 4);
            parameter = command.Parameters.AddWithValue("@draws", 6);
            parameter = command.Parameters.AddWithValue("@url", "Another URL");
            resultTextBlock.Inlines.Add(new Run(command.ExecuteNonQuery() + " rows inserted"));
            resultTextBlock.Inlines.Add(new LineBreak());
            command.Parameters.Clear();
            WriteInfo("Inserted");

            WriteInfo("Updating");
            command.CommandText =
                @"WAITFOR DELAY '0:0:03';
                  UPDATE [dbo].[Teams]
                  SET [PictureUrl] = @url
                  WHERE [Id] = @id;";
            parameter = command.Parameters.AddWithValue("@id", "578595bc-f18c-4ba3-9fbc-10e8b34075cf");
            parameter = command.Parameters.AddWithValue("@url", "Updated URL");
            resultTextBlock.Inlines.Add(new Run(await command.ExecuteNonQueryAsync() + " rows updated"));
            resultTextBlock.Inlines.Add(new LineBreak());
            command.Parameters.Clear();
            WriteInfo("Update");

            WriteInfo("Deleting");
            command.CommandText =
                @"WAITFOR DELAY '0:0:03';
                  DELETE FROM [dbo].[Teams]
                  WHERE [Id] = @id;";
            parameter = new SqlParameter("@id", "e75d4311-9479-408e-84d3-38d601765e4c");
            command.Parameters.Add(parameter);
            resultTextBlock.Inlines.Add(new Run(await command.ExecuteNonQueryAsync() + " rows deleted"));
            resultTextBlock.Inlines.Add(new LineBreak());
            command.Parameters.Clear();
            WriteInfo("Deleted");

            connection.Close();
        }

        private async Task ReadDataAsync()
        {
            WriteInfo("Read started");
            StringBuilder sb = new StringBuilder();
            using (SqlDataReader reader = await selectCommand.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    for (int i = 0; i < 6; i++)
                    {
                        sb.Append(reader[i]);
                        sb.Append('\t');
                    }
                    resultTextBlock.Inlines.Add(new Run(sb.ToString()));
                    resultTextBlock.Inlines.Add(new LineBreak());
                    sb.Clear();
                }
            }
            WriteInfo("Read finished");
        }


        private void WriteInfo(string action)
        {
            infoTextBlock.Inlines.Add(
                new Run(action + ' ' + DateTime.Now.ToString("mm:ss.ffff")));
            infoTextBlock.Inlines.Add(new LineBreak());
        }
    }
}
