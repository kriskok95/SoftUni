using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using InitialSetup;

namespace ChangeTownNamesCasing
{
    class StartUp
    {
        static void Main(string[] args)
        {
            string countryForSearch = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(Configuration.connection))
            {
                connection.Open();

                UpdateTowns(connection, countryForSearch);

                PrintResult(connection, countryForSearch);
            }
        }

        private static void UpdateTowns(SqlConnection connection, string countryForSearch)
        {
            string updateTowns = @"UPDATE Towns
               SET Name = UPPER(Name)
               WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

            using (SqlCommand command = new SqlCommand(updateTowns, connection))
            {
                command.Parameters.AddWithValue("@countryName", countryForSearch);
                int rowsAffected = command.ExecuteNonQuery();

            }
        }

        private static void PrintResult(SqlConnection connection, string countryForSearch)
        {
            string selectTowns = @"SELECT t.Name 
                    FROM Towns as t
                    JOIN Countries AS c ON c.Id = t.CountryCode
                    WHERE c.Name = @countryName";

            using (SqlCommand command = new SqlCommand(selectTowns, connection))
            {
                command.Parameters.AddWithValue("@countryName", countryForSearch);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<string> towns = new List<string>();

                    while (reader.Read())
                    {
                        towns.Add((string)reader[0]);
                    }

                    if (towns.Count == 0)
                    {
                        Console.WriteLine("No town names were affected.");
                    }
                    else
                    {
                        Console.WriteLine($"{towns.Count} town names were affected.");
                        Console.WriteLine($"[{string.Join(", ", towns)}]");
                    }
                }
            }
        }
    }
}
