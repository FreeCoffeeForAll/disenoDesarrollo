using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDo.Application.Contracts.Services;
using ToDo.Application.Services;
using ToDo.Domain.DTOs.Task;
using ToDo.Domain.Entities.ValueObjects;
using ToDo.Infrastructure.Contants;
using ToDo.UI.Models;
using ToDo.UI.Models.ViewModels;

namespace ToDo.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, ITaskService taskService, IUserService userService)
        {
            _logger = logger;
            _taskService = taskService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var tasks = 
                User.Identity.IsAuthenticated
                    ? _taskService.GetAll()
                    : new List<ListTaskDTO>();
            return View(tasks);
        }

        [HttpGet]
        [Authorize(Policy = PolicyType.CreateTask)]
        public IActionResult Create()
        {
            var model = new CreateTaskViewModel();
            model.Users = _userService.GetAll().ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateTaskViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var result = _taskService.Create(model.Task);
                if (result.IsSuccess) 
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, result.Error);
            }

            model.Users = _userService.GetAll().ToList();
            return View(model);
        }

        [HttpGet("/home/edit/{id}")]
        [Authorize]
        public IActionResult Edit([FromRoute] int id)
        {
            var task = _taskService.Get(id);
            var model = 
                new EditTaskViewModel 
                { 
                    Subject = task.Subject,  
                    Weeks = task.Effort.Weeks,
                    Days = task.Effort.Days,
                    Hours = task.Effort.Hours,
                    Minutes = task.Effort.Minutes,
                    Completed = task.Completed
                };

            return View(model);
        }

        [HttpPost("/home/edit/{id}")]
        [Authorize]
        public IActionResult Edit([FromRoute] int id, EditTaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var effort = new Effort(model.Weeks, model.Days, model.Hours, model.Minutes);
                var task = new EditTaskDTO(id, model.Subject, effort, model.Completed);
                var result = _taskService.Edit(task);
                
                if (result.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, result.Error);
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}