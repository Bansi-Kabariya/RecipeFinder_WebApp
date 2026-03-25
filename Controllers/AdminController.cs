using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeFinder_WebApp.Data;
using RecipeFinder_WebApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;

namespace RecipeFinder_WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") == "true")
                return RedirectToAction("Dashboard");

            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
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
        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Index");

            try
            {
                ViewBag.TotalRecipes = await _context.Recipes.CountAsync();
                ViewBag.TotalUsers = await _context.Users.CountAsync();
                ViewBag.TotalCategories = await _context.Categories.CountAsync();

                var recent = await _context.Recipes
                    .OrderByDescending(x => x.RecipeId)
                    .Take(3)
                    .ToListAsync();

                return View(recent);
            }
            catch (Exception ex)
            {
                ViewBag.DbError = "Unable to connect to the database. " + ex.Message;
                ViewBag.TotalRecipes =0;
                ViewBag.TotalUsers =0;
                ViewBag.TotalCategories =0;

                return View(new List<Recipes>());
            }
        }
        public async Task<IActionResult> Recipes()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true") return RedirectToAction("Index");

            try
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                var recipes = await _context.Recipes.Include(r => r.Categories).ToListAsync();
                return View(recipes);
            }
            catch (Exception ex)
            {
                ViewBag.DbError = "Unable to load recipes. " + ex.Message;
                ViewBag.Categories = new List<Categories>();
                return View(new List<Recipes>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecipe(Recipes recipe, IFormFile ImageFile)
        {
            ModelState.Remove("Categories");
            ModelState.Remove("ImagePath");
            ModelState.Remove("ImageUrl");

            if (ImageFile != null && ImageFile.Length >0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/recipes");

                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                string filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                recipe.ImagePath = "/images/recipes/" + fileName;
                recipe.ImageUrl = recipe.ImagePath; 
            }

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Recipes));
        }
        public IActionResult Categories()
        {
            return View();
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

            var reviews = await _context.Reviews
                .Include(r => r.Recipe)  
                .Include(r => r.User)   
                .ToListAsync();

            return View(reviews);
        }

        public IActionResult RecipeDetails(string name)
        {
            var model = new RecipeFinder_WebApp.Models.Recipes
            {
                RecipeId =0,
                RecipeName = name ?? "Recipe",
                CategoriesId =0,
                Description = "This is a static description for the recipe. Replace with real content.",
                Instructions = "1. Prepare ingredients.\n2. Cook as required.\n3. Serve hot.",
                Calories =250,
                CookingTime =30,
                Servings =2,
                ImageUrl = "/images/recipes/sample.jpg",
                ImagePath = "/images/recipes/sample.jpg",
            };
            return View(model);
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult UserDetails(string name)
        {
            var user = new RecipeFinder_WebApp.Models.User
            {
                UserId =1,
                Username = name ?? "Bansi",
                FullName = name ?? "Bansi Kabariya",
                Email = "bansi@gmail.com",
                MobileNumber = "7848745854",
                Address = "Some Address",
                Role = "Administrator",
                ProfilePicture = "/images/users/default.png",
                CreatedAt = DateTime.Now
            };
            return View(user);
        }

        public IActionResult Feedback()
        {
            return View();
        }

        public IActionResult ContactMessages()
        {
            return View();
        }
    }
}