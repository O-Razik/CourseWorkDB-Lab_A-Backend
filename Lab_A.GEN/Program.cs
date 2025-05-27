using Lab_A.GEN.Generators;
using Lab_A.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LabA.DAL.Data;

namespace Lab_A.GEN
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LabAContext>();
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=Lab-A;User Id=sa;Password=MSPass123;TrustServerCertificate=True;");

            var unitOfWork = new UnitOfWork(new LabAContext(optionsBuilder.Options));

            var dataGeneration = new DataGeneration(unitOfWork);
            await dataGeneration.Generate();
        }
    }
}