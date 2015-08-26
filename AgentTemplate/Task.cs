using IET.Inspec.Fairburn.Framework;
using IET.Inspec.Fairburn.AgentTemplate.Proxy;
using System;

namespace IET.Inspec.Fairburn.AgentTemplate {

	public partial class Task : AgentTask {

#pragma warning disable 0649
		private int _TestMode;
#pragma warning restore 0649

        private TaskResponse _Task;
        private PostResultsRequest _Results;

#region Constructors

		public Task() {
		}

#endregion

#region Methods

		public override void Initialise(IAgentTaskHost taskHost, int instance) {
            base.Initialise(taskHost, instance);
		}

		public override bool GetTask() {
            using (Client service = new Client())
            {
                GetTaskRequest request = new GetTaskRequest();
                request.TestMode = _TestMode;
                _Task = service.GetTask(request);
                return (_Task != null);
            }
		}

		public override void ProcessTask() {
            _Results = new PostResultsRequest();
            _Results.Lock = _Task.Lock;

			// ...

		}

		public override void PostResults() {
            using (Client service = new Client())
            {
                service.PostResults(_Results);
            }
		}

#endregion

	}

}
