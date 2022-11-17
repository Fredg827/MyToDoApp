using Microsoft.EntityFrameworkCore;
using ToDoApp.DataAccess.DomainModels;

namespace ToDoApp.DataAccess
{
    public class ToDoContext : DbContext
    {
        public DbSet<DomainToDoItem> ToDoItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer
                ("Server=localhost\\SQLEXPRESS;Database=ToDoApp;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}