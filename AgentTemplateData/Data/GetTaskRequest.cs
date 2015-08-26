using System;
using System.Runtime.Serialization;

namespace IET.Inspec.Fairburn.AgentTemplate.Contract
{

    [DataContract]
    public class GetTaskRequest
    {

        private int _TestMode;

        [DataMember]
        public int TestMode
        {
            get
            {
                return _TestMode;
            }
            set
            {
                _TestMode = value;
            }
        }

    }

}
