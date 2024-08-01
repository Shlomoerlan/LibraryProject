using LibraryProject.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Data
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }

        public void Seed()
        {
            if(! Librarys.Any())
            {
                LibraryModel library = new()
                {
                    Genre = ("Torah"),
                    Shelves = [
                        new ()
                        {
                            Width = 80,
                            Height = 40,
                            BooksSets = [
                                new ()
                                {
                                    SetName = "Bible",
                                    Books = [
                                        new ()
                                        {
                                            BookName = "Begining",
                                            Width = 4,
                                            Height = 32,
                                            Genre = "Torah"
                                        },

                                         new ()
                                        {
                                            BookName = "Names",
                                            Width = 3,
                                            Height = 32,
                                            Genre = "Torah"
                                        }
                                        ]
                                }
                                ]
                        }
                        ]
                };
                Librarys.Add( library );
                SaveChanges();
            }
        }

        public DbSet<LibraryModel> Librarys { get; set; }
        public DbSet<ShelfModel> Shelves { get; set; }
        public DbSet<BooksSetModel> BooksSets { get; set; }
        public DbSet<BookModel> Books { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<LibraryModel>()
                .HasMany(l => l.Shelves)
                .WithOne(s => s.Library)
                .HasForeignKey(s => s.LibraryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShelfModel>()
               .HasMany(shelf => shelf.BooksSets)
               .WithOne(bs => bs.Shelf)
               .HasForeignKey(bs => bs.ShelfId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BooksSetModel>()
                .HasMany(bs => bs.Books)
                .WithOne(b => b.BooksSet)
                .HasForeignKey(b => b.BooksSetId)
                .OnDelete(DeleteBehavior.Cascade);
		}
	}

}
