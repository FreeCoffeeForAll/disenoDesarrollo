using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDiseño.Models;
using ProyectoFinalDiseño.Models.user;
using ProyectoFinalDiseño.Services;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalDiseño.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User_Application> _userManager; // registration
        private readonly SignInManager<User_Application> _signInManager; // login and logout
        private readonly RoleManager<IdentityRole> _roleManager; // role management
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<User_Application> userManager,
            SignInManager<User_Application> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IEmailSender emailSender
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _emailSender = emailSender;
        }



        //------------------------------------------------------------- REGISTRATION
        // GET -> /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST -> /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User_Registration model)
        {
            if (ModelState.IsValid)
            {
                var user = new User_Application
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Name = model.Name,
                    Lastname = model.Lastname,
                    DateOfBirth = model.DateOfBirth,
                    ProfilePicture = model.ProfilePicture ?? "string"
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign the "Default" role to the user
                    await _userManager.AddToRoleAsync(user, "Client");

                    // Auto-login after registration
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        //------------------------------------------------------------- LOGIN
        // GET -> /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST -> Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User_Login model)
        {
            if (ModelState.IsValid)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); // Redirect to home
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt."); // Login error
                }
            }
            return View(model);
        }

        //------------------------------------------------------------- LOGOUT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        //------------------------------------------------------------- ROLE MANAGEMENT
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (!await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest($"Role '{role}' does not exist.");
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return RedirectToAction("UserList");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ManageRoles()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();

            var model = new ManageRolesViewModel
            {
                Users = users,
                Roles = roles
            };

            return View(model);
        }

        public class ManageRolesViewModel
        {
            public List<User_Application> Users { get; set; }
            public List<IdentityRole> Roles { get; set; }
        }

        //------------------------------------------------------------- USERS LIST
        // GET -> /Account/UserList
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UserList(int? page)
        {
            var users = _userManager.Users.ToList();
            var model = users.Select(user => new User_ListViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
            }).ToList();

            ViewBag.CurrentUserId = _userManager.GetUserId(User);
            return View(model);
        }

        //------------------------------------------------------------- EDIT USER
        //[Authorize(Roles = "Admin")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new User_EditViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Lastname = user.Lastname,
                DateOfBirth = user.DateOfBirth,
                ProfilePicture = user.ProfilePicture ?? "string",
                SelectedRoles = userRoles.FirstOrDefault(),  // Get the first role (or null if no roles)
                AllRoles = allRoles
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(User_EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.Name = model.Name;
                user.Lastname = model.Lastname;
                user.DateOfBirth = model.DateOfBirth;
                user.ProfilePicture = model.ProfilePicture ?? "string";

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                    return View(model);
                }

                // Manage roles only if SelectedRoles has changed
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (!string.IsNullOrEmpty(model.SelectedRoles))
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(user, model.SelectedRoles);
                    if (!addRoleResult.Succeeded)
                    {
                        foreach (var error in addRoleResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                        return View(model);
                    }
                }

                //  admin rol validation
                bool isAdmin = User.IsInRole("Admin");
                if (!isAdmin)
                {
                    return RedirectToAction("ProfileView");
                }
                else {
                    return RedirectToAction("UserList");
                }

                
            }

            // Reload roles if there's a validation error
            model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(model);
        }

        //------------------------------------------------------------- DELETE USER
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //  Delete validation
            var currentUserId = _userManager.GetUserId(User);
            if (id == currentUserId)
            {
                ModelState.AddModelError("", "You cannot delete your own account!");
                return RedirectToAction("UserList");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("UserList");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("UserList");
        }


        //------------------------------------------------------------- USER VALIDATION
        // Controller (AccountController.cs)
        [Authorize(Roles = "Admin,Trainer")]
        [HttpGet]
        public IActionResult EntryCheck()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EntryCheck(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                ViewBag.Message = "Please enter a username.";
                ViewBag.AlertClasss = "alert-warning";
                return View();
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                ViewBag.Message = "User not found.";
                ViewBag.AlertClass = "alert-danger";
                return View();
            }

            var subscription = _context.Subscriptions
                .Where(s => s.UserId == user.Id)
                .OrderByDescending(s => s.EndDate)
                .FirstOrDefault();

            if (subscription == null || !subscription.IsActive) 
            {
                ViewBag.Message = "User does no have an active subscrition";
                ViewBag.AlertClass = "alert-danger";
                return View();
            }

            // ✅ Register visit
            var visit = new User_Visit
            {
                UserId = user.Id,
                VisitTime = DateTime.Now
            };

            _context.UserVisits.Add(visit);
            await _context.SaveChangesAsync();

            // Log entry or any additional logic
            ViewBag.Message = $"Welcome {user.Name} {user.Lastname}! Entry recorded.";
            ViewBag.AlertClass = "alert-success";
            return View();
        }


        //------------------------------------------------------------- PROFILE VIEW
        public async Task<IActionResult> ProfileView()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        //------------------------------------------------------------- CHANGE PASSWORD
        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(User_ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                ViewBag.StatusMessage = "Your password has been changed.";
                return View();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }


        //------------------------------------------------------------- Forgot Password
        
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // No revelamos si el usuario existe
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);

            string emailBody = $"Please reset your password by clicking here: <a href='{resetLink}'>Reset Password</a>";

            await _emailSender.SendEmailAsync(model.Email, "Password Reset", emailBody);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //------------------------------------------------------------- Reset Password

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token.");
            }
            return View(new ResetPasswordViewModel { Token = token, Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // No revelamos si existe
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        
    }
}
