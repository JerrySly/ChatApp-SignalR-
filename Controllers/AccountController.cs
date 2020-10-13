using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ChatApp_SignalR_.Models;
using Sitecore.FakeDb;
using Xceed.Wpf.Toolkit;
//using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;

namespace ChatApp_SignalR_.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext _context;
        public AccountController(ApplicationContext context)
        {
            _context = context;
        }
        // тестирование SignalR
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    if(user.RoleId!=3){
                    await Authenticate(user); // аутентификация
                    return RedirectToAction("Index", "Account");// переадресация на метод Index
                    }
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
      [HttpGet]
    public IActionResult  Register(){
        return View();
    }
    [HttpPost]
        public async Task<IActionResult>  Register(RegisterModel register)
        {
            if(ModelState.IsValid){
               if( _context.Users.Where(x=>x.Email==register.Email).Count()!=0)
                    return View(register);
               else{
                   User user=new User();
                   user.Email=register.Email;
                   user.Password=register.Password;
                   user.Role=_context.Roles.ToArray()[1];
                     _context.Users.Add(user);
                     _context.SaveChanges();
                    await Authenticate(user);
                return RedirectToAction("Index", "Account");// переадресация на метод Index
               }
            }
            return RedirectToAction("Ban", "Account");// переадресация на метод Ban
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        [HttpPost]
        public  IActionResult  Ban(string email){
            if(!string.IsNullOrEmpty(email)){
                User user= _context.Users.Where(x=>x.Email==email).First();
                if(user!=null && user.RoleId!=0){
                    user.RoleId=3; 
                    _context.SaveChanges();
                }
                
            }
            return RedirectToAction("Index");
        } 
    }
}
