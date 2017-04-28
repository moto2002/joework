using Assets.Scripts.GameLogic;
using Assets.Scripts.GameSystem;
using CSProtocol;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.Framework
{
	[MessageHandlerClass]
	public class FrameSynchr : Singleton<FrameSynchr>, IGameModule
	{
		public const byte MIN_FRAME_SPEED = 1;

		public const byte MAX_FRAME_SPEED = 8;

		private const int FrameDelay_Limit = 200;

		private const float JitterCoverage = 0.85f;

		private const int StatDelayCnt = 30;

		private bool _bActive;

		private bool bRunning;

		public uint FrameDelta = 66u;

		private uint EndBlockWaitNum;

		public uint PreActFrames = 5u;

		public int nDriftFactor = 4;

		public uint SvrFrameLater;

		public uint CacheSetLater;

		public uint SvrFrameDelta = 66u;

		private uint SvrFrameIndex;

		private uint KeyFrameRate = 1u;

		private uint ServerSeed = 12345u;

		private uint backstepFrameCounter;

		private uint uCommandId;

		private Queue<IFrameCommand> commandQueue = new Queue<IFrameCommand>();

		private byte frameSpeed = 1;

		public int GameSvrPing;

		private int _CurPkgDelay;

		private int AvgFrameDelay;

		private int JitterDelay;

		private int JitterDamper;

		private int[] StatDelaySet = new int[30];

		private int StatDelayIdx;

		public bool bShowJitterChart;

		public int tryCount;

		public int PauseCancelCount;

		public uint m_MaxPing;

		public uint m_MinPing = 4294967295u;

		public uint m_AvePing;

		public uint m_PingIdx;

		public float m_PingTimeBegin;

		public float m_NetAccPingTimeBegin;

		public uint m_LastPing;

		public int m_Abnormal_PingCount;

		public long m_ping300Count;

		public long m_ping150to300;

		public long m_ping150;

		public long m_SendHeartSeq;

		public long m_LastReceiveHeartSeq;

		public long m_pingLost;

		public List<uint> m_pingRecords = new List<uint>();

		public List<uint> m_pingWobble = new List<uint>();

		public uint m_realpingStartTime;

		public Ping m_ping;

		public bool m_NetAccSetIPAndPortInvoked;

		public ulong m_receiveMoveCmdtotalCount;

		public ulong m_ExecMoveCmdTotalCount;

		public uint[] m_MoveCMDSendTime = new uint[256];

		public uint[] m_MoveCMDReceiveTime = new uint[256];

		public int m_receiveMoveCmdAverage;

		public int m_execMoveCmdAverage;

		public uint m_receiveMoveCmdMax;

		public uint m_execMoveCmdMax;

		public int m_PingAverage;

		public int m_PingVariance;

		public bool isCmdExecuting
		{
			get;
			private set;
		}

		public uint CurFrameNum
		{
			get;
			private set;
		}

		public uint EndFrameNum
		{
			get;
			private set;
		}

		public ulong LogicFrameTick
		{
			get;
			private set;
		}

		public float startFrameTime
		{
			get;
			private set;
		}

		public byte FrameSpeed
		{
			get
			{
				return this.frameSpeed;
			}
			set
			{
				this.frameSpeed = (byte)Mathf.Clamp((int)value, 1, 8);
				if (this._bActive)
				{
					this.ResetStartTime();
				}
			}
		}

		public int RealSvrPing
		{
			get
			{
				return this.GameSvrPing + this.AvgFrameDelay;
			}
		}

		public long nMultiFrameDelta
		{
			get;
			private set;
		}

		public bool bActive
		{
			get
			{
				return this._bActive;
			}
			set
			{
				if (this._bActive != value)
				{
					Singleton<GameLogic>.get_instance().UpdateTails();
					this._bActive = value;
				}
			}
		}

		public bool isRunning
		{
			get
			{
				return this.bRunning;
			}
		}

		public uint NewCommandId
		{
			get
			{
				this.uCommandId += 1u;
				return this.uCommandId;
			}
			set
			{
				this.uCommandId = value;
			}
		}

		public uint svrLogicFrameNum
		{
			get
			{
				return this.SvrFrameIndex;
			}
		}

		public int nJitterDelay
		{
			get;
			private set;
		}

		public int CalculateJitterDelay(long nDelayMs)
		{
			this._CurPkgDelay = ((nDelayMs <= 0L) ? 0 : ((int)nDelayMs));
			this.AvgFrameDelay = (29 * this.AvgFrameDelay + this._CurPkgDelay) / 30;
			return this.AvgFrameDelay;
		}

		public override void Init()
		{
			FrameCommandFactory.PrepareRegisterCommand();
			Assembly assembly = typeof(FrameSynchr).get_Assembly();
			Type[] types = assembly.GetTypes();
			int num = 0;
			while (types != null && num < types.Length)
			{
				Type type = types[num];
				object[] customAttributes = type.GetCustomAttributes(typeof(FrameCommandClassAttribute), true);
				for (int i = 0; i < customAttributes.Length; i++)
				{
					FrameCommandClassAttribute frameCommandClassAttribute = customAttributes[i] as FrameCommandClassAttribute;
					if (frameCommandClassAttribute != null)
					{
						CreatorDelegate creator = this.GetCreator(type);
						if (creator != null)
						{
							FrameCommandFactory.RegisterCommandCreator(frameCommandClassAttribute.get_ID(), type, creator);
						}
					}
				}
				object[] customAttributes2 = type.GetCustomAttributes(typeof(FrameCSSYNCCommandClassAttribute), true);
				for (int j = 0; j < customAttributes2.Length; j++)
				{
					FrameCSSYNCCommandClassAttribute frameCSSYNCCommandClassAttribute = customAttributes2[j] as FrameCSSYNCCommandClassAttribute;
					if (frameCSSYNCCommandClassAttribute != null)
					{
						CreatorCSSyncDelegate cSSyncCreator = this.GetCSSyncCreator(type);
						if (cSSyncCreator != null)
						{
							FrameCommandFactory.RegisterCSSyncCommandCreator(frameCSSYNCCommandClassAttribute.get_ID(), type, cSSyncCreator);
						}
					}
				}
				object[] customAttributes3 = type.GetCustomAttributes(typeof(FrameSCSYNCCommandClassAttribute), true);
				for (int k = 0; k < customAttributes3.Length; k++)
				{
					FrameSCSYNCCommandClassAttribute frameSCSYNCCommandClassAttribute = customAttributes3[k] as FrameSCSYNCCommandClassAttribute;
					if (frameSCSYNCCommandClassAttribute != null)
					{
						FrameCommandFactory.RegisterSCSyncCommandCreator(frameSCSYNCCommandClassAttribute.get_ID(), type, null);
					}
				}
				num++;
			}
			this.ResetSynchr();
		}

		private CreatorDelegate GetCreator(Type InType)
		{
			MethodInfo[] methods = InType.GetMethods();
			int num = 0;
			while (methods != null && num < methods.Length)
			{
				MethodInfo methodInfo = methods[num];
				if (methodInfo.get_IsStatic())
				{
					object[] customAttributes = methodInfo.GetCustomAttributes(typeof(FrameCommandCreatorAttribute), true);
					for (int i = 0; i < customAttributes.Length; i++)
					{
						FrameCommandCreatorAttribute frameCommandCreatorAttribute = customAttributes[i] as FrameCommandCreatorAttribute;
						if (frameCommandCreatorAttribute != null)
						{
							return (CreatorDelegate)Delegate.CreateDelegate(typeof(CreatorDelegate), methodInfo);
						}
					}
				}
				num++;
			}
			return null;
		}

		private CreatorCSSyncDelegate GetCSSyncCreator(Type InType)
		{
			MethodInfo[] methods = InType.GetMethods();
			int num = 0;
			while (methods != null && num < methods.Length)
			{
				MethodInfo methodInfo = methods[num];
				if (methodInfo.get_IsStatic())
				{
					object[] customAttributes = methodInfo.GetCustomAttributes(typeof(FrameCommandCreatorAttribute), true);
					for (int i = 0; i < customAttributes.Length; i++)
					{
						FrameCommandCreatorAttribute frameCommandCreatorAttribute = customAttributes[i] as FrameCommandCreatorAttribute;
						if (frameCommandCreatorAttribute != null)
						{
							return (CreatorCSSyncDelegate)Delegate.CreateDelegate(typeof(CreatorCSSyncDelegate), methodInfo);
						}
					}
				}
				num++;
			}
			return null;
		}

		public bool SetKeyFrameIndex(uint svrNum)
		{
			this.SvrFrameIndex = svrNum;
			this.EndFrameNum = (svrNum + this.SvrFrameLater) * this.KeyFrameRate;
			this.CalcBackstepTimeSinceStart(svrNum);
			return true;
		}

		public void ResetSynchr()
		{
			this.bActive = false;
			this.bRunning = true;
			this.FrameDelta = 33u;
			this.CurFrameNum = 0u;
			this.EndFrameNum = 0u;
			this.LogicFrameTick = 0uL;
			this.EndBlockWaitNum = 0u;
			this.PreActFrames = 5u;
			this.SvrFrameDelta = this.FrameDelta;
			this.SvrFrameLater = 0u;
			this.SvrFrameIndex = 0u;
			this.CacheSetLater = 0u;
			this.KeyFrameRate = 1u;
			this.frameSpeed = 1;
			this.GameSvrPing = 0;
			this._CurPkgDelay = 0;
			this.AvgFrameDelay = 0;
			this.JitterDelay = 0;
			this.JitterDamper = 0;
			this.StatDelayIdx = 0;
			Array.Clear(this.StatDelaySet, 0, this.StatDelaySet.Length);
			this.NewCommandId = 0u;
			this.startFrameTime = Time.time;
			this.backstepFrameCounter = 0u;
			this.commandQueue.Clear();
			this.m_MaxPing = 0u;
			this.m_MinPing = 4294967295u;
			this.m_AvePing = 0u;
			this.m_PingIdx = 0u;
			this.m_pingRecords.Clear();
			this.m_pingWobble.Clear();
			this.ClearMoveCMDTime();
			this.m_realpingStartTime = 0u;
			this.m_ping = null;
			this.m_NetAccSetIPAndPortInvoked = false;
			this.m_PingTimeBegin = 0f;
			this.m_NetAccPingTimeBegin = 0f;
			this.m_LastPing = 0u;
			this.m_Abnormal_PingCount = 0;
			this.m_ping300Count = 0L;
			this.m_ping150to300 = 0L;
			this.m_pingLost = 0L;
			this.m_SendHeartSeq = 0L;
			this.m_LastReceiveHeartSeq = 0L;
			this.m_ping150 = 0L;
			this.PauseCancelCount = 0;
		}

		public void SetSynchrConfig(uint svrDelta, uint frameLater, uint preActNum, uint randSeed)
		{
			this.SvrFrameDelta = svrDelta;
			this.SvrFrameLater = 0u;
			this.CacheSetLater = this.SvrFrameLater;
			this.KeyFrameRate = 1u;
			this.PreActFrames = preActNum;
			this.ServerSeed = randSeed;
		}

		public void SwitchSynchrLocal()
		{
			if (this.bActive)
			{
				this.bActive = false;
				this.ResetStartTime();
			}
		}

		public void ResetStartTime()
		{
			if (this.bActive)
			{
				this.startFrameTime = (Time.realtimeSinceStartup * (float)this.frameSpeed - this.LogicFrameTick * 0.001f) / (float)this.frameSpeed;
			}
			else
			{
				this.startFrameTime = Time.time - this.LogicFrameTick * 0.001f + Time.deltaTime;
			}
		}

		public void SetSynchrRunning(bool bRun)
		{
			this.bRunning = bRun;
		}

		public void ResetSynchrSeed()
		{
			FrameRandom.ResetSeed(this.ServerSeed);
		}

		public void CacheFrameLater(uint frameLater)
		{
			this.CacheSetLater = frameLater;
		}

		private void UpdateFrameLater()
		{
			if (this.CacheSetLater != this.SvrFrameLater)
			{
				this.SvrFrameLater = 0u;
				this.EndFrameNum = (this.SvrFrameIndex + this.SvrFrameLater) * this.KeyFrameRate;
			}
		}

		public void StartSynchr()
		{
			this.bActive = true;
			this.bRunning = false;
			this.SvrFrameIndex = 0u;
			this.FrameDelta = this.SvrFrameDelta / this.KeyFrameRate;
			this.CurFrameNum = 0u;
			this.EndFrameNum = 0u;
			this.LogicFrameTick = 0uL;
			this.EndBlockWaitNum = 0u;
			this.frameSpeed = 1;
			this.GameSvrPing = 0;
			this._CurPkgDelay = 0;
			this.AvgFrameDelay = 0;
			this.JitterDelay = 0;
			this.JitterDamper = 0;
			this.StatDelayIdx = 0;
			Array.Clear(this.StatDelaySet, 0, this.StatDelaySet.Length);
			this.commandQueue.Clear();
			this.NewCommandId = 0u;
			this.startFrameTime = Time.realtimeSinceStartup;
			this.backstepFrameCounter = 0u;
		}

		public void CalcBackstepTimeSinceStart(uint inSvrNum)
		{
			if (Singleton<WatchController>.get_instance().IsRelayCast || this.backstepFrameCounter == inSvrNum)
			{
				return;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			ulong num = (ulong)inSvrNum * (ulong)this.SvrFrameDelta;
			float num2 = realtimeSinceStartup - num * 0.001f;
			float num3 = num2 - this.startFrameTime;
			if (num3 < 0f)
			{
				this.startFrameTime = num2;
			}
			this.backstepFrameCounter = inSvrNum;
		}

		private void UpdateMultiFrame()
		{
			Singleton<GameLogic>.GetInstance().UpdateTails();
			int value = (int)((this.EndFrameNum - this.CurFrameNum) / (uint)this.nDriftFactor);
			this.tryCount = Mathf.Clamp(value, 1, 100);
			int i = this.tryCount;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			long num = (long)((double)(realtimeSinceStartup - this.startFrameTime) * 1000.0);
			long nDelayMs = num - (long)((ulong)((this.SvrFrameIndex + 1u) * this.SvrFrameDelta));
			this.nJitterDelay = this.CalculateJitterDelay(nDelayMs);
			num *= (long)this.frameSpeed;
			while (i > 0)
			{
				long num2 = (long)((ulong)this.CurFrameNum * (ulong)this.FrameDelta);
				this.nMultiFrameDelta = num - num2;
				this.nMultiFrameDelta -= (long)this.nJitterDelay;
				if (this.nMultiFrameDelta >= (long)((ulong)this.FrameDelta))
				{
					if (this.CurFrameNum >= this.EndFrameNum)
					{
						this.EndBlockWaitNum += 1u;
						i = 0;
					}
					else
					{
						this.EndBlockWaitNum = 0u;
						this.CurFrameNum += 1u;
						this.LogicFrameTick += (ulong)this.FrameDelta;
						this.isCmdExecuting = true;
						while (this.commandQueue.get_Count() > 0)
						{
							IFrameCommand frameCommand = this.commandQueue.Peek();
							uint num3 = (frameCommand.frameNum + this.SvrFrameLater) * this.KeyFrameRate;
							if (num3 > this.CurFrameNum)
							{
								break;
							}
							frameCommand.frameNum = num3;
							frameCommand = this.commandQueue.Dequeue();
							frameCommand.ExecCommand();
						}
						this.isCmdExecuting = false;
						Singleton<GameLogic>.GetInstance().UpdateLogic((int)this.FrameDelta, i == 1 && this.nMultiFrameDelta < (long)(2uL * (ulong)this.FrameDelta));
						i--;
					}
				}
				else
				{
					i = 0;
				}
			}
		}

		private void UpdateSingleFrame()
		{
			Singleton<GameLogic>.GetInstance().UpdateTails();
			long num = (long)(Time.deltaTime * 1000f);
			num = (long)Mathf.Clamp((int)num, 0, 100);
			this.CurFrameNum += 1u;
			this.LogicFrameTick += (ulong)num;
			this.isCmdExecuting = true;
			while (this.commandQueue.get_Count() > 0)
			{
				IFrameCommand frameCommand = this.commandQueue.Dequeue();
				frameCommand.ExecCommand();
			}
			this.isCmdExecuting = false;
			if (num > 0L)
			{
				Singleton<GameLogic>.GetInstance().UpdateLogic((int)num, false);
			}
		}

		public void UpdateFrame()
		{
			if (this.bActive)
			{
				if (this.bRunning)
				{
					this.UpdateMultiFrame();
					FrameSynchr.UpdatePing();
				}
			}
			else if (this.bRunning)
			{
				this.UpdateSingleFrame();
			}
		}

		public void PushFrameCommand(IFrameCommand command)
		{
			command.cmdId = this.NewCommandId;
			if (this.bActive)
			{
				command.OnReceive();
			}
			else
			{
				command.frameNum = this.CurFrameNum;
			}
			this.commandQueue.Enqueue(command);
		}

		private void ReqKeyFrameLaterModify(uint nLater)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1096u);
			cSPkg.stPkgData.get_stKFrapsLaterChgReq().bKFrapsLater = (byte)nLater;
			Singleton<NetworkModule>.get_instance().SendGameMsg(ref cSPkg, 0u);
		}

		private void ClearMoveCMDTime()
		{
			this.m_receiveMoveCmdAverage = 0;
			this.m_execMoveCmdAverage = 0;
			this.m_receiveMoveCmdMax = 0u;
			this.m_execMoveCmdMax = 0u;
			this.m_receiveMoveCmdtotalCount = 0uL;
			this.m_ExecMoveCmdTotalCount = 0uL;
			for (int i = 0; i < this.m_MoveCMDSendTime.Length; i++)
			{
				this.m_MoveCMDSendTime[i] = 0u;
			}
			for (int j = 0; j < this.m_MoveCMDReceiveTime.Length; j++)
			{
				this.m_MoveCMDReceiveTime[j] = 0u;
			}
		}

		[MessageHandler(1260)]
		public static void onRelaySvrPingMsg(CSPkg msg)
		{
			long num = (long)((ulong)msg.stPkgData.get_stRelaySvrPing().dwSeqNo);
			if (Singleton<FrameSynchr>.get_instance().m_LastReceiveHeartSeq <= 0L)
			{
				Singleton<FrameSynchr>.get_instance().m_LastReceiveHeartSeq = num;
			}
			else
			{
				long num2 = num - Singleton<FrameSynchr>.get_instance().m_LastReceiveHeartSeq;
				if (num2 >= 2L)
				{
					Singleton<FrameSynchr>.get_instance().m_pingLost += num2;
				}
				Singleton<FrameSynchr>.get_instance().m_LastReceiveHeartSeq = num;
			}
			if (Singleton<FrameSynchr>.get_instance().bActive && Singleton<FrameSynchr>.get_instance().bRunning)
			{
				uint num3 = (uint)(Time.realtimeSinceStartup * 1000f) - msg.stPkgData.get_stRelaySvrPing().dwTime;
				if (Singleton<FrameSynchr>.GetInstance().m_SendHeartSeq == num + 1L)
				{
					Singleton<FrameSynchr>.get_instance().m_pingRecords.Add(num3);
					Singleton<FrameSynchr>.get_instance().m_PingIdx += 1u;
					Singleton<FrameSynchr>.get_instance().m_AvePing = (Singleton<FrameSynchr>.get_instance().m_AvePing * (Singleton<FrameSynchr>.get_instance().m_PingIdx - 1u) + num3) / Singleton<FrameSynchr>.get_instance().m_PingIdx;
					Singleton<FrameSynchr>.get_instance().m_pingWobble.Add((uint)Mathf.Abs(num3 - Singleton<FrameSynchr>.get_instance().m_AvePing));
					if (num3 > Singleton<FrameSynchr>.get_instance().m_MaxPing)
					{
						Singleton<FrameSynchr>.get_instance().m_MaxPing = num3;
					}
					if (num3 < Singleton<FrameSynchr>.get_instance().m_MinPing)
					{
						Singleton<FrameSynchr>.get_instance().m_MinPing = num3;
					}
					if (num3 > 300u)
					{
						Singleton<FrameSynchr>.get_instance().m_ping300Count += 1L;
					}
					if (num3 >= 150u && num3 <= 300u)
					{
						Singleton<FrameSynchr>.get_instance().m_ping150to300 += 1L;
					}
					else if (num3 < 150u)
					{
						Singleton<FrameSynchr>.get_instance().m_ping150 += 1L;
					}
				}
				Singleton<FrameSynchr>.get_instance().GameSvrPing = (int)((ulong)num3 + (ulong)((long)Singleton<FrameSynchr>.get_instance().GameSvrPing)) / 2;
				FrameSynchr.SetNetAccIPAndPort();
				if (NetworkAccelerator.enabled && !NetworkAccelerator.started && Time.unscaledTime - Singleton<FrameSynchr>.get_instance().m_NetAccPingTimeBegin > 1f)
				{
					Singleton<FrameSynchr>.get_instance().m_NetAccPingTimeBegin = Time.unscaledTime;
					NetworkAccelerator.OnNetDelay((int)num3);
				}
				if (Time.unscaledTime - Singleton<FrameSynchr>.get_instance().m_PingTimeBegin > 5f)
				{
					Singleton<FrameSynchr>.get_instance().m_PingTimeBegin = Time.unscaledTime;
					if (Singleton<FrameSynchr>.get_instance().m_LastPing == 0u)
					{
						Singleton<FrameSynchr>.get_instance().m_LastPing = num3;
					}
					if (Math.Abs((long)((ulong)(Singleton<FrameSynchr>.get_instance().m_LastPing - num3))) > 100L)
					{
						Singleton<FrameSynchr>.get_instance().m_Abnormal_PingCount++;
						Singleton<FrameSynchr>.get_instance().m_LastPing = num3;
					}
				}
				if (Singleton<FrameSynchr>.get_instance().m_pingRecords.get_Count() > 100 && Singleton<FrameSynchr>.get_instance().GameSvrPing > 300)
				{
					FrameSynchr.RealPing();
				}
			}
			msg.Release();
		}

		private static void SetNetAccIPAndPort()
		{
			if (Singleton<FrameSynchr>.get_instance().m_NetAccSetIPAndPortInvoked || ApolloConfig.echoPort == 0 || string.IsNullOrEmpty(ApolloConfig.loginOnlyIp))
			{
				return;
			}
			NetworkAccelerator.setRecommendationGameIP(ApolloConfig.loginOnlyIp, (int)ApolloConfig.echoPort);
			Singleton<FrameSynchr>.get_instance().m_NetAccSetIPAndPortInvoked = true;
		}

		private static void RealPing()
		{
			if (Singleton<FrameSynchr>.get_instance().m_realpingStartTime > 0u || Singleton<FrameSynchr>.get_instance().m_ping != null)
			{
				return;
			}
			if (string.IsNullOrEmpty(ApolloConfig.loginOnlyIp))
			{
				return;
			}
			Singleton<FrameSynchr>.get_instance().m_realpingStartTime = (uint)(Time.realtimeSinceStartup * 1000f);
			Singleton<FrameSynchr>.get_instance().m_ping = new Ping(ApolloConfig.loginOnlyIp);
		}

		private static void UpdatePing()
		{
			if (Singleton<FrameSynchr>.get_instance().m_ping == null)
			{
				return;
			}
			if (Singleton<FrameSynchr>.get_instance().m_ping.isDone)
			{
				uint num = (uint)(Time.realtimeSinceStartup * 1000f) - Singleton<FrameSynchr>.get_instance().m_realpingStartTime;
				Singleton<FrameSynchr>.get_instance().m_ping = null;
			}
		}

		public void ReportPingToBeacon()
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			list.Add(new KeyValuePair<string, string>("WorldID", MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString()));
			list.Add(new KeyValuePair<string, string>("platform", Singleton<ApolloHelper>.GetInstance().CurPlatform.ToString()));
			if (NetworkAccelerator.started)
			{
				if (NetworkAccelerator.isAccerating())
				{
					list.Add(new KeyValuePair<string, string>("AccState", "Acc"));
				}
				else
				{
					list.Add(new KeyValuePair<string, string>("AccState", "Direct"));
				}
			}
			else
			{
				list.Add(new KeyValuePair<string, string>("AccState", "Off"));
			}
			this.m_PingAverage = 0;
			this.m_PingVariance = 0;
			if (this.m_pingRecords != null && this.m_pingRecords.get_Count() > 100)
			{
				double num = 0.0;
				for (int i = 0; i < this.m_pingRecords.get_Count(); i++)
				{
					num += this.m_pingRecords.get_Item(i);
				}
				double num2 = num / (double)this.m_pingRecords.get_Count();
				int num3 = Mathf.FloorToInt((float)num2 / 100f) * 100;
				this.m_PingAverage = num3;
				Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("PingAverage_" + num3.ToString(), list, true);
				num = 0.0;
				for (int j = 0; j < this.m_pingRecords.get_Count(); j++)
				{
					num += Math.Pow(this.m_pingRecords.get_Item(j) - num2, 2.0);
				}
				num2 = num / (double)this.m_pingRecords.get_Count();
				num3 = Mathf.FloorToInt((float)num2 / 1000f) * 1000;
				this.m_PingVariance = num3;
				if (num3 <= 10000)
				{
					Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("PingVariance_" + num3.ToString(), list, true);
				}
				else
				{
					Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("PingVariance_>10000", list, true);
				}
			}
			int value = 0;
			if (this.m_pingWobble != null && this.m_pingWobble.get_Count() > 100)
			{
				int num4 = 0;
				for (int k = 0; k < this.m_pingWobble.get_Count(); k++)
				{
					if (Mathf.Abs(this.m_pingWobble.get_Item(k)) <= 100f)
					{
						num4++;
					}
				}
				value = Mathf.FloorToInt((float)num4 / (float)this.m_pingWobble.get_Count() * 100f / 10f) * 10;
				Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("PingWobble<100_" + value.ToString() + "%", list, true);
			}
			if (NetworkAccelerator.started && NetworkAccelerator.isAccerating())
			{
				PlayerPrefs.SetInt("ACC_PING_AVERAGE", this.m_PingAverage);
				PlayerPrefs.SetInt("ACC_PING_VARIANCE", this.m_PingVariance);
				PlayerPrefs.SetInt("ACC_PING_WOBBLE", value);
			}
			else
			{
				PlayerPrefs.SetInt("NOACC_PING_AVERAGE", this.m_PingAverage);
				PlayerPrefs.SetInt("NOACC_PING_VARIANCE", this.m_PingVariance);
				PlayerPrefs.SetInt("NOACC_PING_WOBBLE", value);
			}
			if (PlayerPrefs.HasKey("ACC_PING_AVERAGE") && PlayerPrefs.HasKey("NOACC_PING_AVERAGE"))
			{
				List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
				list2.Add(new KeyValuePair<string, string>("WorldID", MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString()));
				list2.Add(new KeyValuePair<string, string>("platform", Singleton<ApolloHelper>.GetInstance().CurPlatform.ToString()));
				list2.Add(new KeyValuePair<string, string>("pingAccRate", ((float)PlayerPrefs.GetInt("NOACC_PING_AVERAGE") / (float)PlayerPrefs.GetInt("ACC_PING_AVERAGE")).ToString()));
				list2.Add(new KeyValuePair<string, string>("pingAccDiff", (PlayerPrefs.GetInt("NOACC_PING_AVERAGE") - PlayerPrefs.GetInt("ACC_PING_AVERAGE")).ToString()));
				list2.Add(new KeyValuePair<string, string>("pingVarienceRate", ((float)PlayerPrefs.GetInt("NOACC_PING_VARIANCE") / (float)PlayerPrefs.GetInt("ACC_PING_VARIANCE")).ToString()));
				list2.Add(new KeyValuePair<string, string>("pingWobbleRate", ((float)PlayerPrefs.GetInt("ACC_PING_WOBBLE") / (float)PlayerPrefs.GetInt("NOACC_PING_WOBBLE")).ToString()));
				list2.Add(new KeyValuePair<string, string>("g_version ", "Service" + CVersion.GetResourceVersion()));
				Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("NetAccStatistics", list2, true);
			}
		}

		public void PingVariance()
		{
			this.m_PingAverage = 0;
			this.m_PingVariance = 0;
			if (this.m_pingRecords != null && this.m_pingRecords.get_Count() > 0)
			{
				double num = 0.0;
				for (int i = 0; i < this.m_pingRecords.get_Count(); i++)
				{
					num += this.m_pingRecords.get_Item(i);
				}
				double num2 = num / (double)this.m_pingRecords.get_Count();
				this.m_PingAverage = (int)num2;
				num2 = 0.0;
				num = 0.0;
				for (int j = 0; j < this.m_pingRecords.get_Count(); j++)
				{
					num += Math.Pow(this.m_pingRecords.get_Item(j) - num2, 2.0);
				}
				num2 = num / (double)this.m_pingRecords.get_Count();
				this.m_PingVariance = (int)num2;
			}
		}

		[MessageHandler(5333)]
		public static void OnServerPingClientMsg(CSPkg msg)
		{
			if (Singleton<BattleLogic>.get_instance().isRuning)
			{
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5334u);
				Singleton<NetworkModule>.GetInstance().SendGameMsg(ref cSPkg, 0u);
			}
		}
	}
}
