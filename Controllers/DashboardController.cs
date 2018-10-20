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
    public class DashboardController : Controller
    {
        // Context Service
        private BeltContext dbContext;
        public DashboardController(BeltContext context)
        {
            dbContext = context;
        }

        // Routes
        [HttpGet("/dashboard")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("UserId") == null) 
            {
                return RedirectToAction("Main", "Home");
            } 
            else 
            {
                int? UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.UserId = UserId;
                User LogUser = dbContext.Users.FirstOrDefault(user => user.UserId == UserId);
                ViewBag.LogUser = LogUser;
                // queries
                List<Post> AllPosts = dbContext.Posts.Include(p => p.User).Include(p => p.Likes).ThenInclude(l => l.User).OrderByDescending(p => p.LikeAmount).ToList();
                ViewBag.AllPosts = AllPosts;
                return View("Dashboard");
            }
        }

        [HttpPost("remove/{id}")]
        public IActionResult Remove(int id)
        {
            Post SelectedPost = dbContext.Posts.FirstOrDefault(p => p.PostId == id);
            dbContext.Remove(SelectedPost);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost("/post/new")]
        public IActionResult AddPost(Post post) 
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(post);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("Dashboard");
        }

        [HttpPost("/like")]
        public IActionResult AddLike(Like like)
        {
            // Select the post to update its LikeAmount
            Post SelectedPost = dbContext.Posts.FirstOrDefault(p => p.PostId == like.PostId);
            SelectedPost.LikeAmount = SelectedPost.LikeAmount + 1;
            System.Console.WriteLine("HHHHHHEEEEEEEEEEEEEEEEEEYYYYYYYYYYYYY");
            System.Console.WriteLine("CurrentLikeAmount: "+SelectedPost.LikeAmount);
            Like connection = new Like()
            {
                UserId = like.UserId,
                PostId = like.PostId,
            };
            dbContext.Likes.Add(connection);
            dbContext.SaveChanges();
            System.Console.WriteLine("Added connection");
            return RedirectToAction("Dashboard");
        }

        [HttpGet("post/{id}")]
        public IActionResult PostDetail(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null) 
            {
                return RedirectToAction("Main", "Home");
            } 
            else 
            {
                int? UserId = HttpContext.Session.GetInt32("UserId");
                User LogUser = dbContext.Users.FirstOrDefault(user => user.UserId == UserId);
                ViewBag.LogUser = LogUser;
                Post SelectedPost = dbContext.Posts.Include(p => p.User).Include(p => p.Likes).ThenInclude(l => l.User).FirstOrDefault(p => p.PostId == id);
                ViewBag.Post = SelectedPost;
                List<User> Users = SelectedPost.Likes.Select(l => l.User).Distinct().ToList();
                ViewBag.Users = Users;
                return View("Detail");
            }
        }

        [HttpGet("user/{id}")]
        public IActionResult UserDetail(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null) 
            {
                return RedirectToAction("Main", "Home");
            } 
            else 
            {
                int? UserId = HttpContext.Session.GetInt32("UserId");
                User LogUser = dbContext.Users.FirstOrDefault(user => user.UserId == UserId);
                ViewBag.LogUser = LogUser;
                User SelectedUser = dbContext.Users.Include(u => u.Posts).Include(u => u.Likes).FirstOrDefault(u => u.UserId == id);
                ViewBag.User = SelectedUser;
                return View("User");
            }
        }
    }
}
