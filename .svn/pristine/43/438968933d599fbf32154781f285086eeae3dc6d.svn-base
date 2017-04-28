using CSProtocol;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameLogic
{
	public class CCampKDAStat
	{
		private uint[] m_campTotalDamage = new uint[2];

		private uint[] m_campTotalTakenDamage = new uint[2];

		private uint[] m_campTotalToHeroDamage = new uint[2];

		public uint camp1TotalDamage
		{
			get
			{
				return this.GetTeamTotalDamage(1);
			}
		}

		public uint camp1TotalTakenDamage
		{
			get
			{
				return this.GetTeamTotalTakenDamage(1);
			}
		}

		public uint camp1TotalToHeroDamage
		{
			get
			{
				return this.GetTeamTotalToHeroDamage(1);
			}
		}

		public uint camp2TotalDamage
		{
			get
			{
				return this.GetTeamTotalDamage(2);
			}
		}

		public uint camp2TotalTakenDamage
		{
			get
			{
				return this.GetTeamTotalTakenDamage(2);
			}
		}

		public uint camp2TotalToHeroDamage
		{
			get
			{
				return this.GetTeamTotalToHeroDamage(1);
			}
		}

		public void Initialize(DictionaryView<uint, PlayerKDA> playerKDAStat)
		{
			for (int i = 1; i <= 2; i++)
			{
				this.m_campTotalDamage[i - 1] = 0u;
				this.m_campTotalTakenDamage[i - 1] = 0u;
				this.m_campTotalToHeroDamage[i - 1] = 0u;
			}
			this.GetTeamKDA(playerKDAStat);
		}

		private void GetTeamInfoByPlayerKda(PlayerKDA kda)
		{
			if (kda == null)
			{
				return;
			}
			if (kda.PlayerCamp != 1 && kda.PlayerCamp != 2)
			{
				return;
			}
			ListView<HeroKDA>.Enumerator enumerator = kda.GetEnumerator();
			while (enumerator.MoveNext())
			{
				HeroKDA current = enumerator.get_Current();
				if (current != null)
				{
					this.m_campTotalDamage[kda.PlayerCamp - 1] += (uint)current.hurtToEnemy;
					this.m_campTotalTakenDamage[kda.PlayerCamp - 1] += (uint)current.hurtTakenByEnemy;
					this.m_campTotalToHeroDamage[kda.PlayerCamp - 1] += (uint)current.hurtToHero;
				}
			}
		}

		private void GetTeamKDA(DictionaryView<uint, PlayerKDA> playerKDAStat)
		{
			if (playerKDAStat == null)
			{
				return;
			}
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = playerKDAStat.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
				PlayerKDA value = current.get_Value();
				this.GetTeamInfoByPlayerKda(value);
			}
		}

		public uint GetTeamTotalDamage(COM_PLAYERCAMP camp)
		{
			if (camp < 1 || camp > 2)
			{
				return 0u;
			}
			return this.m_campTotalDamage[camp - 1];
		}

		public uint GetTeamTotalTakenDamage(COM_PLAYERCAMP camp)
		{
			if (camp < 1 || camp > 2)
			{
				return 0u;
			}
			return this.m_campTotalTakenDamage[camp - 1];
		}

		public uint GetTeamTotalToHeroDamage(COM_PLAYERCAMP camp)
		{
			if (camp < 1 || camp > 2)
			{
				return 0u;
			}
			return this.m_campTotalToHeroDamage[camp - 1];
		}
	}
}
