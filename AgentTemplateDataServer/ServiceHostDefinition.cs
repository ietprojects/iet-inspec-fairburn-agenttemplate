using IET.Inspec.Fairburn.Framework;
using System;
using System.ComponentModel;
using System.ServiceProcess;

namespace IET.Inspec.Fairburn.AgentTemplate {

	[RunInstaller(true)]
	public class ServiceHostDefinition : WindowsServiceDefinition {

		public const string ServiceName = "FairburnAgentTemplateServer";
        public const string DisplayName = "Fairburn Agent Template Server";
        public const string Description = "Hosts the Agent Template WCF service.";
		public const ServiceStartMode StartMode = ServiceStartMode.Automatic;
		public const ServiceAccount Account = ServiceAccount.NetworkService;

#region Constructors

		public ServiceHostDefinition()
			: base() {
			SetServiceName(ServiceName);
			SetDisplayName(DisplayName);
			SetDescription(Description);
			SetStartType(StartMode);
			SetAccount(Account);
			InstallService();
		}

#endregion

	}

}
