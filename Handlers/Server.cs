using Exiled.Events;
using Exiled.Events.EventArgs;

namespace kirun9.scpsl.plugins.CoinBlocksDoors.Handlers
{
	public class Server
	{
		public static CBDConfig Config { get; internal set; }

		public Server(CBDConfig config)
		{
			Config = config;
		}

		public void OnRoundStarted()
		{
			CBDPlugin.DoorsBlocked = 0;
		}
	}
}
