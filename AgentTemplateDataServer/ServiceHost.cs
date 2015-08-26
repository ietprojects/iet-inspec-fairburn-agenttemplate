using IET.Inspec.Fairburn.Framework;
using System;
using System.Data.SqlClient;
using System.Reflection;

namespace IET.Inspec.Fairburn.AgentTemplate {
    	
	public class ServiceHost : WindowsService {

		private Type _ServiceType;
		private WcfSelfHost _Host;

#region Constructors

		public ServiceHost() {
			base.ServiceName = ServiceHostDefinition.ServiceName;
			_ServiceType = typeof(Service);
		}

#endregion

#region Methods

		static void Main(string[] args) {
			if (args.Length > 0) {
				switch (args[0].ToUpperInvariant()) {
				case "-PING":
					PerformPing();
					return;
				}
			}
			AddUsage("-ping");
			WindowsService.ProgramEntry(args, typeof(ServiceHost), ServiceHostDefinition.ServiceName);
		}

		protected override void OnStart(string[] args) {
			try {
				AppDomain.CurrentDomain.SetData("FairburnWindowsService", this);
				_Host = new WcfSelfHost(_ServiceType);
				LogEntry("Agent service host started.");
			} catch (Exception ex) {
				LogError("Service start failed." + Environment.NewLine + ex.ToString());
			}
		}

		protected override void OnStop() {
			try {
				if (_Host != null) {
					_Host.Dispose();
					_Host = null;
				}
				AppDomain.CurrentDomain.SetData("FairburnWindowsService", null);
				LogEntry("Agent service host stopped.");
			} catch (Exception ex) {
				LogError("Service stop failed." + Environment.NewLine + ex.ToString());
			}
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (_Host != null) {
					_Host.Dispose();
					_Host = null;
				}
			}
			base.Dispose(disposing);
		}

#endregion

#region Private

		private static void PerformPing() {
			string connectionString = AppSettings.GetString("AgentTemplateDbConnectionString");
			int commandTimeoutSeconds = AppSettings.GetInt32("AgentTemplateDbCommandTimeoutSeconds", 30);
			DatabaseConnection dbc = new DatabaseConnection(connectionString, commandTimeoutSeconds);
			AssemblyName exe = Assembly.GetExecutingAssembly().GetName();
			Console.WriteLine(exe.Name + ", Version=" + exe.Version);
			Console.WriteLine("Database Connection String: " + dbc.ConnectionString);
			Console.WriteLine("Ping:");
			try {
				using (DatabaseRequest db = new DatabaseRequest(dbc, "PingGetDatabaseInfo")) {
					using (SqlDataReader reader = db.ExecuteSingleReader()) {
						if (reader.Read()) {
							Console.WriteLine("   Timestamp: " + reader.GetDateTime(0).ToString());
							Console.WriteLine("   Server Instance: " + reader.GetString(1));
							Console.WriteLine("   Version: " + reader.GetString(2));
							Console.WriteLine("   Edition: " + reader.GetString(3));
							Console.WriteLine("   Database: " + reader.GetString(4));
							Console.WriteLine("   User: " + reader.GetString(5));
						} else {
							Console.WriteLine("   <no data returned>");
						}
					}
				}
			} catch (Exception ex) {
				Console.WriteLine("Ping failed." + Environment.NewLine + ex.ToString());
			}
		}

#endregion

	}

}
