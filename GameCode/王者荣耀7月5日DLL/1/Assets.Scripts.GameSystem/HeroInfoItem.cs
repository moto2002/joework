using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class HeroInfoItem
	{
		public COM_PLAYERCAMP listCamp;

		public int listIndex;

		private HeroKDA _heroInfo;

		private GameObject _baseInfoItem;

		private GameObject _equipInfoItem;

		private Image _headImg;

		private Text _reviveTxt;

		private Text _levelText;

		private Text _kdaText;

		private Text _moneyText;

		private GameObject _emptyGo;

		private Image[] _equipIcons;

		private GameObject _hostGo;

		public HeroKDA HeroInfo
		{
			get
			{
				return this._heroInfo;
			}
		}

		public HeroInfoItem(COM_PLAYERCAMP _listCamp, int _listIndex, HeroKDA _heroKDA, GameObject baseInfoItem, GameObject equipInfoItem)
		{
			this.listCamp = _listCamp;
			this.listIndex = _listIndex;
			this._heroInfo = _heroKDA;
			this._baseInfoItem = baseInfoItem;
			this._equipInfoItem = equipInfoItem;
			this._baseInfoItem.GetComponent<CUIEventScript>().enabled = true;
			this._headImg = Utility.GetComponetInChild<Image>(baseInfoItem, "Head");
			this._reviveTxt = Utility.GetComponetInChild<Text>(baseInfoItem, "Revive");
			this._levelText = Utility.GetComponetInChild<Text>(baseInfoItem, "Level");
			this._kdaText = Utility.GetComponetInChild<Text>(baseInfoItem, "KDA");
			this._moneyText = Utility.GetComponetInChild<Text>(baseInfoItem, "Money");
			this._hostGo = Utility.FindChild(baseInfoItem, "Host");
			this._hostGo.CustomSetActive(this._heroInfo.actorHero.get_handle().ObjID == Singleton<CBattleSystem>.GetInstance().WatchForm.TargetHeroId);
			this._emptyGo = Utility.FindChild(baseInfoItem, "Empty");
			this._emptyGo.CustomSetActive(false);
			this._equipInfoItem.GetComponent<CUIEventScript>().enabled = true;
			this._equipIcons = new Image[6];
			for (int i = 0; i < 6; i++)
			{
				this._equipIcons[i] = Utility.GetComponetInChild<Image>(equipInfoItem, "Icon_" + i);
				this._equipIcons[i].gameObject.CustomSetActive(true);
			}
			this._headImg.SetSprite(string.Format("{0}{1}", CUIUtility.s_Sprite_Dynamic_BustCircle_Dir, CSkinInfo.GetHeroSkinPic((uint)this._heroInfo.HeroId, 0u)), Singleton<CBattleSystem>.GetInstance().WatchFormScript, true, false, false, false);
			this.ValidateLevel();
			this.ValidateKDA();
			this.ValidateMoney();
			this.ValidateEquip();
			this.ValidateReviceCD();
		}

		public void ValidateLevel()
		{
			this._levelText.text = this._heroInfo.actorHero.get_handle().ValueComponent.actorSoulLevel.ToString();
		}

		public void ValidateKDA()
		{
			this._kdaText.text = string.Concat(new object[]
			{
				this._heroInfo.numKill,
				" / ",
				this._heroInfo.numDead,
				" / ",
				this._heroInfo.numAssist
			});
		}

		public void ValidateMoney()
		{
			this._moneyText.text = this._heroInfo.TotalCoin.ToString();
		}

		public void ValidateEquip()
		{
			stEquipInfo[] equips = this._heroInfo.actorHero.get_handle().EquipComponent.GetEquips();
			for (int i = 0; i < 6; i++)
			{
				Image image = this._equipIcons[i];
				ushort equipID = equips[i].m_equipID;
				bool flag = false;
				if (equipID > 0)
				{
					ResEquipInBattle dataByKey = GameDataMgr.m_equipInBattleDatabin.GetDataByKey((uint)equipID);
					if (dataByKey != null)
					{
						image.gameObject.CustomSetActive(true);
						string prefabPath = string.Format("{0}{1}", CUIUtility.s_Sprite_System_BattleEquip_Dir, dataByKey.szIcon);
						CUIUtility.SetImageSprite(image, prefabPath, Singleton<CBattleSystem>.GetInstance().WatchFormScript, true, false, false, false);
						flag = true;
					}
				}
				if (!flag)
				{
					image.SetSprite(string.Format((this.listCamp != 1) ? "{0}EquipmentSpaceRed" : "{0}EquipmentSpace", CUIUtility.s_Sprite_Dynamic_Talent_Dir), Singleton<CBattleSystem>.GetInstance().WatchFormScript, true, false, false, false);
				}
			}
		}

		public void ValidateReviceCD()
		{
			if (this._heroInfo != null && this._heroInfo.actorHero && this._heroInfo.actorHero.get_handle().ActorControl.IsDeadState)
			{
				this._reviveTxt.text = string.Format("{0}", Mathf.FloorToInt((float)this._heroInfo.actorHero.get_handle().ActorControl.ReviveCooldown * 0.001f));
				this._headImg.color = CUIUtility.s_Color_Grey;
			}
			else
			{
				this._reviveTxt.text = string.Empty;
				this._headImg.color = CUIUtility.s_Color_Full;
			}
		}

		public void LateUpdate()
		{
			this.ValidateReviceCD();
		}

		public static void MakeEmpty(GameObject baseInfoItem, GameObject equipInfoItem)
		{
			baseInfoItem.GetComponent<CUIEventScript>().enabled = false;
			Utility.FindChild(baseInfoItem, "Empty").CustomSetActive(true);
			equipInfoItem.GetComponent<CUIEventScript>().enabled = false;
			for (int i = 0; i < 6; i++)
			{
				Utility.FindChild(equipInfoItem, "Icon_" + i).CustomSetActive(false);
			}
		}
	}
}
