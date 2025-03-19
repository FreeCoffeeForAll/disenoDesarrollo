using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ProyectoFinalDiseño.Models;

namespace YourApp.Controllers
{
    [Authorize(Roles = "Admin,Trainer,Client")] // Only clients can access subscription-related actions
    public class SubscriptionController : Controller
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly ApplicationDbContext _context;

        public SubscriptionController(UserManager<UserApplication> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: /Subscription
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                                              .FirstOrDefaultAsync(s => s.UserId == user.Id);

            if (subscription == null)
            {
                // If no subscription exists, offer an option to subscribe
                return View("Subscribe");
            }

            // If a subscription exists, display its details
            return View("SubscriptionDetails", subscription);
        }

        // GET: /Subscription/Subscribe
        [HttpGet]
        public IActionResult Subscribe()
        {
            return View();
        }

        // POST: /Subscription/Subscribe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(string subscriptionType)
        {
            if (string.IsNullOrEmpty(subscriptionType))
            {
                ModelState.AddModelError("", "Please select a subscription type.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var subscription = new Subscription
            {
                UserId = user.Id,
                Plan = subscriptionType,
                StartDate = DateTime.Now,
                EndDate = null // Set initially to no expiration
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: /Subscription/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                                              .FirstOrDefaultAsync(s => s.UserId == user.Id);

            if (subscription != null)
            {
                subscription.EndDate = DateTime.Now; // Mark subscription as ended
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
