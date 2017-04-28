using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using CSProtocol;
using System;

namespace Assets.Scripts.GameLogic
{
	public class PlayerKDA
	{
		public uint PlayerId;

		public bool IsHost;

		public COM_PLAYERCAMP PlayerCamp;

		public string PlayerName;

		public bool IsComputer;

		public int WorldId;

		public ulong PlayerUid;

		public int PlayerLv;

		public uint PlayerVipLv;

		public string PlayerOpenId;

		public int CampPos;

		public uint m_firstMoveTime;

		public uint[] m_reviveMoveTime = new uint[20];

		public int m_nReviveCount;

		public uint m_lastReviveTime;

		private bool m_bRevive;

		public uint m_Camp1TowerFirstAttackTime;

		public uint m_Camp2TowerFirstAttackTime;

		private ListView<HeroKDA> m_HeroKDA = new ListView<HeroKDA>();

		public ListView<CHostHeroDamage> m_hostHeroDamage = new ListView<CHostHeroDamage>();

		private bool m_bHangup;

		private bool m_bRunaway;

		private bool m_bDisconnect;

		public bool bRunaway
		{
			get
			{
				return this.m_bRunaway;
			}
		}

		public bool bDisconnect
		{
			get
			{
				return this.m_bDisconnect;
			}
		}

		public bool bHangup
		{
			get
			{
				return this.m_bHangup;
			}
		}

