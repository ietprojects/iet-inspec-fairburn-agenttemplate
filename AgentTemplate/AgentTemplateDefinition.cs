using IET.Inspec.Fairburn.Framework;
using System;
using System.ComponentModel;
using System.ServiceProcess;

namespace IET.Inspec.Fairburn.AgentTemplate {

	[RunInstaller(true)]
	public class AgentTemplateDefinition : WindowsServiceDefinition {

		public const string ServiceName = "FairburnAgentTemplate";
		public const string DisplayName = "Fairburn Agent Template";
		public const string Description = "Description of Agent Template...";
		public const ServiceStartMode StartMode = ServiceStartMode.Automatic;
		public const ServiceAccount Account = ServiceAccount.NetworkService;

#region Constructors

		public AgentTemplateDefinition()
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
