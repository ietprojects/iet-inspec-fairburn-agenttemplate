using System;
using System.Runtime.Serialization;

namespace IET.Inspec.Fairburn.AgentTemplate.Contract
{

    [DataContract]
    public class TaskResponse
    {

        private Guid _Lock;

        [DataMember]
        public Guid Lock
        {
            get
            {
                return _Lock;
            }
            set
            {
                _Lock = value;
            }
        }

    }

}
