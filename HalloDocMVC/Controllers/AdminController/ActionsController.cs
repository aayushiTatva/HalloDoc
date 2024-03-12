using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using HalloDocMVC.DBEntity.ViewModels.AdminPanel;
using HalloDocMVC.Repositories.Admin.Repository;
using HalloDocMVC.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HalloDocMVC.Controllers.AdminController
{
    public class ActionsController : Controller
    {
        #region Configuration
        private readonly IActions _IActions;
        private readonly IComboBox _IComboBox;
        private readonly INotyfService _INotyfService;
        private readonly ILogger<ActionsController> _logger;
        public ActionsController(IActions iActions, IComboBox iComboBox, INotyfService iNotyfService, ILogger<ActionsController> logger)
        {
            _IActions = iActions;
            _IComboBox = iComboBox;
            _INotyfService = iNotyfService;
            _logger = logger;
        }
        #endregion Configuration

        #region ViewCase
        public async Task<IActionResult> ViewCase(int Id)
        {
            ViewBag.ComboBoxRegion = await _IComboBox.ComboBoxRegions();
            ViewBag.ComboBoxCaseReason = await _IComboBox.ComboBoxCaseReasons();
            ViewCaseModel vcm = _IActions.GetRequestForViewCase(Id);
            return View("~/Views/AdminPanel/Actions/ViewCase.cshtml", vcm);
        }
        #endregion ViewCase

        #region EditCase
        public IActionResult EditCase(ViewCaseModel vcm)
        {
            bool result = _IActions.EditCase(vcm);
            if (result)
            {
                return RedirectToAction("ViewCase", "Actions", new { id = vcm.RequestId });
            }
            else
            {
                return View("~/Views/AdminPanel/Actions/ViewCase.cshtml", vcm);
            }
        }
        #endregion EditCase
      
        #region AssignProvider
        public async Task<IActionResult> AssignProvider(int requestid, int ProviderId, string Notes)
        {
            if (await _IActions.AssignProvider(requestid, ProviderId, Notes))
            {
                _INotyfService.Success("Physician Assigned successfully...");
            }
            else
            {
                _INotyfService.Error("Physician Not Assigned...");
            }
            return RedirectToAction("Index", "Dashboard");
        }
        #endregion

        #region ProviderbyRegion
        public IActionResult ProviderbyRegion(int? Regionid)
        {
            var data = _IComboBox.ProviderByRegion(Regionid);
            return Json(data);
        }
        #endregion ProviderbyRegion

        #region CancelCase
        public IActionResult CancelCase(int RequestID, string Note, string CaseTag)
        {
            bool CancelCase = _IActions.CancelCase(RequestID, Note, CaseTag);
            if (CancelCase)
            {
                _INotyfService.Success("Case Cancelled Successfully");

            }
            else
            {
                _INotyfService.Error("Case Not Cancelled");

            }
            return RedirectToAction("Index", "Dashboard");
        }
        #endregion CancelCase

        #region BlockCase
        public IActionResult BlockCase(int RequestID, string Note)
        {
            bool BlockCase = _IActions.BlockCase(RequestID, Note);
            if (BlockCase)
            {
                _INotyfService.Success("Case Blocked Successfully");
            }
            else
            {
                _INotyfService.Error("Case Not Blocked");
            }
            return RedirectToAction("Index", "Dashboard");
        }
        #endregion BlockCase

        #region ClearCase
        public IActionResult ClearCase(int RequestID)
        {
            bool ClearCase = _IActions.ClearCase(RequestID);
            if (ClearCase)
            {
                _INotyfService.Success("Cleared Case Successfully");
            }
            else
            {
                _INotyfService.Error("Case Not Cleared");
            }
            return RedirectToAction("Index", "Dashboard");
        }
        #endregion

        #region TransferPhysician
        public async Task<IActionResult> TransferPhysician(int requestid, int ProviderId, string Notes)
        {
            if (await _IActions.TransferPhysician(requestid, ProviderId, Notes))
            {
                _INotyfService.Success("Physician Transferred Successfully.");
            }
            else
            {
                _INotyfService.Error("Physician Not Transferred.");
            }
            return RedirectToAction("Index", "Dashboard");
        }
        #endregion TransferPhysician

        #region View_Notes
        public IActionResult ViewNotes(int id)
        {
            ViewNotesModel vnm = _IActions.getNotes(id);
            return View("~/Views/AdminPanel/Actions/ViewNotes.cshtml", vnm);
        }
        #endregion

        #region Edit_Notes
        public IActionResult EditViewNotes(int RequestID, string? adminnotes, string? physiciannotes)
        {
            if (adminnotes != null || physiciannotes != null)
            {
                bool result = _IActions.EditViewNotes(adminnotes, physiciannotes, RequestID);
                if (result)
                {
                    _INotyfService.Success("Notes Updated successfully...");
                    return RedirectToAction("ViewNotes", new { id = RequestID });
                }
                else
                {
                    _INotyfService.Error("Notes Not Updated");
                    return View("~/Views/AdminPanel/Actions/ViewNotes.cshtml");
                }
            }
            else
            {
                _INotyfService.Information("Please Select one of the note!!");
                TempData["Errormassage"] = "Please Select one of the note!!";
                return RedirectToAction("ViewNotes", new { id = RequestID });
            }
        }
        #endregion

        #region ViewDocuments
        public async Task<IActionResult> ViewDocuments(int? id)
        {
            ViewUploadModel vum = await _IActions.GetDocument(id);
            return View("~/Views/AdminPanel/Actions/ViewUploads.cshtml", vum);
        }
        #endregion

        #region UploadDocuments
        public IActionResult UploadDocuments(int Requestid, IFormFile file)
        {
            if(_IActions.UploadDocuments(Requestid, file))
            {
                _INotyfService.Success("File Uploaded Successfully");
            }
            else
            {
                _INotyfService.Error("File mot uploaded");
            }
            return RedirectToAction("ViewDocuments", "Actions", new { id = Requestid });
        }
        #endregion
        #region DeleteFile
        public async Task<IActionResult>DeleteFile(int? id, int Requestid)
        {
            if(await _IActions.DeleteDocuments(id.ToString()))
            {
                _INotyfService.Success("File Deleted Successfully");
            }
            else
            {
                _INotyfService.Error("File Not deleted");
            }
            return RedirectToAction("ViewDocuments", "Actions", new { id = Requestid });
        }
        #endregion
        #region DeleteAllFiles
        public async Task<IActionResult> DeleteAllFiles(string deleteids, int Requestid)
        {
            if (await _IActions.DeleteDocuments(deleteids))
            {
                _INotyfService.Success("All Files have been deleted successfully");
            }
            else
            {
                _INotyfService.Error("All Files have not been deleted successfully");
            }
            return RedirectToAction("ViewDocuments", "Actions", new { id = Requestid });
        }
        #endregion
        public IActionResult SendOrder()
        {
            return View("../AdminPanel/Actions/SendOrder");
        }
    }
}