using Microsoft.AspNetCore.Mvc;

namespace RecipeFinder_WebApp.Controllers
{
 public class RecipeController : Controller
 {
 // All recipes static page
 public IActionResult Index()
 {
 return View();
 }

 // Category page placeholder
 public IActionResult Category(string id)
 {
 ViewBag.CategoryName = id;
 return View();
 }

 // User-facing recipe details (static)
 public IActionResult Details(string name)
 {
 // Pass the recipe name as model to the view for simple static rendering
 return View((object)name);
 }
 }
}
