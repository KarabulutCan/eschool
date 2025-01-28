using Uyumsoft;

namespace NCSEFatura.Uyumsoft
{
    public class InvoiceTasks
    {
        public static InvoiceTasks Instance = new InvoiceTasks();

        private InvoiceTasks()
        {

        }

        public BasicIntegrationClient CreateClient(bool test)
        {
            var username = "";
            var password = "";
            var serviceuri = "";
            if (test)
            {
                username = "Uyumsoft";
                password = "Uyumsoft";
                serviceuri = "http://efatura-test.uyumsoft.com.tr/Services/BasicIntegration";
            }
            else
            {
                //username = Settings1.Default.WebServiceLiveUsername;
                //password = Settings1.Default.WebServiceLivePassword;
                //serviceuri = Settings1.Default.WebServiceLiveUri;
            }

            //var client = new BasicIntegrationClient();
            //client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceuri);
            //client.ClientCredentials.UserName.UserName = username;
            //client.ClientCredentials.UserName.Password = password;
            //return client;

            var client = new BasicIntegrationClient();
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(serviceuri);
            client.ClientCredentials.UserName.UserName = username;
            client.ClientCredentials.UserName.Password = password;
            return client;
        }

        public UserInformation GetUserInfo(bool test)
        {
            UserInformation userInfo = new UserInformation();

            if (test)
            {
                userInfo.Username = "Uyumsoft";
                userInfo.Password = "Uyumsoft";
            }
            else
            {
                userInfo.Username = "";
                userInfo.Password = "";
            }

            return userInfo;
        }
    }
}
