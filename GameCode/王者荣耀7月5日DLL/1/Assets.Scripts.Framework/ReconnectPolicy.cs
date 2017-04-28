using Apollo;
using System;
using UnityEngine;

namespace Assets.Scripts.Framework
{
	public class ReconnectPolicy
	{
		private BaseConnector connector;

		private tryReconnectDelegate callback;

		private bool sessionStopped;

		private float reconnectTime;

		private uint reconnectCount = 4u;

		private uint tryCount;

		private uint connectTimeout = 10u;

		public bool shouldReconnect;

		public void SetConnector(BaseConnector inConnector, tryReconnectDelegate inEvent, uint tryMax)
		{
			this.StopPolicy();
			this.connector = inConnector;
			this.callback = inEvent;
			this.reconnectCount = tryMax;
		}

		public void StopPolicy()
		{
			this.sessionStopped = false;
			this.shouldReconnect = false;
			this.reconnectTime = this.connectTimeout;
			this.tryCount = 0u;
		}

		public void StartPolicy(ApolloResult result, int timeWait)
		{
			switch (result)
			{
			case 0:
				this.shouldReconnect = false;
				this.sessionStopped = false;
				return;
			case 1:
				IL_18:
				switch (result)
				{
				case 120:
				case 122:
					goto IL_81;
				case 121:
					IL_2D:
					if (result != 103 && result != 127)
					{
						this.shouldReconnect = true;
						this.sessionStopped = true;
						this.reconnectTime = (float)((this.tryCount != 0u) ? timeWait : 0);
						return;
					}
					goto IL_81;
				}
				goto IL_2D;
			case 2:
				this.shouldReconnect = true;
				this.sessionStopped = false;
				this.reconnectTime = (float)((this.tryCount != 0u) ? timeWait : 0);
				return;
			case 3:
				goto IL_81;
			}
			goto IL_18;
			IL_81:
			this.shouldReconnect = true;
			this.sessionStopped = true;
			this.reconnectTime = (float)((this.tryCount != 0u) ? timeWait : 0);
		}

		public void UpdatePolicy(bool bForce)
		{
			if (this.connector != null && !this.connector.connected)
			{
				if (bForce)
				{
					this.reconnectTime = this.connectTimeout;
					this.tryCount = this.reconnectCount;
					if (this.sessionStopped)
					{
						this.connector.RestartConnector();
					}
					else
					{
						this.connector.RestartConnector();
					}
				}
				else
				{
					this.reconnectTime -= Time.unscaledDeltaTime;
					if (this.reconnectTime < 0f)
					{
						this.tryCount += 1u;
						this.reconnectTime = this.connectTimeout;
						uint num = this.tryCount;
						if (this.callback != null)
						{
							num = this.callback(num, this.reconnectCount);
						}
						if (num > this.reconnectCount)
						{
							return;
						}
						this.tryCount = num;
						if (this.sessionStopped)
						{
							this.connector.RestartConnector();
						}
						else
						{
							this.connector.RestartConnector();
						}
					}
				}
			}
		}
	}
}
