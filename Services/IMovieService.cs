using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using movies.Entities;

namespace movies.Services
{
    public interface IMovieService
    {
        Task<(bool IsSuccess, Exception Exception, Movie Movie)> CreateAsync(Movie movie);
        Task<List<Movie>> GetAllAsync();
        Task<Movie> GetAsync(Guid id);
        Task<List<Movie>> GetAllAsync(string title);
        Task<(bool IsSuccess, Exception Exception)> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<(bool IsSuccess, Exception Exception, Movie Movie)> UpdateMovieAsync(Movie movie);
        Task<(bool IsSuccess, Exception Exception)> CreateImagesAsync(List<Image> images);
        Task<Image> GetImageAsync(Guid id);
        Task<List<Image>> GetImagesAsync(Guid id);
        Task<bool> ImageExistsAsync(Guid id);
    }
}