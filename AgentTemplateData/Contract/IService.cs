using IET.Inspec.Fairburn.Framework;
using System;
using System.ServiceModel;

namespace IET.Inspec.Fairburn.AgentTemplate.Contract
{

    [ServiceContract]
    public interface IService
    {

        [OperationContract]
        AgentPingResponse Ping();

        [OperationContract]
        TaskResponse GetTask(GetTaskRequest request);

        [OperationContract]
        AgentAcknowledgeResponse PostResults(PostResultsRequest request);

    }

}
