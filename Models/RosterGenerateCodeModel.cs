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
    public class RosterGenerateCodeModel
    {
        public int CodeId { get; set; }
        public int AvailabilityId { get; set; }
        public string GeneratedCode { get; set; }
        public bool IsUsed { get; set; }

        public RosterGenerateCodeModel()
        { }

        public RosterGenerateCodeModel(RosterGeneratedCode model)
        {
            this.CodeId = model.CodeId;
            this.AvailabilityId = model.AvailabilityId;
            this.GeneratedCode = model.GeneratedCode;
            this.IsUsed = model.IsUsed;     
        }
    }

}