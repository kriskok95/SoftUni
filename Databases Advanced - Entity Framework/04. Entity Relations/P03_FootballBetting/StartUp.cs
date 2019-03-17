using System;
using P03_FootballBetting.Data;

namespace P03_FootballBetting
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (FootballBettingContext dbContext = new FootballBettingContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Database.EnsureDeleted();
            }
        }
    }
}
