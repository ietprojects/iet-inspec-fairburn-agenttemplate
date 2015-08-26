using IET.Inspec.Fairburn.AgentTemplate;
using IET.Inspec.Fairburn.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace AgentTemplateNUnit.Test
{
    [TestFixture]
    public class ClientTest
    {
		private static WcfSelfHost _Host;
		
		[SetUp]
		public void MyTestInitialize() {
			_Host = new WcfSelfHost(typeof(Service));
		}
		
		[TearDown]
		public void MyTestCleanup() {
            _Host.Dispose();
            _Host = null;
		}

        [Test]
		public void ClientConstructorTest() {
			Client x = new Client();
			Assert.IsInstanceOf(typeof(Client), x);
			Assert.AreEqual("NetTcpBinding_IService", x.EndpointName);
            Assert.AreEqual("net.tcp://localhost:42171/Fairburn/AgentTemplate/", x.EndpointAddress);
		}

        [Test]
		public void MainTests() {
			Client x = new Client();
			DoPing(x);
			DoGetTask(x);
			DoPostResults(x);
		}

		private void DoPing(Client client) {
			AgentPingResponse resp = client.Ping();
            DatabaseConnection db = new DatabaseConnection(AppSettings.GetString("AgentTemplateDbConnectionString"));
			Assert.AreEqual(resp.Database, db["Database"]);
		}

		private void DoGetTask(Client client) {
            IET.Inspec.Fairburn.AgentTemplate.Proxy.GetTaskRequest req = new IET.Inspec.Fairburn.AgentTemplate.Proxy.GetTaskRequest();
			req.TestMode = 1;
            IET.Inspec.Fairburn.AgentTemplate.Proxy.TaskResponse resp = client.GetTask(req);
			Assert.IsNull(resp);
		}

		private void DoPostResults(Client client) {
            IET.Inspec.Fairburn.AgentTemplate.Proxy.PostResultsRequest req = new IET.Inspec.Fairburn.AgentTemplate.Proxy.PostResultsRequest();
			req.TestMode = 1;
			AgentAcknowledgeResponse resp = client.PostResults(req);
			Assert.IsInstanceOf(typeof(AgentAcknowledgeResponse), resp);
		}

	}
}
