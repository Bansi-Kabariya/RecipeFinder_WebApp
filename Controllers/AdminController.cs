using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using RecipeFinder_WebApp.Data;
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

        // ... Login and Dashboard code remains same ...

        public async Task<IActionResult> Recipes()
        {
            // FIX: Use .Categories to match the navigation property in your Recipes model
            var recipes = await _context.Recipes.Include(r => r.Categories).ToListAsync();
            return View(recipes);
        }

        public IActionResult Reviews()
        {
            // Make sure 'Recipes' matches exactly what is inside your Reviews.cs file
            var reviews = _context.Reviews
                .Include(r => r.RecipeId)
                .Include(r => r.UserId)
                .ToList();
            return View(reviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecipe(Recipes recipe, IFormFile? ImageFile)
        {
            // This prevents the "required" errors for things we handle in code
            ModelState.Remove("Categories");
            ModelState.Remove("ImageUrl");
            ModelState.Remove("ImagePath");

            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/recipes");

                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    using (var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    recipe.ImagePath = "/images/recipes/" + fileName;
                    recipe.ImageUrl = recipe.ImagePath;
                }

                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Recipes));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(recipe);
        }
    }
}