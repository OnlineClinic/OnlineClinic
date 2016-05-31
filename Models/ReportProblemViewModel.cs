using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyOnlineClinic.Entity;

namespace MyOnlineClinic.Web.Models
{
    public class ReportProblemViewModel :  BasedEntity
    {
        public int ReportProblemId { get; set; }
        public string ReportProblemSubject { get; set; }
        public Guid SenderId { get; set; }
        public Guid RecevierId { get; set; }
        public int RoleType { get; set; }
        public bool IsRead { get; set; }
        public bool IsCompleted { get; set; }


        public int ReportProblemReplyId { get; set; }
        
        public Guid ReplyById { get; set; }
        public string Message { get; set; }
        public string ReplyeMessage { get; set; }
        public string UserName { get; set; }
        
        
    }
}