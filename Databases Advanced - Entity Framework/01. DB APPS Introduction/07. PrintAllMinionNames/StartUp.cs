using InitialSetup;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PrintAllMinionNames
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            List<string> names = new List<string>();

            using(SqlConnection connection = new SqlConnection(Configuration.connection))
            {
                connection.Open();

                string sqlQuery = "SELECT name FROM Minions";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            names.Add((string)reader[0]);
                        }
                    }
                }
            }

            for (int i = 0; i < names.Count / 2; i++)
            {
                Console.WriteLine(names[i]);
                Console.WriteLine(names[names.Count - 1 - i]);
            }

            if (names.Count % 2 != 0)
            {
                Console.WriteLine(names[names.Count / 2]);
            }
        }
    }
}
