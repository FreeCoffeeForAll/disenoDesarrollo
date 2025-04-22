using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDiseño.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoFinalDiseño.Controllers
{
    [Authorize(Roles = "Admin,Trainer,Client")] // Only clients can access subscription-related actions
    public class SubscriptionController : Controller
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly ApplicationDbContext _context;

        private readonly Dictionary<string, decimal> _subscriptionPrices = new Dictionary<string, decimal>
{
    { "Basic", 15000m },
    { "Premium", 25000m }
};


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
            return View("Index", subscription);
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

            var price = _subscriptionPrices[subscriptionType];


            var subscription = new Subscription
            {
                UserId = user.Id,
                Plan = subscriptionType,
                StartDate = DateTime.Now,
                EndDate = null, // Set initially to no expiration
                Price = price // <-- Set the price
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

        // GET: /Subscription/ChangePlan
        [HttpGet]
        public async Task<IActionResult> ChangePlan()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (subscription == null) return RedirectToAction("Subscribe");

            ViewBag.CurrentPlan = subscription.Plan;
            return View(subscription); // You can reuse the Subscription model
        }

        // POST: /Subscription/ChangePlan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePlan(string newPlan)
        {
            if (string.IsNullOrEmpty(newPlan) || !_subscriptionPrices.ContainsKey(newPlan))
            {
                ModelState.AddModelError("", "Invalid subscription type.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (subscription == null) return RedirectToAction("Subscribe");

            // Only change if it's a different plan
            if (subscription.Plan != newPlan)
            {
                subscription.Plan = newPlan;
                subscription.Price = _subscriptionPrices[newPlan];
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}
