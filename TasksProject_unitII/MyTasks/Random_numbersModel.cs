using MySql.Data.MySqlClient;



class Random_numbersModel
{
    string connectionCredentials { get; set; }
    MySqlConnection connection { get; set; }
    Config config { get; set; }

    public Random_numbersModel()
    {
        this.config = new Config();
        this.connectionCredentials = $"server={config.host};port={config.port};database={config.dataBase};uid={config.user};password={config.passwrod};";
        this.connection = new MySqlConnection(connectionCredentials);
    }


    public async Task<List<AllDataContainer>> getAllNumbers()
    {
        List<AllDataContainer> rows = new List<AllDataContainer>();
        try
        {
            await this.connection.OpenAsync();

            // Connection opened successfully
            string sql = "SELECT * FROM random_numbers";
            MySqlCommand command = new MySqlCommand(sql, this.connection);
            MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                rows.Add(new AllDataContainer(reader.GetInt32("id"), reader.GetInt32("myRandomNumber")));
            }

            reader.Close();
        }
        catch (MySqlException e)
        {
            // Handle exception
            Console.WriteLine($"there was something wrong with the model connection: {e}");
        }
        finally
        {
            await this.connection.CloseAsync();
        }
        return rows;
    }

    public async Task deleteAllNumbers(){
        try
        {
            await this.connection.OpenAsync();

            // Connection opened successfully
            string sql = "DELETE FROM random_numbers";
            MySqlCommand command = new MySqlCommand(sql, this.connection);

            int affectedRouws = await command.ExecuteNonQueryAsync();
            Console.WriteLine($"The number of rows deleted is: {affectedRouws}");
        }
        catch (Exception e)
        {
            // Handle exception
            Console.WriteLine($"there was something wrong with the model connection: {e}");
        }
        finally
        {
            await this.connection.CloseAsync();
        }

    }

}