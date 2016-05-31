using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Web.Models;
using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    [AuthorizeRole("Admin")]
    public class MemberShipController : Controller
    {
        private readonly IMembershipService _membershipServices;
        public MemberShipController(IMembershipService membershipService)
        {
            _membershipServices = membershipService;
        }

        //
        // GET: /Admin/City/
        public ActionResult Index()
        {
            var memberships = _membershipServices.GetMembershipList().Select(x => new MembershipViewModel { Name = StringCipher.Decrypt(x.Name), MemberShipId = x.Id, Duration = x.Duration, Price = x.Price,IsActive=x.IsActive }).ToList();
            return View(memberships);
        }

        // GET: /Admin/Country/Create
        public ActionResult Add(int? id)
        {
            if (id.HasValue)
            {
                var model = _membershipServices.GetMembershipById(Convert.ToInt32(id));
                model.Name = StringCipher.Decrypt(model.Name);
                MembershipViewModel cModel = new MembershipViewModel(model);
                return View(cModel);
            }
            else
            {
                MembershipViewModel model = new MembershipViewModel();
                return View(model);
            }
        }

        //
        // POST: /Admin/Country/Create
        [HttpPost]
        public ActionResult Add(MembershipViewModel model)
        {
            try
            {
                Membership cModel = new Membership();
                cModel = model.GetEditModel(model);

                if (model.MemberShipId > 0)
                {
                    _membershipServices.Update(cModel);
                }
                else
                {
                    _membershipServices.Add(cModel);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }


        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            var ids = collection[0];
            _membershipServices.DeleteMembership(Convert.ToString(ids));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Active(FormCollection collection)
        {
            var ids = collection[0];
            _membershipServices.ActivateMembership(Convert.ToString(ids));
            return RedirectToAction("Index");
        }

    }
}
