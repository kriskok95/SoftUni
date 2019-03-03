using System;
using System.Data.SqlClient;
using InitialSetup;

namespace AddMinion
{
    class StartUp
    {
        static void Main()
        {
            string[] inputMinion = Console.ReadLine().Split();
            string[] inputVillain = Console.ReadLine().Split();

            string minionName = inputMinion[1];
            int minionAge = int.Parse(inputMinion[2]);
            string minionTown = inputMinion[2];

            string villainName = inputVillain[1];

            using (SqlConnection connection = new SqlConnection(Configuration.connection))
            {
                connection.Open();
                
                int townId = CheckTownAndGetId(connection, minionTown);

                CheckVillain(connection, villainName);          

                AddMinionToDb(connection, minionName, minionAge, townId, villainName);
                    
                connection.Close();
            }
        }

        private static void AddMinionToDb(SqlConnection connection, string minionName, int minionAge, int townId, string villainName)
        {
            string addMinionSql = "INSERT INTO Minions(Name, Age, TownId) VALUES (@name, @age, @townId)";

            using (SqlCommand command = new SqlCommand(addMinionSql, connection))
            {
                command.Parameters.AddWithValue("@name", minionName);
                command.Parameters.AddWithValue("@age", minionAge);
                command.Parameters.AddWithValue("@townId", townId);
                command.ExecuteNonQuery();

                ConnectMinionToVillain(minionName, villainName, connection);

            }
        }

        private static void ConnectMinionToVillain(string minionName, string villainName, SqlConnection connection)
        {
            int minnionId = GetMinionId(minionName, connection);
            int villainId = GetVillainId(villainName, connection);

            string connectMinionVillainSql =
                "INSERT INTO MinionsVillains(MinionId, VillainId) VALUES (@minionId, @villainId)";

            using (SqlCommand command = new SqlCommand(connectMinionVillainSql, connection))
            {
                command.Parameters.AddWithValue("@minionId", minnionId);
                command.Parameters.AddWithValue("@villainId", villainId);
                command.ExecuteNonQuery();
                Console.WriteLine($"Successfully added <{minionName}> to be minion of <{villainName}>.");
            }
        }

        private static int GetVillainId(string villainName, SqlConnection connection)
        {
            string villainIdSql = "SELECT Id FROM villains WHERE Name = @villainName";

            using (SqlCommand command = new SqlCommand(villainIdSql, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);

                return (int)command.ExecuteScalar();
            }
        }

        private static int GetMinionId(string minionName, SqlConnection connection)
        {
            string minionIdSql = "SELECT Id FROM Minions WHERE Name = @minionName";

            using (SqlCommand command = new SqlCommand(minionIdSql, connection))
            {
                command.Parameters.AddWithValue("@minionName", minionName);
                
                return (int)command.ExecuteScalar();
            }
        }

        private static void CheckVillain(SqlConnection connection, string villainName)
        {
            string villainSql = "SELECT Name FROM Villains WHERE Name = @villainName";

            using (SqlCommand command = new SqlCommand(villainSql, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);
                if (command.ExecuteScalar() == null)
                {
                    AddVillain(connection, villainName);
                }
            }
        }

        private static void AddVillain(SqlConnection connection, string villainName)
        {
            string insertVillain = "INSERT INTO Villains(Name, EvilnessFactorId) VALUES (@villainName, 4)";

            using (SqlCommand command = new SqlCommand(insertVillain, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);
                command.ExecuteNonQuery();
                Console.WriteLine($"Villain <{villainName}> was added to the database");
            }
        }

        private static int CheckTownAndGetId(SqlConnection connection, string minionTown)
        {
            string townSql = "SELECT Id FROM Towns WHERE Name = @townName";

            using (SqlCommand command = new SqlCommand(townSql, connection))
            {
                command.Parameters.AddWithValue("@townName", minionTown);
                if (command.ExecuteScalar() == null)
                {
                    AddTown(connection, minionTown);
                }
                return (int) command.ExecuteScalar();
            }     
        }

        private static void AddTown(SqlConnection connection, string minionTown)
        {
            string insertTown = "INSERT INTO Towns(Name) VALUES (@townName)";

            using (SqlCommand command = new SqlCommand(insertTown, connection))
            {
                command.Parameters.AddWithValue("@townName", minionTown);
                command.ExecuteNonQuery();
                Console.WriteLine($"Town <{minionTown}> was added to the database.");
            }
        }
    }
}
