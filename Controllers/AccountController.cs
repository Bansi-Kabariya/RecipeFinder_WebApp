using Microsoft.AspNetCore.Mvc;

namespace RecipeFinder_WebApp.Controllers
{
 public class AccountController : Controller
 {
 // Show login page (fallback)
 public IActionResult Login()
 {
 return View();
 }

 [HttpPost]
 [ValidateAntiForgeryToken]
 public IActionResult Login(string email, string password)
 {
 // Simple placeholder authentication: accept any non-empty email.
 if (!string.IsNullOrEmpty(email))
 {
 TempData["LoginMessage"] = "Logged in successfully.";
 return RedirectToAction("Index", "User");
 }

 TempData["LoginMessage"] = "Invalid credentials.";
 return RedirectToAction("Index", "User");
 }

 // GET: Register
 public IActionResult Register()
 {
 return View();
 }

 // POST: Register
 [HttpPost]
 [ValidateAntiForgeryToken]
 public IActionResult Register(string fullName, string email, string mobile, string gender, string address, string password)
 {
 // Placeholder: normally create user and persist to DB.
 TempData["RegisterMessage"] = "Registration successful. Please login.";
 return RedirectToAction("Login");
 }
 }
}
