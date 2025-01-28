using System;
using System.IO;
using Ionic.Zip;
using System.ServiceModel;
using VeribanEFatura;
using NCSEFatura;

namespace NCSEFatura
{
	internal class EFatura
    {
		NCSEFatura.Logging Log = new NCSEFatura.Logging();

        //internal readonly string TestUserName = "TESTER@TEST";
        //internal readonly string TestPassword = "123456";
        //internal bool UseTestService = false;

        //internal static OperationResult Transfer(string fileFullPath, Guid guid,string email)
        //{
        //    //String envelopeId = "e002da78-223f-438c-addb-16badeb047b5";
        //    //String fileFullPath = envelopeId + ".xml";

        //    //Xml dosyasını diskten okumak için
        //    var xmlDocument = new XmlDocument();
        //    xmlDocument.Load(fileFullPath);
        //    var invoiceXmlContent = xmlDocument.InnerXml;

        //    var invoiceUUID = guid.ToString();

        //    //Gönderilecek dosya ZipBinaryArray'e dönüştürülür.
        //    byte[] zipFileBinaryDataArray = null;
        //    using (MemoryStream memoryStreamOutput = new MemoryStream())
        //    {
        //        using (ZipFile zip = new ZipFile())
        //        {
        //            zip.AddFile(fileFullPath, string.Empty);
        //            zip.Save(memoryStreamOutput);
        //        }
        //        zipFileBinaryDataArray = memoryStreamOutput.ToArray();
        //    }

        //    //Zip Binary Data Array'in Standart MD5 Hash bilgisi hesaplanır.
        //    var zipFileHash = MD5Cryptor.GetHashedPassword(zipFileBinaryDataArray);

        //    var client = new TransferDocumentServiceClient();

        //    if (Login.UseTestService)
        //        client.Endpoint.Address = new EndpointAddress("http://transfertest.veriban.com.tr");

        //    var transferDocument = new TransferDocument()
        //    {
        //        BinaryDataArray = zipFileBinaryDataArray,       //Gönderilen ZipDosyasının Binary Data Array karşılığı.
        //        BinaryDataHash = zipFileHash, //zipFileHash,                   //Gönderilen ZipBinaryDataArray in Hash karşılığı.
        //        DocumentType = DocumentType.Xml,                //ZIP dosyası içerisindeki dosya formatı XML.
        //        UUID = invoiceUUID,                             //transfer edilecek dosya için Unique bir değere ihtiyaç vardır
        //        FileName = invoiceUUID + ".zip",                //transfer edilecek dosya adı unique id olmalıdır ve dosya uzantısı ZIP olmalıdır.
        //        Alias = "urn:"+ email,    //standart posta kutusu etiketi bilgisi
        //        IsDirectSend = true,                             //true : Fatura direk imzalanarak GİB'na gönderilir. false: Fatura onay sürecinden geçtikten sonra GİB'na gönderilir.
        //    };

        //    var sessionID = string.Empty;
        //    client.Login(out sessionID, Login.UserName, Login.Password);
        //    var result = client.Transfer(transferDocument, sessionID);
        //    return result;
        //}

        public TransferResult Transfer(string fileFullPath, Guid guid, string email)
        {
            TransferResult transferResult = null;
            try
            {
                //gönderilecek fatura, zip formatına çevriliyor
                byte[] zipFileBinaryDataArray = null;
                using (MemoryStream memoryStreamOutput = new MemoryStream())
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AddFile(fileFullPath, string.Empty);
                        zip.Save(memoryStreamOutput);
                    }
                    zipFileBinaryDataArray = memoryStreamOutput.ToArray();
                }

                //md5 hash özeti alınıyor
                HashGenerator hashGenerator = new HashGenerator();
                string zipFileHash = hashGenerator.GetMD5Hash(zipFileBinaryDataArray);


