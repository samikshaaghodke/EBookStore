﻿using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db) //to connect to database service
        {
            _db = db;
        }

        //To get list of Genres
        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _db.Genres.ToListAsync();
        }

        //Gets filtered list of books based on Search Term and Genre
        public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Book> books = await (from book in _db.Books
                         join genre in _db.Genres
                         on book.GenreId equals genre.Id
                         where string.IsNullOrWhiteSpace(sTerm) || (book != null && book.BookName.ToLower().StartsWith(sTerm))
                         select new Book
                         {
                             Id = book.Id,
                             Image = book.Image,
                             AuthorName = book.AuthorName,
                             BookName = book.BookName,
                             GenreId = book.GenreId,
                             Price = book.Price,
                             GenreName = genre.GenreName
                         }
                         ).ToListAsync();
            if (genreId > 0)
            {
                books = books.Where(a => a.GenreId == genreId).ToList();
            }
            return books;
        }
    }
}
