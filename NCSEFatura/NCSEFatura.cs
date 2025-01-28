using System;
using System.IO;
using System.Linq;
using Uyumsoft;
using System.Xml;
using System.Xml.Serialization;
using NCSEFatura.Uyumsoft;

namespace NCSEFatura
{
    public class NCSEFatura
    {
        Logging Log = new Logging();
        private string _dPath;

        public NCSEFatura()
        {
            Log.DeleteLog();
            Log.Write("NCSEFatura Startup ctor.");
        }

        public void Execute(string sp, string dPath)
        {
            Log.Write("========= Efatura Execute =========");
            Log.Write("Service Provider:" + sp + " Data path:" + dPath);

            try
            {
                _dPath = dPath;

                ServiceProvider serviceProvider;
                if (sp.ToLower() == "veriban")
                {
                    serviceProvider = ServiceProvider.Veriban;
                    Veriban(serviceProvider);
                }
                else if (sp.ToLower() == "uyumsoft")
                {
                    serviceProvider = ServiceProvider.Uyumsoft;
                    Uyumsoft(serviceProvider);
                }
                else
                {
                    serviceProvider = ServiceProvider.Gecersiz;
                    Log.Write("Geçersiz EFatura servis saglayicisi.");
                }
            }
            catch (Exception ex)
            {
                Log.Write("Exception :" + ex.Message);

                if (ex.InnerException != null)
                {
                    Log.Write("-------------------------------");
                    Log.Write("Inner Exception");
                    Log.Write(ex.InnerException.Message);
                    Log.Write("-------------------------------");
                }
            }
        }

        private void Veriban(ServiceProvider serviceProvider)
        {
            Log.Write("ServiceProvider : VERIBAN, start");

            bool enableConsoleWrite = true;

            var faturaAdress = CsvHelper.GetFaturaAdres(_dPath + "\\FATGIDEN.CSV");
            //Log.Write("FaturaAdres :" + faturaAdress.ToString());

            var faturaAdres = faturaAdress.FirstOrDefault();
            Login.UserName = faturaAdres.ServisKullaniciAd;
            Login.Password = faturaAdres.ServisSife;
            Login.UseTestService = false;

            if (Login.UserName == "TESTER@VRBN")
                Login.UseTestService = true;

            Log.Write("Login username:" + Login.UserName);
            string sessionCode = EArsiv.LoginTest();

            //Console.WriteLine("LoginTest =>" + b);
            //Console.WriteLine(Login.UserName);
            //Console.WriteLine(Login.Password);

            //if (enableConsoleWrite)
            //    Log.Write(b.ToString());
            //  Console.WriteLine(b);

            var sayac = 0;
            var count = faturaAdress.Count;

            //Log.Write("sessionCode aliniyor");
            //string sessionCode = EArsiv.getSessionCode();
            //Log.Write("sessionCode :" + sessionCode);

            if (sessionCode != "")
            {
                foreach (var item in faturaAdress)
                {
                    sayac++;

                    var fileFullPath = string.Format(_dPath + "\\FAT{0}.XML", item.FaturaSeriNo);
                    Log.Write("");
                    Log.Write("fileFullPath :" + fileFullPath.ToString());

                    if (string.IsNullOrEmpty(item.VergiNo))
                    {
                        item.ServisBasari = "0";
                        item.ServisKod = "";
                        item.ServisAciklama = "Geçersiz vergi no";
                        continue;
                    }

                    if (!string.IsNullOrEmpty(item.ServisKod))
                        continue;

                    Log.Write("mukellef sorgu basladi");
                    var operationResult = EArsiv.EFaturaMukkelefKontrol(item.VergiNo, sessionCode);
                    Log.Write("mukellef sorgu bitti");

                    Guid guid = Guid.NewGuid();

                    if (operationResult.OperationCompleted)
                    {
                        Log.Write("XMLUpdate basladi");
                        XmlHelper.Update(fileFullPath, "EARSIVFATURA", guid, serviceProvider);
                        Log.Write("XMLUpdate tamamlandi");

                        var transferResult = EArsiv.Transfer(fileFullPath, sessionCode);

                        if (transferResult.OperationCompleted)
                        {
                            item.ServisBasari = "1";
                            Log.Write("EArsiv.Transfer basarili");
                        }
                        else
                        {
                            item.ServisBasari = "0";
                            Log.Write("EArsiv.Transfer basarisiz");
                        }

                        item.ServisKod = transferResult.TransferFileUniqueId;
                        item.ServisAciklama = transferResult.Description;

                        if (enableConsoleWrite)
                        {
                            Log.Write("OperationCompleted:" + transferResult.OperationCompleted.ToString());
                            Log.Write("Description:" + transferResult.Description);
                            Log.Write("TransferFileUniqueId:" + transferResult.TransferFileUniqueId);
                            //Console.WriteLine(transferResult.OperationCompleted);
                            //Console.WriteLine(transferResult.Description);
                            //Console.WriteLine(transferResult.TransferFileUniqueId);
                        }
                    }
                    else
                    {
                        var faturaprofile = item.FaturaProfile;
                        if (faturaprofile == "")
                        {
                            faturaprofile = "TEMELFATURA";
                        }

                        Log.Write("TEMELFATURA");

                        XmlHelper.Update(fileFullPath, faturaprofile, guid, serviceProvider);
                        Log.Write("XmlHelper.Update tamamlandi");

                        var Ef = new EFatura();
                        var result = Ef.Transfer(fileFullPath, guid, item.Eposta);

                        if (result.OperationCompleted)
                            item.ServisBasari = "1";
                        else
                            item.ServisBasari = "0";
                        Log.Write("EfTransfer tamamlandi " + item.ServisBasari.ToString());

                        item.ServisKod = result.TransferFileUniqueId;
                        item.ServisAciklama = result.Description;

                        if (enableConsoleWrite)
                        {
                            Log.Write("OperationCompleted:" + result.OperationCompleted.ToString());
                            Log.Write("Description:" + result.Description);
                        }
                    }
                }

                CsvHelper.Save(_dPath + "\\FATGIDEN.CSV", faturaAdress);

                if (enableConsoleWrite)
                {
                    Log.Write("Bitti");
                }
            }
            else
                Log.Write("sessionCode bos geldi");


            Log.Write("ServiceProvider : VERIBAN, end");
        }

