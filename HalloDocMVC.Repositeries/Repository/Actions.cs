//Implementation :logic of page 
using HalloDocMVC.DBEntity.DataContext;
using HalloDocMVC.DBEntity.DataModels;
using HalloDocMVC.DBEntity.ViewModels.AdminPanel;
using HalloDocMVC.Repositories.Admin.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HalloDocMVC.Repositories.Admin.Repository
{
    public class Actions : IActions
    {
        private readonly HalloDocContext _context;
        public Actions(HalloDocContext context)
        {
            _context = context;
        }

        public ViewCaseModel GetRequestForViewCase(int id)
        {
            var n = _context.Requests.FirstOrDefault(E => E.Requestid == id);
            var l = _context.Requestclients.FirstOrDefault(E => E.Requestid == id);
            var region = _context.Regions.FirstOrDefault(E => E.Regionid == l.Regionid);
            ViewCaseModel requestforviewcase = new()
            {
                RequestId = id,
                Region = region.Name,
                RequestTypeId = n.Requesttypeid,
                FirstName = l.Firstname,
                LastName = l.Lastname,
                ConfirmationNumber = n.Confirmationnumber,
                PhoneNumber = l.Phonenumber,
                Email = l.Email,
                Address = l.Street + "," + l.City + "," + l.State,
                Notes = l.Notes,
                Room = l.Address,
                DateOfBirth = new DateTime((int)l.Intyear, DateTime.ParseExact(l.Strmonth, "MMMM", new CultureInfo("en-us")).Month, (int)l.Intdate),
            };
            return requestforviewcase;
        }
        #region EditCase
        public bool EditCase(ViewCaseModel model)
        {
            try
            {
                int monthnum = model.DateOfBirth.Month;
                int year = model.DateOfBirth.Year;
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthnum);
                int date = model.DateOfBirth.Day;
                Requestclient client = _context.Requestclients.FirstOrDefault(E => E.Requestid == model.RequestId);
                if (client != null)
                {
                    client.Firstname = model.FirstName;
                    client.Lastname = model.LastName;
                    client.Email = model.Email;
                    client.Phonenumber = model.PhoneNumber;
                    client.Intdate = model.DateOfBirth.Day; //or date
                    client.Intyear = model.DateOfBirth.Year;
                    client.Strmonth = monthName;
                    client.Notes = model.Notes;
                    List<string> location = model.Address.Split(',').ToList(); //It splits the model's Address property by the comma character and converts the resulting array into a list of strings. It assigns this list to a variable named location.
                    client.Street = location[0];
                    client.City = location[1];
                    client.State = location[2];
                    client.Address = model.Room;
                    _context.Requestclients.Update(client);
                    _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region AssignProvider
        public async Task<bool> AssignProvider(int RequestId, int ProviderId, string notes)
        {

            var request = await _context.Requests.FirstOrDefaultAsync(req => req.Requestid == RequestId);
            request.Physicianid = ProviderId;
            request.Status = 2;
            _context.Requests.Update(request);
            _context.SaveChanges();

            Requeststatuslog rsl = new ()
            {
                Requestid = RequestId,
                Physicianid = ProviderId,

                Createddate = DateTime.Now,
            };
            _context.Requeststatuslogs.Update(rsl);
            _context.SaveChanges();

            return true;


        }
        #endregion

        #region Cancel Case
        public bool CancelCase(int RequestId, string Note, string CaseTag)
        {
            try
            {
                var requestData = _context.Requests.FirstOrDefault(e => e.Requestid == RequestId);
                if (requestData != null)
                {
                    requestData.Casetag = CaseTag;
                    requestData.Status = 8;
                    _context.Requests.Update(requestData);
                    _context.SaveChanges();
                    Requeststatuslog rsl = new Requeststatuslog
                    {
                        Requestid = RequestId,
                        Notes = Note,
                        Status = 3,
                        Createddate = DateTime.Now
                    };
                    _context.Requeststatuslogs.Add(rsl);
                    _context.SaveChanges();
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Clear Case
        public bool ClearCase(int RequestID)
        {
            try
            {
                var request = _context.Requests.FirstOrDefault(req => req.Requestid == RequestID);
                if (request != null)
                {
                    request.Status = 10;
                    _context.Requests.Update(request);
                    _context.SaveChanges();

                    Requeststatuslog rsl = new()
                    {
                        Requestid = RequestID,
                        Status = 10,
                        Createddate = DateTime.Now
                    };
                    _context.Requeststatuslogs.Add(rsl);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Block Case
        public bool BlockCase(int RequestID, string Note)
        {
            try
            {
                var requestData = _context.Requests.FirstOrDefault(e => e.Requestid == RequestID);
                if (requestData != null)
                {
                    requestData.Status = 11;
                    _context.Requests.Update(requestData);
                    _context.SaveChanges();
                    Blockrequest blc = new Blockrequest
                    {
                        Requestid = requestData.Requestid.ToString(),
                        Phonenumber = requestData.Phonenumber,
                        Email = requestData.Email,
                        Reason = Note,
                        Createddate = DateTime.Now,
                        Modifieddate = DateTime.Now
                    };
                    _context.Blockrequests.Add(blc);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
        
        
        #region TransferPhysician
        public async Task<bool> TransferPhysician(int RequestId, int ProviderId, string Note)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(req => req.Requestid == RequestId);

            Requeststatuslog rsl = new()
            {
                Requestid = RequestId,
                Status = 2,
                Physicianid = request.Physicianid,
                Transtophysicianid = ProviderId,
                Notes = Note,
                Createddate = DateTime.Now
            };
            _context.Requeststatuslogs.Update(rsl);
            _context.SaveChanges();

            request.Physicianid = ProviderId;
            request.Status = 2;
            _context.Requests.Update(request);
            _context.SaveChanges();
            return true;
        }
        #endregion TransferPhysician

        #region getNotes
        public ViewNotesModel getNotes(int id)
        {
            var request = _context.Requests.FirstOrDefault(e => e.Requestid == id);
            var symptoms = _context.Requestclients.FirstOrDefault(e => e.Requestid == id);
            var transfer = (from rs in _context.Requeststatuslogs
                               join py in _context.Physicians on rs.Physicianid equals py.Physicianid into pyGroup
                               from py in pyGroup.DefaultIfEmpty()
                               join p in _context.Physicians on rs.Transtophysicianid equals p.Physicianid into pGroup
                               from p in pGroup.DefaultIfEmpty()
                               join a in _context.Admins on rs.Adminid equals a.Adminid into aGroup
                               from a in aGroup.DefaultIfEmpty()
                               where rs.Requestid == id && rs.Status == 2
                               select new TransferNotesModel
                               {
                                   TransToPhysician = p.Firstname,
                                   Admin = a.Firstname,
                                   Physician = py.Firstname,
                                   RequestId = rs.Requestid,
                                   Notes = rs.Notes,
                                   Status = rs.Status,
                                   PhysicianId = rs.Physicianid,
                                   CreatedDate = rs.Createddate,
                                   RequestStatusLogId = rs.Requeststatuslogid,
                                   TransToAdmin = rs.Transtoadmin,
                                   TransToPhysicianId = rs.Transtophysicianid
                               }).ToList();
            var cancelbyprovider = _context.Requeststatuslogs.Where(e => e.Requestid == id && (e.Transtoadmin != null));
            var cancelbyadmin = _context.Requeststatuslogs.Where(e => e.Requestid == id && (e.Status == 7 || e.Status == 3));
            var model = _context.Requestnotes.FirstOrDefault(e => e.Requestid == id);
            ViewNotesModel vn = new ViewNotesModel();
            vn.RequestId = id;
            vn.PatientNotes = symptoms.Notes;
            if (model == null)
            {
                vn.PhysicianNotes = "-";
                vn.AdminNotes = "-";
            }
            else
            {
                vn.Status = request.Status;
                vn.RequestNotesId = model.Requestnotesid;
                vn.PhysicianNotes = model.Physiciannotes ?? "-";
                vn.AdminNotes = model.Adminnotes ?? "-";
            }

            List<TransferNotesModel> transnotes = new List<TransferNotesModel>();
            foreach (var item in transfer)
            {
                transfer.Add(new TransferNotesModel
                {
                    TransToPhysician = item.TransToPhysician,
                    Admin = item.Admin,
                    Physician = item.Physician,
                    RequestId = item.RequestId,
                    Notes = item.Notes ?? "-",
                    Status = item.Status,
                    PhysicianId = item.PhysicianId,
                    CreatedDate = item.CreatedDate,
                    RequestStatusLogId = item.RequestStatusLogId,
                    TransToAdmin = item.TransToAdmin,
                    TransToPhysicianId = item.TransToPhysicianId
                });
            }
            vn.transfernotes = transfer;
            List<TransferNotesModel> cancelbyphysician = new List<TransferNotesModel>();
            foreach (var item in cancelbyprovider)
            {
                cancelbyphysician.Add(new TransferNotesModel
                {
                    RequestId = item.Requestid,
                    Notes = item.Notes ?? "-",
                    Status = item.Status,
                    PhysicianId = item.Physicianid,
                    CreatedDate = item.Createddate,
                    RequestStatusLogId = item.Requeststatuslogid,
                    TransToAdmin = item.Transtoadmin,
                    TransToPhysicianId = item.Transtophysicianid
                });
            }
            vn.cancelbyphysician = cancelbyphysician;

            List<TransferNotesModel> cancelrq = new List<TransferNotesModel>();
            foreach (var item in cancelbyadmin)
            {
                cancelrq.Add(new TransferNotesModel
                {
                    RequestId = item.Requestid,
                    Notes = item.Notes ?? "-",
                    Status = item.Status,
                    PhysicianId = item.Physicianid,
                    CreatedDate = item.Createddate,
                    RequestStatusLogId = item.Requeststatuslogid,
                    TransToAdmin = item.Transtoadmin,
                    TransToPhysicianId = item.Transtophysicianid
                });
            }
            vn.cancel = cancelrq;

            return vn;
        }
        #endregion

        #region Edit_notes
        public bool EditViewNotes(string? adminnotes, string? physiciannotes, int RequestID)
        {
            try
            {
                Requestnote notes = _context.Requestnotes.FirstOrDefault(E => E.Requestid == RequestID);
                if (notes != null)
                {
                    if (physiciannotes != null)
                    {
                        if (notes != null)
                        {
                            notes.Physiciannotes = physiciannotes;
                            notes.Modifieddate = DateTime.Now;
                            _context.Requestnotes.Update(notes);
                            _context.SaveChangesAsync();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (adminnotes != null)
                    {
                        if (notes != null)
                        {
                            notes.Adminnotes = adminnotes;
                            notes.Modifieddate = DateTime.Now;
                            _context.Requestnotes.Update(notes);
                            _context.SaveChangesAsync();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Requestnote rn = new Requestnote
                    {
                        Requestid = RequestID,
                        Adminnotes = adminnotes,
                        Physiciannotes = physiciannotes,
                        Createddate = DateTime.Now,
                        Createdby = "001e35a5 - cd12 - 4ec8 - a077 - 95db9d54da0f"
                    };
                    _context.Requestnotes.Add(rn);
                    _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
