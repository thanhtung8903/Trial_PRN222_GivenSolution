using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Project2.Hubs;

namespace Project2.Pages.Movies
{
    public class DirectorMovieModel : PageModel
    {
        private readonly TrialContext _context;
        private readonly IHubContext<MovieHub> _hubContext;

        public DirectorMovieModel(TrialContext context, IHubContext<MovieHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public List<Director> Directors { get; set; }
        public List<Movie> Movies { get; set; }
        [BindProperty]
        public int? SelectedDirectorId { get; set; }

        public void OnGet(int? directorId)
        {
            // Load all directors
            Directors = _context.Directors.ToList();
            SelectedDirectorId = directorId;

            // Load movies, either all or filtered by director
            if (directorId.HasValue)
            {
                Movies = _context.Movies
                    .Include(m => m.Director)
                    .Include(m => m.Producer)
                    .Include(m => m.Stars)
                    .Where(m => m.DirectorId == directorId)
                    .ToList();
            }
            else
            {
                Movies = _context.Movies
                    .Include(m => m.Director)
                    .Include(m => m.Producer)
                    .Include(m => m.Stars)
                    .ToList();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var movie = _context.Movies.Include(m => m.Stars).Include(m => m.Genres).FirstOrDefault(m => m.Id == id);

            if (movie == null) return RedirectToPage(new { directorId = SelectedDirectorId });

            movie.Stars.Clear();
            movie.Genres.Clear();

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            // Notify all clients via SignalR
            await _hubContext.Clients.All.SendAsync("MovieDeleted", id);
            return RedirectToPage(new { directorId = SelectedDirectorId });
        }
    }
}