        private void Uyumsoft(ServiceProvider serviceProvider)
        {
            Log.Write("ServiceProvider : UYUMSOFT, Start");
            bool enableConsoleWrite = false;

            var faturaAdress = CsvHelper.GetFaturaAdres(_dPath + "\\FATGIDEN.CSV");
            Log.Write("faturaAdres:" + faturaAdress);

            var faturaAdres = faturaAdress.FirstOrDefault();
            Login.UserName = faturaAdres.ServisKullaniciAd;
            Login.Password = faturaAdres.ServisSife;
            Login.UseTestService = false;

            Login.UserName = "Uyumsoft";
            Login.Password = "Uyumsoft";

            if (Login.UserName == "Uyumsoft")
                Login.UseTestService = true;

            var sayac = 0;
            var count = faturaAdress.Count;

            foreach (var item in faturaAdress)
            {
                sayac++;

                var fileFullPath = string.Format(_dPath + "\\FAT{0}.XML", item.FaturaSeriNo);
                Log.Write("fileFullPath : " + fileFullPath.ToString());

                if (string.IsNullOrEmpty(item.VergiNo))
                {
                    item.ServisBasari = "0";
                    item.ServisKod = "";
                    item.ServisAciklama = "Geçersiz vergi no";
                    //File.Delete(fileFullPath);
                    continue;
                }

                if (!string.IsNullOrEmpty(item.ServisKod))
                    continue;

                Login.UserName = faturaAdres.ServisKullaniciAd;
                Login.Password = faturaAdres.ServisSife;
                Login.UseTestService = false;

                if (Login.UserName == "Uyumsoft" && Login.UserName == "Uyumsoft")
                    Login.UseTestService = true;

                Guid guid = Guid.NewGuid();
                CreateInvoiceFromXml(fileFullPath, guid, item.VergiNo);
            }

            CsvHelper.Save(_dPath + "\\FATGIDEN.CSV", faturaAdress);

            if (enableConsoleWrite)
            {
                Log.Write("Bitti");
                //Console.WriteLine("Bitti");
                //Console.ReadKey();
            }
            Log.Write("ServiceProvider : UYUMSOFT, end");
        }

