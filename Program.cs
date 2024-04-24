using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.ComponentModel.DataAnnotations;

const string connectionString = @"Data Source=..\..\..\UPO.db;";

DbContextOptions<DataContext> options = new DbContextOptionsBuilder<DataContext>()
    .UseSqlite(connectionString)
    .Options;

using var db = new DataContext(options);
//await db.Database.EnsureCreatedAsync();

//await Insert();
//await Query();
//await Update();
//await Delete();

async Task Insert()
{
    db.Studenti.Add(new Studente
    {
        Matricola = 123456,
        Nome = "Mario",
        Cognome = "Rossi"
    });

    db.Studenti.Add(new Studente
    {
        Matricola = 654321,
        Nome = "Luigi",
        Cognome = "Verdi"
    });

    await db.SaveChangesAsync();
}

async Task Query()
{
    var studenti = await db.Studenti.ToListAsync();
    foreach (var studente in studenti) Console.WriteLine(studente);
}

async Task Update()
{
    var studente = await db.Studenti.FindAsync((long)123456);
    if (studente is not null)
    {
        studente.Nome = "Giovanni";
        await db.SaveChangesAsync();
    }
}

async Task Delete()
{
    var studente = await db.Studenti.FindAsync((long)123456);
    if (studente is not null)
    {
        db.Studenti.Remove(studente);
        await db.SaveChangesAsync();
    }
}

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    { }

    public DbSet<Studente> Studenti => Set<Studente>();
}

public record Studente
{
    [Key]
    public long Matricola { get; set; }

    required public string Nome { get; set; }

    required public string Cognome { get; set; }

    //public double MediaVoti { get; set; }
}

public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlite("Data Source=blog.db");

        return new DataContext(optionsBuilder.Options);
    }
}