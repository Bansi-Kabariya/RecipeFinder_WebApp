using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeFinder_WebApp.Data;
using RecipeFinder_WebApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace RecipeFinder_WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- LOGIN & AUTH ---

        // GET: Admin/Index (This is the Login Page)
        public IActionResult Index()
        {
            // If already logged in, go straight to dashboard
            if (HttpContext.Session.GetString("AdminLoggedIn") == "true")
                return RedirectToAction("Dashboard");

            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Update these credentials as needed
            if (email == "admin@quickcook.com" && password == "admin123")
            {
                HttpContext.Session.SetString("AdminLoggedIn", "true");
                return RedirectToAction("Dashboard");
            }

            ViewBag.ErrorMessage = "Invalid Login Credentials";
            return View("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // --- DASHBOARD ---
        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Index");

            ViewBag.TotalRecipes = await _context.Recipes.CountAsync();
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalCategories = await _context.Categories.CountAsync();

            var recent = await _context.Recipes
                .OrderByDescending(x => x.RecipeId)
                .Take(3)
                .ToListAsync();

            return View(recent);
        }

        // --- RECIPES ---
        public async Task<IActionResult> Recipes()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Index");

            ViewBag.Categories = await _context.Categories.ToListAsync();
            var recipes = await _context.Recipes.Include(r => r.Categories).ToListAsync();
            return View(recipes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecipe(Recipes recipe, IFormFile ImageFile)
        {
            // Remove validation for navigation properties to ensure Save works
            ModelState.Remove("Categories");
            ModelState.Remove("ImagePath");
            ModelState.Remove("ImageUrl");

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/recipes");

                // Ensure directory exists
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                string filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                recipe.ImagePath = "/images/recipes/" + fileName;
                recipe.ImageUrl = recipe.ImagePath; // Syncing both fields
            }

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Recipes));
        }

        // --- CATEGORIES ---
        public async Task<IActionResult> Categories()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Index");

            var cats = await _context.Categories.ToListAsync();
            return View(cats);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Categories category)
        {
            if (!string.IsNullOrEmpty(category.CategoriesName))
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Categories));
        }

        public async Task<IActionResult> Reviews()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
                return RedirectToAction("Index");

            // FIX: Include the Navigation Properties (Objects), not the ID fields
            var reviews = await _context.Reviews
                .Include(r => r.Recipe)  // Changed from RecipeId
                .Include(r => r.User)    // Changed from UserId
                .ToListAsync();

            return View(reviews);
        }

    //    // --- REVIEWS ---
    //    public async Task<IActionResult> Reviews()
    //    {
    //        if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Index");

    //        var reviews = await _context.Reviews
    //            .Include(r => r.RecipeId)
    //            .Include(r => r.UserId)
    //            .ToListAsync();
    //        return View(reviews);
    //    }
    }
}