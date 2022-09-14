using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using movies.Entities;

namespace movies.Services
{
    public interface IGenreService
    {
        Task<(bool IsSuccess, Exception Exception, Genre Genre)> CreateAsync(Genre genre);
        Task<List<Genre>> GetAllAsync();
        Task<Genre> GetAsync(Guid id);
        Task<(bool IsSuccess, Exception exception)> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsAsync(string name);
    }
}