		public int numKill
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().numKill;
				}
				return num;
			}
		}

		public int numAssist
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().numAssist;
				}
				return num;
			}
		}

		public int numDead
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().numDead;
				}
				return num;
			}
		}

		public int TotalCoin
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().TotalCoin;
				}
				return num;
			}
		}

		public int TotalHurt
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().hurtToEnemy;
				}
				return num;
			}
		}

		public int TotalBeHurt
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().hurtTakenByEnemy;
				}
				return num;
			}
		}

		public int TripleKillNum
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().TripleKillNum;
				}
				return num;
			}
		}

		public int QuataryKillNum
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().QuataryKillNum;
				}
				return num;
			}
		}

		public int PentaKillNum
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().PentaKillNum;
				}
				return num;
			}
		}

		public int LegendaryNum
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().LegendaryNum;
				}
				return num;
			}
		}

		public float KDAValue
		{
			get
			{
				float num = 0f;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().KDAValue;
				}
				return num;
			}
		}

		public float MvpValue
		{
			get
			{
				float num = GameDataMgr.GetGlobeValue(250) / 10000f;
				float num2 = GameDataMgr.GetGlobeValue(251) / 10000f;
				float num3 = GameDataMgr.GetGlobeValue(252) / 10000f;
				float num4 = GameDataMgr.GetGlobeValue(253) / 10000f;
				float num5 = GameDataMgr.GetGlobeValue(254) / 10000f;
				float num6 = GameDataMgr.GetGlobeValue(255) / 10000f;
				CPlayerKDAStat playerKDAStat = Singleton<BattleStatistic>.get_instance().m_playerKDAStat;
				float teamKDA = playerKDAStat.GetTeamKDA(this.PlayerCamp);
				int teamKillNum = playerKDAStat.GetTeamKillNum(this.PlayerCamp);
				int teamDeadNum = playerKDAStat.GetTeamDeadNum(this.PlayerCamp);
				int teamHurt = playerKDAStat.GetTeamHurt(this.PlayerCamp);
				int teamBeHurt = playerKDAStat.GetTeamBeHurt(this.PlayerCamp);
				int teamCoin = playerKDAStat.GetTeamCoin(this.PlayerCamp);
				float num7 = 0f;
				float num8 = 0f;
				float num9 = 0f;
				float num10 = 0f;
				float num11 = 0f;
				float num12 = 0f;
				float num13 = 0f;
				float num14 = 0f;
				if (teamKDA > 0f)
				{
					num7 = this.KDAValue * 1f / teamKDA;
				}
				if (teamKillNum > 0)
				{
					num8 = (float)(this.numKill + this.numAssist) * 1f / (float)teamKillNum;
				}
				if (teamKillNum > 0)
				{
					num9 = (float)this.numKill * 1f / (float)teamKillNum;
				}
				if (teamDeadNum > 0)
				{
					num10 = (float)this.numDead * 1f / (float)teamDeadNum;
				}
				if (teamHurt > 0)
				{
					num11 = (float)this.TotalHurt * num / (float)teamHurt;
				}
				if (teamBeHurt > 0)
				{
					num12 = (float)this.TotalBeHurt * num / (float)teamBeHurt;
				}
				if (teamCoin > 0)
				{
					num13 = (float)this.TotalCoin * num2 / (float)teamCoin;
				}
				if (this.LegendaryNum > 0)
				{
					num14 += num3;
				}
				if (this.PentaKillNum > 0)
				{
					num14 += num6;
				}
				else if (this.QuataryKillNum > 0)
				{
					num14 += num5;
				}
				else if (this.TripleKillNum > 0)
				{
					num14 += num4;
				}
				return num7 + num8 + num9 - num10 + num11 + num12 + num13 + num14;
			}
		}

		public int numKillMonster
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().numKillMonster;
				}
				return num;
			}
		}

		public int numKillSoldier
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().numKillSoldier;
				}
				return num;
			}
		}

		public int numKillOrgan
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().numKillOrgan;
				}
				return num;
			}
		}

		public int numDestroyBase
		{
			get
			{
				int num = 0;
				ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
				while (enumerator.MoveNext())
				{
					num += enumerator.get_Current().numDestroyBase;
				}
				return num;
			}
		}

		public ListView<HeroKDA>.Enumerator GetEnumerator()
		{
			return this.m_HeroKDA.GetEnumerator();
		}

		public uint GetPlayerCoinAtTimeWithType(int iTimeIndex, KDAStat.GET_COIN_CHANNEL_TYPE type)
		{
			uint num = 0u;
			ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.get_Current() != null)
				{
					uint num2 = 0u;
					if (enumerator.get_Current().m_arrCoinInfos[(int)type] != null)
					{
						while (!enumerator.get_Current().m_arrCoinInfos[(int)type].TryGetValue((uint)iTimeIndex, ref num2))
						{
							iTimeIndex--;
							if (iTimeIndex < 0)
							{
								break;
							}
						}
					}
					num += num2;
				}
			}
			return num;
		}

		private void OnCampTowerFirstAttackTime(COM_PLAYERCAMP comp)
		{
			if (comp == 1)
			{
				if (this.m_Camp1TowerFirstAttackTime == 0u)
				{
					this.m_Camp1TowerFirstAttackTime = (uint)Singleton<FrameSynchr>.get_instance().LogicFrameTick;
				}
			}
			else if (comp == 2 && this.m_Camp2TowerFirstAttackTime == 0u)
			{
				this.m_Camp2TowerFirstAttackTime = (uint)Singleton<FrameSynchr>.get_instance().LogicFrameTick;
			}
		}

		private void OnFirstMoved(Player player)
		{
			if (player.PlayerId == this.PlayerId)
			{
				if (!this.m_bRevive)
				{
					this.m_firstMoveTime = (uint)Singleton<FrameSynchr>.get_instance().LogicFrameTick;
				}
				else if (this.m_nReviveCount <= 20)
				{
					this.m_reviveMoveTime[this.m_nReviveCount - 1] = (uint)Singleton<FrameSynchr>.get_instance().LogicFrameTick - this.m_lastReviveTime;
				}
			}
		}

		private void OnReviveTime(Player player)
		{
			if (player.PlayerId == this.PlayerId)
			{
				this.m_bRevive = true;
				this.m_lastReviveTime = (uint)Singleton<FrameSynchr>.get_instance().LogicFrameTick;
				this.m_nReviveCount++;
				player.m_bMoved = false;
			}
		}

		private void onPlayerRunAway(Player runningMan)
		{
			if (runningMan.PlayerId == this.PlayerId)
			{
				this.m_bRunaway = true;
			}
		}

		private void OnHangupNtf(HANGUP_TYPE hangupType, uint playerId)
		{
			if (playerId == this.PlayerId)
			{
				this.m_bHangup = (hangupType == 1);
			}
		}

		private void OnDisConnect(bool bDisconnect, uint playerId)
		{
			if (playerId == this.PlayerId)
			{
				this.m_bDisconnect = bDisconnect;
			}
		}

		public void initialize(Player player)
		{
			if (player == null)
			{
				return;
			}
			this.clear();
			Singleton<EventRouter>.get_instance().AddEventHandler<Player>(EventID.PlayerRunAway, new Action<Player>(this.onPlayerRunAway));
			Singleton<EventRouter>.get_instance().AddEventHandler<HANGUP_TYPE, uint>(EventID.HangupNtf, new Action<HANGUP_TYPE, uint>(this.OnHangupNtf));
			Singleton<EventRouter>.get_instance().AddEventHandler<bool, uint>(EventID.DisConnectNtf, new Action<bool, uint>(this.OnDisConnect));
			Singleton<EventRouter>.get_instance().AddEventHandler<Player>(EventID.FirstMoved, new Action<Player>(this.OnFirstMoved));
			Singleton<EventRouter>.get_instance().AddEventHandler<Player>(EventID.PlayerReviveTime, new Action<Player>(this.OnReviveTime));
			Singleton<EventRouter>.get_instance().AddEventHandler<COM_PLAYERCAMP>(EventID.CampTowerFirstAttackTime, new Action<COM_PLAYERCAMP>(this.OnCampTowerFirstAttackTime));
			this.PlayerId = player.PlayerId;
			this.PlayerCamp = player.PlayerCamp;
			this.PlayerName = player.Name;
			this.IsComputer = player.Computer;
			this.WorldId = player.LogicWrold;
			this.PlayerUid = player.PlayerUId;
			this.IsHost = (Singleton<GamePlayerCenter>.get_instance().GetHostPlayer() != null && player.PlayerId == Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().PlayerId);
			this.PlayerLv = player.Level;
			this.PlayerVipLv = player.VipLv;
			this.PlayerOpenId = player.OpenId;
			this.CampPos = player.CampPos;
			ReadonlyContext<PoolObjHandle<ActorRoot>>.Enumerator enumerator = player.GetAllHeroes().GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.get_Current())
				{
					HeroKDA heroKDA = new HeroKDA();
					heroKDA.Initialize(enumerator.get_Current(), this.CampPos);
					this.m_HeroKDA.Add(heroKDA);
					if (this.IsHost && this.m_hostHeroDamage != null)
					{
						CHostHeroDamage cHostHeroDamage = new CHostHeroDamage();
						cHostHeroDamage.Init(enumerator.get_Current());
						this.m_hostHeroDamage.Add(cHostHeroDamage);
					}
				}
			}
		}

		public void clear()
		{
			ListView<HeroKDA>.Enumerator enumerator = this.m_HeroKDA.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.get_Current().unInit();
			}
			this.m_HeroKDA.Clear();
			if (this.m_hostHeroDamage != null)
			{
				ListView<CHostHeroDamage>.Enumerator enumerator2 = this.m_hostHeroDamage.GetEnumerator();
				while (enumerator2.MoveNext())
				{
					if (enumerator2.get_Current() != null)
					{
						enumerator2.get_Current().UnInit();
					}
				}
				this.m_hostHeroDamage.Clear();
			}
			Singleton<EventRouter>.get_instance().RemoveEventHandler<Player>(EventID.PlayerRunAway, new Action<Player>(this.onPlayerRunAway));
			Singleton<EventRouter>.get_instance().RemoveEventHandler<HANGUP_TYPE, uint>(EventID.HangupNtf, new Action<HANGUP_TYPE, uint>(this.OnHangupNtf));
			Singleton<EventRouter>.get_instance().RemoveEventHandler<bool, uint>(EventID.DisConnectNtf, new Action<bool, uint>(this.OnDisConnect));
			Singleton<EventRouter>.get_instance().RemoveEventHandler<Player>(EventID.FirstMoved, new Action<Player>(this.OnFirstMoved));
			Singleton<EventRouter>.get_instance().RemoveEventHandler<Player>(EventID.PlayerReviveTime, new Action<Player>(this.OnReviveTime));
			Singleton<EventRouter>.get_instance().RemoveEventHandler<COM_PLAYERCAMP>(EventID.CampTowerFirstAttackTime, new Action<COM_PLAYERCAMP>(this.OnCampTowerFirstAttackTime));
		}
	}
}
