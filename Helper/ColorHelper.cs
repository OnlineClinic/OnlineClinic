using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web
{
    public class ColorHelper
    {
        private Dictionary<int, string> colorClassItems { get; set; }
        public ColorHelper()
        {
            colorClassItems = new Dictionary<int, string>()
            {
                { 1,"yellow"},
                { 2,"green"},
                { 3,"red"},
                { 4,"orange"},
                { 5,"cyan"},
                { 6,"purple"},
                { 7,"forest-green"},               
                { 8,"base-alt"},
                { 9,"light"},
                { 10,"alpha"},
            };
        }

        public string getColorCode(int code)
        {
            string color=string.Empty;
            colorClassItems.TryGetValue(code,out color);
            return color;
        }
    }
}