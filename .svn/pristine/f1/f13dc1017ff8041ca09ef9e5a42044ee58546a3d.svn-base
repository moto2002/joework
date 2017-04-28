using Assets.Scripts.Framework;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using UnityEngine;

namespace Assets.Scripts.GameSystem
{
	public class CUseableManager
	{
		public static CUseable CreateUseable(COM_ITEM_TYPE useableType, ulong objID, uint baseID, int bCount = 0, int addTime = 0)
		{
			CUseable result = null;
			if (useableType == 2)
			{
				result = new CItem(objID, baseID, bCount, addTime);
			}
			else if (useableType == 3)
			{
				result = new CEquip(objID, baseID, bCount, addTime);
			}
			else if (useableType == 4)
			{
				result = new CHeroItem(objID, baseID, bCount, addTime);
			}
			else if (useableType == 5)
			{
				result = new CSymbolItem(objID, baseID, bCount, addTime);
			}
			else if (useableType == 7)
			{
				result = new CHeroSkin(objID, baseID, bCount, addTime);
			}
			else if (useableType == 8)
			{
				result = new CHeadImg(objID, baseID, 0);
			}
			return result;
		}

		public static CUseable CreateUseable(COM_ITEM_TYPE useableType, uint baseID, int bCount = 0)
		{
			CUseable result = null;
			if (useableType == 2)
			{
				result = new CItem(0uL, baseID, bCount, 0);
			}
			else if (useableType == 3)
			{
				result = new CEquip(0uL, baseID, bCount, 0);
			}
			else if (useableType == 4)
			{
				result = new CHeroItem(0uL, baseID, bCount, 0);
			}
			else if (useableType == 5)
			{
				result = new CSymbolItem(0uL, baseID, bCount, 0);
			}
			else if (useableType == 7)
			{
				result = new CHeroSkin(0uL, baseID, bCount, 0);
			}
			else if (useableType == 8)
			{
				result = new CHeadImg(0uL, baseID, 0);
			}
			return result;
		}

		public static CUseable CreateVirtualUseable(enVirtualItemType vType, int bCount)
		{
			return new CVirtualItem(vType, bCount);
		}

		public static CUseable CreateExpUseable(COM_ITEM_TYPE useableType, ulong objID, uint expDays, uint baseID, int bCount = 0, int addTime = 0)
		{
			CUseable result = null;
			if (useableType == 4)
			{
				result = new CExpHeroItem(0uL, baseID, expDays, bCount, 0);
			}
			else if (useableType == 7)
			{
				result = new CExpHeroSkin(0uL, baseID, expDays, bCount, 0);
			}
			return result;
		}

		public static CUseable CreateCoinUseable(RES_SHOPBUY_COINTYPE coinType, int count)
		{
			enVirtualItemType bType = enVirtualItemType.enNull;
			switch (coinType)
			{
			case 2:
				bType = enVirtualItemType.enDianQuan;
				goto IL_91;
			case 4:
				bType = enVirtualItemType.enGoldCoin;
				goto IL_91;
			case 5:
				bType = enVirtualItemType.enHeart;
				goto IL_91;
			case 6:
				bType = enVirtualItemType.enArenaCoin;
				goto IL_91;
			case 7:
				bType = enVirtualItemType.enSkinCoin;
				goto IL_91;
			case 9:
				bType = enVirtualItemType.enSymbolCoin;
				goto IL_91;
			case 10:
			case 11:
				bType = enVirtualItemType.enDiamond;
				goto IL_91;
			}
			Debug.LogError("CoinType:" + coinType.ToString() + " is not supported!");
			IL_91:
			return new CVirtualItem(bType, count);
		}

