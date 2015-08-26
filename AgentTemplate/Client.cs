using IET.Inspec.Fairburn.Framework;
using System;
using IET.Inspec.Fairburn.AgentTemplate.Proxy;

namespace IET.Inspec.Fairburn.AgentTemplate
{

    public class Client : WcfObject<ServiceClient>
    {

        #region Constructors

        public Client()
            : base(GetServiceClient())
        {
        }

        #endregion

        #region Properties

        public string EndpointName
        {
            get
            {
                return this.Object.Endpoint.Name;
            }
        }

        public string EndpointAddress
        {
            get
            {
                return this.Object.Endpoint.Address.ToString();
            }
        }

        #endregion

        #region Methods

        public AgentPingResponse Ping()
        {
            return InvokeAndCheck<AgentPingResponse>(() => this.Object.Ping());
        }

        public TaskResponse GetTask(GetTaskRequest request)
        {
            return InvokeAndCheck<TaskResponse>(() => this.Object.GetTask(request));
        }

        public AgentAcknowledgeResponse PostResults(PostResultsRequest request)
        {
            return InvokeAndCheck<AgentAcknowledgeResponse>(() => this.Object.PostResults(request));
        }

        #endregion

        #region Private

        private static ServiceClient GetServiceClient()
        {
            string endpoint = AppSettings.GetString("AgentEndpointName", "Agent.NetTcpEndpoint");
            string address = AppSettings.GetString("AgentRemoteAddress");
            if (string.IsNullOrEmpty(address))
            {
                return new ServiceClient(endpoint);
            }
            else
            {
                return new ServiceClient(endpoint, address);
            }
        }

        #endregion

    }

}
