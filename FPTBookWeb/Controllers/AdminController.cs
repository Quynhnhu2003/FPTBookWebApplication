using FPTBookWeb.Models;
using FPTBookWeb.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FPTBookWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly DbFptbookContext _context;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, DbFptbookContext context)
        {
            _roleManager = roleManager; _userManager = userManager; _context = context;
        }

        [HttpGet]
        public IActionResult ListAllRoles()
        {
            var roles =_roleManager.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> ManageStoreOwner()
        {
            var store = await _userManager.GetUsersInRoleAsync("Store Owner");
            return View(store.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> ManageCustomer()
        {
            var customers = await _userManager.GetUsersInRoleAsync("Customer");
            return View(customers.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> DetailUser(string Id) { 
            var user = await _userManager.FindByIdAsync(Id);
            var role = await _userManager.GetRolesAsync(user);
            EditUser newUser = new()
            {
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                RoleName = role[0]
            };        
            return View(newUser);
        }


        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpGet]
        public IActionResult EditStoreOwner()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new()
                {
                    Name = model.RoleName
                };

                var result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListAllRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditStoreOwner(string Id, [Bind("FullName, Email, PhoneNumber")] User user)
        {
            var userById = await _userManager.FindByIdAsync(Id);
            
            if (userById == null)
            {
                return NotFound();
            }

            var newUser = userById;
            newUser.FullName = user.FullName;
            newUser.Email = user.Email;
            newUser.PhoneNumber = user.PhoneNumber;
            newUser.UserName = user.Email;
            if (ModelState.IsValid)
            {
                IdentityResult result = await _userManager.UpdateAsync(newUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("ManageStoreOwner");
                }
            }

            return View(newUser);
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer(string Id, [Bind("FullName, Email, PhoneNumber")] User user)
        {
            var userById = await _userManager.FindByIdAsync(Id);

            if (userById == null)
            {
                return NotFound();
            }

            var newUser = userById;
            newUser.FullName = user.FullName;
            newUser.Email = user.Email;
            newUser.PhoneNumber = user.PhoneNumber;
            newUser.UserName = user.Email;
            if (ModelState.IsValid)
            {
                IdentityResult result = await _userManager.UpdateAsync(newUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("ManageCustomer");
                }
            }

            return View(newUser);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewData["ErrorMessage"] = $"No role with Id '{id}' was found";
                return View("Error");
            }
            else
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListAllRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(role);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser (string id)
        {
           
            var user = await _userManager.FindByIdAsync(id);
            var role = await _userManager.GetRolesAsync(user);
            EditUser newUser = new()
            {
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                RoleName = role[0],
                Id = user.Id
            };
            return View(newUser);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserConfirm(string id)
        {
            if (id == null)
            {
               return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            var role = await _userManager.GetRolesAsync(user);
            
            if (user == null)
            {
                ViewData["ErrorMessage"] = $"No user with Id '{id}' was found";
                return View("Error");
            }
            else
            {
                if (role[0] == "Store Owner")
                {
                    var orderDetail = _context.OrderDetails.Include(x => x.Order).Include(o=> o.Book).Where(u=> u.Book.UserId == id);
                    foreach (var item in orderDetail)
                    {
                        _context.OrderDetails.Remove(item);
                        _context.SaveChanges();
                    }
                    var result = await _userManager.DeleteAsync(user);

                if(result.Succeeded)
                {
                    
                    return RedirectToAction("ManageStoreOwner");
                };
                }
                else
                {
                    var orderDetail = _context.OrderDetails.Where(o=>o.UserId==id).ToList();
                    var orders= _context.Orders.Include(o=>o.Customer).Where(o=>o.Customer.Id == id).ToList();

                    foreach (var item in orderDetail)
                    {
                        _context.OrderDetails.Remove(item);
                        _context.SaveChanges();
                    }
                    foreach (var item in orders)
                    {
                        _context.Orders.Remove(item);
                        _context.SaveChanges();
                    }
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                    return RedirectToAction("ManageCustomer");

                    }
                }
                
                /*foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }*/
                return RedirectToAction("DeleteUser");
            }
        }
    }
}