		public static CUseable CreateUsableByServerType(COM_REWARDS_TYPE type, int cnt, uint baseId)
		{
			CUseable result = null;
			switch (type)
			{
			case 0:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enGoldCoin, cnt);
				break;
			case 1:
				result = CUseableManager.CreateUseable(2, 0uL, baseId, cnt, 0);
				break;
			case 2:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enExp, cnt);
				break;
			case 3:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enDianQuan, cnt);
				break;
			case 4:
				result = CUseableManager.CreateUseable(3, 0uL, baseId, cnt, 0);
				break;
			case 5:
				result = CUseableManager.CreateUseable(4, baseId, cnt);
				break;
			case 6:
				result = CUseableManager.CreateUseable(5, 0uL, baseId, cnt, 0);
				break;
			case 7:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enBurningCoin, cnt);
				break;
			case 8:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enArenaCoin, cnt);
				break;
			case 9:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enHeart, cnt);
				break;
			case 10:
				result = CUseableManager.CreateUseable(7, 0uL, baseId, cnt, 0);
				break;
			case 11:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enGoldCoin, cnt);
				break;
			case 12:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enExpPool, cnt);
				break;
			case 13:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enSkinCoin, cnt);
				break;
			case 14:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enSymbolCoin, cnt);
				break;
			case 16:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enDiamond, cnt);
				break;
			case 17:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enHuoyueDu, cnt);
				break;
			case 18:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enMatchPersonalPoint, cnt);
				break;
			case 19:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enMatchTeamPoint, cnt);
				break;
			case 20:
				result = CUseableManager.CreateUseable(8, 0uL, baseId, cnt, 0);
				break;
			case 21:
				CUseableManager.CreateVirtualUseable(enVirtualItemType.enAchievementPoint, cnt);
				break;
			case 22:
				result = CUseableManager.CreateVirtualUseable(enVirtualItemType.enMentorPoint, cnt);
				break;
			}
			return result;
		}

		public static CUseable CreateUsableByServerType(RES_REWARDS_TYPE type, int cnt, uint baseId)
		{
			COM_REWARDS_TYPE type2;
			CUseableManager.ResRewardTypeToComRewardType(type, out type2);
			return CUseableManager.CreateUsableByServerType(type2, cnt, baseId);
		}

		public static CUseable CreateUsableByRandowReward(RES_RANDOM_REWARD_TYPE type, int cnt, uint baseId)
		{
			COM_REWARDS_TYPE type2;
			CUseableManager.RandomRewardTypeToComRewardType(type, out type2);
			return CUseableManager.CreateUsableByServerType(type2, cnt, baseId);
		}

		public static ListView<CUseable> CreateUsableListByRandowReward(RES_RANDOM_REWARD_TYPE type, int cnt, uint baseId)
		{
			ListView<CUseable> listView = new ListView<CUseable>();
			ResRandomRewardStore dataByKey;
			if (type != 3)
			{
				CUseable cUseable = CUseableManager.CreateUsableByRandowReward(type, cnt, baseId);
				if (cUseable != null)
				{
					listView.Add(cUseable);
				}
			}
			else if ((dataByKey = GameDataMgr.randomRewardDB.GetDataByKey(baseId)) != null)
			{
				for (int i = 0; i < dataByKey.astRewardDetail.Length; i++)
				{
					if (dataByKey.astRewardDetail[i].bItemType == 0 || dataByKey.astRewardDetail[i].bItemType >= 18)
					{
						break;
					}
					listView.AddRange(CUseableManager.CreateUsableListByRandowReward(dataByKey.astRewardDetail[i].bItemType, (int)dataByKey.astRewardDetail[i].dwLowCnt, dataByKey.astRewardDetail[i].dwItemID));
				}
			}
			return listView;
		}

		public static void ResRewardTypeToComRewardType(RES_REWARDS_TYPE rType, out COM_REWARDS_TYPE cType)
		{
			switch (rType)
			{
			case 2:
				cType = 1;
				return;
			case 3:
				cType = 2;
				return;
			case 4:
				cType = 3;
				return;
			case 5:
				cType = 4;
				return;
			case 6:
				cType = 6;
				return;
			case 8:
				cType = 8;
				return;
			case 9:
				cType = 11;
				return;
			case 10:
				cType = 13;
				return;
			case 11:
				cType = 12;
				return;
			case 13:
				cType = 10;
				return;
			case 14:
				cType = 14;
				return;
			case 16:
				cType = 16;
				return;
			case 17:
				cType = 17;
				return;
			case 20:
				cType = 20;
				return;
			case 22:
				cType = 5;
				return;
			case 23:
				cType = 22;
				return;
			}
			cType = 23;
		}

		public static void RandomRewardTypeToComRewardType(RES_RANDOM_REWARD_TYPE rType, out COM_REWARDS_TYPE cType)
		{
			switch (rType)
			{
			case 1:
				cType = 1;
				return;
			case 2:
				cType = 4;
				return;
			case 4:
				cType = 5;
				return;
			case 5:
				cType = 6;
				return;
			case 6:
				cType = 9;
				return;
			case 7:
				cType = 0;
				return;
			case 8:
				cType = 3;
				return;
			case 9:
				cType = 7;
				return;
			case 10:
				cType = 8;
				return;
			case 11:
				cType = 10;
				return;
			case 12:
				cType = 13;
				return;
			case 13:
				cType = 12;
				return;
			case 14:
				cType = 14;
				return;
			case 15:
				cType = 16;
				return;
			case 16:
				cType = 20;
				return;
			}
			cType = 23;
		}

		public static CUseable GetUseableByRewardInfo(ResRandomRewardStore inRewardInfo)
		{
			if (inRewardInfo != null)
			{
				COM_REWARDS_TYPE type;
				CUseableManager.RandomRewardTypeToComRewardType(inRewardInfo.astRewardDetail[0].bItemType, out type);
				int dwLowCnt = (int)inRewardInfo.astRewardDetail[0].dwLowCnt;
				uint dwItemID = inRewardInfo.astRewardDetail[0].dwItemID;
				return CUseableManager.CreateUsableByServerType(type, dwLowCnt, dwItemID);
			}
			return null;
		}

		public static ListView<CUseable> GetUseableListFromReward(COMDT_REWARD_DETAIL reward)
		{
			ListView<CUseable> listView = new ListView<CUseable>();
			int i = 0;
			while (i < (int)reward.bNum)
			{
				switch (reward.astRewardDetail[i].bType)
				{
				case 0:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwCoin, 0u);
					listView.Add(cUseable);
					break;
				}
				case 1:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.get_stItem().dwCnt, reward.astRewardDetail[i].stRewardInfo.get_stItem().dwItemID);
					if (cUseable != null)
					{
						if (reward.astRewardDetail[i].bFromType == 1)
						{
							cUseable.ExtraFromType = (int)reward.astRewardDetail[i].bFromType;
							cUseable.ExtraFromData = (int)reward.astRewardDetail[i].stFromInfo.get_stHeroInfo().dwHeroID;
						}
						else if (reward.astRewardDetail[i].bFromType == 2)
						{
							cUseable.ExtraFromType = (int)reward.astRewardDetail[i].bFromType;
							cUseable.ExtraFromData = (int)reward.astRewardDetail[i].stFromInfo.get_stSkinInfo().dwSkinID;
						}
						listView.Add(cUseable);
					}
					break;
				}
				case 3:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwCoupons, 0u);
					listView.Add(cUseable);
					break;
				}
				case 4:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.get_stEquip().dwCnt, reward.astRewardDetail[i].stRewardInfo.get_stEquip().dwEquipID);
					listView.Add(cUseable);
					break;
				}
				case 5:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.get_stHero().dwCnt, reward.astRewardDetail[i].stRewardInfo.get_stHero().dwHeroID);
					listView.Add(cUseable);
					break;
				}
				case 6:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.get_stSymbol().dwCnt, reward.astRewardDetail[i].stRewardInfo.get_stSymbol().dwSymbolID);
					listView.Add(cUseable);
					break;
				}
				case 7:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwBurningCoin, 0u);
					listView.Add(cUseable);
					break;
				}
				case 8:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwArenaCoin, 0u);
					listView.Add(cUseable);
					break;
				}
				case 9:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwAP, 0u);
					listView.Add(cUseable);
					break;
				}
				case 10:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.get_stSkin().dwCnt, reward.astRewardDetail[i].stRewardInfo.get_stSkin().dwSkinID);
					listView.Add(cUseable);
					break;
				}
				case 11:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwPvpCoin, 0u);
					listView.Add(cUseable);
					break;
				}
				case 12:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwHeroPoolExp, 0u);
					listView.Add(cUseable);
					break;
				}
				case 13:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwSkinCoin, 0u);
					listView.Add(cUseable);
					break;
				}
				case 14:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwSymbolCoin, 0u);
					if (cUseable != null)
					{
						listView.Add(cUseable);
					}
					break;
				}
				case 16:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwDiamond, 0u);
					listView.Add(cUseable);
					break;
				}
				case 17:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwHuoYueDu, 0u);
					if (cUseable != null)
					{
						listView.Add(cUseable);
					}
					break;
				}
				case 18:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwMatchPointPer, 0u);
					listView.Add(cUseable);
					break;
				}
				case 19:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwMatchPointGuild, 0u);
					listView.Add(cUseable);
					break;
				}
				case 20:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, 1, reward.astRewardDetail[i].stRewardInfo.get_stHeadImage().dwHeadImgID);
					if (cUseable != null)
					{
						listView.Add(cUseable);
					}
					break;
				}
				case 21:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwAchieve, 0u);
					if (cUseable != null)
					{
						listView.Add(cUseable);
					}
					break;
				}
				case 22:
				{
					CUseable cUseable = CUseableManager.CreateUsableByServerType(reward.astRewardDetail[i].bType, (int)reward.astRewardDetail[i].stRewardInfo.dwMasterPoint, 0u);
					if (cUseable != null)
					{
						listView.Add(cUseable);
					}
					break;
				}
				}
				IL_5E1:
				i++;
				continue;
				goto IL_5E1;
			}
			return listView;
		}

		public static ListView<CUseable> GetUseableListFromItemList(COMDT_REWARD_ITEMLIST itemList)
		{
			ListView<CUseable> listView = new ListView<CUseable>();
			for (int i = 0; i < (int)itemList.wRewardCnt; i++)
			{
				ushort wItemType = itemList.astRewardList[i].wItemType;
				ushort wItemCnt = itemList.astRewardList[i].wItemCnt;
				uint dwItemID = itemList.astRewardList[i].dwItemID;
				CUseable cUseable = CUseableManager.CreateUseable(wItemType, 0uL, dwItemID, (int)wItemCnt, 0);
				if (cUseable != null)
				{
					byte bFromType = itemList.astRewardList[i].bFromType;
					if (bFromType != 1)
					{
						if (bFromType == 2)
						{
							cUseable.ExtraFromType = (int)itemList.astRewardList[i].bFromType;
							cUseable.ExtraFromData = (int)itemList.astRewardList[i].stFromInfo.get_stSkinInfo().dwSkinID;
						}
					}
					else
					{
						cUseable.ExtraFromType = (int)itemList.astRewardList[i].bFromType;
						cUseable.ExtraFromData = (int)itemList.astRewardList[i].stFromInfo.get_stHeroInfo().dwHeroID;
					}
					listView.Add(cUseable);
				}
			}
			return listView;
		}

		public static void ShowUseableItem(CUseable item)
		{
			if (item == null)
			{
				return;
			}
			if (item.m_type == 2 || item.MapRewardType == 1)
			{
				if (item.ExtraFromType == 1)
				{
					int extraFromData = item.ExtraFromData;
					CUICommonSystem.ShowNewHeroOrSkin((uint)extraFromData, 0u, enUIEventID.Activity_HeroShow_Back, true, 5, true, null, enFormPriority.Priority1, (uint)item.m_stackCount, 0);
				}
				else if (item.ExtraFromType == 2)
				{
					int extraFromData2 = item.ExtraFromData;
					CUICommonSystem.ShowNewHeroOrSkin(0u, (uint)extraFromData2, enUIEventID.Activity_HeroShow_Back, true, 10, true, null, enFormPriority.Priority1, (uint)item.m_stackCount, 0);
				}
				else
				{
					CUseable[] items = new CUseable[]
					{
						item
					};
					Singleton<CUIManager>.GetInstance().OpenAwardTip(items, Singleton<CTextManager>.GetInstance().GetText("gotAward"), true, enUIEventID.None, false, true, "Form_Award");
				}
			}
			else if (item is CHeroSkin)
			{
				CHeroSkin cHeroSkin = item as CHeroSkin;
				CUICommonSystem.ShowNewHeroOrSkin(cHeroSkin.m_heroId, cHeroSkin.m_skinId, enUIEventID.Activity_HeroShow_Back, true, 10, true, null, enFormPriority.Priority1, 0u, 0);
			}
			else if (item is CHeroItem)
			{
				CUICommonSystem.ShowNewHeroOrSkin(item.m_baseID, 0u, enUIEventID.Activity_HeroShow_Back, true, 5, true, null, enFormPriority.Priority1, 0u, 0);
			}
			else
			{
				CUseable[] items2 = new CUseable[]
				{
					item
				};
				Singleton<CUIManager>.GetInstance().OpenAwardTip(items2, Singleton<CTextManager>.GetInstance().GetText("gotAward"), true, enUIEventID.None, false, true, "Form_Award");
			}
		}
	}
}
