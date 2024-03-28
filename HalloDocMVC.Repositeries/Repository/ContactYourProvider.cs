using HalloDocMVC.DBEntity.DataContext;
using HalloDocMVC.DBEntity.DataModels;
using HalloDocMVC.DBEntity.ViewModels.AdminPanel;
using HalloDocMVC.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocMVC.Repositories.Admin.Repository
{
    public class ContactYourProvider : IContactYourProvider
    {
        #region Configuration
        private readonly HalloDocContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ContactYourProvider(HalloDocContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
        }
        #endregion
        public async Task<List<ProviderModel>> GetContacts()
        {
            List<ProviderModel> upload = await (from r in _context.Physicians
                                                join Notifications in _context.Physiciannotifications
                                                on r.Physicianid equals Notifications.Physicianid into aspGroup
                                                from nof in aspGroup.DefaultIfEmpty()
                                                join role in _context.Roles
                                                on r.Roleid equals role.Roleid into roleGroup
                                                from roles in roleGroup.DefaultIfEmpty()
                                                where r.Isdeleted == new BitArray(1)
                                                select new ProviderModel
                                                {
                                                    NotificationId = nof.Id,
                                                    CreatedDate = r.Createddate,
                                                    PhysicianId = r.Physicianid,
                                                    Address1 = r.Address1,
                                                    Address2 = r.Address2,
                                                    AltPhoneNumber = r.Altphone,
                                                    BusinessName = r.Businessname,
                                                    BusinessWebsite = r.Businesswebsite,
                                                    City = r.City,
                                                    FirstName = r.Firstname,
                                                    LastName = r.Lastname,
                                                    Notification = nof.Isnotificationstopped,
                                                    RoleName = roles.Name,
                                                    Status = r.Status,
                                                    Email = r.Email,
                                                    Isnondisclosuredoc = r.Isnondisclosuredoc == null ? false : true
                                                }).ToListAsync();
                           
            return upload;



        }
        public async Task<List<ProviderModel>> PhysicianByRegion(int? region)
        {
            List<ProviderModel> details = await (from pr in _context.Physicianregions
                                                 join ph in _context.Physicians
                                                 on pr.Physicianid equals ph.Physicianid into rGroup
                                                 from r in rGroup.DefaultIfEmpty()
                                                 join Notifications in _context.Physiciannotifications
                                                 on r.Physicianid equals Notifications.Physicianid into aspGroup
                                                 from nof in aspGroup.DefaultIfEmpty()
                                                 join role in _context.Roles
                                                 on r.Roleid equals role.Roleid into roleGroup
                                                 from roles in roleGroup.DefaultIfEmpty()
                                                 where pr.Regionid == region
                                                 select new ProviderModel
                                                 {
                                                     CreatedDate = r.Createddate,
                                                     PhysicianId = r.Physicianid,
                                                     Address1 = r.Address1,
                                                     Address2 = r.Address2,
                                                     AdminNotes = r.Adminnotes,
                                                     AltPhoneNumber = r.Altphone,
                                                     BusinessName = r.Businessname,
                                                     BusinessWebsite = r.Businesswebsite,
                                                     City = r.City,
                                                     FirstName = r.Firstname,
                                                     LastName = r.Lastname,
                                                     Notification = nof.Isnotificationstopped,
                                                     RoleName = roles.Name,
                                                     Status = r.Status
                                                 }).ToListAsync();
            return details;
        }

        #region ChangeNotification
        public async Task<bool> ChangeNotification(Dictionary<int, bool> changeValueDict)
        {
            try
            {
                if (changeValueDict == null)
                {
                    return false;
                }
                else
                {
                    foreach(var item in changeValueDict)
                    {
                        var ar = _context.Physiciannotifications.Where(r => r.Physicianid == item.Key).FirstOrDefault();
                        if (ar != null)
                        {
                            ar.Isnotificationstopped[0] = item.Value;
                            _context.Physiciannotifications.Update(ar);
                            _context.SaveChanges();
                        }
                        else
                        {
                            Physiciannotification pn = new Physiciannotification();
                            pn.Physicianid = item.Key;
                            pn.Isnotificationstopped = new BitArray(1);
                            pn.Isnotificationstopped[0] = item.Value;
                            _context.Physiciannotifications.Add(pn);
                            _context.SaveChanges();
                        }
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
