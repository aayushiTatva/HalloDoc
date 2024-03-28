using AspNetCoreHero.ToastNotification.Abstractions;
using HalloDocMVC.DBEntity.DataContext;
using HalloDocMVC.DBEntity.ViewModels;
using HalloDocMVC.DBEntity.ViewModels.AdminPanel;
using HalloDocMVC.Repositories.Admin.Repository;
using HalloDocMVC.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HalloDocMVC.Controllers.AdminController
{
    [CheckProviderAccess("Admin")]
    public class ProvidersController : Controller
    {
        #region Configuration
        private readonly HalloDocContext _context;
        private readonly IAdminDashboard _IAdminDashboard;
        private readonly IContactYourProvider _IContactYourProvider;
        private readonly IComboBox _IComboBox;
        private readonly INotyfService _INotyfService;
        public ProvidersController(HalloDocContext context,IAdminDashboard iAdminDashboard,IContactYourProvider iContactYourProvider, IComboBox iComboBox, INotyfService iNotyfService)
        {
            _context = context;
            _IAdminDashboard = iAdminDashboard;
            _IContactYourProvider = iContactYourProvider;
            _IComboBox = iComboBox;
            _INotyfService = iNotyfService;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(int? region)
        {
            ViewBag.RegionComboBox = await _IComboBox.ComboBoxRegions();
            var data = await _IContactYourProvider.GetContacts();
            if (region == null)
            {
                data = await _IContactYourProvider.GetContacts();
            }
            else
            {
                data = await _IContactYourProvider.PhysicianByRegion(region);
            }
            return View("~/Views/AdminPanel/Admin/Provider/Index.cshtml", data);
        }
        #endregion
        #region ChangeNotification
        public async Task<IActionResult> ChangeNotification(string changedValues)
        {
            Dictionary<int, bool> changeValueDict = JsonConvert.DeserializeObject<Dictionary<int, bool>>(changedValues);
            _IContactYourProvider.ChangeNotification(changeValueDict);
            return RedirectToAction("Index");
        }
        #endregion
        public IActionResult Index1()
        {
            return View("~/Views/AdminPanel/Admin/Provider/Index1.cshtml");
        }


    }
}
