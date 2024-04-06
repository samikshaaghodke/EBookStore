public interface IAdminRepository
{
    Task<IEnumerable<Genre>> GetAllGenresAsync();
    Task<Genre> GetGenreByIdAsync(int id);
    Task<bool> AddGenreAsync(Genre genre);
    Task UpdateGenreAsync(Genre genre);
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book> GetBookByIdAsync(int id);
    Task<bool> AddBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task<bool> BookExistsAsync(string bookName, string authorName, int genreId, int? excludeBookId = null);

}
