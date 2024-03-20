﻿using HalloDocMVC.Controllers.AdminController;
using Microsoft.AspNetCore.Mvc;

namespace HalloDocMVC.Controllers.AdminController
{
    [CheckProviderAccess("Admin")]
    public class AccessController : Controller
    {
        public IActionResult Index()
        {
            return View("../AdminPanel/Admin/Access/Index");
        }
    }
}
