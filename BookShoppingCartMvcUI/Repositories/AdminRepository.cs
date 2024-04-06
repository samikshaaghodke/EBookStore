using Microsoft.EntityFrameworkCore;

public class AdminRepository : IAdminRepository
{
    private readonly ApplicationDbContext _context;

    public AdminRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Genre>> GetAllGenresAsync() =>
        await _context.Genres.ToListAsync();

    public async Task<Genre> GetGenreByIdAsync(int id) => await _context.Genres.FindAsync(id);

    public async Task<bool> AddGenreAsync(Genre genre)
    {
        // Check if the genre already exists
        var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreName == genre.GenreName);
        if (existingGenre != null)
        {
            // Genre already exists, return false to indicate failure
            return false;
        }

        _context.Add(genre);
        await _context.SaveChangesAsync();

        // Genre added successfully, return true to indicate success
        return true;
    }

    public async Task UpdateGenreAsync(Genre genre)
    {
        _context.Update(genre);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync() =>
        await _context.Books.Include(b => b.Genre).ToListAsync();

    public async Task<Book> GetBookByIdAsync(int id) =>
        await _context.Books.Include(b => b.Genre).FirstOrDefaultAsync(b => b.Id == id);

    public async Task<bool> AddBookAsync(Book book)
    {
        bool bookExists = await _context.Books.AnyAsync(b => b.BookName == book.BookName && b.AuthorName == book.AuthorName);
        if (bookExists)
        {
            return false;
        }

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task UpdateBookAsync(Book book)
    {
        _context.Update(book);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> BookExistsAsync(string bookName, string authorName, int genreId, int? excludeBookId = null)
    {
        var query = _context.Books.AsQueryable();

        if (excludeBookId.HasValue)
        {
            query = query.Where(b => b.Id != excludeBookId.Value);
        }

        return await query.AnyAsync(b => b.BookName.Equals(bookName) && b.AuthorName.Equals(authorName) && b.GenreId == genreId);
    }
}
