using IET.Inspec.Fairburn.Framework;
using System;
using System.Reflection;

namespace IET.Inspec.Fairburn.AgentTemplate {

	public class AgentTemplateService : AgentService {

#region Constructors

        public AgentTemplateService()
        {
			base.ServiceName = AgentTemplateDefinition.ServiceName;
		}

#endregion

#region Methods

		static void Main(string[] args) {
			if (args == null) throw new ArgumentNullException("args");
			if (args.Length > 0) {
				switch (args[0].ToUpperInvariant()) {
				case "-PING":
					PerformPing();
					return;
				}
			}
			AddUsage("-ping");
            AgentService.ProgramEntry(args, typeof(AgentTemplateService), AgentTemplateDefinition.ServiceName);
		}

		protected override WindowsServiceThread[] CreateThreads(int count) {
			WindowsServiceThread[] threads = new WindowsServiceThread[count];
			for (int x = 0; x < count; x++) {
				threads[x] = new AgentThread<Task>(this, x + 1);
			}
			return threads;
		}

#endregion

#region Private

        private static void PerformPing()
        {
            Console.WriteLine("In PerformPing() method.");
            using (Client service = new Client())
            {
                AssemblyName exe = Assembly.GetExecutingAssembly().GetName();
                Console.WriteLine(exe.Name + ", Version=" + exe.Version);
                Console.WriteLine("Endpoint Name: " + service.EndpointName);
                Console.WriteLine("Endpoint Address: " + service.EndpointAddress);
                Console.WriteLine("Ping:");
                try
                {
                    AgentPingResponse response = service.Ping();
                    if (response != null)
                    {
                        Console.WriteLine("   Timestamp: " + response.Timestamp.ToString());
                        Console.WriteLine("   Server Instance: " + response.ServerInstance);
                        Console.WriteLine("   Version: " + response.Version);
                        Console.WriteLine("   Edition: " + response.Edition);
                        Console.WriteLine("   Database: " + response.Database);
                        Console.WriteLine("   User: " + response.User);
                    }
                    else
                    {
                        Console.WriteLine("   <no data returned>");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ping failed." + Environment.NewLine + ex.ToString());
                }
            }
		}

#endregion

	}

}
