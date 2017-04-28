using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.DataCenter;
using Assets.Scripts.GameSystem;
using ResData;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameLogic
{
	public class EquipComponent : LogicComponent
	{
		public const int c_equipGridCount = 6;

		public const int m_iEquipActiveSkillShowCountMax = 1;

		public static uint s_equipEffectSequence;

		private stEquipInfo[] m_equipInfos = new stEquipInfo[6];

		private int m_equipCount;

		private Dictionary<ushort, CEquipPassiveSkillInfoGroup> m_equipPassiveSkillInfoMap = new Dictionary<ushort, CEquipPassiveSkillInfoGroup>();

		private Dictionary<ushort, CEquipBuffInfoGroup> m_equipBuffInfoMap = new Dictionary<ushort, CEquipBuffInfoGroup>();

		private CExistEquipInfoSet m_existEquipInfoSet = new CExistEquipInfoSet();

		private ListView<CRecommendEquipInfo> m_recommendEquipInfos = new ListView<CRecommendEquipInfo>(6);

		private Dictionary<ushort, uint> m_equipBoughtHistory = new Dictionary<ushort, uint>(24);

		private List<CEquipPassiveCdInfo> m_equipPassiveCdList = new List<CEquipPassiveCdInfo>();

		private int m_frame;

		public bool m_isInEquipBoughtArea;

		public bool m_hasLeftEquipBoughtArea;

		public int m_iFastBuyEquipCount;

		public int m_iBuyEquipCount;

		private ENUM_EQUIP_ACTIVESKILL_STATUS[] m_enmEquipActiveSkillStatus = new ENUM_EQUIP_ACTIVESKILL_STATUS[6];

		private List<CEquipPassiveCdInfo> m_equipActiveSkillCdList = new List<CEquipPassiveCdInfo>();

		private EQUIPACTIVESKILLSLOTINFO[] m_stEquipActiveSkillInfo = new EQUIPACTIVESKILLSLOTINFO[1];

		public ushort m_horizonEquipId;

		public override void OnUse()
		{
			base.OnUse();
			for (int i = 0; i < 6; i++)
			{
				this.m_equipInfos[i] = default(stEquipInfo);
				this.m_enmEquipActiveSkillStatus[i] = ENUM_EQUIP_ACTIVESKILL_STATUS.ENM_EQUIP_ACTIVESKILL_STATUS_WHITHOUTACTIVESKILL;
			}
			this.m_equipCount = 0;
			this.m_frame = 0;
			this.m_equipPassiveSkillInfoMap.Clear();
			this.m_equipBuffInfoMap.Clear();
			this.m_existEquipInfoSet.Clear();
			this.m_recommendEquipInfos.Clear();
			this.m_equipBoughtHistory.Clear();
			this.m_equipPassiveCdList.Clear();
			this.m_isInEquipBoughtArea = false;
			this.m_hasLeftEquipBoughtArea = false;
			this.m_iFastBuyEquipCount = 0;
			this.m_iBuyEquipCount = 0;
			this.m_equipActiveSkillCdList.Clear();
			for (int j = 0; j < 1; j++)
			{
				this.m_stEquipActiveSkillInfo[j] = default(EQUIPACTIVESKILLSLOTINFO);
			}
			this.m_horizonEquipId = 0;
		}

		public override void Init()
		{
			base.Init();
			this.ClearEquips();
			this.InitializeRecommendEquips();
			this.InitHorizonEquipID();
		}

		public override void Uninit()
		{
			base.Uninit();
			this.ClearEquips();
		}

		public override void Deactive()
		{
			this.ClearEquips();
			base.Deactive();
		}

		public override void UpdateLogic(int nDelta)
		{
			int i = 0;
			while (i < this.m_equipPassiveCdList.get_Count())
			{
				this.m_equipPassiveCdList.get_Item(i).m_passiveCd -= nDelta;
				if (this.m_equipPassiveCdList.get_Item(i).m_passiveCd <= 0)
				{
					this.m_equipPassiveCdList.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			int j = 0;
			while (j < this.m_equipActiveSkillCdList.get_Count())
			{
				this.m_equipActiveSkillCdList.get_Item(j).m_passiveCd -= nDelta;
				if (this.m_equipActiveSkillCdList.get_Item(j).m_passiveCd <= 0)
				{
					this.m_equipActiveSkillCdList.RemoveAt(j);
				}
				else
				{
					j++;
				}
			}
			this.m_frame++;
			if ((int)((long)this.m_frame + (long)((ulong)this.actor.ObjID)) % 30 == 0 && this.IsPermitedToBuyEquip(Singleton<CBattleSystem>.GetInstance().m_battleEquipSystem.IsInEquipLimitedLevel()))
			{
				this.TryBuyEquipmentIfComputer();
			}
		}

		private bool RemoveEquipPassiveCd(uint passiveSkillId, out int cd, List<CEquipPassiveCdInfo> equipSkillCdList = null)
		{
			cd = 0;
			if (equipSkillCdList == null)
			{
				equipSkillCdList = this.m_equipPassiveCdList;
			}
			if (equipSkillCdList != null)
			{
				for (int i = 0; i < equipSkillCdList.get_Count(); i++)
				{
					if (equipSkillCdList.get_Item(i).m_passiveSkillId == passiveSkillId)
					{
						cd = equipSkillCdList.get_Item(i).m_passiveCd;
						equipSkillCdList.RemoveAt(i);
						return true;
					}
				}
			}
			return false;
		}

		private void AddEquipPassiveCdInfo(uint passiveSkillId, int cd, List<CEquipPassiveCdInfo> equipSkillCdList = null)
		{
			if (equipSkillCdList == null)
			{
				equipSkillCdList = this.m_equipPassiveCdList;
			}
			if (equipSkillCdList != null)
			{
				for (int i = 0; i < equipSkillCdList.get_Count(); i++)
				{
					if (equipSkillCdList.get_Item(i).m_passiveSkillId == passiveSkillId)
					{
						equipSkillCdList.get_Item(i).m_passiveCd = cd;
						return;
					}
				}
			}
			CEquipPassiveCdInfo cEquipPassiveCdInfo = new CEquipPassiveCdInfo(passiveSkillId, cd);
			equipSkillCdList.Add(cEquipPassiveCdInfo);
		}

		public bool IsEquipCanAddedToGrid(ushort equipID, ref CBattleEquipSystem.CEquipBuyPrice equipBuyPrice)
		{
			for (int i = 0; i < 6; i++)
			{
				if (this.m_equipInfos[i].m_equipID == equipID && this.m_equipInfos[i].m_amount < this.m_equipInfos[i].m_maxAmount)
				{
					return true;
				}
			}
			if (this.m_equipCount < 6)
			{
				return true;
			}
			for (int j = 0; j < 6; j++)
			{
				if (this.m_equipInfos[j].m_equipID == 0 || this.m_equipInfos[j].m_amount == 0u)
				{
					return true;
				}
				for (int k = 0; k < equipBuyPrice.m_swappedPreEquipInfoCount; k++)
				{
					if (equipBuyPrice.m_swappedPreEquipInfos[k].m_equipID > 0 && equipBuyPrice.m_swappedPreEquipInfos[k].m_swappedAmount > 0u && this.m_equipInfos[j].m_equipID == equipBuyPrice.m_swappedPreEquipInfos[k].m_equipID && this.m_equipInfos[j].m_amount <= equipBuyPrice.m_swappedPreEquipInfos[k].m_swappedAmount)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void AddEquip(ushort equipID)
		{
			for (int i = 0; i < 6; i++)
			{
				if (this.m_equipInfos[i].m_equipID == equipID && this.m_equipInfos[i].m_amount < this.m_equipInfos[i].m_maxAmount)
				{
					stEquipInfo[] expr_51_cp_0 = this.m_equipInfos;
					int expr_51_cp_1 = i;
					expr_51_cp_0[expr_51_cp_1].m_amount = expr_51_cp_0[expr_51_cp_1].m_amount + 1u;
					this.AddEquipEffect(equipID, i);
					this.AddEquipToBuyHistory(equipID);
					this.m_existEquipInfoSet.Refresh(this.m_equipInfos);
					this.HandleEquipActiveSkillWhenEquipChange(true, i);
					Singleton<EventRouter>.GetInstance().BroadCastEvent<uint, stEquipInfo[], bool, int>("HeroEquipInBattleChange", this.actor.ObjID, this.m_equipInfos, true, i);
					return;
				}
			}
			if (this.m_equipCount >= 6)
			{
				return;
			}
			int num = -1;
			for (int j = 0; j < 6; j++)
			{
				if (this.m_equipInfos[j].m_equipID == 0)
				{
					num = j;
					break;
				}
			}
			if (num >= 0)
			{
				this.m_equipInfos[num].m_equipID = equipID;
				this.m_equipInfos[num].m_amount = 1u;
				this.m_equipInfos[num].m_maxAmount = 1u;
				this.m_equipCount++;
				this.AddEquipEffect(equipID, num);
				this.AddEquipToBuyHistory(equipID);
				this.m_existEquipInfoSet.Refresh(this.m_equipInfos);
				this.HandleEquipActiveSkillWhenEquipChange(true, num);
				Singleton<EventRouter>.GetInstance().BroadCastEvent<uint, stEquipInfo[], bool, int>("HeroEquipInBattleChange", this.actor.ObjID, this.m_equipInfos, true, num);
			}
		}

		public void RemoveEquip(ushort equipID)
		{
			for (int i = 0; i < 6; i++)
			{
				if (this.m_equipInfos[i].m_equipID == equipID && this.m_equipInfos[i].m_amount > 0u)
				{
					this.RemoveEquip(i);
					return;
				}
			}
		}

		public void RemoveEquip(int equipIndex)
		{
			if (this.m_equipCount <= 0 || equipIndex < 0 || equipIndex >= 6 || this.m_equipInfos[equipIndex].m_equipID == 0 || this.m_equipInfos[equipIndex].m_amount == 0u)
			{
				return;
			}
			this.RemoveEquipEffect(this.m_equipInfos[equipIndex].m_equipID, equipIndex);
			this.HandleEquipActiveSkillWhenEquipChange(false, equipIndex);
			stEquipInfo[] expr_73_cp_0 = this.m_equipInfos;
			expr_73_cp_0[equipIndex].m_amount = expr_73_cp_0[equipIndex].m_amount - 1u;
			if (this.m_equipInfos[equipIndex].m_amount == 0u)
			{
				this.m_equipInfos[equipIndex].m_equipID = 0;
				this.m_equipInfos[equipIndex].m_amount = 0u;
				this.m_equipInfos[equipIndex].m_maxAmount = 1u;
				this.m_equipCount--;
			}
			this.m_existEquipInfoSet.Refresh(this.m_equipInfos);
			Singleton<EventRouter>.GetInstance().BroadCastEvent<uint, stEquipInfo[], bool, int>("HeroEquipInBattleChange", this.actor.ObjID, this.m_equipInfos, false, equipIndex);
		}

		public CExistEquipInfoSet GetExistEquipInfoSet()
		{
			return this.m_existEquipInfoSet;
		}

		public bool HasEquip(ushort equipID, uint amount = 1u)
		{
			uint equipAmount = this.GetEquipAmount(equipID);
			return equipAmount >= amount;
		}

		public uint GetEquipAmount(ushort equipID)
		{
			uint num = 0u;
			for (int i = 0; i < 6; i++)
			{
				if (this.m_equipInfos[i].m_equipID == equipID)
				{
					num += this.m_equipInfos[i].m_amount;
				}
			}
			return num;
		}

		public bool HasEquipInGroup(ushort group)
		{
			for (int i = 0; i < 6; i++)
			{
				if (this.m_equipInfos[i].m_equipID != 0)
				{
					ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint)this.m_equipInfos[i].m_equipID);
					if (dataByKey.wGroup == group)
					{
						return true;
					}
				}
			}
			return false;
		}

		public stEquipInfo[] GetEquips()
		{
			return this.m_equipInfos;
		}

		public ListView<CRecommendEquipInfo> GetRecommendEquipInfos()
		{
			return this.m_recommendEquipInfos;
		}

		public Dictionary<ushort, uint> GetEquipBoughtHistory()
		{
			return this.m_equipBoughtHistory;
		}

		public void ResetHasLeftEquipBoughtArea()
		{
			this.m_hasLeftEquipBoughtArea = false;
		}

		public bool IsPermitedToBuyEquip(bool isInEquipLimitedLevel)
		{
			return !isInEquipLimitedLevel || this.actor.ActorControl.IsDeadState || (this.m_isInEquipBoughtArea && !this.m_hasLeftEquipBoughtArea);
		}

		public void UpdateEquipEffect()
		{
			Dictionary<ushort, CEquipPassiveSkillInfoGroup>.Enumerator enumerator = this.m_equipPassiveSkillInfoMap.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<ushort, CEquipPassiveSkillInfoGroup> current = enumerator.get_Current();
				this.UpdateEquipPassiveSkill(current.get_Value());
			}
			Dictionary<ushort, CEquipBuffInfoGroup>.Enumerator enumerator2 = this.m_equipBuffInfoMap.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				KeyValuePair<ushort, CEquipBuffInfoGroup> current2 = enumerator2.get_Current();
				this.UpdateEquipBuff(current2.get_Value());
			}
		}

		private void ClearEquips()
		{
			for (int i = 0; i < 6; i++)
			{
				this.m_equipInfos[i].m_equipID = 0;
				this.m_equipInfos[i].m_amount = 0u;
				this.m_equipInfos[i].m_maxAmount = 1u;
				this.m_enmEquipActiveSkillStatus[i] = ENUM_EQUIP_ACTIVESKILL_STATUS.ENM_EQUIP_ACTIVESKILL_STATUS_WHITHOUTACTIVESKILL;
			}
			this.m_equipCount = 0;
			this.m_frame = 0;
			this.m_equipPassiveSkillInfoMap.Clear();
			this.m_equipBuffInfoMap.Clear();
			this.m_existEquipInfoSet.Clear();
			this.m_recommendEquipInfos.Clear();
			this.m_equipBoughtHistory.Clear();
			this.m_isInEquipBoughtArea = false;
			this.m_hasLeftEquipBoughtArea = false;
			this.m_iFastBuyEquipCount = 0;
			this.m_iBuyEquipCount = 0;
			this.m_equipActiveSkillCdList.Clear();
			for (int j = 0; j < 1; j++)
			{
				this.m_stEquipActiveSkillInfo[j].bIsFirstInit = true;
				this.m_stEquipActiveSkillInfo[j].bIsFirstShow = false;
				this.m_stEquipActiveSkillInfo[j].iEquipSlotIndex = -1;
			}
			this.m_horizonEquipId = 0;
		}

		private void AddEquipEffect(ushort equipID, int iEquipSlotIndex)
		{
			ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint)equipID);
			if (dataByKey == null)
			{
				return;
			}
			this.AddEquipPropertyValue(dataByKey);
			for (int i = 0; i < dataByKey.astPassiveSkill.Length; i++)
			{
				if (dataByKey.astPassiveSkill[i].dwID > 0u)
				{
					this.AddEquipPassiveSkill(equipID, dataByKey.astPassiveSkill[i].dwID, dataByKey.astPassiveSkill[i].wUniquePassiveGroup, dataByKey.dwBuyPrice, dataByKey.astPassiveSkill[i].PassiveRmvSkillFuncID, EquipComponent.s_equipEffectSequence++);
				}
			}
			for (int j = 0; j < dataByKey.astEffectCombine.Length; j++)
			{
				if (dataByKey.astEffectCombine[j].dwID > 0u)
				{
					this.AddEquipBuff(equipID, dataByKey.astEffectCombine[j].dwID, dataByKey.astEffectCombine[j].wUniquePassiveGroup, dataByKey.dwBuyPrice, EquipComponent.s_equipEffectSequence++);
				}
			}
		}

		private void RemoveEquipEffect(ushort equipID, int iEquipSlotIndex)
		{
			ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint)equipID);
			if (dataByKey == null)
			{
				return;
			}
			this.RemoveEquipPropertyValue(dataByKey);
			for (int i = 0; i < dataByKey.astPassiveSkill.Length; i++)
			{
				if (dataByKey.astPassiveSkill[i].dwID > 0u)
				{
					this.RemoveEquipPassiveSkill(equipID, dataByKey.astPassiveSkill[i].dwID, dataByKey.astPassiveSkill[i].wUniquePassiveGroup);
				}
			}
			for (int j = 0; j < dataByKey.astEffectCombine.Length; j++)
			{
				if (dataByKey.astEffectCombine[j].dwID > 0u)
				{
					this.RemoveEquipBuff(equipID, dataByKey.astEffectCombine[j].dwID, dataByKey.astEffectCombine[j].wUniquePassiveGroup);
				}
			}
			SkillSlot skillSlot;
			if (this.GetEquipActiveSkillSlotInfo(iEquipSlotIndex) == ENUM_EQUIP_ACTIVESKILL_STATUS.ENM_EQUIP_ACTIVESKILL_STATUS_ISSHOWING && this.actorPtr && this.actorPtr.get_handle().SkillControl != null && this.actorPtr.get_handle().SkillControl.TryGetSkillSlot(SkillSlotType.SLOT_SKILL_9, out skillSlot))
			{
				int cd = skillSlot.CurSkillCD;
				this.AddEquipPassiveCdInfo((uint)skillSlot.SkillObj.SkillID, cd, this.m_equipActiveSkillCdList);
			}
		}

		private void AddEquipPropertyValue(ResEquipInBattle resEquipInBattle)
		{
			this.HandleEquipPropertyValue(resEquipInBattle, 1);
		}

		private void RemoveEquipPropertyValue(ResEquipInBattle resEquipInBattle)
		{
			this.HandleEquipPropertyValue(resEquipInBattle, -1);
		}

		private void HandleEquipPropertyValue(ResEquipInBattle resEquipInBattle, int flag)
		{
			if (resEquipInBattle.dwPhyAttack > 0u)
			{
				this.actor.ValueComponent.mActorValue[1].addValue += (int)(resEquipInBattle.dwPhyAttack * (uint)flag);
			}
			if (resEquipInBattle.dwAttackSpeed > 0u)
			{
				this.actor.ValueComponent.mActorValue[18].addValue += (int)(resEquipInBattle.dwAttackSpeed * (uint)flag);
			}
			if (resEquipInBattle.dwCriticalHit > 0u)
			{
				this.actor.ValueComponent.mActorValue[6].addValue += (int)(resEquipInBattle.dwCriticalHit * (uint)flag);
			}
			if (resEquipInBattle.dwHealthSteal > 0u)
			{
				this.actor.ValueComponent.mActorValue[9].addValue += (int)(resEquipInBattle.dwHealthSteal * (uint)flag);
			}
			if (resEquipInBattle.dwMagicAttack > 0u)
			{
				this.actor.ValueComponent.mActorValue[2].addValue += (int)(resEquipInBattle.dwMagicAttack * (uint)flag);
			}
			if (resEquipInBattle.dwCDReduce > 0u)
			{
				this.actor.ValueComponent.mActorValue[20].addValue += (int)(resEquipInBattle.dwCDReduce * (uint)flag);
			}
			if (this.actor.ValueComponent.IsEnergyType(EnergyType.MagicResource))
			{
				if (resEquipInBattle.dwMagicPoint > 0u)
				{
					VFactor vFactor = new VFactor((long)this.actor.ValueComponent.actorEp, (long)this.actor.ValueComponent.mActorValue[32].totalValue);
					this.actor.ValueComponent.mActorValue[32].addValue += (int)(resEquipInBattle.dwMagicPoint * (uint)flag);
					if (flag > 0 && !this.actor.ActorControl.IsDeadState && this.actor.ValueComponent.actorEp > 0)
					{
						this.actor.ValueComponent.actorEp = (vFactor * (long)this.actor.ValueComponent.mActorValue[32].totalValue).get_roundInt();
					}
				}
				if (resEquipInBattle.dwMagicRecover > 0u)
				{
					this.actor.ValueComponent.mActorValue[33].addValue += (int)(resEquipInBattle.dwMagicRecover * (uint)flag);
				}
			}
			if (resEquipInBattle.dwPhyDefence > 0u)
			{
				this.actor.ValueComponent.mActorValue[3].addValue += (int)(resEquipInBattle.dwPhyDefence * (uint)flag);
			}
			if (resEquipInBattle.dwMagicDefence > 0u)
			{
				this.actor.ValueComponent.mActorValue[4].addValue += (int)(resEquipInBattle.dwMagicDefence * (uint)flag);
			}
			if (resEquipInBattle.dwHealthPoint > 0u)
			{
				VFactor vFactor2 = new VFactor((long)this.actor.ValueComponent.actorHp, (long)this.actor.ValueComponent.mActorValue[5].totalValue);
				this.actor.ValueComponent.mActorValue[5].addValue += (int)(resEquipInBattle.dwHealthPoint * (uint)flag);
				if (flag > 0 && !this.actor.ActorControl.IsDeadState)
				{
					this.actor.ValueComponent.actorHp = (vFactor2 * (long)this.actor.ValueComponent.mActorValue[5].totalValue).get_roundInt();
				}
			}
			if (resEquipInBattle.dwHealthRecover > 0u)
			{
				this.actor.ValueComponent.mActorValue[16].addValue += (int)(resEquipInBattle.dwHealthRecover * (uint)flag);
			}
			if (resEquipInBattle.dwMoveSpeed > 0u)
			{
				this.actor.ValueComponent.mActorValue[15].addRatio += (int)(resEquipInBattle.dwMoveSpeed * (uint)flag);
			}
		}

		private void AddEquipPassiveSkill(ushort equipID, uint passiveSkillID, ushort passiveSkillGroupID, uint equipBuyPrice, uint[] passiveSkillRelatedFuncIDs, uint sequence)
		{
			CEquipPassiveSkillInfoGroup cEquipPassiveSkillInfoGroup = null;
			if (this.m_equipPassiveSkillInfoMap.ContainsKey(passiveSkillGroupID))
			{
				this.m_equipPassiveSkillInfoMap.TryGetValue(passiveSkillGroupID, ref cEquipPassiveSkillInfoGroup);
			}
			else
			{
				cEquipPassiveSkillInfoGroup = new CEquipPassiveSkillInfoGroup(passiveSkillGroupID);
				this.m_equipPassiveSkillInfoMap.Add(passiveSkillGroupID, cEquipPassiveSkillInfoGroup);
			}
			cEquipPassiveSkillInfoGroup.m_isChanged = true;
			ListView<CEquipPassiveSkillInfo> equipPassiveSkillInfos = cEquipPassiveSkillInfoGroup.m_equipPassiveSkillInfos;
			CEquipPassiveSkillInfo cEquipPassiveSkillInfo = null;
			for (int i = 0; i < equipPassiveSkillInfos.get_Count(); i++)
			{
				if (equipPassiveSkillInfos.get_Item(i).m_isNeedRemoved && equipPassiveSkillInfos.get_Item(i).IsEqual(passiveSkillID, passiveSkillGroupID, passiveSkillRelatedFuncIDs))
				{
					cEquipPassiveSkillInfo = equipPassiveSkillInfos.get_Item(i);
					cEquipPassiveSkillInfo.m_isNeedRemoved = false;
					cEquipPassiveSkillInfo.m_equipID = equipID;
					cEquipPassiveSkillInfo.m_equipBuyPrice = (int)equipBuyPrice;
					cEquipPassiveSkillInfo.m_sequence = sequence;
					break;
				}
			}
			if (cEquipPassiveSkillInfo == null)
			{
				cEquipPassiveSkillInfo = new CEquipPassiveSkillInfo(equipID, equipBuyPrice, passiveSkillID, passiveSkillGroupID, passiveSkillRelatedFuncIDs, sequence);
				equipPassiveSkillInfos.Add(cEquipPassiveSkillInfo);
			}
		}

		private void RemoveEquipPassiveSkill(ushort equipID, uint passiveSkillID, ushort passiveSkillGroupID)
		{
			CEquipPassiveSkillInfoGroup cEquipPassiveSkillInfoGroup = null;
			if (this.m_equipPassiveSkillInfoMap.ContainsKey(passiveSkillGroupID))
			{
				this.m_equipPassiveSkillInfoMap.TryGetValue(passiveSkillGroupID, ref cEquipPassiveSkillInfoGroup);
				cEquipPassiveSkillInfoGroup.m_isChanged = true;
				ListView<CEquipPassiveSkillInfo> equipPassiveSkillInfos = cEquipPassiveSkillInfoGroup.m_equipPassiveSkillInfos;
				for (int i = 0; i < equipPassiveSkillInfos.get_Count(); i++)
				{
					if (equipPassiveSkillInfos.get_Item(i).m_equipID == equipID && equipPassiveSkillInfos.get_Item(i).m_passiveSkillID == passiveSkillID && equipPassiveSkillInfos.get_Item(i).m_passiveSkillGroupID == passiveSkillGroupID)
					{
						equipPassiveSkillInfos.get_Item(i).m_isNeedRemoved = true;
						break;
					}
				}
			}
		}

		private void UpdateEquipPassiveSkill(CEquipPassiveSkillInfoGroup equipPassiveSkillInfoGroup)
		{
			if (!equipPassiveSkillInfoGroup.m_isChanged)
			{
				return;
			}
			ListView<CEquipPassiveSkillInfo> equipPassiveSkillInfos = equipPassiveSkillInfoGroup.m_equipPassiveSkillInfos;
			int i = 0;
			while (i < equipPassiveSkillInfos.get_Count())
			{
				if (equipPassiveSkillInfos.get_Item(i).m_isNeedRemoved)
				{
					this.DisableEquipPassiveSkill(equipPassiveSkillInfos.get_Item(i));
					equipPassiveSkillInfos.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			if (equipPassiveSkillInfoGroup.m_groupID == 0)
			{
				for (int j = 0; j < equipPassiveSkillInfos.get_Count(); j++)
				{
					this.EnableEquipPassiveSkill(equipPassiveSkillInfos.get_Item(j));
				}
			}
			else
			{
				equipPassiveSkillInfos.Sort();
				for (int k = 0; k < equipPassiveSkillInfos.get_Count(); k++)
				{
					if (k == equipPassiveSkillInfos.get_Count() - 1)
					{
						this.EnableEquipPassiveSkill(equipPassiveSkillInfos.get_Item(k));
					}
					else
					{
						this.DisableEquipPassiveSkill(equipPassiveSkillInfos.get_Item(k));
					}
				}
			}
			equipPassiveSkillInfoGroup.m_isChanged = false;
		}

		private void EnableEquipPassiveSkill(CEquipPassiveSkillInfo equipPassiveSkillInfo)
		{
			if (equipPassiveSkillInfo.m_isEnabled)
			{
				return;
			}
			int cdTime = 0;
			if (this.RemoveEquipPassiveCd(equipPassiveSkillInfo.m_passiveSkillID, out cdTime, null))
			{
				this.actor.SkillControl.talentSystem.InitTalent((int)equipPassiveSkillInfo.m_passiveSkillID, cdTime);
			}
			else
			{
				this.actor.SkillControl.talentSystem.InitTalent((int)equipPassiveSkillInfo.m_passiveSkillID);
			}
			equipPassiveSkillInfo.m_isEnabled = true;
		}

		private void DisableEquipPassiveSkill(CEquipPassiveSkillInfo equipPassiveSkillInfo)
		{
			if (!equipPassiveSkillInfo.m_isEnabled)
			{
				return;
			}
			int talentCDTime = this.actor.SkillControl.talentSystem.GetTalentCDTime((int)equipPassiveSkillInfo.m_passiveSkillID);
			if (talentCDTime > 0)
			{
				this.AddEquipPassiveCdInfo(equipPassiveSkillInfo.m_passiveSkillID, talentCDTime, null);
			}
			this.actor.SkillControl.talentSystem.RemoveTalent((int)equipPassiveSkillInfo.m_passiveSkillID);
			if (this.actor.BuffHolderComp != null && equipPassiveSkillInfo.m_passiveSkillRelatedFuncIDs != null && equipPassiveSkillInfo.m_passiveSkillRelatedFuncIDs.Length > 0)
			{
				for (int i = 0; i < equipPassiveSkillInfo.m_passiveSkillRelatedFuncIDs.Length; i++)
				{
					if (equipPassiveSkillInfo.m_passiveSkillRelatedFuncIDs[i] > 0u)
					{
						this.actor.BuffHolderComp.RemoveBuff((int)equipPassiveSkillInfo.m_passiveSkillRelatedFuncIDs[i]);
					}
				}
			}
			equipPassiveSkillInfo.m_isEnabled = false;
		}

		private void AddEquipBuff(ushort equipID, uint buffID, ushort buffGroupID, uint equipBuyPrice, uint sequence)
		{
			CEquipBuffInfoGroup cEquipBuffInfoGroup = null;
			if (this.m_equipBuffInfoMap.ContainsKey(buffGroupID))
			{
				this.m_equipBuffInfoMap.TryGetValue(buffGroupID, ref cEquipBuffInfoGroup);
			}
			else
			{
				cEquipBuffInfoGroup = new CEquipBuffInfoGroup(buffGroupID);
				this.m_equipBuffInfoMap.Add(buffGroupID, cEquipBuffInfoGroup);
			}
			cEquipBuffInfoGroup.m_isChanged = true;
			ListView<CEquipBuffInfo> equipBuffInfos = cEquipBuffInfoGroup.m_equipBuffInfos;
			CEquipBuffInfo cEquipBuffInfo = null;
			for (int i = 0; i < equipBuffInfos.get_Count(); i++)
			{
				if (equipBuffInfos.get_Item(i).m_isNeedRemoved && equipBuffInfos.get_Item(i).IsEqual(buffID, buffGroupID))
				{
					cEquipBuffInfo = equipBuffInfos.get_Item(i);
					cEquipBuffInfo.m_isNeedRemoved = false;
					cEquipBuffInfo.m_equipID = equipID;
					cEquipBuffInfo.m_equipBuyPrice = (int)equipBuyPrice;
					cEquipBuffInfo.m_sequence = sequence;
					break;
				}
			}
			if (cEquipBuffInfo == null)
			{
				cEquipBuffInfo = new CEquipBuffInfo(equipID, equipBuyPrice, buffID, buffGroupID, sequence);
				equipBuffInfos.Add(cEquipBuffInfo);
			}
		}

		private void RemoveEquipBuff(ushort equipID, uint buffID, ushort buffGroupID)
		{
			CEquipBuffInfoGroup cEquipBuffInfoGroup = null;
			if (this.m_equipBuffInfoMap.ContainsKey(buffGroupID))
			{
				this.m_equipBuffInfoMap.TryGetValue(buffGroupID, ref cEquipBuffInfoGroup);
				cEquipBuffInfoGroup.m_isChanged = true;
				ListView<CEquipBuffInfo> equipBuffInfos = cEquipBuffInfoGroup.m_equipBuffInfos;
				for (int i = 0; i < equipBuffInfos.get_Count(); i++)
				{
					if (equipBuffInfos.get_Item(i).m_equipID == equipID && equipBuffInfos.get_Item(i).m_buffID == buffID && equipBuffInfos.get_Item(i).m_buffGroupID == buffGroupID)
					{
						equipBuffInfos.get_Item(i).m_isNeedRemoved = true;
						break;
					}
				}
			}
		}

		private void UpdateEquipBuff(CEquipBuffInfoGroup equipBuffInfoGroup)
		{
			if (!equipBuffInfoGroup.m_isChanged)
			{
				return;
			}
			ListView<CEquipBuffInfo> equipBuffInfos = equipBuffInfoGroup.m_equipBuffInfos;
			int i = 0;
			while (i < equipBuffInfos.get_Count())
			{
				if (equipBuffInfos.get_Item(i).m_isNeedRemoved)
				{
					this.DisableEquipBuff(equipBuffInfos.get_Item(i));
					equipBuffInfos.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			if (equipBuffInfoGroup.m_groupID == 0)
			{
				for (int j = 0; j < equipBuffInfos.get_Count(); j++)
				{
					this.EnableEquipBuff(equipBuffInfos.get_Item(j));
				}
			}
			else
			{
				equipBuffInfos.Sort();
				for (int k = 0; k < equipBuffInfos.get_Count(); k++)
				{
					if (k == equipBuffInfos.get_Count() - 1)
					{
						this.EnableEquipBuff(equipBuffInfos.get_Item(k));
					}
					else
					{
						this.DisableEquipBuff(equipBuffInfos.get_Item(k));
					}
				}
			}
			equipBuffInfoGroup.m_isChanged = false;
		}

		private void EnableEquipBuff(CEquipBuffInfo equipBuffInfo)
		{
			if (equipBuffInfo.m_isEnabled)
			{
				return;
			}
			SkillUseParam skillUseParam = default(SkillUseParam);
			skillUseParam.SetOriginator(this.actorPtr);
			skillUseParam.SlotType = SkillSlotType.SLOT_SKILL_COUNT;
			this.actor.SkillControl.SpawnBuff(this.actorPtr, ref skillUseParam, (int)equipBuffInfo.m_buffID, false);
			equipBuffInfo.m_isEnabled = true;
		}

		private void DisableEquipBuff(CEquipBuffInfo equipBuffInfo)
		{
			if (!equipBuffInfo.m_isEnabled)
			{
				return;
			}
			this.actor.SkillControl.RemoveBuff(this.actorPtr, (int)equipBuffInfo.m_buffID);
			equipBuffInfo.m_isEnabled = false;
		}

		private void InitializeRecommendEquips()
		{
			DebugHelper.Assert(this.actor != null, "InitializeRecommendEquips with actor==null");
			ActorServerData actorServerData = default(ActorServerData);
			IGameActorDataProvider actorDataProvider = Singleton<ActorDataCenter>.GetInstance().GetActorDataProvider(GameActorDataProviderType.ServerDataProvider);
			if (actorDataProvider != null)
			{
				actorDataProvider.GetActorServerData(ref this.actor.TheActorMeta, ref actorServerData);
			}
			else
			{
				DebugHelper.Assert(false, "Failed Get gameActorDataProvider");
			}
			DebugHelper.Assert(actorServerData.m_customRecommendEquips != null, "actorServerData.m_customRecommendEquips==null");
			for (int i = 0; i < actorServerData.m_customRecommendEquips.Length; i++)
			{
				if (actorServerData.m_customRecommendEquips[i] != 0)
				{
					ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint)actorServerData.m_customRecommendEquips[i]);
					if (dataByKey != null && dataByKey.bInvalid <= 0)
					{
						this.m_recommendEquipInfos.Add(new CRecommendEquipInfo(actorServerData.m_customRecommendEquips[i], dataByKey));
					}
				}
			}
			Singleton<EventRouter>.GetInstance().BroadCastEvent<uint>("HeroRecommendEquipInit", this.actor.ObjID);
		}

		private void InitHorizonEquipID()
		{
			SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
			if (!curLvelContext.m_bEnableShopHorizonTab || !curLvelContext.m_bEnableOrnamentSlot)
			{
				return;
			}
			CBattleEquipSystem battleEquipSystem = Singleton<CBattleSystem>.GetInstance().m_battleEquipSystem;
			if (battleEquipSystem == null)
			{
				return;
			}
			List<ushort> usageLevelEquipList = battleEquipSystem.GetUsageLevelEquipList(enEquipUsage.Horizon, 1);
			if (usageLevelEquipList != null)
			{
				for (int i = 0; i < usageLevelEquipList.get_Count(); i++)
				{
					ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint)usageLevelEquipList.get_Item(i));
					if (dataByKey != null && dataByKey.dwActiveSkillID != 0u && (ulong)dataByKey.dwActiveSkillID == (ulong)((long)curLvelContext.m_ornamentSkillId))
					{
						this.m_horizonEquipId = usageLevelEquipList.get_Item(i);
						return;
					}
				}
			}
		}

		private void AddEquipToBuyHistory(ushort equipID)
		{
			if (!this.m_equipBoughtHistory.ContainsKey(equipID))
			{
				uint num = (uint)(Singleton<FrameSynchr>.get_instance().LogicFrameTick / 1000uL);
				this.m_equipBoughtHistory.Add(equipID, num);
			}
			for (int i = 0; i < this.m_recommendEquipInfos.get_Count(); i++)
			{
				if (this.m_recommendEquipInfos.get_Item(i).m_equipID == equipID && !this.m_recommendEquipInfos.get_Item(i).m_hasBeenBought)
				{
					this.m_recommendEquipInfos.get_Item(i).m_hasBeenBought = true;
					break;
				}
			}
		}

		public void TryBuyEquipmentIfComputer()
		{
			CBattleEquipSystem battleEquipSystem = Singleton<CBattleSystem>.GetInstance().m_battleEquipSystem;
			if (battleEquipSystem == null)
			{
				return;
			}
			if (this.actor.ActorAgent.IsAutoAI())
			{
				ushort[] quicklyBuyEquipList = battleEquipSystem.GetQuicklyBuyEquipList(ref this.actorPtr);
				for (int i = 0; i < quicklyBuyEquipList.Length; i++)
				{
					if (quicklyBuyEquipList[i] > 0)
					{
						battleEquipSystem.ExecuteBuyEquipFrameCommand(quicklyBuyEquipList[i], ref this.actorPtr);
						break;
					}
				}
			}
		}

		public void SetEquipActiveSkillSlotInfo(int iEquipSlotIndex, ENUM_EQUIP_ACTIVESKILL_STATUS enmStatus)
		{
			if (this.m_enmEquipActiveSkillStatus == null || iEquipSlotIndex < 0 || iEquipSlotIndex >= 6)
			{
				return;
			}
			if (this.m_enmEquipActiveSkillStatus[iEquipSlotIndex] == enmStatus)
			{
				return;
			}
			this.m_enmEquipActiveSkillStatus[iEquipSlotIndex] = enmStatus;
		}

		public ENUM_EQUIP_ACTIVESKILL_STATUS GetEquipActiveSkillSlotInfo(int iEquipSlotIndex)
		{
			ENUM_EQUIP_ACTIVESKILL_STATUS result = ENUM_EQUIP_ACTIVESKILL_STATUS.ENM_EQUIP_ACTIVESKILL_STATUS_WHITHOUTACTIVESKILL;
			if (this.m_enmEquipActiveSkillStatus != null && iEquipSlotIndex >= 0 && iEquipSlotIndex < 6)
			{
				result = this.m_enmEquipActiveSkillStatus[iEquipSlotIndex];
			}
			return result;
		}

		public int GetShowingEquipActiveSkillSlotCount()
		{
			int num = 0;
			if (this.m_stEquipActiveSkillInfo != null)
			{
				for (int i = 0; i < 1; i++)
				{
					if (this.m_stEquipActiveSkillInfo[i].iEquipSlotIndex != -1)
					{
						num++;
					}
				}
			}
			return num;
		}

		public void AddEquipActiveSkillCdInfo(int iEquipSlotIndex, int cd)
		{
			if (this.m_equipInfos != null && iEquipSlotIndex >= 0 && iEquipSlotIndex < 6)
			{
				ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint)this.m_equipInfos[iEquipSlotIndex].m_equipID);
				if (dataByKey != null)
				{
					uint passiveSkillId = (dataByKey.dwActiveSkillGroupID == 0u) ? dataByKey.dwActiveSkillID : dataByKey.dwActiveSkillGroupID;
					this.AddEquipPassiveCdInfo(passiveSkillId, cd, this.m_equipActiveSkillCdList);
				}
			}
		}

		public bool RemoveEquipActiveSkillCdInfo(int iEquipSlotIndex, out int cd)
		{
			cd = 0;
			if (this.m_equipInfos != null && iEquipSlotIndex >= 0 && iEquipSlotIndex < 6)
			{
				ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint)this.m_equipInfos[iEquipSlotIndex].m_equipID);
				if (dataByKey != null)
				{
					uint passiveSkillId = (dataByKey.dwActiveSkillGroupID == 0u) ? dataByKey.dwActiveSkillID : dataByKey.dwActiveSkillGroupID;
					return this.RemoveEquipPassiveCd(passiveSkillId, out cd, this.m_equipActiveSkillCdList);
				}
			}
			return false;
		}

		public bool GetEquipActiveSkillCdInfo(int iEquipSlotIndex, out int cd)
		{
			cd = 0;
			if (this.m_equipInfos != null && iEquipSlotIndex >= 0 && iEquipSlotIndex < 6 && this.m_equipActiveSkillCdList != null)
			{
				ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint)this.m_equipInfos[iEquipSlotIndex].m_equipID);
				if (dataByKey == null)
				{
					return false;
				}
				uint num = (dataByKey.dwActiveSkillGroupID == 0u) ? dataByKey.dwActiveSkillID : dataByKey.dwActiveSkillGroupID;
				int count = this.m_equipActiveSkillCdList.get_Count();
				for (int i = 0; i < count; i++)
				{
					if (this.m_equipActiveSkillCdList.get_Item(i).m_passiveSkillId == num)
					{
						cd = this.m_equipActiveSkillCdList.get_Item(i).m_passiveCd;
						return true;
					}
				}
			}
			return false;
		}

		public SkillSlotType GetShowEquipActiveSkillSlot()
		{
			SkillSlotType result = SkillSlotType.SLOT_SKILL_VALID;
			int showingEquipActiveSkillSlotCount = this.GetShowingEquipActiveSkillSlotCount();
			if (showingEquipActiveSkillSlotCount < 1)
			{
				for (int i = 0; i < 1; i++)
				{
					if (this.m_stEquipActiveSkillInfo[i].iEquipSlotIndex == -1)
					{
						result = SkillSlotType.SLOT_SKILL_9 + i;
						break;
					}
				}
			}
			else
			{
				result = SkillSlotType.SLOT_SKILL_9;
			}
			return result;
		}

		public SkillSlotType GetShowingSkillSlotByEquipSlot(int iEquipSlotIndex)
		{
			SkillSlotType result = SkillSlotType.SLOT_SKILL_VALID;
			for (int i = 0; i < 1; i++)
			{
				if (this.m_stEquipActiveSkillInfo[i].iEquipSlotIndex == iEquipSlotIndex)
				{
					result = SkillSlotType.SLOT_SKILL_9 + i;
					break;
				}
			}
			return result;
		}

		public int GetEquipSlotBySkillSlot(SkillSlotType skillSlot)
		{
			int result = -1;
			int num = skillSlot - SkillSlotType.SLOT_SKILL_9;
			if (num < 0 || num >= 1)
			{
				return result;
			}
			return this.m_stEquipActiveSkillInfo[num].iEquipSlotIndex;
		}

		private void HandleEquipActiveSkillWhenEquipChange(bool bIsAdd, int iEquipSlotIndex)
		{
			bool flag = false;
			if (bIsAdd)
			{
				if (this.m_equipInfos == null)
				{
					return;
				}
				ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint)this.m_equipInfos[iEquipSlotIndex].m_equipID);
				if (dataByKey != null && dataByKey.dwActiveSkillID > 0u)
				{
					flag = true;
				}
				if (flag)
				{
					if (this.GetShowingEquipActiveSkillSlotCount() < 1)
					{
						SkillSlotType showEquipActiveSkillSlot = this.GetShowEquipActiveSkillSlot();
						if (showEquipActiveSkillSlot != SkillSlotType.SLOT_SKILL_VALID && this.ChangeEquipSkillSlot(showEquipActiveSkillSlot, iEquipSlotIndex))
						{
							this.SetEquipActiveSkillSlotInfo(iEquipSlotIndex, ENUM_EQUIP_ACTIVESKILL_STATUS.ENM_EQUIP_ACTIVESKILL_STATUS_ISSHOWING);
						}
					}
					else
					{
						this.SetEquipActiveSkillSlotInfo(iEquipSlotIndex, ENUM_EQUIP_ACTIVESKILL_STATUS.ENM_EQUIP_ACTIVESKILL_STATUS_NOTSHOW);
					}
				}
				else
				{
					this.SetEquipActiveSkillSlotInfo(iEquipSlotIndex, ENUM_EQUIP_ACTIVESKILL_STATUS.ENM_EQUIP_ACTIVESKILL_STATUS_WHITHOUTACTIVESKILL);
				}
			}
			else
			{
				ENUM_EQUIP_ACTIVESKILL_STATUS equipActiveSkillSlotInfo = this.GetEquipActiveSkillSlotInfo(iEquipSlotIndex);
				if (equipActiveSkillSlotInfo == ENUM_EQUIP_ACTIVESKILL_STATUS.ENM_EQUIP_ACTIVESKILL_STATUS_ISSHOWING)
				{
					SkillSlotType showingSkillSlotByEquipSlot = this.GetShowingSkillSlotByEquipSlot(iEquipSlotIndex);
					if (showingSkillSlotByEquipSlot != SkillSlotType.SLOT_SKILL_VALID)
					{
						SkillSlot skillSlot;
						if (this.actorPtr.get_handle().SkillControl.TryGetSkillSlot(showingSkillSlotByEquipSlot, out skillSlot))
						{
							int cd = skillSlot.CurSkillCD;
							this.AddEquipActiveSkillCdInfo(iEquipSlotIndex, cd);
						}
						bool flag2 = false;
						for (int i = 0; i < 6; i++)
						{
							if (i != iEquipSlotIndex && this.m_equipInfos[i].m_equipID > 0 && this.GetEquipActiveSkillSlotInfo(i) == ENUM_EQUIP_ACTIVESKILL_STATUS.ENM_EQUIP_ACTIVESKILL_STATUS_NOTSHOW && this.ChangeEquipSkillSlot(showingSkillSlotByEquipSlot, i))
							{
								this.SetEquipActiveSkillSlotInfo(i, ENUM_EQUIP_ACTIVESKILL_STATUS.ENM_EQUIP_ACTIVESKILL_STATUS_ISSHOWING);
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							this.SetEquipActiveSkillSlotInfo(iEquipSlotIndex, ENUM_EQUIP_ACTIVESKILL_STATUS.ENM_EQUIP_ACTIVESKILL_STATUS_WHITHOUTACTIVESKILL);
						}
						else
						{
							this.SetEquipActiveSkillSlotInfo(iEquipSlotIndex, ENUM_EQUIP_ACTIVESKILL_STATUS.ENM_EQUIP_ACTIVESKILL_STATUS_WHITHOUTACTIVESKILL_REMOVECURSKILL);
							this.m_stEquipActiveSkillInfo[showingSkillSlotByEquipSlot - SkillSlotType.SLOT_SKILL_9].iEquipSlotIndex = -1;
						}
					}
				}
				else
				{
					this.SetEquipActiveSkillSlotInfo(iEquipSlotIndex, ENUM_EQUIP_ACTIVESKILL_STATUS.ENM_EQUIP_ACTIVESKILL_STATUS_WHITHOUTACTIVESKILL);
				}
			}
		}

		public bool ChangeEquipSkillSlot(SkillSlotType slot, int iEquipSlotIndex)
		{
			bool flag = false;
			bool bIsFirstInit = false;
			SkillComponent skillControl = this.actorPtr.get_handle().SkillControl;
			if (this.actorPtr.get_handle().BuffHolderComp != null && skillControl != null)
			{
				SkillSlot skillSlot = skillControl.SkillSlotArray[(int)slot];
				ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint)this.m_equipInfos[iEquipSlotIndex].m_equipID);
				if (dataByKey != null)
				{
					if (skillSlot == null || skillSlot.SkillObj == null)
					{
						skillControl.InitSkillSlot((int)slot, (int)dataByKey.dwActiveSkillID, 0);
						if (skillControl.TryGetSkillSlot(slot, out skillSlot))
						{
							skillSlot.SetSkillLevel(1);
							skillSlot.CurSkillCD = 0;
							bIsFirstInit = true;
							flag = true;
						}
					}
					else if (skillControl.TryGetSkillSlot(slot, out skillSlot))
					{
						BuffChangeSkillRule changeSkillRule = this.actorPtr.get_handle().BuffHolderComp.changeSkillRule;
						if (changeSkillRule != null)
						{
							int nValue = 0;
							this.actorPtr.get_handle().EquipComponent.GetEquipActiveSkillCdInfo(iEquipSlotIndex, out nValue);
							skillSlot.CurSkillCD = nValue;
							bIsFirstInit = false;
							if ((ulong)dataByKey.dwActiveSkillID != (ulong)((long)skillSlot.SkillObj.SkillID))
							{
								changeSkillRule.ChangeSkillSlot(slot, (int)dataByKey.dwActiveSkillID, skillSlot.SkillObj.SkillID);
							}
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				if (this.m_stEquipActiveSkillInfo[0 - (slot - SkillSlotType.SLOT_SKILL_9)].iEquipSlotIndex != -1)
				{
					this.m_stEquipActiveSkillInfo[0 - (slot - SkillSlotType.SLOT_SKILL_9)].bIsFirstShow = true;
					this.m_stEquipActiveSkillInfo[slot - SkillSlotType.SLOT_SKILL_9].bIsFirstShow = false;
				}
				else
				{
					this.m_stEquipActiveSkillInfo[slot - SkillSlotType.SLOT_SKILL_9].bIsFirstShow = true;
				}
				this.m_stEquipActiveSkillInfo[slot - SkillSlotType.SLOT_SKILL_9].bIsFirstInit = bIsFirstInit;
				this.m_stEquipActiveSkillInfo[slot - SkillSlotType.SLOT_SKILL_9].iEquipSlotIndex = iEquipSlotIndex;
			}
			return flag;
		}

		public bool GetEquipActiveSkillInitFlag(SkillSlotType slot)
		{
			return this.m_stEquipActiveSkillInfo[slot - SkillSlotType.SLOT_SKILL_9].bIsFirstInit;
		}

		public bool GetEquipActiveSkillShowFlag(SkillSlotType slot)
		{
			return this.m_stEquipActiveSkillInfo[slot - SkillSlotType.SLOT_SKILL_9].iEquipSlotIndex != -1;
		}

		public bool CheckEquipActiveSKillCanUse(SkillSlotType slot, int uiSkillId)
		{
			if (slot != SkillSlotType.SLOT_SKILL_10 && slot != SkillSlotType.SLOT_SKILL_9)
			{
				return true;
			}
			if (this.m_stEquipActiveSkillInfo[slot - SkillSlotType.SLOT_SKILL_9].iEquipSlotIndex == -1)
			{
				return false;
			}
			int iEquipSlotIndex = this.m_stEquipActiveSkillInfo[slot - SkillSlotType.SLOT_SKILL_9].iEquipSlotIndex;
			ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint)this.m_equipInfos[iEquipSlotIndex].m_equipID);
			return dataByKey != null && (long)uiSkillId == (long)((ulong)dataByKey.dwActiveSkillID);
		}
	}
}
