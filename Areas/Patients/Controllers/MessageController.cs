using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Models;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Areas.Patients.Controllers
{
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        //
        // GET: /Patients/Message/
        public ActionResult Index()
        {
            var list = _messageService.GetMessageList().Where(x => x.ReceiverId == base.loginUserModel.LoginId);
            var model = list.Select(x => new MessageViewModels { SenderName = StringCipher.Decrypt(x.logins.Member.FirstName) + " " + StringCipher.Decrypt(x.logins.Member.SurName), Title = StringCipher.Decrypt(x.Title), IsRead = x.IsRead, CreatedDate = x.CreatedDate }).ToList();
            return View(model);
        }

        //
        // GET: /Patients/Message/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Patients/Message/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Patients/Message/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Patients/Message/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Patients/Message/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Patients/Message/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Patients/Message/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
