using Microsoft.AspNetCore.Mvc;
using RecipeFinder_WebApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace RecipeFinder_WebApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Feedback() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Feedback(string name, string email, int rating, string message)
        {
            TempData["FeedbackSent"] = "Thank you for your feedback!";
            return RedirectToAction("Index");
        }

        // GET: Profile
        public IActionResult Profile(string? name = null, string? mobile = null, string? gender = null, string? address = null, string? pic = null)
        {
            var user = new User
            {
                UserId = 1,
                Username = name ?? "Bansi",
                FullName = name ?? "Bansi Kabariya",
                Email = "bansi@gmail.com",
                MobileNumber = mobile ?? "7848745854",
                Address = address ?? "amreli",
                Role = "User",
                ProfilePicture = pic ?? "/images/users/default.png",
                CreatedAt = System.DateTime.Now,
                Gender = gender ?? "female"
            };

            return View(user);
        }

        // GET: EditProfile 
        // Logic: Redirect to Profile. The View will handle showing the overlay.
        public IActionResult EditProfile()
        {
            return RedirectToAction("Profile");
        }

        // POST: Edit profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(string? fullName, string? mobile, string? gender, string? address, IFormFile? profilePicture)
        {
            string? picPath = null;
            if (profilePicture != null && profilePicture.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "users");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var fileName = System.Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }
                picPath = "/images/users/" + fileName;
            }

            return RedirectToAction("Profile", new
            {
                name = fullName,
                mobile = mobile,
                gender = gender,
                address = address,
                pic = picPath
            });
        }
    }
}