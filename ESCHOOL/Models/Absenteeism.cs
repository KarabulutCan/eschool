using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class Absenteeism
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AbsenteeismID { get; set; }
        public int SchoolID { get; set; }

        [Column(TypeName = "char(9)")]
        [Display(Name = "Period", ResourceType = typeof(Resources.Resource))]
        public string Period { get; set; }

        public int StudentID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        public Nullable<DateTime> Date { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day01 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day02 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day03 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day04 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day05 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day06 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day07 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day08 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day09 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day10 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day11 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day12 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day13 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day14 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day15 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day16 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day17 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day18 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day19 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day20 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day21 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day22 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day23 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day24 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day25 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day26 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day27 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day28 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day29 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day30 { get; set; }

        [Column(TypeName = "char(1)")]
        public string Day31 { get; set; }
        public Nullable<int> Total1 { get; set; }
        public Nullable<int> Total2 { get; set; }
    }
}
