using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MyOnlineClinic.Web.Models;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Models
{
    public class MessageViewModels : BasedEntity
    {
        public string senderId { get; set; }
        public string reveiverId { get; set; }
        public string message { get; set; }
        public bool IsRead { get; set; }
        public string SenderName { get; set; }
        public string Title { get; set; }

    }
}
