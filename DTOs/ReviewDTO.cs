using Microsoft.AspNetCore.Identity;
using MoviesApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        [Range(1, 5)]
        public int Score { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

    }
}
