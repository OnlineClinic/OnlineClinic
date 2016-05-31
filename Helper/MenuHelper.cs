using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web
{
    public class MenuHelper
    {
        public Dictionary<string, string> menuItems { get; set; }
        public MenuHelper()
        {
            menuItems = new Dictionary<string, string>()
            {
                { "Home","Home"},
                { "Features","Home"},
                { "How it Works","Home"},
                { "BT Medical devices","#btdivces"},
                { "Pricing","Home"}
                //{ "casa","Home"},
                //{ "Empregos","Home"},                
                //{ "Dinheiro","Home"},
                //{ "Tecnologia","Home"},               
                //{ "Viajar","Home"},
                //{ "Saúde","Home"},
                //{ "Receitas","Home"}
                //{ "Jogos","Home"},
                //{ "Wallpapers","Home"},
            };
        }

    }
}