        private void CreateInvoiceFromXml(string path, Guid guid, string vergiNo)
        {
            Log.Write("CreateInvoiceFromXml");
            Log.Write("path:" + path + " guid:" + guid.ToString() + " vergiNo:" + vergiNo);

            var client = new BasicIntegrationClient();

            if (Login.UseTestService)
            {
                client.Endpoint.Address = new System.ServiceModel.EndpointAddress("http://efatura-test.uyumsoft.com.tr/Services/BasicIntegration");
            }
            client.Endpoint.EndpointBehaviors.Add(new MyEndpointBehavior());

            client.ClientCredentials.UserName.UserName = Login.UserName;
            client.ClientCredentials.UserName.Password = Login.Password;

            UserInformation userInformation = new UserInformation();
            userInformation.Username = Login.UserName;
            userInformation.Password = Login.Password;

            var isEInvoiceUser = client.IsEInvoiceUserAsync(userInformation, vergiNo, string.Empty).GetAwaiter().GetResult();

            if (isEInvoiceUser.Value)
            {
                XmlHelper.Update(path, "TEMELFATURA", guid, ServiceProvider.Uyumsoft);
            }
            else
            {
                //path =@"C:\Users\Ahmet\Desktop\feb7bf4a-b7b8-4112-a95a-7da4a62729cb.xml";
                XmlHelper.Update(path, "EARSIVFATURA", guid, ServiceProvider.Uyumsoft);
            }

            StreamReader stream = new StreamReader(path);

            var rootAttribute = new XmlRootAttribute("Invoice")
            {
                Namespace = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2",
                IsNullable = false
            };

            XmlSerializer serializer = new XmlSerializer(typeof(InvoiceType), rootAttribute);

            var obj = serializer.Deserialize(stream.BaseStream);
            var invoice = obj as InvoiceType;
            stream.Close();
            stream.Dispose();
            //invoice.EArchiveInvoiceInfo = new EArchiveInvoiceInformation { DeliveryType = InvoiceDeliveryType.Paper };
            //invoice.EArchiveInvoiceInfo.InternetSalesInfo =  new InternetSalesInformation {  WebAddress="www.turkcell.com.tr", PaymentDate= Convert.ToDateTime('20151101') , ShipmentInfo = new ShipmentInformation {  } }

            InvoiceInfo[] invoiceInfo = new InvoiceInfo[] { new InvoiceInfo { Invoice = invoice } };
            //var client = InvoiceTasks.Instance.CreateClient(true);

            //var response = client.SaveAsDraft(InvoiceTasks.Instance.GetUserInfo(true), invoiceInfo);
            //var response = client.SaveAsDraft(userInformation, invoiceInfo);
            var response = client.SendInvoiceAsync(userInformation, invoiceInfo).GetAwaiter().GetResult();
            if (response.IsSucceded)
            {
                Log.Write("Fatura Gönderildi");
                Log.Write("UUID:" + response.Value[0].Id.ToString());
                Log.Write("ID:" + response.Value[0].Number.ToString());
                //Console.WriteLine("Fatura Gönderildi\n UUID:{0} \n ID:{1} ", response.Value[0].Id.ToString(), response.Value[0].Number.ToString());
            }
            else
            {
                Log.Write("Not Succeded, " + response.Message);
                //Console.WriteLine(response.Message);
            }
        }

        #region Logging
        public class Logging
        {
            public void Write(string sInfo)
            {
                StreamWriter sw = File.AppendText("NCSLog.txt");
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff ") + sInfo.ToString());
                sw.Flush();
                sw.Close();
            }
            public void DeleteLog()
            {
                if (File.Exists("NCSLog.txt"))
                    File.Delete("NCSLog.txt");
            }
        }
        #endregion
    }
}
