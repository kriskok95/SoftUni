using System;
using System.Data.SqlClient;
using InitialSetup;

namespace VillianNames
{
    class StartUp
    {
        static void Main()
        {
            using (SqlConnection connection = new SqlConnection(Configuration.connection))
            {
                connection.Open();

                string villianNames =
                    "SELECT v.Name, COUNT(mv.MinionId) AS minionsCount FROM Villains AS v JOIN MinionsVillains AS mv ON mv.VillainId = v.Id  GROUP BY v.Name  HAVING COUNT(mv.MinionId) > 3  ORDER BY minionsCount DESC";

                using (SqlCommand command = new SqlCommand(villianNames, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0]} - {reader[1]}");
                        }
                    }
                }        

                connection.Close();
            }
        }
    }
}
