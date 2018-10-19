using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Belt.Models;
// add these lines for validation
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
// add these lines for session
using Microsoft.AspNetCore.Http;

namespace Belt.Controllers
{
    public class HomeController : Controller
    {
        // Context Service
        private BeltContext dbContext;
        public HomeController(BeltContext context)
        {
            dbContext = context;
        }

        // Routes
        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("Main");
        }

        [HttpGet("/main")]
        public IActionResult Main()
        {
            return View("Main");
        }

        [HttpPost("/register")]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists!");
                    return View("Main");
                }
                // Check if the email matches with any email from the database
                PasswordHasher<User> Hasher = new PasswordHasher<User>(); 
                user.Password = Hasher.HashPassword(user, user.Password); 
                // Add User
                dbContext.Add(user);
                dbContext.SaveChanges();
                // Add user Id to session
                HttpContext.Session.SetInt32("UserId", user.UserId);
                // redirect to different Dashboard based on user's level
                return RedirectToAction("Dashboard", "Dashboard");

            }
            return View("Main");
        }

        [HttpPost("/signin")]
        public IActionResult SignIn(LoginUser userSubmission)
        {
            if (ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.LogEmail);
                Console.WriteLine("Current login user: " + userInDb);
                // if user does not exist in db
                if (userInDb == null)
                {
                    ModelState.AddModelError("LogEmail", "Email does not exist");
                    return View("Main");
                }
                else 
                {
                    // initialize hasher obj
                    var hasher = new PasswordHasher<LoginUser>();
                    // varify input pw against hash in db
                    var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LogPassword);
                    // if wrong password
                    if (result == 0)
                    {
                        ModelState.AddModelError("LogPassword", "Invalid Password");
                        return View("Main");
                    }
                    else 
                    {
                        // else, store current user id in session
                        HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                }
            } 
            return View("Main");
        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Main");
        }
    }
}
