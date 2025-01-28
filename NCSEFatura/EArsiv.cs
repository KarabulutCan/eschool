using System;
using System.ServiceModel;
using Ionic.Zip;
using System.IO;
using VeribanEArsiv;
using static NCSEFatura.NCSEFatura;


namespace NCSEFatura
{

    internal static class EArsiv
    {


        internal static string LoginTest()
        {
            LogWrite("LoginTest basladi");
            string sessionCode = null;

            using (var serviceClient = new IntegrationServiceClient())
            {
                if (Login.UseTestService)
                {
                    LogWrite("Login.UseTestService");
                    serviceClient.Endpoint.Address = new EndpointAddress("http://earsivtransfertest.veriban.com.tr/IntegrationService.svc");
                }

                try
                {
                    sessionCode = serviceClient.LoginAsync(Login.UserName, Login.Password).GetAwaiter().GetResult();
                }
                catch (TimeoutException timeProblem)
                {
                    var err = timeProblem.Message;
                    LogWrite("TimeoutException err:" + err.ToString());
                }
                catch (FaultException<VeribanServiceFault> veribanFault)
                {
                    var err = veribanFault.Detail.FaultCode;
                    err = veribanFault.Detail.FaultDescription;
                    LogWrite("FaultException err:" + err.ToString());
                }
                catch (CommunicationException commProblem)
                {
                    var err = commProblem.Message;
                    LogWrite("CommunicationException err:" + err.ToString());
                }
                catch (Exception unknownEx)
                {
                    var err = unknownEx.Message;
                    LogWrite("unknownEx err:" + err.ToString());
                }
            }
            LogWrite("LoginTest bitti, sessionCode: " + sessionCode);
            return sessionCode;
        }


        //internal static bool LoginTest()
        //{
        //    var b = false;
        //    using (var serviceClient = new IntegrationServiceClient())
        //    {
        //        if (Login.UseTestService)
        //            serviceClient.Endpoint.Address = new EndpointAddress("http://earsivtransfertest.veriban.com.tr/IntegrationService.svc");

        //        string sessionCode = null;

        //        try
        //        {
        //            sessionCode = serviceClient.LoginAsync(Login.UserName, Login.Password).GetAwaiter().GetResult();
        //            b = true;
        //        }
        //        catch (TimeoutException timeProblem)
        //        {
        //            var err = timeProblem.Message;
        //            b = false;
        //        }
        //        catch (FaultException<VeribanServiceFault> veribanFault)
        //        {
        //            var err = veribanFault.Detail.FaultCode;
        //            err = veribanFault.Detail.FaultDescription;

        //            b = false;
        //        }
        //        catch (CommunicationException commProblem)
        //        {
        //            var err = commProblem.Message;
        //            b = false;
        //        }
        //        catch (Exception unknownEx)
        //        {
        //            b = false;
        //            var err = unknownEx.Message;
        //        }
        //    }

        //    return b;
        //}

        /// <summary>
        /// OperationResult->OperationCompleted değeri false ise E-Fatura Mükkellefi  true ise E-Arşiv Mükellefi
        /// </summary>
        /// <param name="registerNumber"></param>
        /// <returns></returns>
        internal static OperationResult EFaturaMukkelefKontrol(string registerNumber, string sessionCode)
        {
            OperationResult operationResult = null;

            using (var serviceClient = new IntegrationServiceClient())
            {
                if (Login.UseTestService)
                    serviceClient.Endpoint.Address = new EndpointAddress("http://earsivtransfertest.veriban.com.tr/IntegrationService.svc");

                //string sessionCode = null;
                try
                {
                    //sessionCode = serviceClient.LoginAsync(Login.UserName, Login.Password).GetAwaiter().GetResult();

                    operationResult = serviceClient.CheckRegisterNumberIsEInvoiceCustomerAsync(sessionCode, registerNumber).GetAwaiter().GetResult();
                }
                catch (TimeoutException timeProblem)
                {
                    operationResult = new OperationResult()
                    {
                        OperationCompleted = false,
                        Description = string.Format("TimeoutException: {0}", timeProblem.Message)
                    };
                }
                catch (FaultException<VeribanServiceFault> veribanFault)
                {
                    operationResult = new OperationResult()
                    {
                        OperationCompleted = false,
                        Description = string.Format("Code:[{0}], Message: {1}", veribanFault.Detail.FaultCode, veribanFault.Detail.FaultDescription)
                    };
                }
                catch (CommunicationException commProblem)
                {
                    operationResult = new OperationResult()
                    {
                        OperationCompleted = false,
                        Description = string.Format("CommunicationException: {0}", commProblem.Message)
                    };
                }
                catch (Exception unknownEx)
                {
                    operationResult = new OperationResult()
                    {
                        OperationCompleted = false,
                        Description = string.Format("UnknownException: {0}", unknownEx.Message)
                    };
                }
            }

            return operationResult;
        }

        internal static void LogWrite(string sInfo)
        {
            StreamWriter sw = File.AppendText("NCSLog.txt");
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff ") + sInfo.ToString());
            sw.Flush();
            sw.Close();

        }

        //internal static string getSessionCode()
        //{
        //    LogWrite("getSessionCode");

        //    string sessionCode = null;
        //    using (var serviceClient = new IntegrationServiceClient())
        //    {
        //        if (Login.UseTestService)
        //        {
        //            serviceClient.Endpoint.Address = new EndpointAddress("http://earsivtransfertest.veriban.com.tr/IntegrationService.svc");
        //            LogWrite("Login.UseTestService");
        //        }
        //        sessionCode = serviceClient.LoginAsync(Login.UserName, Login.Password).GetAwaiter().GetResult();
        //        LogWrite("serviceClient.LoginAsync tamamlandi");
        //    }