                EInvoiceTransferFile transferFileArg = new EInvoiceTransferFile()
                {
                    //ZIP dosya ismi
                    FileNameWithExtension = Path.GetFileNameWithoutExtension(fileFullPath) + ".zip",

                    //ZIP dosyası içerisindeki dosya formatı
                    FileDataType = TransferDocumentDataTypes.XML_INZIP,

                    //Gönderilen ZipDosyasının Binary Data Array karşılığı.
                    BinaryData = zipFileBinaryDataArray,

                    //Gönderilen dosyasnın ZIP Hash karşılığı.
                    BinaryDataHash = zipFileHash,

                    //Standart posta kutusu etiketi bilgisi, etiket bilgisi boş gönderilir ise var olan etiket bilgisi sistem tarafından atanır.
                    CustomerAlias = "urn:" + email,

                    //true : Fatura direk imzalanarak GİB'na gönderilir. false: Fatura onay sürecinden geçtikten sonra GİB'na gönderilir.
                    IsDirectSend = true

                };

                
                IntegrationServiceClient client = new IntegrationServiceClient();

                if (Login.UseTestService)
                    client.Endpoint.Address = new EndpointAddress("http://efaturatransfertest.veriban.com.tr/IntegrationService.svc");

                var sessionCode = client.LoginAsync(Login.UserName, Login.Password).GetAwaiter().GetResult();

                using (client)
                {

                     transferResult = client.TransferSalesInvoiceFileAsync(sessionCode, transferFileArg).GetAwaiter().GetResult();

                    if (transferResult.OperationCompleted)
                    {
                        Log.Write("!!! TRANSFER SUCCESS !!!");
                        //Transfer durumu sorgulamak için gerekli
                        Log.Write("TRANSFER DOCUMENT NUMBER [ " + transferResult.TransferFileUniqueId + " ]");

                        //Console.WriteLine("!!! TRANSFER SUCCESS !!!");
                        ////Transfer durumu sorgulamak için gerekli
                        //Console.WriteLine("TRANSFER DOCUMENT NUMBER [ " + transferResult.TransferFileUniqueId + " ]");
                    }
                    else
                    {
                        Log.Write("!!! TRANSFER FAILURE !!!");
                        //Console.WriteLine("!!! TRANSFER FAILURE !!!");
                    }
                }
            }
            catch (TimeoutException timeProblem)
            {
                Log.Write("timeout:" + timeProblem.Message);
                //Console.WriteLine(timeProblem.Message);
            }
            catch (FaultException<VeribanServiceFault> veribanFault)
            {
                Log.Write("FaultCode:" + veribanFault.Detail.FaultCode);
                Log.Write("FaultDescription:" + veribanFault.Detail.FaultDescription);
                //Console.WriteLine(veribanFault.Detail.FaultCode);
                //Console.WriteLine(veribanFault.Detail.FaultDescription);
            }
            catch (CommunicationException commProblem)
            {
                Log.Write("commProblem:" + commProblem.Message);
                //Console.WriteLine(commProblem.Message);
            }
            catch (Exception unknownEx)
            {
                Log.Write("unknownEx:" + unknownEx.Message);
                //Console.WriteLine(unknownEx.Message);
            }

            return transferResult;
        }

        //internal static List<CustomerData> GetCustomerData(string customerRegisterNumber,string email)
        //{
        //    List<CustomerData> result = null;

        //     var client = new TransferDocumentServiceClient();

        //    if (Login.UseTestService)
        //        client.Endpoint.Address = new EndpointAddress("http://transfertest.veriban.com.tr");

        //    var sessionID = string.Empty;
        //    client.Login(out sessionID, Login.UserName, Login.Password);

        //    var customers = client.CheckIsThereCustomer(customerRegisterNumber, sessionID);
        //    if (customers != null)
        //    {
        //        var list = customers.ToList();
        //        result = list.Where(p => p.Alias.Contains(email)).ToList();

        //        //foreach (var item in customers)
        //        //{
        //        //    Console.WriteLine("IdentifierNumber:" + item.IdentifierNumber + "---   Alias:" + item.Alias + "---   Title:" + item.Title);
        //        //}
        //    }

        //    return result;
        //}
    }
}
