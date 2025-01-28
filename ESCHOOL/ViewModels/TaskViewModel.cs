using ESCHOOL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ESCHOOL.ViewModels
{
    public class TaskViewModel
    {
        public int ViewModelID { get; set; }
        public int TaskID { get; set; }
        public int TaskTypeID { get; set; }
        public int ClassroomID { get; set; }
        public int SchoolID { get; set; }
        public int StudentID { get; set; }
        public int UserID { get; set; }

        public string StudentPicture { get; set; }
        public string Name { get; set; }
        public string StudentClassroom { get; set; }
        public string StudentNumber { get; set; }

        public string Title { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Start { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime End { get; set; }
        public string Description { get; set; }
        public string RecurrenceId { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public Nullable<bool> IsAllDay { get; set; }
        public Nullable<int> OwnerID { get; set; }

        public string SelectedCulture { get; set; }
        public string PdfName { get; set; }
        public int ResourceDefaultValue { get; set; }

        public string Color01Name { get; set; }
        public string Color02Name { get; set; }
        public string Color03Name { get; set; }
        public string Color04Name { get; set; }
        public string Color05Name { get; set; }
        public string Color06Name { get; set; }
        public string Color07Name { get; set; }
        public string Color08Name { get; set; }
        public string Color09Name { get; set; }
        public string Color10Name { get; set; }
        public string Color11Name { get; set; }
        public string Color12Name { get; set; }
        public string Color13Name { get; set; }
        public string Color14Name { get; set; }
        public string Color15Name { get; set; }
        public string Color16Name { get; set; }
        public string Color17Name { get; set; }
        public string Color18Name { get; set; }
        public string Color19Name { get; set; }
        public string Color20Name { get; set; }

        public int Color01Value { get; set; }
        public int Color02Value { get; set; }
        public int Color03Value { get; set; }
        public int Color04Value { get; set; }
        public int Color05Value { get; set; }
        public int Color06Value { get; set; }
        public int Color07Value { get; set; }
        public int Color08Value { get; set; }
        public int Color09Value { get; set; }
        public int Color10Value { get; set; }
        public int Color11Value { get; set; }
        public int Color12Value { get; set; }
        public int Color13Value { get; set; }
        public int Color14Value { get; set; }
        public int Color15Value { get; set; }
        public int Color16Value { get; set; }
        public int Color17Value { get; set; }
        public int Color18Value { get; set; }
        public int Color19Value { get; set; }
        public int Color20Value { get; set; }

        public string Color01 { get; set; }
        public string Color02 { get; set; }
        public string Color03 { get; set; }
        public string Color04 { get; set; }
        public string Color05 { get; set; }
        public string Color06 { get; set; }
        public string Color07 { get; set; }
        public string Color08 { get; set; }
        public string Color09 { get; set; }
        public string Color10 { get; set; }
        public string Color11 { get; set; }
        public string Color12 { get; set; }
        public string Color13 { get; set; }
        public string Color14 { get; set; }
        public string Color15 { get; set; }
        public string Color16 { get; set; }
        public string Color17 { get; set; }
        public string Color18 { get; set; }
        public string Color19 { get; set; }
        public string Color20 { get; set; }

        public string Type11 { get; set; }
        public string Type12 { get; set; }
        public string Type13 { get; set; }
        public string Type14 { get; set; }
        public string Type15 { get; set; }
        public string Type16 { get; set; }
        public string Type17 { get; set; }
        public string Type18 { get; set; }
        public string Type19 { get; set; }
        public string Type20 { get; set; }

    }
}
