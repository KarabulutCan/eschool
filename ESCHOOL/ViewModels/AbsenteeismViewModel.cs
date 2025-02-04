﻿using ESCHOOL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ESCHOOL.ViewModels
{
    public class AbsenteeismViewModel
    {
        public int AbsenteeismID { get; set; }
        public string Period { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public int UserID { get; set; }
        public int ClassroomID { get; set; }
        public string ClassroomName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> Date { get; set; }

        public string MonthText { get; set; }
        public string StudentName { get; set; }

        public string HDay01 { get; set; }
        public string HDay02 { get; set; }
        public string HDay03 { get; set; }
        public string HDay04 { get; set; }
        public string HDay05 { get; set; }
        public string HDay06 { get; set; }
        public string HDay07 { get; set; }
        public string HDay08 { get; set; }
        public string HDay09 { get; set; }
        public string HDay10 { get; set; }
        public string HDay11 { get; set; }
        public string HDay12 { get; set; }
        public string HDay13 { get; set; }
        public string HDay14 { get; set; }
        public string HDay15 { get; set; }
        public string HDay16 { get; set; }
        public string HDay17 { get; set; }
        public string HDay18 { get; set; }
        public string HDay19 { get; set; }
        public string HDay20 { get; set; }
        public string HDay21 { get; set; }
        public string HDay22 { get; set; }
        public string HDay23 { get; set; }
        public string HDay24 { get; set; }
        public string HDay25 { get; set; }
        public string HDay26 { get; set; }
        public string HDay27 { get; set; }
        public string HDay28 { get; set; }
        public string HDay29 { get; set; }
        public string HDay30 { get; set; }
        public string HDay31 { get; set; }

        public string HTotal1 { get; set; }
        public string HTotal2 { get; set; }

        public string Day01 { get; set; }
        public string Day02 { get; set; }
        public string Day03 { get; set; }
        public string Day04 { get; set; }
        public string Day05 { get; set; }
        public string Day06 { get; set; }
        public string Day07 { get; set; }
        public string Day08 { get; set; }
        public string Day09 { get; set; }
        public string Day10 { get; set; }

        public string Day11 { get; set; }
        public string Day12 { get; set; }
        public string Day13 { get; set; }
        public string Day14 { get; set; }
        public string Day15 { get; set; }
        public string Day16 { get; set; }
        public string Day17 { get; set; }
        public string Day18 { get; set; }
        public string Day19 { get; set; }
        public string Day20 { get; set; }

        public string Day21 { get; set; }
        public string Day22 { get; set; }
        public string Day23 { get; set; }
        public string Day24 { get; set; }
        public string Day25 { get; set; }
        public string Day26 { get; set; }
        public string Day27 { get; set; }
        public string Day28 { get; set; }
        public string Day29 { get; set; }
        public string Day30 { get; set; }
        public string Day31 { get; set; }

        public int Total1 { get; set; }
        public int Total2 { get; set; }
        public string SelectedCulture { get; set; }

        public string Char01 { get; set; }
        public string Char02 { get; set; }
        public string Char03 { get; set; }
        public string Char04 { get; set; }
        public string Char05 { get; set; }
        public string Char06 { get; set; }

        public bool IsChar01 { get; set; }
        public bool IsChar02 { get; set; }
        public bool IsChar03 { get; set; }
        public bool IsChar04 { get; set; }
        public bool IsChar05 { get; set; }
        public bool IsChar06 { get; set; }

        public string Char01Explanation { get; set; }
        public string Char02Explanation { get; set; }
        public string Char03Explanation { get; set; }
        public string Char04Explanation { get; set; }
        public string Char05Explanation { get; set; }
        public string Char06Explanation { get; set; }

        public Nullable<int> Char01Max { get; set; }
        public Nullable<int> Char02Max { get; set; }
        public Nullable<int> Char03Max { get; set; }
        public Nullable<int> Char04Max { get; set; }
        public Nullable<int> Char05Max { get; set; }
        public Nullable<int> Char06Max { get; set; }

        public string HMonth01 { get; set; }
        public string HMonth02 { get; set; }
        public string HMonth03 { get; set; }
        public string HMonth04 { get; set; }
        public string HMonth05 { get; set; }
        public string HMonth06 { get; set; }
        public string HMonth07 { get; set; }
        public string HMonth08 { get; set; }
        public string HMonth09 { get; set; }
        public string HMonth10 { get; set; }
        public string HMonth11 { get; set; }
        public string HMonth12 { get; set; }

        public decimal Total101 { get; set; }
        public decimal Total102 { get; set; }
        public decimal Total103 { get; set; }
        public decimal Total104 { get; set; }
        public decimal Total105 { get; set; }
        public decimal Total106 { get; set; }
        public decimal Total107 { get; set; }
        public decimal Total108 { get; set; }
        public decimal Total109 { get; set; }
        public decimal Total110 { get; set; }
        public decimal Total111 { get; set; }
        public decimal Total112 { get; set; }

        public decimal Total201 { get; set; }
        public decimal Total202 { get; set; }
        public decimal Total203 { get; set; }
        public decimal Total204 { get; set; }
        public decimal Total205 { get; set; }
        public decimal Total206 { get; set; }
        public decimal Total207 { get; set; }
        public decimal Total208 { get; set; }
        public decimal Total209 { get; set; }
        public decimal Total210 { get; set; }
        public decimal Total211 { get; set; }
        public decimal Total212 { get; set; }

        public decimal GrandTotal1 { get; set; }
        public decimal GrandTotal2 { get; set; }

        public int WarningSW { get; set; }
    }
}
