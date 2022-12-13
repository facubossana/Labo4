using FinalLaboIV.Data;
using FinalLaboIV.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLaboIV.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ManageUsersController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ManageUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var users = _context.Users.ToList();

            var model = new List<ManageUserModel>();

            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();
            foreach (var user in users)
            {
                ManageUserModel manageUserModel = new ManageUserModel();
                manageUserModel.user = user;
                var userRole = _context.UserRoles.FirstOrDefault(r => r.UserId == user.Id);
                if (userRole != null)
                {
                    var role = _context.Roles.FirstOrDefault(r => r.Id == userRole.RoleId);
                    manageUserModel.role = role;
                }
                else
                {
                    manageUserModel.role = null;
                }
                model.Add(manageUserModel);
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            ManageUserModel manageUserModel = new ManageUserModel();
            manageUserModel.user = user;
            var userRole = _context.UserRoles.FirstOrDefault(r => r.UserId == user.Id);
            if (userRole != null)
            {
                var role = _context.Roles.FirstOrDefault(r => r.Id == userRole.RoleId);
                manageUserModel.role = role;
            }
            else
            {
                manageUserModel.role = null;
            }

            var availableRoles = from d in _context.Roles
                                 orderby d.Name
                                 select d;

            SelectList newSelectList = new SelectList(availableRoles, "Id", "Name");

            foreach (var item in newSelectList)
            {
                if (manageUserModel.role.Name == "Administrator")
                {
                    if (manageUserModel.role.Id.ToString() == item.Value)
                    {
                        item.Selected = true;
                        item.Text = "Administrador";
                    }
                    else
                    {
                        item.Text = "Usuario sin privilegios";
                    }
                }
                else
                {
                    if (manageUserModel.role.Id.ToString() == item.Value)
                    {
                        item.Selected = true;
                        item.Text = "Usuario sin privilegios";
                    }
                    else
                    {
                        item.Text = "Administrador";
                    }
                }
            }

            ViewBag.Roles = newSelectList;

            return View(manageUserModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("user,role")] ManageUserModel manageUserModel)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (id != manageUserModel.user.Id)
            {
                return NotFound();
            }

            manageUserModel.user = user;

            var role = _context.Roles.FirstOrDefault(r => r.Id == manageUserModel.role.Id);
            if (role != null)
            {
                manageUserModel.role = role;
            }
            else
            {
                var everyOneRole = _context.Roles.FirstOrDefault(r => r.Name == "Everyone");
                manageUserModel.role = everyOneRole;
            }

            var newRoleId = Request.Form["role"].ToString();
            var newRole = _context.Roles.FirstOrDefault(r => r.Id == newRoleId);

            if (ModelState.IsValid)
            {
                try
                {
                    if (manageUserModel.role.Name != newRole.Name)
                    {

                        IdentityUserRole<string> newUserRole = new IdentityUserRole<string>();
                        newUserRole.UserId = manageUserModel.user.Id;
                        newUserRole.RoleId = newRole.Id;

                        _context.Add(newUserRole);

                        IdentityUserRole<string> removeUserRole = new IdentityUserRole<string>();
                        removeUserRole.UserId = manageUserModel.user.Id;
                        removeUserRole.RoleId = manageUserModel.role.Id;

                        _context.Remove(removeUserRole);

                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.UserRoles.Any(ur => ur.RoleId == manageUserModel.role.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(manageUserModel);
        }
    }
}
