using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using InitialSetup;

namespace MinionNames
{
    class StartUp
    {
        static void Main()
        {
            int villianId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.connection))
            {
                connection.Open();

                string villianName = GetVillianName(connection, villianId);

                if (villianName == null)
                {
                    Console.WriteLine($"No villain with ID {villianId} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villain: {villianName}");
                    PrintNames(connection, villianId);
                }

                connection.Close();
            }
        }

        private static void PrintNames(SqlConnection connection, int villianId)
        {
            string minionsNamesSql =
                "SELECT m.Name, m.Age FROM Minions AS m JOIN MinionsVillains AS mv ON mv.MinionId = m.Id WHERE mv.VillainId = @Id";

            using (SqlCommand command = new SqlCommand(minionsNamesSql, connection))
            {
                command.Parameters.AddWithValue("@Id", villianId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    int rowNumber = 1;
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("(no minions)");
                    }
                    while (reader.Read())
                    {
                        Console.WriteLine($"{rowNumber++} {reader[0]} {reader[1]}");
                    }
                }
            }
        }

        private static string GetVillianName(SqlConnection connection, int villianId)
        {

            string villianNameSql = "SELECT v.Name FROM Villains AS v WHERE v.Id = @Id";

            using (SqlCommand command = new SqlCommand(villianNameSql, connection))
            {
                command.Parameters.AddWithValue("@Id", villianId);
                return (string)command.ExecuteScalar();
            }
        }
    }
}
