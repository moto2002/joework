using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	internal class CSingleGameSettleView
	{
		public static void ShowBurnWinLose(CUIFormScript form, bool bWin)
		{
			if (bWin)
			{
				form.gameObject.transform.Find("Win").gameObject.CustomSetActive(true);
			}
			else
			{
				form.gameObject.transform.Find("Lose").gameObject.CustomSetActive(true);
			}
		}

		public static void SetBurnSettleData(CUIFormScript form, ref COMDT_SETTLE_RESULT_DETAIL settleData)
		{
			CPlayerKDAStat playerKDAStat = Singleton<BattleLogic>.GetInstance().battleStat.m_playerKDAStat;
			CSingleGameSettleView.SetWin(form.gameObject, settleData.stGameInfo.bGameResult == 1);
			int num = 1;
			int num2 = 1;
			int num3 = 0;
			int num4 = 0;
			string text = string.Empty;
			int num5 = 1;
			COM_PLAYERCAMP cOM_PLAYERCAMP = 1;
			string text2 = string.Empty;
			int num6 = 1;
			COM_PLAYERCAMP cOM_PLAYERCAMP2 = 1;
			DictionaryView<uint, PlayerKDA>.Enumerator enumerator = playerKDAStat.GetEnumerator();
			while (enumerator.MoveNext())
			{
				bool flag;
				if (settleData.stGameInfo.bGameResult == 1)
				{
					KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
					flag = current.get_Value().IsHost;
				}
				else
				{
					KeyValuePair<uint, PlayerKDA> current2 = enumerator.get_Current();
					flag = !current2.get_Value().IsHost;
				}
				if (flag)
				{
					KeyValuePair<uint, PlayerKDA> current3 = enumerator.get_Current();
					ListView<HeroKDA>.Enumerator enumerator2 = current3.get_Value().GetEnumerator();
					while (enumerator2.MoveNext() && num <= 3)
					{
						GameObject gameObject = form.gameObject.transform.Find("Panel/Left_Player" + num).gameObject;
						GameObject arg_122_1 = gameObject;
						KeyValuePair<uint, PlayerKDA> current4 = enumerator.get_Current();
						CSingleGameSettleView.SetPlayerStat(form, arg_122_1, current4.get_Value(), enumerator2.get_Current());
						num++;
					}
					int arg_154_0 = num3;
					KeyValuePair<uint, PlayerKDA> current5 = enumerator.get_Current();
					num3 = arg_154_0 + current5.get_Value().numKill;
					KeyValuePair<uint, PlayerKDA> current6 = enumerator.get_Current();
					text = current6.get_Value().PlayerName;
					KeyValuePair<uint, PlayerKDA> current7 = enumerator.get_Current();
					cOM_PLAYERCAMP = current7.get_Value().PlayerCamp;
					KeyValuePair<uint, PlayerKDA> current8 = enumerator.get_Current();
					num5 = current8.get_Value().PlayerLv;
				}
				else
				{
					KeyValuePair<uint, PlayerKDA> current9 = enumerator.get_Current();
					ListView<HeroKDA>.Enumerator enumerator3 = current9.get_Value().GetEnumerator();
					while (enumerator3.MoveNext() && num2 <= 3)
					{
						GameObject gameObject2 = form.gameObject.transform.Find("Panel/Right_Player" + num2).gameObject;
						GameObject arg_1FD_1 = gameObject2;
						KeyValuePair<uint, PlayerKDA> current10 = enumerator.get_Current();
						CSingleGameSettleView.SetPlayerStat(form, arg_1FD_1, current10.get_Value(), enumerator3.get_Current());
						num2++;
					}
					int arg_230_0 = num4;
					KeyValuePair<uint, PlayerKDA> current11 = enumerator.get_Current();
					num4 = arg_230_0 + current11.get_Value().numKill;
					KeyValuePair<uint, PlayerKDA> current12 = enumerator.get_Current();
					text2 = current12.get_Value().PlayerName;
					KeyValuePair<uint, PlayerKDA> current13 = enumerator.get_Current();
					cOM_PLAYERCAMP2 = current13.get_Value().PlayerCamp;
					KeyValuePair<uint, PlayerKDA> current14 = enumerator.get_Current();
					num6 = current14.get_Value().PlayerLv;
				}
			}
			for (int i = num; i <= 3; i++)
			{
				GameObject gameObject3 = form.gameObject.transform.Find("Panel/Left_Player" + i).gameObject;
				gameObject3.CustomSetActive(false);
			}
			for (int j = num2; j <= 3; j++)
			{
				GameObject gameObject4 = form.gameObject.transform.Find("Panel/Right_Player" + j).gameObject;
				gameObject4.CustomSetActive(false);
			}
			Text component = form.gameObject.transform.Find("Panel/PanelABg/Image/ImageLeft/Txt_LeftPlayerName").gameObject.GetComponent<Text>();
			component.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = string.Format("Lv.{0}", num5);
			component.text = text;
			component.color = ((cOM_PLAYERCAMP != 1) ? new Color(0.545f, 0f, 0f, 1f) : new Color(0.031f, 0.961f, 0f, 1f));
			component = form.gameObject.transform.Find("Panel/PanelABg/Image/ImageRight/Txt_RightPlayerName").gameObject.GetComponent<Text>();
			component.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = string.Format("Lv.{0}", num6);
			component.text = text2;
			component.color = ((cOM_PLAYERCAMP2 != 1) ? new Color(0.545f, 0f, 0f, 1f) : new Color(0.031f, 0.961f, 0f, 1f));
		}

		private static void SetWin(GameObject root, bool bWin)
		{
			Utility.FindChild(root, "Panel/PanelABg/Image/WinOrLose/win").CustomSetActive(bWin);
			Utility.FindChild(root, "Panel/PanelABg/Image/WinOrLose/lose").CustomSetActive(!bWin);
		}

		private static void SetPlayerStat(CUIFormScript formScript, GameObject item, PlayerKDA playerKDA, HeroKDA kda)
		{
			Text componetInChild = Utility.GetComponetInChild<Text>(item, "Txt_PlayerName");
			componetInChild.text = playerKDA.PlayerName;
			componetInChild.color = ((playerKDA.PlayerCamp != 1) ? new Color(0.545f, 0f, 0f, 1f) : new Color(0.031f, 0.961f, 0f, 1f));
			ResHeroCfgInfo dataByKey = GameDataMgr.heroDatabin.GetDataByKey((uint)kda.HeroId);
			DebugHelper.Assert(dataByKey != null);
			item.transform.Find("Txt_HeroName").gameObject.GetComponent<Text>().text = StringHelper.UTF8BytesToString(ref dataByKey.szName);
			string text = (kda.numKill >= 10) ? kda.numKill.ToString() : string.Format(" {0} ", kda.numKill.ToString());
			string text2 = (kda.numDead >= 10) ? kda.numDead.ToString() : string.Format(" {0} ", kda.numDead.ToString());
			item.transform.Find("Txt_KDA").gameObject.GetComponent<Text>().text = string.Format("{0} / {1}", text, text2);
			item.transform.Find("Txt_Hurt").gameObject.GetComponent<Text>().text = kda.hurtToEnemy.ToString();
			item.transform.Find("Txt_HurtTaken").gameObject.GetComponent<Text>().text = kda.hurtTakenByEnemy.ToString();
			item.transform.Find("Txt_Heal").gameObject.GetComponent<Text>().text = kda.hurtToHero.ToString();
			Image component = item.transform.Find("KillerImg").gameObject.GetComponent<Image>();
			component.SetSprite(string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_Icon_Dir, CSkinInfo.GetHeroSkinPic((uint)kda.HeroId, 0u)), formScript, true, false, false, false);
			item.CustomSetActive(true);
		}
	}
}
