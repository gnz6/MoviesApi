namespace MoviesApi.Services
{
    public interface IStorage
    {
        Task<string> StoreFile(byte[] content, string extension, string container, string contentType);
        Task<string> EditFile(byte[] content, string extension, string container, string contentType, string route);
        Task DeleteFile( string container , string route );


    }
}
