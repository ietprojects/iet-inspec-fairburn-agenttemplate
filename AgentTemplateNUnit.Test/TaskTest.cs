using IET.Inspec.Fairburn.AgentTemplate;
using IET.Inspec.Fairburn.AgentTemplate.Proxy;
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
    public class TaskTest
    {

        [Test]
        public void GetTaskTest()
        {
            WcfSelfHost host = new WcfSelfHost(typeof(Service));
            IET.Inspec.Fairburn.AgentTemplate.Task x = new IET.Inspec.Fairburn.AgentTemplate.Task();
            var privateObject = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(x);
            x.Initialise(new Host(), 0);
            privateObject.SetField("_TestMode", 1);
            bool ok = Convert.ToBoolean(privateObject.Invoke("GetTask"));
            Assert.IsFalse(ok);
            host.Dispose();
            host = null;
        }

        [Test]
        public void ProcessTaskTest()
        {
            IET.Inspec.Fairburn.AgentTemplate.Task x = new IET.Inspec.Fairburn.AgentTemplate.Task();
            var privateObject = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(x);
            x.Initialise(new Host(), 0);
            TaskResponse resp = new TaskResponse();
            privateObject.SetField("_Task", resp);
            privateObject.Invoke("ProcessTask");
        }

        [Test]
        public void PostResultsTests()
        {
            WcfSelfHost host = new WcfSelfHost(typeof(Service));
            IET.Inspec.Fairburn.AgentTemplate.Task x = new IET.Inspec.Fairburn.AgentTemplate.Task();
            var privateObject = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(x);
            x.Initialise(new Host(), 0);
            PostResultsRequest req = new PostResultsRequest();
            req.TestMode = 1;
            privateObject.SetField("_Results", req);
            privateObject.Invoke("PostResults");
            //privateObject.Invoke("APrivateMethod", new object[] { param1, param2}); -- Call Private method with parameters
            host.Dispose();
            host = null;
        }

        private class Host : IAgentTaskHost
        {

            public List<String> Messages = new List<string>();

            public bool Starting
            {
                get
                {
                    return true;
                }
            }

            public bool Started
            {
                get
                {
                    return true;
                }
            }

            public bool Stopping
            {
                get
                {
                    return false;
                }
            }

            public bool Stopped
            {
                get
                {
                    return false;
                }
            }

            public void LogEntry(string entry)
            {
                Messages.Add("Entry: " + entry);
            }

            public void LogWarning(string warning)
            {
                Messages.Add("Warning: " + warning);
            }

            public void LogError(string err)
            {
                Messages.Add("Error: " + err);
            }

            public void LogEntryAfterFailure(string entry)
            {
                Messages.Add("EntryAfterFailure: " + entry);
            }

        }
    }


}
