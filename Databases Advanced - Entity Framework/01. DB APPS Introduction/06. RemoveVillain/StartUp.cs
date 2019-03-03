using System;
using System.Data.SqlClient;
using InitialSetup;

namespace RemoveVillain
{
    class StartUp
    {
        static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());
            string villainName = "";

            using (SqlConnection connection = new SqlConnection(Configuration.connection))
            {
                connection.Open();

                string villainQuery = "SELECT Name FROM Villains WHERE Id = @villainId";

                using (SqlCommand command = new SqlCommand(villainQuery, connection))
                {
                    command.Parameters.AddWithValue("@villainId", villainId);
                    villainName = (string)command.ExecuteScalar();

                    if (villainName == null)
                    {
                        Console.WriteLine("No such villain was found.");
                        return;
                    }
                }

                int affectedRows = DeleteFromMinionVillains(connection, villainId);
                DeleteFromVillains(connection, villainId);
                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{affectedRows} minions were released.");
            }
        }

        private static void DeleteFromVillains(SqlConnection connection, int villainId)
        {
            string delteVillainQuery = @"DELETE FROM Villains
            WHERE Id = @villainId";

            using (SqlCommand command = new SqlCommand(delteVillainQuery, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);
                command.ExecuteNonQuery();
            }

            using (SqlCommand command = new SqlCommand(delteVillainQuery, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);
                command.ExecuteNonQuery();
            }
        }

        private static int DeleteFromMinionVillains(SqlConnection connection, int villainId)
        {
            string delteVillainQuery = "DELETE FROM MinionsVillains WHERE VillainId = @villainId";

            using (SqlCommand command = new SqlCommand(delteVillainQuery, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);
                return command.ExecuteNonQuery();
            }
        }
    }
}
