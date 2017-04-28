using System;

public class CheatWindowExternalIntializer : Singleton<CheatWindowExternalIntializer>
{
	public override void Init()
	{
		base.Init();
		Singleton<CheatCommandRegister>.get_instance().Register(typeof(CheatCommandNetworking).get_Assembly());
		MonoSingleton<ConsoleWindow>.GetInstance();
	}
}