        //    LogWrite("getSessionCode, sesionCode:" + sessionCode);
        //    return sessionCode;
        //}

        internal static TransferResult Transfer(string fileFullPath, string sessionCode)
        {
            TransferResult transferResult = null;

            LogWrite("Transfer basladi");
            using (var serviceClient = new IntegrationServiceClient())
            {
                if (Login.UseTestService)
                {
                    serviceClient.Endpoint.Address = new EndpointAddress("http://earsivtransfertest.veriban.com.tr/IntegrationService.svc");
                    LogWrite("Login.UseTestService");
                }

                //string sessionCode = null;
                try
                {
                    LogWrite("Gönderilecek dosya ZipBinaryArray'e dönüştürülür");

                    //Gönderilecek dosya ZipBinaryArray'e dönüştürülür.
                    byte[] zipFileBinaryDataArray = null;
                    using (MemoryStream memoryStreamOutput = new MemoryStream())
                    {
                        using (var zip = new ZipFile())
                        {
                            zip.AddFile(fileFullPath, string.Empty);
                            zip.Save(memoryStreamOutput);
                            LogWrite("zip.Save");
                        }

                        zipFileBinaryDataArray = memoryStreamOutput.ToArray();
                    }

                    //Zip Binary Data Array'in Standart MD5 Hash bilgisi hesaplanır.
                    HashGenerator hashGenerator = new HashGenerator();
                    string zipFileHash = hashGenerator.GetMD5Hash(zipFileBinaryDataArray);
                    LogWrite("hashGenerator.GetMD5Hash tamamlandi");

                    EArchiveTransferFile transferFile = new EArchiveTransferFile()
                    {
                        FileNameWithExtension = Path.GetFileNameWithoutExtension(fileFullPath) + ".zip",    //Transfer edilecek dosya adı, dosya uzantısı .zip olmalıdır.
                        FileDataType = TransferDocumentDataTypes.XML_INZIP,           //ZIP dosyası içerisindeki dosya formatı XML.
                        BinaryData = zipFileBinaryDataArray,                                                //ZIP dosyası Binary64 Data
                        BinaryDataHash = zipFileHash,                                                       //ZIP dosyası Binary64 Data MD5 Hash değeri

                        //ReceiverMailTargetAddresses = new string[] { "ABC@XXX.COM", "XYZ@XXX.NET" },        //(Opsiyonel) Mail gönderilecek alıcı mail adresleri.

                        InvoiceTransportationType = InvoiceTransportationTypes.ELEKTRONIK,
                        IsInvoiceCreatedAtDelivery = false,
                        IsInternetSalesInvoice = false,
                    };

                    //////if(sessionCode == null)
                    //////{
                    //////    sessionCode = serviceClient.LoginAsync(Login.UserName, Login.Password).GetAwaiter().GetResult();
                    //////    LogWrite("serviceClient.LoginAsync tamamlandi");
                    //////}

                    transferResult = serviceClient.TransferSalesInvoiceFileAsync(sessionCode, transferFile).GetAwaiter().GetResult();
                    LogWrite("serviceClient.TransferSalesInvoceFileAsync");

                }
                catch (TimeoutException timeProblem)
                {
                    transferResult = new TransferResult()
                    {
                        OperationCompleted = false,
                        Description = string.Format("TimeoutException: {0}", timeProblem.Message)
                    };
                    LogWrite("TimeoutException: " + timeProblem.Message.ToString());
                }
                catch (FaultException<VeribanServiceFault> veribanFault)
                {
                    transferResult = new TransferResult()
                    {
                        OperationCompleted = false,
                        Description = string.Format("Code:[{0}], Message: {1}", veribanFault.Detail.FaultCode, veribanFault.Detail.FaultDescription)
                    };
                    LogWrite("Exception Code:[" + veribanFault.Detail.FaultCode.ToString() + "], Message: [" + veribanFault.Detail.FaultDescription.ToString() + "]");
                }
                catch (CommunicationException commProblem)
                {
                    transferResult = new TransferResult()
                    {
                        OperationCompleted = false,
                        Description = string.Format("CommunicationException: {0}", commProblem.Message)
                    };
                    LogWrite("CommunicationException: " + commProblem.Message.ToString());
                }
                catch (Exception unknownEx)
                {
                    transferResult = new TransferResult()
                    {
                        OperationCompleted = false,
                        Description = string.Format("UnknownException: {0}", unknownEx.Message)
                    };
                    LogWrite("UnknownException: " + unknownEx.Message.ToString());
                }

                //if (transferResult.OperationCompleted)
                //{
                //    System.Console.ForegroundColor = ConsoleColor.Green;
                //    System.Console.WriteLine("!!! TRANSFER BAŞARILI !!!");

                //    System.Console.ForegroundColor = ConsoleColor.Yellow;
                //    System.Console.WriteLine(Environment.NewLine + "TRANSFER DÖKÜMAN NUMARASI [ " + transferResult.TransferFileUniqueId + " ]");
                //}
                //else
                //{
                //    System.Console.ForegroundColor = ConsoleColor.Red;
                //    System.Console.WriteLine("!!! TRANSFER BAŞARISIZ !!!");
                //}

                //System.Console.ResetColor();
                //System.Console.WriteLine(Environment.NewLine + transferResult.Description);
            }

            return transferResult;
        }
    }

}
