using ESCHOOL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;

namespace ESCHOOL.Controllers
{
    public class ThemeController : Controller
    {
        public string GetTheme(string themeParamater)
        {
            string theme = "https://kendo.cdn.telerik.com/2021.2.616/styles/kendo." + themeParamater + ".min.css";

            return theme;
        }
        public string GetThemeMobile(string themeParamater)
        {
            string theme = "https://kendo.cdn.telerik.com/2021.2.616/styles/kendo." + themeParamater + ".mobile.min.css";

            return theme;
        }
        public string GetThemeColor(string colorParamater)
        {
            string color = "#ffffff}";
            if (colorParamater == "black" || colorParamater == "metroblack" || colorParamater == "highcontrast" || colorParamater == "moonlight") color = "#222";

            return color;
        }
    }
}