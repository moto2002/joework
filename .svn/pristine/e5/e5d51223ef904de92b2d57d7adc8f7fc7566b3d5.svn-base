using Assets.Scripts.Common;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.DataCenter;
using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class KillNotifyUT
	{
		public static string GetSoundEvent(KillDetailInfoType Type, bool bSrcAllies, bool bSelfKillORKilled, ActorTypeDef actorType)
		{
			string text = (!bSrcAllies) ? "Enemy_" : "Self_";
			if (Type == KillDetailInfoType.Info_Type_Soldier_Boosted)
			{
				return string.Empty;
			}
			if (actorType != ActorTypeDef.Actor_Type_Monster && actorType != ActorTypeDef.Actor_Type_Organ)
			{
				switch (Type)
				{
				case KillDetailInfoType.Info_Type_First_Kill:
					return "First_Blood";
				case KillDetailInfoType.Info_Type_Kill:
					if (bSrcAllies)
					{
						if (bSelfKillORKilled)
						{
							text = "Self_YouKill";
						}
						else
						{
							text = "Self_OneKill";
						}
					}
					else if (bSelfKillORKilled)
					{
						text = "Self_OneDie";
					}
					else
					{
						text = "Self_TeamDie";
					}
					return text;
				case KillDetailInfoType.Info_Type_DoubleKill:
					text += "DoubleKill";
					return text;
				case KillDetailInfoType.Info_Type_TripleKill:
					text += "TripleKill";
					return text;
				case KillDetailInfoType.Info_Type_QuataryKill:
					text += "QuadraKill";
					return text;
				case KillDetailInfoType.Info_Type_PentaKill:
					text += "PentaKill";
					return text;
				case (KillDetailInfoType)7:
				case (KillDetailInfoType)8:
				case (KillDetailInfoType)9:
				case (KillDetailInfoType)10:
				case (KillDetailInfoType)16:
				case (KillDetailInfoType)17:
				case (KillDetailInfoType)18:
				case (KillDetailInfoType)19:
				case (KillDetailInfoType)20:
					IL_AD:
					switch (Type)
					{
					case KillDetailInfoType.Info_Type_Game_Start_Wel:
						text = "Play_5V5_sys_1_01";
						return text;
					case KillDetailInfoType.Info_Type_Soldier_Activate_Countdown3:
						text = "Play_5V5_sys_2";
						return text;
					case KillDetailInfoType.Info_Type_Soldier_Activate_Countdown5:
						text = "Play_5V5_sys_3";
						return text;
					case KillDetailInfoType.Info_Type_Soldier_Activate:
						text = "Play_5V5_war_1";
						return text;
					default:
						switch (Type)
						{
						case KillDetailInfoType.Info_Type_Kill_3V3_Dragon:
							text += "BaoJunSkill";
							return text;
						case KillDetailInfoType.Info_Type_Kill_5V5_SmallDragon:
							text += "BaoJunSkill";
							return text;
						case KillDetailInfoType.Info_Type_Kill_5V5_BigDragon:
							text += "BaoJunSkill";
							return text;
						default:
							if (Type == KillDetailInfoType.Info_Type_AllDead)
							{
								text = "Common_Ace";
								return text;
							}
							if (Type != KillDetailInfoType.Info_Type_StopMultiKill)
							{
								text = string.Empty;
								return text;
							}
							return "ShutDown";
						}
						break;
					}
					break;
				case KillDetailInfoType.Info_Type_MonsterKill:
					text += "KillingSpree1";
					return text;
				case KillDetailInfoType.Info_Type_DominateBattle:
					text += "KillingSpree2";
					return text;
				case KillDetailInfoType.Info_Type_Legendary:
					text += "KillingSpree3";
					return text;
				case KillDetailInfoType.Info_Type_TotalAnnihilat:
					text += "KillingSpree4";
					return text;
				case KillDetailInfoType.Info_Type_Odyssey:
					text += "KillingSpree5";
					return text;
				case KillDetailInfoType.Info_Type_DestroyTower:
					text += "TowerDie";
					return text;
				}
				goto IL_AD;
			}
			if (Type == KillDetailInfoType.Info_Type_DestroyTower)
			{
				return text + "TowerDie";
			}
			return "Executed";
		}

		public static List<string> GetAllAnimations()
		{
			List<string> list = new List<string>();
			Array values = Enum.GetValues(typeof(KillDetailInfoType));
			for (int i = 0; i < values.get_Length(); i++)
			{
				KillDetailInfoType type = (KillDetailInfoType)((int)values.GetValue(i));
				string animation = KillNotifyUT.GetAnimation(type, true);
				if (!string.IsNullOrEmpty(animation))
				{
					list.Add(animation);
				}
				animation = KillNotifyUT.GetAnimation(type, false);
				if (!string.IsNullOrEmpty(animation))
				{
					list.Add(animation);
				}
			}
			return list;
		}

		public static string GetAnimation(KillDetailInfoType Type, bool bSrc)
		{
			string text = (!bSrc) ? "_B" : "_A";
			string text2 = string.Empty;
			bool flag = false;
			switch (Type)
			{
			case KillDetailInfoType.Info_Type_First_Kill:
				text2 = "FirstBlood";
				goto IL_23A;
			case KillDetailInfoType.Info_Type_Kill:
				text2 = "NormalKill";
				goto IL_23A;
			case KillDetailInfoType.Info_Type_DoubleKill:
				text2 = "DoubleKill";
				goto IL_23A;
			case KillDetailInfoType.Info_Type_TripleKill:
				text2 = "TrebleKill";
				goto IL_23A;
			case KillDetailInfoType.Info_Type_QuataryKill:
				text2 = "QuataryKill";
				goto IL_23A;
			case KillDetailInfoType.Info_Type_PentaKill:
				text2 = "PentaKill";
				goto IL_23A;
			case (KillDetailInfoType)7:
			case (KillDetailInfoType)8:
			case (KillDetailInfoType)9:
			case (KillDetailInfoType)10:
			case (KillDetailInfoType)16:
			case (KillDetailInfoType)17:
			case (KillDetailInfoType)18:
			case (KillDetailInfoType)19:
			case (KillDetailInfoType)20:
				IL_7C:
				switch (Type)
				{
				case KillDetailInfoType.Info_Type_Cannon_Spawned:
					text2 = "GongChengCheJiaRu";
					goto IL_23A;
				case KillDetailInfoType.Info_Type_Soldier_Boosted:
					text2 = "XiaoBingZengQiang";
					goto IL_23A;
				case KillDetailInfoType.Info_Type_Game_Start_Wel:
					return "Welcome";
				case KillDetailInfoType.Info_Type_Soldier_Activate_Countdown3:
					return "ThreeSecond";
				case KillDetailInfoType.Info_Type_Soldier_Activate_Countdown5:
					return "FiveSecond";
				case KillDetailInfoType.Info_Type_Soldier_Activate:
					return "Battle";
				default:
					switch (Type)
					{
					case KillDetailInfoType.Info_Type_Kill_3V3_Dragon:
						text2 = "DragonKill";
						goto IL_23A;
					case KillDetailInfoType.Info_Type_Kill_5V5_SmallDragon:
						text2 = "KillJuLong";
						goto IL_23A;
					case KillDetailInfoType.Info_Type_Kill_5V5_BigDragon:
						text2 = "KillZhuZai";
						goto IL_23A;
					default:
						switch (Type)
						{
						case KillDetailInfoType.Info_Type_RunningMan:
							text2 = "ExitGame";
							goto IL_23A;
						case KillDetailInfoType.Info_Type_Reconnect:
							text2 = "ChongLian";
							goto IL_23A;
						case KillDetailInfoType.Info_Type_Disconnect:
							text2 = "ExitGame";
							goto IL_23A;
						default:
							switch (Type)
							{
							case KillDetailInfoType.Info_Type_FireHole_first:
								text2 = "YouShi";
								goto IL_23A;
							case KillDetailInfoType.Info_Type_FireHole_second:
								text2 = "JiJiangShengLi";
								goto IL_23A;
							case KillDetailInfoType.Info_Type_FireHole_third:
								text2 = "ShengLi";
								goto IL_23A;
							default:
								if (Type == KillDetailInfoType.Info_Type_AllDead)
								{
									text2 = "Ace";
									goto IL_23A;
								}
								if (Type != KillDetailInfoType.Info_Type_StopMultiKill)
								{
									flag = true;
									goto IL_23A;
								}
								text2 = "EndKill";
								goto IL_23A;
							}
							break;
						}
						break;
					}
					break;
				}
				break;
			case KillDetailInfoType.Info_Type_MonsterKill:
				text2 = "DaShaTeSha";
				goto IL_23A;
			case KillDetailInfoType.Info_Type_DominateBattle:
				text2 = "ShaRenRuMa";
				goto IL_23A;
			case KillDetailInfoType.Info_Type_Legendary:
				text2 = "WuRenNenDang";
				goto IL_23A;
			case KillDetailInfoType.Info_Type_TotalAnnihilat:
				text2 = "HengSaoQianJun";
				goto IL_23A;
			case KillDetailInfoType.Info_Type_Odyssey:
				text2 = "TianXiaWuShuang";
				goto IL_23A;
			case KillDetailInfoType.Info_Type_DestroyTower:
				text2 = "BreakTower";
				goto IL_23A;
			}
			goto IL_7C;
			IL_23A:
			if (flag)
			{
				return string.Empty;
			}
			return text2 + text;
		}

		public static void SetImageSprite(Image img, string spt)
		{
			if (img != null && !string.IsNullOrEmpty(spt))
			{
				img.SetSprite(spt, Singleton<CBattleSystem>.GetInstance().FormScript, true, false, false, false);
			}
		}

		public static KillInfo Convert_DetailInfo_KillInfo(KillDetailInfo detail)
		{
			KillDetailInfoType killDetailInfoType = KillDetailInfoType.Info_Type_None;
			PoolObjHandle<ActorRoot> killer = detail.Killer;
			PoolObjHandle<ActorRoot> victim = detail.Victim;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (killer)
			{
				flag = (killer.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Organ);
				flag2 = (killer.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Monster);
				flag3 = (killer.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero);
			}
			KillInfo result = default(KillInfo);
			result.KillerImgSrc = (result.VictimImgSrc = string.Empty);
			result.MsgType = detail.Type;
			result.bPlayerSelf_KillOrKilled = detail.bPlayerSelf_KillOrKilled;
			result.actorType = ((!killer) ? ActorTypeDef.Invalid : killer.get_handle().TheActorMeta.ActorType);
			result.bSrcAllies = detail.bSelfCamp;
			result.assistImgSrc = new string[4];
			if (flag2)
			{
				result.KillerImgSrc = KillNotify.monster_icon;
			}
			if (flag)
			{
				result.KillerImgSrc = KillNotify.building_icon;
			}
			if (flag3)
			{
				result.KillerImgSrc = KillNotifyUT.GetHero_Icon(detail.Killer, false);
			}
			if (killer)
			{
				if (killer.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Organ)
				{
					result.KillerImgSrc = KillNotify.building_icon;
				}
				if (killer.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Monster)
				{
					MonsterWrapper monsterWrapper = killer.get_handle().AsMonster();
					if (killer.get_handle().TheActorMeta.ConfigId == Singleton<BattleLogic>.get_instance().DragonId)
					{
						result.KillerImgSrc = KillNotify.dragon_icon;
					}
					else if (monsterWrapper.cfgInfo != null && monsterWrapper.cfgInfo.bMonsterType == 2)
					{
						result.KillerImgSrc = KillNotify.yeguai_icon;
					}
					else
					{
						result.KillerImgSrc = KillNotify.monster_icon;
					}
				}
				if (killer.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero)
				{
					result.KillerImgSrc = KillNotifyUT.GetHero_Icon(killer, false);
				}
			}
			if (victim)
			{
				if (victim.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero)
				{
					result.VictimImgSrc = KillNotifyUT.GetHero_Icon(victim, false);
				}
				if (victim.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Organ)
				{
					result.VictimImgSrc = KillNotify.building_icon;
				}
				if (victim.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Monster && victim.get_handle().TheActorMeta.ConfigId == Singleton<BattleLogic>.get_instance().DragonId)
				{
					result.VictimImgSrc = KillNotify.dragon_icon;
				}
			}
			if (detail.assistList != null && result.assistImgSrc != null)
			{
				for (int i = 0; i < detail.assistList.get_Count(); i++)
				{
					uint num = detail.assistList.get_Item(i);
					for (int j = 0; j < Singleton<GameObjMgr>.get_instance().HeroActors.get_Count(); j++)
					{
						if (num == Singleton<GameObjMgr>.get_instance().HeroActors.get_Item(j).get_handle().ObjID)
						{
							result.assistImgSrc[i] = KillNotifyUT.GetHero_Icon(Singleton<GameObjMgr>.get_instance().HeroActors.get_Item(j), false);
						}
					}
				}
			}
			if (detail.Type == KillDetailInfoType.Info_Type_Soldier_Boosted)
			{
				result.KillerImgSrc = ((!detail.bSelfCamp) ? KillNotify.red_soldier_icon : KillNotify.blue_soldier_icon);
				return result;
			}
			if (detail.Type == KillDetailInfoType.Info_Type_Reconnect || detail.Type == KillDetailInfoType.Info_Type_RunningMan)
			{
				result.VictimImgSrc = string.Empty;
				return result;
			}
			if (detail.HeroMultiKillType != killDetailInfoType)
			{
				result.MsgType = detail.HeroMultiKillType;
				return result;
			}
			if (detail.Type == KillDetailInfoType.Info_Type_StopMultiKill)
			{
				return result;
			}
			if (detail.Type == KillDetailInfoType.Info_Type_First_Kill)
			{
				return result;
			}
			if (detail.Type == KillDetailInfoType.Info_Type_DestroyTower)
			{
				return result;
			}
			if (detail.Type == KillDetailInfoType.Info_Type_Kill_3V3_Dragon || detail.Type == KillDetailInfoType.Info_Type_Kill_5V5_SmallDragon || detail.Type == KillDetailInfoType.Info_Type_Kill_5V5_BigDragon)
			{
				if (flag2)
				{
					result.KillerImgSrc = KillNotify.monster_icon;
				}
				if (flag)
				{
					result.KillerImgSrc = KillNotify.building_icon;
				}
				if (flag3)
				{
					result.KillerImgSrc = KillNotifyUT.GetHero_Icon(detail.Killer, false);
				}
				result.bSrcAllies = detail.bSelfCamp;
				return result;
			}
			if (detail.bAllDead)
			{
				result.MsgType = KillDetailInfoType.Info_Type_AllDead;
				return result;
			}
			if (detail.HeroContiKillType != killDetailInfoType)
			{
				result.MsgType = detail.HeroContiKillType;
				return result;
			}
			if (detail.Type == KillDetailInfoType.Info_Type_Kill)
			{
				return result;
			}
			return result;
		}

		public static string GetHero_Icon(ref ActorMeta actorMeta, bool bSmall = false)
		{
			string result = string.Empty;
			IGameActorDataProvider actorDataProvider = Singleton<ActorDataCenter>.get_instance().GetActorDataProvider(GameActorDataProviderType.ServerDataProvider);
			ActorServerData actorServerData = default(ActorServerData);
			if (actorDataProvider != null && actorDataProvider.GetActorServerData(ref actorMeta, ref actorServerData))
			{
				result = KillNotifyUT.GetHero_Icon((uint)actorServerData.TheActorMeta.ConfigId, 0u, bSmall);
			}
			return result;
		}

		public static string GetHero_Icon(PoolObjHandle<ActorRoot> actor, bool bSmall = false)
		{
			if (!actor)
			{
				return string.Empty;
			}
			return KillNotifyUT.GetHero_Icon(ref actor.get_handle().TheActorMeta, bSmall);
		}

		public static string GetHero_Icon(uint ConfigID, uint SkinID = 0u, bool bSmall = false)
		{
			string heroSkinPic = CSkinInfo.GetHeroSkinPic(ConfigID, SkinID);
			return string.Format("{0}{1}", (!bSmall) ? CUIUtility.s_Sprite_Dynamic_BustCircle_Dir : CUIUtility.s_Sprite_Dynamic_BustCircleSmall_Dir, heroSkinPic);
		}

		public static string GetHero_Icon(ActorRoot actor, bool bSmall)
		{
			string result = string.Empty;
			if (actor != null)
			{
				result = KillNotifyUT.GetHero_Icon(ref actor.TheActorMeta, bSmall);
			}
			return result;
		}
	}
}
