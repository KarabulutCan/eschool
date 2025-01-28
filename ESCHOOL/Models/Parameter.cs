using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    /* 
   1	Mesleği
   2	Yakınlığı
   3	Kayıt Şekli
   4	Kayıt Durumu
   5	Ödeme Şekli
   6	İndirim Dilekçesinde Çıkan İndirimler
   7	Çek / Senet Pozisyonları	
   8	Sınıf Tipleri	
   9	Muhasebe Girişinde Kullanılan Kod	
   10	Servis Durumu	
   11	Servis Hattı	
   12	Geldiği Okul	
   13	Geldiği Bölüm	
   14	İl ve İlçeler	
   15	Uyruğu	L1	
   16	Dini	L1	
   17	Kan Grubu	
   18	Nüfus Cüzdanı	
   19	Entegratörler	
   20	Cinsiyeti	
   21	Muhasebe Fiş Tipi	
   22	Kullanıcı İzinleri
   23	Sms. Servis Sağlayıcı
   24	Dil
   25	Denetim İzi Bölümleri
    */

    public class Parameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }
        public Nullable<int> CategorySubID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "CategoryName", ResourceType = typeof(Resources.Resource))]
        public string CategoryName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "CategoryName", ResourceType = typeof(Resources.Resource))]
        public string Language1 { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "CategoryName", ResourceType = typeof(Resources.Resource))]
        public string Language2 { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "CategoryName", ResourceType = typeof(Resources.Resource))]
        public string Language3 { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "CategoryName", ResourceType = typeof(Resources.Resource))]
        public string Language4 { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Display(Name = "Color", ResourceType = typeof(Resources.Resource))]
        public string Color { get; set; }

        [Column(TypeName = "char(2)")]
        public string CategoryLevel { get; set; }

        [Display(Name = "SortOrder", ResourceType = typeof(Resources.Resource))]
        public Nullable<int> SortOrder { get; set; }

        public Nullable<bool> IsActive { get; set; }

        public Nullable<bool> IsProtected { get; set; }

        [Column(TypeName = "char(2)")]
        public string NationalityCode { get; set; }

        public Nullable<bool> IsSelect { get; set; }
        public Nullable<bool> IsDirtySelect { get; set; }

    }
}
