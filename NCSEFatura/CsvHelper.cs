using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NCSEFatura
{
    internal static class CsvHelper
    {
        internal static List<FaturaAdres> GetFaturaAdres(string fileFullPath)
        {
            var result = new List<FaturaAdres>();

            using (TextFieldParser parser = new TextFieldParser(fileFullPath,Encoding.Default))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                while (!parser.EndOfData)
                {
                    var faturaAdres = new FaturaAdres();

                    //Process row
                    string[] fields = parser.ReadFields();

                    faturaAdres.ServisKullaniciAd = fields[0].Trim();
                    faturaAdres.ServisSife = fields[1].Trim();

                    faturaAdres.FaturaProfile = fields[2].Trim();
                    faturaAdres.AdSoyad = fields[3].Trim();
                    faturaAdres.Adres = fields[4].Trim();
                    faturaAdres.Ilce = fields[5].Trim();
                    faturaAdres.Sehir = fields[6].Trim();
                    faturaAdres.Ulke = fields[7].Trim();
                    faturaAdres.VergiDaire = fields[8].Trim();
                    faturaAdres.VergiNo = fields[9].Trim();
                    faturaAdres.KesimTarih = fields[10].Trim();
                    faturaAdres.Eposta = fields[11].Trim();
                    faturaAdres.FaturaSeriNo = fields[12].Trim();

                    if (fields.Length == 15)
                    {
                        faturaAdres.ServisBasari = fields[13].Trim();
                        faturaAdres.ServisKod = fields[14].Trim();
                        faturaAdres.ServisAciklama = fields[15].Trim();
                    }

                    result.Add(faturaAdres);
                }
            }

            return result;
        }

        internal static void Save(string fileFullPath, List<FaturaAdres> faturaAdress)
        {
            //before your loop
            var csv = new StringBuilder();

            foreach (var item in faturaAdress)
            {
                var newLine = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};", 
                    item.ServisKullaniciAd,item.ServisSife, item.FaturaProfile, item.AdSoyad,item.Adres,
                    item.Ilce,item.Sehir,item.Ulke,item.VergiDaire,item.VergiNo,item.KesimTarih,
                    item.Eposta,item.FaturaSeriNo,item.ServisBasari,item.ServisKod,item.ServisAciklama);

                csv.AppendLine(newLine);
            }

            //after your loop
            File.WriteAllText(fileFullPath, csv.ToString(),Encoding.Default);
        }
    }
}
