using ESCHOOL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;

namespace ESCHOOL.Controllers
{
    public class DecadeController : Controller
    {
        public IList Decade(IList mylist, int plusYear)
        {
            int year = (DateTime.Now.Year + plusYear);
            int startYearInt = year - 18;

            int j = 0;
            for (int i = 0; i < 10; i++)
            {
                j++;
                year = year - 1;
                DateTime sYear = new DateTime(year, 1, 1);
                string str = (sYear.ToString("yyyy") + "-" + (sYear.AddYears(1).ToString("yyyy")));
                mylist.Add(new Parameter() { CategoryID = j, CategoryName = str, IsActive = true, SortOrder = j });
            }
            return (mylist);
        }
        public string Period()
        {
            int year = DateTime.Now.Year;
            DateTime sYear = new DateTime(year, 1, 1);
            string str = (sYear.ToString("yyyy") + "-" + (sYear.AddYears(1).ToString("yyyy")));
            return (str);
        }
    }
}
