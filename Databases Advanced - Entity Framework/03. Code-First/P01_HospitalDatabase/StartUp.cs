using P01_HospitalDatabase.Data;

namespace P01_HospitalDatabase
{
    using System;

    public class StartUp
    {
        static void Main(string[] args)
        {
            using (HospitalContext context = new HospitalContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }
}
