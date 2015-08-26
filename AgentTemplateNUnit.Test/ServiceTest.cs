using IET.Inspec.Fairburn.AgentTemplate;
using IET.Inspec.Fairburn.AgentTemplate.Contract;
using IET.Inspec.Fairburn.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentTemplateNUnit.Test
{
    [TestFixture]
    public class ServiceTest
    {
        [Test]
        public void PingTest()
        {
            Service x = new Service();
            AgentPingResponse resp = x.Ping();
            DatabaseConnection db = new DatabaseConnection(AppSettings.GetString("AgentTemplateDbConnectionString"));
            Assert.AreEqual(resp.Database, db["Database"]);
        }

        [Test]
        public void GetTaskTest1()
        {
            Service x = new Service();
            GetTaskRequest req = new GetTaskRequest();
            req.TestMode = 1;
            TaskResponse resp = x.GetTask(req);
            Assert.IsNull(resp);
        }

        [Test]
        public void GetTaskTest2()
        {
            Service x = new Service();
            GetTaskRequest req = new GetTaskRequest();
            req.TestMode = 2;
            TaskResponse resp = x.GetTask(req);
            Assert.IsInstanceOf(typeof(Guid), resp.Lock);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTaskTestBad()
        {
            Service x = new Service();
            x.GetTask(null);
        }

        [Test]
        public void PostResultsTest1()
        {
            Service x = new Service();
            PostResultsRequest req = new PostResultsRequest();
            req.Lock = new Guid();
            req.TestMode = 1;
            AgentAcknowledgeResponse resp = x.PostResults(req);
            Assert.IsInstanceOf(typeof(AgentAcknowledgeResponse), resp);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PostResultsTestBad()
        {
            Service x = new Service();
            x.PostResults(null);
        }

    }
}
