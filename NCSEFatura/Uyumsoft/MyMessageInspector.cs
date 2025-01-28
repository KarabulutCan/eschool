using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace NCSEFatura.Uyumsoft
{
    public class MyMessageInspector : IClientMessageInspector
    {

        public MyMessageInspector()
        {

        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {

        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {

            int index = request.Headers.FindHeader("SoftwareDefinitionId", "IntegrationClient");

            if (index < 0)
            {

                var header = MessageHeader.CreateHeader("SoftwareDefinitionId", "IntegrationClient", " e6759fdc-aff4-4d1e-aacf-ba2590593e25");

                request.Headers.Add(header);

            }

            return null;
        }
    }
}
