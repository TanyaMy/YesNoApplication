using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YesNoPuzzle.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YesNoPuzzle.Models;


namespace YesNoPuzzle.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> DeleteGame(int? id)
        {
            if (id == null)
                return NotFound();

            var game = _db.Games.First(g => g.Id == id);

            var questions = _db.Questions.Where(q => q.Game == game).ToList();

            foreach (var q in questions)
            {
                _db.Questions.Remove(q);
            }

            _db.Games.Remove(game);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Player");
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                var games = await _db.Games.Where(u => u.User.Id == userId).ToListAsync();

                return View(games);
            }
            return View();    
        }

       
        public IActionResult Rules()
        {
            ViewData["Message"] = "YesNo Puzzle.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Our contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
