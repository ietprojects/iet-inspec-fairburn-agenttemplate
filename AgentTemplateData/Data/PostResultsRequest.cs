using System;
using System.Runtime.Serialization;

namespace IET.Inspec.Fairburn.AgentTemplate.Contract
{

    [DataContract]
    public class PostResultsRequest
    {

        private Guid _Lock;
        private int _TestMode;

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
