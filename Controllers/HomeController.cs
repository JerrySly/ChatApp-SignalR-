using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChatApp_SignalR_.Models;

namespace ChatApp_SignalR_.Controllers
{
    public class HomeController : Controller
    {
       

        

        public IActionResult Login()=> View();
        public IActionResult Register()=>View();
        
        public IActionResult RoomChat()=>View();
        

        
    }
}
