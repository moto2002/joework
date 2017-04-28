using Apollo;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.UI;
using CSProtocol;
using ResData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class ShareSys : MonoSingleton<ShareSys>
	{
		private enum HeroShareFormWidgets
		{
			DisplayRect,
			HeroAmount,
			ButtonClose,
			ShareClickText,
			ShareFriendBtn,
			TimeLineBtn,
			SkinAmount
		}

		public enum PVPShareFormWidgets
		{
			DisplayRect,
			ButtonClose,
			ShareImg,
			BtnSave,
			BtnFriend,
			BtnZone
		}

		private enum MysteryDiscountFOrmWigets
		{
			DiscountNum,
			ButtonClose,
			ShareClickText
		}

		public enum QQGameTeamEventType
		{
			join,
			start,
			end,
			leave
		}

		public struct SHARE_INFO
		{
			public uint heroId;

			public uint skinId;

			public COM_REWARDS_TYPE rewardType;

			public float beginDownloadTime;

			public string shareSkinUrl;

			public void clear()
			{
				this.shareSkinUrl = string.Empty;
			}
		}

		public enum ELoadError
		{
			None,
			SUCC,
			NotFound,
			OutOfTime
		}

		public struct CLoadReq
		{
			public string m_Url;

			public ShareSys.ELoadError m_LoadError;

			public string m_CachePath;

			public int m_Type;
		}

		public struct ShareActivityParam
		{
			public byte bShareType;

			public byte bParamCnt;

			public uint[] ShareParam;

			public bool bUsed;

			public ShareActivityParam(bool buse)
			{
				this.bUsed = buse;
				this.bShareType = 0;
				this.bParamCnt = 0;
				this.ShareParam = null;
			}

			public void clear()
			{
				this.bUsed = false;
				this.ShareParam = null;
				this.bParamCnt = 0;
				this.bShareType = 0;
			}

			public void set(byte kShareType, byte kParamCnt, uint[] kShareParam)
			{
				this.clear();
				this.bUsed = true;
				this.bShareType = kShareType;
				this.bParamCnt = kParamCnt;
				this.ShareParam = new uint[(int)kParamCnt];
				for (int i = 0; i < (int)kParamCnt; i++)
				{
					this.ShareParam[i] = kShareParam[i];
				}
			}
		}

		public delegate void LoadRCallBack3(Texture2D image, ShareSys.CLoadReq req);

		private const string HeroShowImgDir = "UGUI/Sprite/Dynamic/HeroShow/";

		public static readonly string SNS_SHARE_COMMON = "SNS_SHARE_SEND_COMMON";

		public static readonly string SNS_SHARE_SEND_HEART = "SNS_SHARE_SEND_HEART";

		public static readonly string SNS_SHARE_RECALL_FRIEND = "SNS_SHARE_RECALL_FRIEND";

		public bool m_bHide;

		public static string s_formShareNewHeroPath = "UGUI/Form/System/ShareUI/Form_ShareNewHero.prefab";

		public static string s_formSharePVPPath = "UGUI/Form/System/ShareUI/Form_SharePVPResult.prefab";

		public static string s_formShareNewAchievementPath = "UGUI/Form/System/Achieve/Form_Achievement_Share.prefab";

		public static string s_formShareMysteryDiscountPath = "UGUI/Form/System/ShareUI/Form_ShareMystery_Discount.prefab";

		public static string s_imageSharePVPAchievement = CUIUtility.s_Sprite_Dynamic_PvpAchievementShare_Dir + "Img_PVP_ShareAchievement_";

		public string m_SharePicCDNCachePath = string.Empty;

		public ShareSys.SHARE_INFO m_ShareInfo;

		private CSPKG_JOINMULTIGAMEREQ m_ShareRoom;

		private COMDT_INVITE_JOIN_INFO m_ShareTeam;

		public COMDT_RECRUITMENT_SOURCE m_RecruitSource;

		private string m_ShareStr = string.Empty;

		private string m_QQGameTeamStr = string.Empty;

		private string m_WakeupOpenId = string.Empty;

		private string m_RoomInfoStr = string.Empty;

		private string m_RoomModeId = string.Empty;

		private string m_MarkStr = string.Empty;

		private bool m_bIsQQGameTeamOwner;

		private float g_fDownloadOutTime = 10f;

		private List<ShareSys.CLoadReq> m_DownLoadSkinList = new List<ShareSys.CLoadReq>();

		private Transform m_ShareSkinPicImage;

		private Image m_FriendBtnImage;

		private Image m_TimeLineBtnImage;

		private string m_ShareSkinPicNotFound = string.Empty;

		private string m_ShareSkinPicOutofTime = string.Empty;

		private string m_ShareSkinPicLoading = string.Empty;

		private bool m_bShareHero;

		private bool m_bClickShareFriendBtn;

		public bool m_bShowTimeline;

		private bool m_bClickTimeLineBtn;

		private Transform m_TimelineBtn;

		private bool m_bAdreo306;

		private string m_sharePic = CFileManager.GetCachePath("share.jpg");

		private bool isRegisterd;

		public ShareSys.ShareActivityParam m_ShareActivityParam = new ShareSys.ShareActivityParam(false);

		public bool m_bWinPVPResult;

		private bool m_bSharePvpForm;

		private static string[] m_Support3rdAppList = new string[]
		{
			"QQGameTeam",
			"PenguinEsport",
			"GameHelper"
		};

		private ListView<CSDT_SHARE_TLOG_INFO> m_ShareReportInfoList = new ListView<CSDT_SHARE_TLOG_INFO>();

		private Rect GetScreenShotRect(GameObject displayeRect)
		{
			RectTransform rectTransform = (!(displayeRect != null)) ? new RectTransform() : displayeRect.GetComponent<RectTransform>();
			float num = rectTransform.rect.width * 0.5f;
			float num2 = rectTransform.rect.height * 0.5f;
			Vector3 position = displayeRect.transform.TransformPoint(new Vector3(-num, -num2, 0f));
			position = Singleton<CUIManager>.get_instance().FormCamera.WorldToScreenPoint(position);
			Vector3 position2 = displayeRect.transform.TransformPoint(new Vector3(num, num2, 0f));
			position2 = Singleton<CUIManager>.get_instance().FormCamera.WorldToScreenPoint(position2);
			return new Rect(position.x, position.y, Math.Abs(position.x - position2.x), Math.Abs(position.y - position2.y));
		}

		private static void OnLoadNewHeroOrSkin3DModel(GameObject rawImage, uint heroId, uint skinId, bool bInitAnima)
		{
			CUI3DImageScript cUI3DImageScript = (!(rawImage != null)) ? null : rawImage.GetComponent<CUI3DImageScript>();
			string objectName = CUICommonSystem.GetHeroPrefabPath(heroId, (int)skinId, true).ObjectName;
			GameObject gameObject = (!(cUI3DImageScript != null)) ? null : cUI3DImageScript.AddGameObject(objectName, false, false);
			CHeroAnimaSystem instance = Singleton<CHeroAnimaSystem>.GetInstance();
			instance.Set3DModel(gameObject);
			if (gameObject == null)
			{
				return;
			}
			if (bInitAnima)
			{
				instance.InitAnimatList();
				instance.InitAnimatSoundList(heroId, skinId);
				instance.OnModePlayAnima("Come");
			}
		}

		[DebuggerHidden]
		public IEnumerator DownloadImageByTag2(string preUrl, ShareSys.CLoadReq req, ShareSys.LoadRCallBack3 callBack, string tagPath)
		{
			ShareSys.<DownloadImageByTag2>c__Iterator2C <DownloadImageByTag2>c__Iterator2C = new ShareSys.<DownloadImageByTag2>c__Iterator2C();
			<DownloadImageByTag2>c__Iterator2C.preUrl = preUrl;
			<DownloadImageByTag2>c__Iterator2C.tagPath = tagPath;
			<DownloadImageByTag2>c__Iterator2C.req = req;
			<DownloadImageByTag2>c__Iterator2C.callBack = callBack;
			<DownloadImageByTag2>c__Iterator2C.<$>preUrl = preUrl;
			<DownloadImageByTag2>c__Iterator2C.<$>tagPath = tagPath;
			<DownloadImageByTag2>c__Iterator2C.<$>req = req;
			<DownloadImageByTag2>c__Iterator2C.<$>callBack = callBack;
			<DownloadImageByTag2>c__Iterator2C.<>f__this = this;
			return <DownloadImageByTag2>c__Iterator2C;
		}

		public void PreLoadShareSkinImage(ShareSys.CLoadReq loadReq)
		{
			if (!this.SharePicIsExist(loadReq.m_Url, this.m_SharePicCDNCachePath, loadReq.m_Type) && !this.isDownLoading(loadReq))
			{
				this.m_DownLoadSkinList.Add(loadReq);
				string text = string.Empty;
				if (loadReq.m_Type == 1)
				{
					text = string.Format("{0}HeroSharePic/{1}.jpg", BannerImageSys.GlobalLoadPath, loadReq.m_Url);
				}
				else if (loadReq.m_Type == 2)
				{
					text = string.Format("{0}SkinSharePic/{1}.jpg", BannerImageSys.GlobalLoadPath, loadReq.m_Url);
				}
				if (!string.IsNullOrEmpty(text))
				{
					base.StartCoroutine(this.DownloadImageByTag2(text, loadReq, delegate(Texture2D text2, ShareSys.CLoadReq tempLoadReq)
					{
						int count = this.m_DownLoadSkinList.get_Count();
						if (count > 0)
						{
							for (int i = this.m_DownLoadSkinList.get_Count() - 1; i >= 0; i--)
							{
								ShareSys.CLoadReq cLoadReq = this.m_DownLoadSkinList.get_Item(i);
								if (cLoadReq.m_Url == tempLoadReq.m_Url)
								{
									this.m_DownLoadSkinList.Remove(cLoadReq);
									if (tempLoadReq.m_LoadError != ShareSys.ELoadError.SUCC)
									{
										this.m_DownLoadSkinList.Add(tempLoadReq);
									}
								}
							}
						}
						Debug.Log("skic share pic tempLoadReq " + tempLoadReq.m_LoadError);
						if (this.m_bShareHero && this.m_ShareSkinPicImage != null && loadReq.m_Url == this.m_ShareInfo.shareSkinUrl)
						{
							if (tempLoadReq.m_LoadError == ShareSys.ELoadError.SUCC)
							{
								this.m_ShareSkinPicImage.gameObject.CustomSetActive(true);
								Image component = this.m_ShareSkinPicImage.GetComponent<Image>();
								if (component)
								{
									component.SetSprite(Sprite.Create(text2, new Rect(0f, 0f, (float)text2.width, (float)text2.height), new Vector2(0.5f, 0.5f)), ImageAlphaTexLayout.None);
								}
								if (this.m_FriendBtnImage)
								{
									this.BtnGray(this.m_FriendBtnImage, true);
								}
								if (this.m_TimeLineBtnImage && !this.m_bShowTimeline)
								{
									this.BtnGray(this.m_TimeLineBtnImage, true);
								}
							}
							else if (tempLoadReq.m_LoadError == ShareSys.ELoadError.OutOfTime)
							{
								Singleton<CUIManager>.GetInstance().OpenTips(this.m_ShareSkinPicOutofTime, false, 1.5f, null, new object[0]);
							}
							else if (tempLoadReq.m_LoadError == ShareSys.ELoadError.NotFound)
							{
								Singleton<CUIManager>.GetInstance().OpenTips(this.m_ShareSkinPicNotFound, false, 1.5f, null, new object[0]);
							}
						}
					}, this.m_SharePicCDNCachePath));
				}
			}
		}

		private void LoadShareSkinImage(ShareSys.CLoadReq loadReq, Image imageObj)
		{
			string url = loadReq.m_Url;
			string cachePath = loadReq.m_CachePath;
			string cDNSharePicUrl = this.GetCDNSharePicUrl(url, loadReq.m_Type);
			string text = MonoSingleton<IDIPSys>.GetInstance().ToMD5(cDNSharePicUrl);
			string text2 = CFileManager.CombinePath(cachePath, text);
			if (File.Exists(text2))
			{
				byte[] data = File.ReadAllBytes(text2);
				Texture2D texture2D = new Texture2D(4, 4, TextureFormat.ARGB32, false);
				if (texture2D.LoadImage(data) && this.m_bShareHero && imageObj)
				{
					imageObj.gameObject.CustomSetActive(true);
					if (this.m_bShareHero && imageObj != null)
					{
						imageObj.SetSprite(Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f)), ImageAlphaTexLayout.None);
					}
					if (this.m_FriendBtnImage)
					{
						this.BtnGray(this.m_FriendBtnImage, true);
					}
					if (this.m_TimeLineBtnImage && !this.m_bShowTimeline)
					{
						this.BtnGray(this.m_TimeLineBtnImage, true);
					}
				}
			}
			else
			{
				ShareSys.ELoadError loadReq2 = this.GetLoadReq(loadReq);
				if (loadReq2 == ShareSys.ELoadError.NotFound)
				{
					Singleton<CUIManager>.GetInstance().OpenTips(this.m_ShareSkinPicNotFound, false, 1.5f, null, new object[0]);
				}
				else if (loadReq2 == ShareSys.ELoadError.OutOfTime)
				{
					Singleton<CUIManager>.GetInstance().OpenTips(this.m_ShareSkinPicOutofTime, false, 1.5f, null, new object[0]);
				}
				else
				{
					Singleton<CUIManager>.GetInstance().OpenTips(this.m_ShareSkinPicLoading, false, 0.5f, null, new object[0]);
				}
			}
		}

		private bool isDownLoading(ShareSys.CLoadReq url)
		{
			for (int i = 0; i < this.m_DownLoadSkinList.get_Count(); i++)
			{
				if (url.m_Url == this.m_DownLoadSkinList.get_Item(i).m_Url)
				{
					return true;
				}
			}
			return false;
		}

		private void RemoveDownLoading(string url)
		{
			for (int i = this.m_DownLoadSkinList.get_Count() - 1; i >= 0; i--)
			{
				if (this.m_DownLoadSkinList.get_Item(i).m_Url == url)
				{
					this.m_DownLoadSkinList.Remove(this.m_DownLoadSkinList.get_Item(i));
				}
			}
		}

		private ShareSys.ELoadError GetLoadReq(ShareSys.CLoadReq url)
		{
			for (int i = 0; i < this.m_DownLoadSkinList.get_Count(); i++)
			{
				if (url.m_Url == this.m_DownLoadSkinList.get_Item(i).m_Url)
				{
					return this.m_DownLoadSkinList.get_Item(i).m_LoadError;
				}
			}
			return ShareSys.ELoadError.None;
		}

		private string GetCDNSharePicUrl(string url, int type)
		{
			string result = string.Empty;
			if (type == 1)
			{
				result = string.Format("{0}HeroSharePic/{1}.jpg", BannerImageSys.GlobalLoadPath, url);
			}
			else if (type == 2)
			{
				result = string.Format("{0}SkinSharePic/{1}.jpg", BannerImageSys.GlobalLoadPath, url);
			}
			return result;
		}

		private bool SharePicIsExist(string url, string tagPath, int type)
		{
			string cDNSharePicUrl = this.GetCDNSharePicUrl(url, type);
			string text = MonoSingleton<IDIPSys>.GetInstance().ToMD5(cDNSharePicUrl);
			string text2 = CFileManager.CombinePath(tagPath, text);
			return File.Exists(text2);
		}

		private void BtnGray(Image imgeBtn, bool bShow)
		{
			if (imgeBtn == null)
			{
				return;
			}
			if (bShow)
			{
				imgeBtn.color = new Color(1f, 1f, 1f, 1f);
				CUIEventScript component = imgeBtn.GetComponent<CUIEventScript>();
				component.enabled = true;
			}
			else
			{
				imgeBtn.color = new Color(0f, 1f, 1f, 1f);
				CUIEventScript component2 = imgeBtn.GetComponent<CUIEventScript>();
				component2.enabled = false;
			}
		}

		private void BtnGray(GameObject imageBtnObj, bool bShow)
		{
			if (imageBtnObj == null)
			{
				return;
			}
			Image component = imageBtnObj.GetComponent<Image>();
			if (component == null)
			{
				return;
			}
			if (bShow)
			{
				component.color = new Color(1f, 1f, 1f, 1f);
				CUIEventScript component2 = component.GetComponent<CUIEventScript>();
				component2.enabled = true;
			}
			else
			{
				component.color = new Color(0f, 1f, 1f, 1f);
				CUIEventScript component3 = component.GetComponent<CUIEventScript>();
				component3.enabled = false;
			}
		}

		public void ShowNewSkinShare(uint heroId, uint skinId, bool bInitAnima = true, COM_REWARDS_TYPE rewardType = 5, bool interactableTransition = false)
		{
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(ShareSys.s_formShareNewHeroPath, false, true);
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CUICommonSystem.s_newHeroOrSkinPath);
			enFormPriority priority = enFormPriority.Priority1;
			if (form != null)
			{
				priority = form.m_priority;
			}
			cUIFormScript.SetPriority(priority);
			cUIFormScript.GetWidget(2).CustomSetActive(true);
			Image component = cUIFormScript.GetWidget(0).GetComponent<Image>();
			component.gameObject.CustomSetActive(false);
			this.m_ShareSkinPicImage = component.transform;
			Text component2 = cUIFormScript.GetWidget(1).GetComponent<Text>();
			if (component2)
			{
				component2.gameObject.CustomSetActive(false);
			}
			this.m_FriendBtnImage = cUIFormScript.GetWidget(4).GetComponent<Image>();
			this.m_TimeLineBtnImage = cUIFormScript.GetWidget(5).GetComponent<Image>();
			if (this.m_FriendBtnImage)
			{
				this.BtnGray(this.m_FriendBtnImage, false);
			}
			if (this.m_TimeLineBtnImage)
			{
				this.BtnGray(this.m_TimeLineBtnImage, false);
			}
			if (!string.IsNullOrEmpty(this.m_ShareInfo.shareSkinUrl))
			{
				this.LoadShareSkinImage(new ShareSys.CLoadReq
				{
					m_Url = this.m_ShareInfo.shareSkinUrl,
					m_CachePath = MonoSingleton<ShareSys>.get_instance().m_SharePicCDNCachePath,
					m_LoadError = ShareSys.ELoadError.None,
					m_Type = 2
				}, component);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips("图片还没有上传", false, 1.5f, null, new object[0]);
			}
			ShareSys.SetSharePlatfText(cUIFormScript.GetWidget(3).GetComponent<Text>());
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				Text component3 = cUIFormScript.GetWidget(6).GetComponent<Text>();
				component3.gameObject.CustomSetActive(true);
				component3.text = masterRoleInfo.GetHeroSkinCount(false).ToString();
			}
		}

		public void ShowNewHeroShare(uint heroId, uint skinId, bool bInitAnima = true, COM_REWARDS_TYPE rewardType = 5, bool interactableTransition = false)
		{
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(ShareSys.s_formShareNewHeroPath, false, true);
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CUICommonSystem.s_newHeroOrSkinPath);
			enFormPriority priority = enFormPriority.Priority1;
			if (form != null)
			{
				priority = form.m_priority;
			}
			cUIFormScript.SetPriority(priority);
			cUIFormScript.GetWidget(2).CustomSetActive(true);
			this.m_FriendBtnImage = cUIFormScript.GetWidget(4).GetComponent<Image>();
			this.m_TimeLineBtnImage = cUIFormScript.GetWidget(5).GetComponent<Image>();
			if (this.m_FriendBtnImage)
			{
				this.BtnGray(this.m_FriendBtnImage, false);
			}
			if (this.m_TimeLineBtnImage)
			{
				this.BtnGray(this.m_TimeLineBtnImage, false);
			}
			Image component = cUIFormScript.GetWidget(0).GetComponent<Image>();
			component.gameObject.CustomSetActive(false);
			this.m_ShareSkinPicImage = component.transform;
			if (!string.IsNullOrEmpty(this.m_ShareInfo.shareSkinUrl))
			{
				this.LoadShareSkinImage(new ShareSys.CLoadReq
				{
					m_Url = this.m_ShareInfo.shareSkinUrl,
					m_CachePath = MonoSingleton<ShareSys>.get_instance().m_SharePicCDNCachePath,
					m_LoadError = ShareSys.ELoadError.None,
					m_Type = 1
				}, component);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips("图片还没有上传", false, 1.5f, null, new object[0]);
			}
			ShareSys.SetSharePlatfText(cUIFormScript.GetWidget(3).GetComponent<Text>());
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				Text component2 = cUIFormScript.GetWidget(1).GetComponent<Text>();
				component2.text = masterRoleInfo.GetHaveHeroCount(false).ToString();
				component2.gameObject.CustomSetActive(true);
			}
			Text component3 = cUIFormScript.GetWidget(6).GetComponent<Text>();
			component3.gameObject.CustomSetActive(false);
		}

		protected override void Init()
		{
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Share_CloseNewHeroorSkin, new CUIEventManager.OnUIEventHandler(this.CloseNewHeroForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Share_NewHero, new CUIEventManager.OnUIEventHandler(this.OpenShareNewHeroForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Share_CloseNewHeroShareForm, new CUIEventManager.OnUIEventHandler(this.CloseShareHeroForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Share_ShareFriend, new CUIEventManager.OnUIEventHandler(this.ShareFriend));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Player_Info_Pvp_Share, new CUIEventManager.OnUIEventHandler(this.ShareMyPlayInfoToFriend));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Share_ShareTimeLine, new CUIEventManager.OnUIEventHandler(this.ShareTimeLine));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Share_ShareSavePic, new CUIEventManager.OnUIEventHandler(this.SavePic));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Share_SharePVPScore, new CUIEventManager.OnUIEventHandler(this.SettlementShareBtnHandle));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Share_SharePVPSCcoreClose, new CUIEventManager.OnUIEventHandler(this.OnCloseShowPVPSCore));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Share_MysteryDiscount, new CUIEventManager.OnUIEventHandler(this.ShareMysteryDiscount));
			uint dwConfValue = GameDataMgr.globalInfoDatabin.GetDataByKey(136u).dwConfValue;
			Singleton<CTimerManager>.GetInstance().AddTimer((int)(dwConfValue * 1000u), -1, new CTimer.OnTimeUpHandler(this.OnReportShareInfo));
			Singleton<EventRouter>.get_instance().AddEventHandler(EventID.SHARE_PVP_SETTLEDATA_CLOSE, new Action(this.On_SHARE_PVP_SETTLEDATA_CLOSE));
			this.m_SharePicCDNCachePath = CFileManager.GetCachePath();
			string cachePath = CFileManager.GetCachePath("SkinSharePic");
			try
			{
				if (!Directory.Exists(cachePath))
				{
					Directory.CreateDirectory(cachePath);
				}
				this.m_SharePicCDNCachePath = cachePath;
			}
			catch (Exception ex)
			{
				Debug.Log("sharesys cannot create dictionary " + ex);
				this.m_SharePicCDNCachePath = CFileManager.GetCachePath();
			}
			this.m_ShareSkinPicNotFound = Singleton<CTextManager>.GetInstance().GetText("Share_Skin_Pic_Error_NotFound");
			this.m_ShareSkinPicOutofTime = Singleton<CTextManager>.GetInstance().GetText("Share_Skin_Pic_Error_OutofTime");
			this.m_ShareSkinPicLoading = Singleton<CTextManager>.GetInstance().GetText("Share_Skin_Pic_Error_Loading");
			this.m_bAdreo306 = this.IsAdreo306();
			this.PreLoadPVPImage();
		}

		private void OnReportShareInfo(int timerSequence)
		{
			if (Singleton<CBattleSystem>.get_instance().FormScript == null)
			{
				this.ReportShareInfo();
			}
		}

		public void CloseNewHeroForm(CUIEvent uiEvent)
		{
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CUICommonSystem.s_newHeroOrSkinPath);
			if (form)
			{
				DynamicShadow.EnableDynamicShow(form.gameObject, false);
			}
			this.RemoveDownLoading(this.m_ShareInfo.shareSkinUrl);
			this.m_ShareInfo.clear();
			this.m_bShowTimeline = false;
			Singleton<CUIManager>.GetInstance().CloseForm(CUICommonSystem.s_newHeroOrSkinPath);
			Singleton<EventRouter>.get_instance().BroadCastEvent(EventID.Mall_Get_Product_OK);
		}

		public void RequestShareHeroSkinForm(uint heroID, uint skinID, COM_REWARDS_TYPE kType)
		{
			this.m_ShareInfo.heroId = heroID;
			this.m_ShareInfo.skinId = skinID;
			this.m_ShareInfo.rewardType = kType;
			int type = 1;
			if (kType == 10)
			{
				type = 2;
				ResHeroSkin heroSkin = CSkinInfo.GetHeroSkin(heroID, skinID);
				if (heroSkin != null)
				{
					this.m_ShareInfo.shareSkinUrl = heroSkin.szShareSkinUrl;
				}
			}
			else
			{
				this.m_ShareInfo.shareSkinUrl = this.m_ShareInfo.heroId.ToString();
			}
			ShareSys.CLoadReq loadReq = default(ShareSys.CLoadReq);
			loadReq.m_Url = this.m_ShareInfo.shareSkinUrl;
			loadReq.m_CachePath = this.m_SharePicCDNCachePath;
			loadReq.m_LoadError = ShareSys.ELoadError.None;
			loadReq.m_Type = type;
			MonoSingleton<ShareSys>.GetInstance().PreLoadShareSkinImage(loadReq);
			this.OpenShareNewHeroForm(null);
		}

		public void OpenShareNewHeroForm(CUIEvent uiEvent)
		{
			this.m_ShareActivityParam.clear();
			this.AddshareReportInfo(0u, 0u);
			this.m_bShareHero = true;
			this.m_bClickShareFriendBtn = false;
			if (this.m_ShareInfo.rewardType == 5)
			{
				this.m_ShareActivityParam.set(3, 1, new uint[]
				{
					this.m_ShareInfo.heroId
				});
				this.ShowNewHeroShare(this.m_ShareInfo.heroId, this.m_ShareInfo.skinId, false, this.m_ShareInfo.rewardType, false);
			}
			else if (this.m_ShareInfo.rewardType == 10)
			{
				uint num = this.m_ShareInfo.skinId;
				if (this.m_ShareInfo.heroId != 0u && this.m_ShareInfo.skinId != 0u)
				{
					num = CSkinInfo.GetSkinCfgId(this.m_ShareInfo.heroId, this.m_ShareInfo.skinId);
				}
				this.m_ShareActivityParam.set(3, 1, new uint[]
				{
					num
				});
				this.ShowNewSkinShare(this.m_ShareInfo.heroId, this.m_ShareInfo.skinId, false, this.m_ShareInfo.rewardType, false);
			}
		}

		public void CloseShareHeroForm(CUIEvent uiEvent)
		{
			Singleton<CUIManager>.GetInstance().CloseForm(ShareSys.s_formShareNewHeroPath);
			this.m_bShareHero = false;
			this.m_bClickShareFriendBtn = false;
			this.m_ShareSkinPicImage = null;
			this.m_FriendBtnImage = null;
			this.m_TimeLineBtnImage = null;
		}

		public bool IsInstallPlatform()
		{
			if (!Singleton<ApolloHelper>.GetInstance().IsPlatformInstalled(Singleton<ApolloHelper>.GetInstance().CurPlatform))
			{
				if (Singleton<ApolloHelper>.GetInstance().CurPlatform == 1)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox("未安装微信，无法使用该功能", false);
				}
				else if (Singleton<ApolloHelper>.GetInstance().CurPlatform == 2)
				{
					Singleton<CUIManager>.GetInstance().OpenMessageBox("未安装手机QQ，无法使用该功能", false);
				}
				return false;
			}
			return true;
		}

		public void ShareFriend(CUIEvent uiEvent)
		{
			Singleton<ApolloHelper>.GetInstance().m_bShareQQBox = false;
			this.m_bClickTimeLineBtn = false;
			if (!this.IsInstallPlatform())
			{
				return;
			}
			GameObject btnClose = this.GetCloseBtn(uiEvent.m_srcFormScript);
			if (btnClose == null)
			{
				return;
			}
			Singleton<CUIManager>.get_instance().CloseTips();
			btnClose.CustomSetActive(false);
			bool bSettltment = false;
			if (uiEvent.m_srcFormScript.m_formPath == SettlementSystem.SettlementFormName)
			{
				bSettltment = true;
				Singleton<SettlementSystem>.GetInstance().SnapScreenShotShowBtn(false);
			}
			GameObject displayPanel = this.GetDisplayPanel(uiEvent.m_srcFormScript);
			if (displayPanel == null)
			{
				return;
			}
			Rect screenShotRect = this.GetScreenShotRect(displayPanel);
			this.m_bClickShareFriendBtn = true;
			GameObject notShowObj = this.GetNotShowBtn(uiEvent.m_srcFormScript);
			if (notShowObj)
			{
				notShowObj.CustomSetActive(true);
			}
			base.StartCoroutine(this.Capture(screenShotRect, delegate(string filename)
			{
				Debug.Log("sns += capture showfriend filename" + filename);
				this.Share("Session", this.m_sharePic);
				btnClose.CustomSetActive(true);
				if (notShowObj)
				{
					notShowObj.CustomSetActive(false);
				}
				if (bSettltment)
				{
					Singleton<SettlementSystem>.GetInstance().SnapScreenShotShowBtn(true);
					bSettltment = false;
				}
			}));
		}

		private void ShareMyPlayInfoToFriend(CUIEvent uiEvent)
		{
			if (!this.IsInstallPlatform())
			{
				return;
			}
			CUIFormScript form = Singleton<CUIManager>.get_instance().GetForm(CPlayerInfoSystem.sPlayerInfoFormPath);
			if (form == null)
			{
				return;
			}
			GameObject widget = form.GetWidget(2);
			GameObject widget2 = form.GetWidget(15);
			GameObject btnGraph = Utility.FindChild(widget, "btnGraph");
			GameObject btnDetail = Utility.FindChild(widget, "btnDetail");
			if (btnGraph == null || btnDetail == null)
			{
				return;
			}
			bool btnGraphActive = btnGraph.activeSelf;
			bool btnDetailActive = btnDetail.activeSelf;
			btnGraph.CustomSetActive(false);
			btnDetail.CustomSetActive(false);
			Rect screenShotRect = this.GetScreenShotRect(widget2);
			base.StartCoroutine(this.Capture(screenShotRect, delegate(string filename)
			{
				this.Share("Session", this.m_sharePic);
				btnGraph.CustomSetActive(btnGraphActive);
				btnDetail.CustomSetActive(btnDetailActive);
			}));
		}

		private void UpdateTimelineBtn()
		{
			if (this.m_TimelineBtn != null)
			{
				GameObject gameObject = this.m_TimelineBtn.gameObject;
				if (this.m_bShowTimeline && gameObject != null)
				{
					CUIEventScript component = gameObject.GetComponent<CUIEventScript>();
					component.enabled = false;
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, 0.37f);
					Text componentInChildren = gameObject.GetComponentInChildren<Text>();
					componentInChildren.color = new Color(componentInChildren.color.r, componentInChildren.color.g, componentInChildren.color.b, 0.37f);
				}
				this.m_TimelineBtn = null;
			}
			Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Share_ClosePVPAchievement);
		}

		public void ShareTimeLine(CUIEvent uiEvent)
		{
			Singleton<ApolloHelper>.GetInstance().m_bShareQQBox = false;
			if (!this.IsInstallPlatform())
			{
				return;
			}
			GameObject btnClose = this.GetCloseBtn(uiEvent.m_srcFormScript);
			if (btnClose == null)
			{
				return;
			}
			Debug.Log(" m_bClickTimeLineBtn " + this.m_bClickTimeLineBtn);
			this.m_TimelineBtn = uiEvent.m_srcWidget.transform;
			this.m_bClickTimeLineBtn = true;
			this.m_bClickShareFriendBtn = false;
			btnClose.CustomSetActive(false);
			Singleton<CUIManager>.get_instance().CloseTips();
			bool bSettltment = false;
			if (uiEvent.m_srcFormScript.m_formPath == SettlementSystem.SettlementFormName)
			{
				bSettltment = true;
				Singleton<SettlementSystem>.GetInstance().SnapScreenShotShowBtn(false);
			}
			GameObject displayPanel = this.GetDisplayPanel(uiEvent.m_srcFormScript);
			if (displayPanel == null)
			{
				return;
			}
			Rect screenShotRect = this.GetScreenShotRect(displayPanel);
			GameObject notShowObj = this.GetNotShowBtn(uiEvent.m_srcFormScript);
			if (notShowObj)
			{
				notShowObj.CustomSetActive(true);
			}
			base.StartCoroutine(this.Capture(screenShotRect, delegate(string filename)
			{
				Debug.Log("sns += capture showfriend filename" + filename);
				this.Share("TimeLine/Qzone", this.m_sharePic);
				btnClose.CustomSetActive(true);
				if (notShowObj)
				{
					notShowObj.CustomSetActive(false);
				}
				if (bSettltment)
				{
					Singleton<SettlementSystem>.GetInstance().SnapScreenShotShowBtn(true);
					bSettltment = false;
				}
			}));
		}

		public void SavePic(CUIEvent uiEvent)
		{
			GameObject btnClose = this.GetCloseBtn(uiEvent.m_srcFormScript);
			if (btnClose == null)
			{
				return;
			}
			if (btnClose)
			{
				btnClose.CustomSetActive(false);
			}
			Singleton<CUIManager>.get_instance().CloseTips();
			bool bSettltment = false;
			if (uiEvent.m_srcFormScript.m_formPath == SettlementSystem.SettlementFormName)
			{
				bSettltment = true;
				Singleton<SettlementSystem>.GetInstance().SnapScreenShotShowBtn(false);
			}
			GameObject displayPanel = this.GetDisplayPanel(uiEvent.m_srcFormScript);
			if (displayPanel == null)
			{
				return;
			}
			Rect screenShotRect = this.GetScreenShotRect(displayPanel);
			GameObject notShowObj = this.GetNotShowBtn(uiEvent.m_srcFormScript);
			if (notShowObj)
			{
				notShowObj.CustomSetActive(true);
			}
			base.StartCoroutine(this.Capture(screenShotRect, delegate(string filename)
			{
				if (btnClose)
				{
					btnClose.CustomSetActive(true);
				}
				if (notShowObj)
				{
					notShowObj.CustomSetActive(false);
				}
				if (bSettltment)
				{
					Singleton<SettlementSystem>.GetInstance().SnapScreenShotShowBtn(true);
				}
				if (Application.platform == RuntimePlatform.Android)
				{
					try
					{
						string text = "/mnt/sdcard/DCIM/Sgame";
						if (!Directory.Exists(text))
						{
							Directory.CreateDirectory(text);
						}
						text = string.Format("{0}/share_{1}.png", text, DateTime.get_Now().ToFileTimeUtc());
						Debug.Log("sns += SavePic " + text);
						FileStream fileStream = new FileStream(this.m_sharePic, 3, 1);
						byte[] array = new byte[fileStream.get_Length()];
						int num = Convert.ToInt32(fileStream.get_Length());
						fileStream.Read(array, 0, num);
						fileStream.Close();
						File.WriteAllBytes(text, array);
						this.RefeshPhoto(text);
						Singleton<CUIManager>.get_instance().OpenTips("成功保存到相册", false, 1.5f, null, new object[0]);
					}
					catch (Exception ex)
					{
						DebugHelper.Assert(false, "Error In Save Pic, {0}", new object[]
						{
							ex.get_Message()
						});
						Singleton<CUIManager>.get_instance().OpenTips("保存到相册出错", false, 1.5f, null, new object[0]);
					}
				}
				else if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					this.RefeshPhoto(this.m_sharePic);
					Singleton<CUIManager>.get_instance().OpenTips("成功保存到相册", false, 1.5f, null, new object[0]);
				}
			}));
			uint dwType = 0u;
			if (this.m_bShareHero)
			{
				dwType = 0u;
			}
			else if (this.m_bSharePvpForm)
			{
				dwType = 1u;
			}
			this.AddshareReportInfo(dwType, 1u);
		}

		private void RefeshPhoto(string filename)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			if (androidJavaClass != null)
			{
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				if (@static != null)
				{
					Debug.Log("RefeshPhoto in unity");
					@static.Call("RefeshPhoto", new object[]
					{
						filename
					});
				}
			}
		}

		private bool IsAdreo306()
		{
			string text = SystemInfo.graphicsDeviceName.ToLower();
			char[] array = new char[]
			{
				' ',
				'\t',
				'\r',
				'\n',
				'+',
				'-',
				':'
			};
			string[] array2 = text.Split(array, 1);
			if (array2 == null || array2.Length == 0)
			{
				return false;
			}
			if (array2[0] == "adreno")
			{
				int num = 0;
				for (int i = 1; i < array2.Length; i++)
				{
					bool flag = int.TryParse(array2[i], ref num);
					if (num == 306)
					{
						return true;
					}
				}
			}
			return false;
		}

		[DebuggerHidden]
		private IEnumerator Capture(Rect screenShotRect, Action<string> callback)
		{
			ShareSys.<Capture>c__Iterator2D <Capture>c__Iterator2D = new ShareSys.<Capture>c__Iterator2D();
			<Capture>c__Iterator2D.screenShotRect = screenShotRect;
			<Capture>c__Iterator2D.callback = callback;
			<Capture>c__Iterator2D.<$>screenShotRect = screenShotRect;
			<Capture>c__Iterator2D.<$>callback = callback;
			<Capture>c__Iterator2D.<>f__this = this;
			return <Capture>c__Iterator2D;
		}

		public void OnShareCallBack()
		{
			IApolloSnsService apolloSnsService = IApollo.get_Instance().GetService(1) as IApolloSnsService;
			if (apolloSnsService == null)
			{
				return;
			}
			if (!this.isRegisterd)
			{
				apolloSnsService.add_onShareEvent(delegate(ApolloShareResult shareResponseInfo)
				{
					string text = string.Format("share result:{0} \n share platform:{1} \n share description:{2}\n share extInfo:{3}", new object[]
					{
						shareResponseInfo.result,
						shareResponseInfo.platform,
						shareResponseInfo.drescription,
						shareResponseInfo.extInfo
					});
					Debug.Log("sns += " + text);
					if (shareResponseInfo.result == null)
					{
						if (!(shareResponseInfo.extInfo == ShareSys.SNS_SHARE_SEND_HEART))
						{
							if (!(shareResponseInfo.extInfo == ShareSys.SNS_SHARE_RECALL_FRIEND))
							{
								if (this.m_bClickTimeLineBtn)
								{
									this.m_bShowTimeline = true;
									Singleton<EventRouter>.get_instance().BroadCastEvent<Transform>(EventID.SHARE_TIMELINE_SUCC, this.m_TimelineBtn);
									this.UpdateTimelineBtn();
								}
								uint dwType = 0u;
								if (this.m_bShareHero)
								{
									dwType = 0u;
								}
								else if (this.m_bSharePvpForm)
								{
									dwType = 1u;
								}
								if (this.m_bClickShareFriendBtn)
								{
									if (ApolloConfig.platform == 1)
									{
										this.AddshareReportInfo(dwType, 3u);
									}
									else if (ApolloConfig.platform == 2)
									{
										this.AddshareReportInfo(dwType, 2u);
									}
								}
								else if (this.m_bClickTimeLineBtn)
								{
									if (ApolloConfig.platform == 1)
									{
										this.AddshareReportInfo(dwType, 5u);
									}
									else if (ApolloConfig.platform == 2)
									{
										this.AddshareReportInfo(dwType, 4u);
									}
								}
								CTaskSys.Send_Share_Task();
								if (this.m_bClickTimeLineBtn)
								{
									this.SendShareActivityDoneMsg();
								}
								this.m_bClickTimeLineBtn = false;
								this.m_bClickShareFriendBtn = false;
							}
						}
					}
					else
					{
						this.m_bClickTimeLineBtn = false;
						this.m_bClickShareFriendBtn = false;
					}
				});
				this.isRegisterd = true;
			}
		}

		private void Share(string buttonType, string sharePathPic)
		{
			IApolloSnsService apolloSnsService = IApollo.get_Instance().GetService(1) as IApolloSnsService;
			if (apolloSnsService == null)
			{
				return;
			}
			FileStream fileStream = new FileStream(sharePathPic, 3, 1);
			byte[] array = new byte[fileStream.get_Length()];
			int num = Convert.ToInt32(fileStream.get_Length());
			fileStream.Read(array, 0, num);
			fileStream.Close();
			this.OnShareCallBack();
			if (ApolloConfig.platform == 1)
			{
				if (buttonType == "TimeLine/Qzone")
				{
					apolloSnsService.SendToWeixinWithPhoto(1, "MSG_INVITE", array, num, string.Empty, "WECHAT_SNS_JUMP_APP");
				}
				else if (buttonType == "Session")
				{
					apolloSnsService.SendToWeixinWithPhoto(0, "apollo test", array, num);
				}
			}
			else if (ApolloConfig.platform == 2)
			{
				if (buttonType == "TimeLine/Qzone")
				{
					apolloSnsService.SendToQQWithPhoto(1, sharePathPic);
				}
				else if (buttonType == "Session")
				{
					apolloSnsService.SendToQQWithPhoto(2, sharePathPic);
				}
			}
		}

		public void GShare(string buttonType, string sharePathPic)
		{
			this.m_bClickTimeLineBtn = true;
			this.Share(buttonType, sharePathPic);
		}

		public void SendShareActivityDoneMsg()
		{
			if (Singleton<ActivitySys>.GetInstance().IsShareTask && this.m_ShareActivityParam.bUsed)
			{
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5234u);
				cSPkg.stPkgData.get_stWealContentShareDone().bShareType = this.m_ShareActivityParam.bShareType;
				cSPkg.stPkgData.get_stWealContentShareDone().bParamCnt = this.m_ShareActivityParam.bParamCnt;
				for (int i = 0; i < (int)this.m_ShareActivityParam.bParamCnt; i++)
				{
					cSPkg.stPkgData.get_stWealContentShareDone().ShareParam[i] = this.m_ShareActivityParam.ShareParam[i];
				}
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
				string message = string.Format("SendShareActivityDoneMsg{0}/{1}/{2} ", this.m_ShareActivityParam.bShareType, this.m_ShareActivityParam.bParamCnt, this.m_ShareActivityParam.ShareParam);
				Debug.Log(message);
			}
			this.m_ShareActivityParam.clear();
		}

		public static void SetSharePlatfText(Text platText)
		{
			if (null == platText)
			{
				return;
			}
			if (ApolloConfig.platform == 2)
			{
				platText.text = "分享空间";
			}
			else
			{
				platText.text = "分享朋友圈";
			}
		}

		private void PreLoadPVPImage()
		{
			string preUrl = string.Format("{0}PVPShare/Img_PVP_ShareDefeat_0.jpg", BannerImageSys.GlobalLoadPath);
			base.StartCoroutine(MonoSingleton<IDIPSys>.GetInstance().DownloadImage(preUrl, delegate(Texture2D text2)
			{
			}, 10));
			int[] array = new int[]
			{
				0,
				3,
				6,
				7,
				8
			};
			preUrl = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				preUrl = string.Format("{0}PVPShare/Img_PVP_ShareAchievement_{1}.jpg", BannerImageSys.GlobalLoadPath, array[i]);
				base.StartCoroutine(MonoSingleton<IDIPSys>.GetInstance().DownloadImage(preUrl, delegate(Texture2D text2D)
				{
				}, 10));
			}
		}

		private void SettlementShareBtnHandle(CUIEvent ievent)
		{
			if (MonoSingleton<NewbieGuideManager>.GetInstance().isNewbieGuiding)
			{
				return;
			}
			Singleton<CChatController>.get_instance().ShowPanel(false, false);
			this.AddshareReportInfo(1u, 0u);
			this.OpenShowSharePVPFrom(8);
		}

		public void OpenShowSharePVPFrom(RES_SHOW_ACHIEVEMENT_TYPE type)
		{
			this.m_ShareActivityParam.clear();
			this.m_bSharePvpForm = true;
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(ShareSys.s_formSharePVPPath, false, true);
			this.UpdateSharePVPForm(cUIFormScript, cUIFormScript.GetWidget(0));
			GameObject shareImg = cUIFormScript.GetWidget(2);
			GameObject btnClose = cUIFormScript.GetWidget(3);
			GameObject btnFriend = cUIFormScript.GetWidget(4);
			GameObject btnZone = cUIFormScript.GetWidget(5);
			this.BtnGray(btnClose, false);
			this.BtnGray(btnFriend, false);
			this.BtnGray(btnZone, false);
			string preUrl = string.Format("{0}PVPShare/Img_PVP_ShareAchievement_{1}.jpg", BannerImageSys.GlobalLoadPath, type);
			base.StartCoroutine(MonoSingleton<IDIPSys>.GetInstance().DownloadImage(preUrl, delegate(Texture2D text2D)
			{
				if (shareImg == null || shareImg.GetComponent<Image>() == null || text2D == null)
				{
					return;
				}
				this.BtnGray(btnClose, true);
				this.BtnGray(btnFriend, true);
				this.BtnGray(btnZone, true);
				shareImg.GetComponent<Image>().SetSprite(Sprite.Create(text2D, new Rect(0f, 0f, (float)text2D.width, (float)text2D.height), new Vector2(0.5f, 0.5f)), ImageAlphaTexLayout.None);
			}, 10));
		}

		public void SetShareDefeatImage(Transform imageTransform, CUIFormScript form)
		{
			if (imageTransform == null || form == null)
			{
				return;
			}
			GameObject btnBarrige = form.GetWidget(3);
			GameObject btnFriend = form.GetWidget(4);
			GameObject btnZone = form.GetWidget(5);
			this.BtnGray(btnBarrige, false);
			this.BtnGray(btnFriend, false);
			this.BtnGray(btnZone, false);
			string preUrl = string.Format("{0}PVPShare/Img_PVP_ShareDefeat_0.jpg", BannerImageSys.GlobalLoadPath);
			base.StartCoroutine(MonoSingleton<IDIPSys>.GetInstance().DownloadImage(preUrl, delegate(Texture2D text2D)
			{
				if (imageTransform == null || imageTransform.GetComponent<Image>() == null || text2D == null)
				{
					return;
				}
				this.BtnGray(btnBarrige, true);
				this.BtnGray(btnFriend, true);
				this.BtnGray(btnZone, true);
				imageTransform.GetComponent<Image>().SetSprite(Sprite.Create(text2D, new Rect(0f, 0f, (float)text2D.width, (float)text2D.height), new Vector2(0.5f, 0.5f)), ImageAlphaTexLayout.None);
			}, 10));
		}

		public void UpdateSharePVPForm(CUIFormScript form, GameObject shareRootGO)
		{
			if (form == null)
			{
				return;
			}
			ShareSys.SetSharePlatfText(Utility.GetComponetInChild<Text>(form.gameObject, "ShareGroup/Button_TimeLine/ClickText"));
			if (this.m_bShowTimeline)
			{
				Transform transform = null;
				Text[] componentsInChildren = form.transform.GetComponentsInChildren<Text>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					Text text = componentsInChildren[i];
					if ((text != null && text.text == "分享空间") || text.text == "分享朋友圈")
					{
						Transform parent = text.transform.parent;
						if (parent.GetComponent<Button>())
						{
							transform = parent;
							break;
						}
					}
				}
				if (transform)
				{
					GameObject gameObject = transform.gameObject;
					if (gameObject || this.m_bShowTimeline)
					{
						CUIEventScript component = gameObject.GetComponent<CUIEventScript>();
						component.enabled = false;
						gameObject.GetComponent<Button>().interactable = false;
						gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, 0.37f);
						Text componentInChildren = gameObject.GetComponentInChildren<Text>();
						componentInChildren.color = new Color(componentInChildren.color.r, componentInChildren.color.g, componentInChildren.color.b, 0.37f);
					}
				}
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				CUIHttpImageScript componetInChild = Utility.GetComponetInChild<CUIHttpImageScript>(shareRootGO, "PlayerHead");
				componetInChild.SetImageUrl(masterRoleInfo.HeadUrl);
				Text componetInChild2 = Utility.GetComponetInChild<Text>(shareRootGO, "PlayerName");
				componetInChild2.text = masterRoleInfo.Name;
				CPlayerKDAStat playerKDAStat = Singleton<BattleLogic>.GetInstance().battleStat.m_playerKDAStat;
				DictionaryView<uint, PlayerKDA>.Enumerator enumerator = playerKDAStat.GetEnumerator();
				PlayerKDA playerKDA = null;
				int[] array = new int[3];
				while (enumerator.MoveNext())
				{
					KeyValuePair<uint, PlayerKDA> current = enumerator.get_Current();
					PlayerKDA value = current.get_Value();
					if (value.IsHost)
					{
						playerKDA = value;
					}
					array[value.PlayerCamp] += value.numKill;
				}
				Utility.FindChild(componetInChild.gameObject, "Mvp").CustomSetActive(Singleton<BattleStatistic>.get_instance().GetMvpPlayer(playerKDA.PlayerCamp, this.m_bWinPVPResult) == playerKDA.PlayerId);
				if (playerKDA != null)
				{
					Text componetInChild3 = Utility.GetComponetInChild<Text>(shareRootGO, "HostKillNum");
					componetInChild3.text = playerKDA.numKill.ToString();
					Text componetInChild4 = Utility.GetComponetInChild<Text>(shareRootGO, "HostDeadNum");
					componetInChild4.text = playerKDA.numDead.ToString();
					Text componetInChild5 = Utility.GetComponetInChild<Text>(shareRootGO, "HostAssistNum");
					componetInChild5.text = playerKDA.numAssist.ToString();
					Text componetInChild6 = Utility.GetComponetInChild<Text>(shareRootGO, "HostKillTotalNum");
					componetInChild6.text = array[playerKDA.PlayerCamp].ToString();
					Text componetInChild7 = Utility.GetComponetInChild<Text>(shareRootGO, "OppoKillTotalNum");
					componetInChild7.text = array[BattleLogic.GetOppositeCmp(playerKDA.PlayerCamp)].ToString();
					ListView<HeroKDA>.Enumerator enumerator2 = playerKDA.GetEnumerator();
					if (enumerator2.MoveNext())
					{
						HeroKDA current2 = enumerator2.get_Current();
						ResHeroCfgInfo dataByKey = GameDataMgr.heroDatabin.GetDataByKey((uint)current2.HeroId);
						Image componetInChild8 = Utility.GetComponetInChild<Image>(shareRootGO, "HeroHead");
						componetInChild8.SetSprite(CUIUtility.s_Sprite_Dynamic_Icon_Dir + StringHelper.UTF8BytesToString(ref dataByKey.szImagePath), form, true, false, false, false);
						int num = 1;
						for (int j = 1; j < 13; j++)
						{
							switch (j)
							{
							case 1:
								if (current2.LegendaryNum > 0)
								{
									CSettlementView.SetAchievementIcon(form, shareRootGO, PvpAchievement.Legendary, num++);
								}
								break;
							case 2:
								if (current2.PentaKillNum > 0)
								{
									CSettlementView.SetAchievementIcon(form, shareRootGO, PvpAchievement.PentaKill, num++);
								}
								break;
							case 3:
								if (current2.QuataryKillNum > 0)
								{
									CSettlementView.SetAchievementIcon(form, shareRootGO, PvpAchievement.QuataryKill, num++);
								}
								break;
							case 4:
								if (current2.TripleKillNum > 0)
								{
									CSettlementView.SetAchievementIcon(form, shareRootGO, PvpAchievement.TripleKill, num++);
								}
								break;
							case 5:
								if (current2.DoubleKillNum > 0)
								{
								}
								break;
							case 6:
								if (current2.bKillMost)
								{
									CSettlementView.SetAchievementIcon(form, shareRootGO, PvpAchievement.KillMost, num++);
								}
								break;
							case 7:
								if (current2.bHurtMost)
								{
									CSettlementView.SetAchievementIcon(form, shareRootGO, PvpAchievement.HurtMost, num++);
								}
								break;
							case 8:
								if (current2.bHurtTakenMost)
								{
									CSettlementView.SetAchievementIcon(form, shareRootGO, PvpAchievement.HurtTakenMost, num++);
								}
								break;
							case 9:
								if (current2.bAsssistMost)
								{
									CSettlementView.SetAchievementIcon(form, shareRootGO, PvpAchievement.AsssistMost, num++);
								}
								break;
							}
						}
						for (int k = num; k <= 6; k++)
						{
							CSettlementView.SetAchievementIcon(form, shareRootGO, PvpAchievement.NULL, k);
						}
					}
				}
			}
		}

		public void OnCloseShowPVPSCore(CUIEvent ievent)
		{
			Singleton<CUIManager>.GetInstance().CloseForm(ShareSys.s_formSharePVPPath);
			this.m_bSharePvpForm = false;
			MonoSingleton<NewbieGuideManager>.GetInstance().CheckTriggerTime(NewbieGuideTriggerTimeType.PvPShareFin, new uint[0]);
			Singleton<CChatController>.get_instance().ShowPanel(true, false);
		}

		public void UpdateShareGradeForm(CUIFormScript form)
		{
			if (form == null)
			{
				return;
			}
			ShareSys.SetSharePlatfText(Utility.GetComponetInChild<Text>(form.gameObject, "ShareGroup/Button_TimeLine/ClickText"));
			if (this.m_bShowTimeline)
			{
				Transform transform = null;
				Text[] componentsInChildren = form.transform.GetComponentsInChildren<Text>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					Text text = componentsInChildren[i];
					if ((text != null && text.text == "分享空间") || text.text == "分享朋友圈")
					{
						Transform parent = text.transform.parent;
						if (parent.GetComponent<Button>())
						{
							transform = parent;
							break;
						}
					}
				}
				if (transform)
				{
					GameObject gameObject = transform.gameObject;
					if (gameObject || this.m_bShowTimeline)
					{
						CUIEventScript component = gameObject.GetComponent<CUIEventScript>();
						component.enabled = false;
						gameObject.GetComponent<Button>().interactable = false;
						gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, 0.37f);
						Text componentInChildren = gameObject.GetComponentInChildren<Text>();
						componentInChildren.color = new Color(componentInChildren.color.r, componentInChildren.color.g, componentInChildren.color.b, 0.37f);
					}
				}
			}
		}

		private void On_SHARE_PVP_SETTLEDATA_CLOSE()
		{
			this.m_bSharePvpForm = false;
		}

		private void ShareMysteryDiscount(CUIEvent uiEvent)
		{
			MySteryShop instance = Singleton<MySteryShop>.GetInstance();
			if (!instance.IsGetDisCount())
			{
				return;
			}
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(ShareSys.s_formShareMysteryDiscountPath, false, true);
			DebugHelper.Assert(cUIFormScript != null, "神秘商店分享form打开失败");
			if (cUIFormScript == null)
			{
				return;
			}
			GameObject widget = cUIFormScript.GetWidget(0);
			if (widget != null)
			{
				Image component = widget.GetComponent<Image>();
				if (component != null)
				{
					component.SetSprite(instance.GetDiscountNumIconPath((uint)instance.GetDisCount()), cUIFormScript, true, false, false, false);
				}
			}
			GameObject widget2 = cUIFormScript.GetWidget(2);
			if (widget2)
			{
				ShareSys.SetSharePlatfText(widget2.GetComponent<Text>());
			}
		}

		private GameObject GetCloseBtn(CUIFormScript form)
		{
			if (form == null)
			{
				return null;
			}
			GameObject result;
			if (form.m_formPath == ShareSys.s_formShareNewHeroPath)
			{
				result = form.GetWidget(2);
			}
			else if (form.m_formPath == ShareSys.s_formSharePVPPath)
			{
				result = form.GetWidget(1);
			}
			else if (form.m_formPath == ShareSys.s_formShareNewAchievementPath)
			{
				result = form.GetWidget(3);
			}
			else if (form.m_formPath == ShareSys.s_formShareMysteryDiscountPath)
			{
				result = form.GetWidget(1);
			}
			else if (form.m_formPath == PvpAchievementForm.s_formSharePVPDefeatPath)
			{
				result = form.GetWidget(0);
			}
			else if (form.m_formPath == SettlementSystem.SettlementFormName)
			{
				result = form.gameObject.transform.FindChild("Panel/Btn_Share_PVP_DATA_CLOSE").gameObject;
			}
			else
			{
				if (!(form.m_formPath == "UGUI/Form/System/ShareUI/Form_SharePVPLadder.prefab"))
				{
					DebugHelper.Assert(false, "ShareSys.GetCloseBtn(): error form path = {0}", new object[]
					{
						form.m_formPath
					});
					return null;
				}
				result = form.gameObject.transform.FindChild("Button_Close").gameObject;
			}
			return result;
		}

		private GameObject GetNotShowBtn(CUIFormScript form)
		{
			GameObject result = null;
			if (form == null)
			{
				return null;
			}
			if (form.m_formPath == SettlementSystem.SettlementFormName)
			{
				result = form.gameObject.transform.FindChild("Panel/Logo").gameObject;
			}
			return result;
		}

		private GameObject GetDisplayPanel(CUIFormScript form)
		{
			if (form == null)
			{
				return null;
			}
			GameObject result;
			if (form.m_formPath == ShareSys.s_formShareNewHeroPath)
			{
				result = form.GetWidget(0);
			}
			else if (form.m_formPath == ShareSys.s_formSharePVPPath)
			{
				result = form.GetWidget(0);
			}
			else if (form.m_formPath == ShareSys.s_formShareNewAchievementPath)
			{
				result = form.GetWidget(4);
			}
			else if (form.m_formPath == PvpAchievementForm.s_formSharePVPDefeatPath)
			{
				result = form.GetWidget(2);
			}
			else if (form.m_formPath == SettlementSystem.SettlementFormName)
			{
				result = form.gameObject.transform.FindChild("Panel").gameObject;
			}
			else if (form.m_formPath == "UGUI/Form/System/ShareUI/Form_SharePVPLadder.prefab")
			{
				result = form.gameObject.transform.FindChild("ShareFrame").gameObject;
			}
			else
			{
				if (!(form.m_formPath == ShareSys.s_formShareMysteryDiscountPath))
				{
					DebugHelper.Assert(false, "ShareSys.GetDisplayPanel(): error form path = {0}", new object[]
					{
						form.m_formPath
					});
					return null;
				}
				result = form.gameObject.transform.FindChild("DiscountShow").gameObject;
			}
			return result;
		}

		private bool CheckEnterShareTeamLimit(ref string paramDevicePlatStr, ref string paramPlatformStr, bool CheckPlat = true, bool CheckDevicePlat = false)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			uint dwConfValue = GameDataMgr.globalInfoDatabin.GetDataByKey(191u).dwConfValue;
			if (masterRoleInfo != null && masterRoleInfo.PvpLevel < dwConfValue)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("Enter_Room_Level_Limit", true, 1f, null, new object[]
				{
					dwConfValue
				});
				return false;
			}
			if (CheckDevicePlat)
			{
				int num = 0;
				if (!int.TryParse(paramDevicePlatStr, ref num) || Application.platform != (RuntimePlatform)num)
				{
					Singleton<CUIManager>.GetInstance().OpenTips("Enter_Room_Different_Device", true, 1.5f, null, new object[0]);
					return false;
				}
			}
			if (CheckPlat)
			{
				int num2 = -1;
				if (!int.TryParse(paramPlatformStr, ref num2) || num2 != ApolloConfig.platform)
				{
					Singleton<CUIManager>.GetInstance().OpenTips("Enter_Room_Different_Platform", true, 1.5f, null, new object[0]);
					return false;
				}
			}
			if (Singleton<GameStateCtrl>.GetInstance().isBattleState || Singleton<GameStateCtrl>.GetInstance().isLoadingState)
			{
				Singleton<CUIManager>.GetInstance().OpenTips("Enter_Room_InBattle", true, 1.5f, null, new object[0]);
				return false;
			}
			return true;
		}

		public string PackRoomData(int iRoomEntity, uint dwRoomID, uint dwRoomSeq, byte bMapType, uint dwMapId, ulong ullFeature)
		{
			string text = string.Format("ShareRoom_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}", new object[]
			{
				iRoomEntity,
				dwRoomID,
				dwRoomSeq,
				bMapType,
				dwMapId,
				ullFeature,
				(int)Application.platform,
				ApolloConfig.platform
			});
			Debug.Log(text);
			return text;
		}

		public bool UnpackInviteSNSData(string data, string wakeupOpenId)
		{
			Debug.Log("rcv " + data);
			if (string.IsNullOrEmpty(data))
			{
				return false;
			}
			string[] array = data.Split(new char[]
			{
				'_'
			});
			if (array != null && array[0] == "mqq")
			{
				this.UnpackRecruitFriendInfo(data);
				this.m_ShareStr = string.Empty;
				return true;
			}
			if (MonoSingleton<NewbieGuideManager>.GetInstance().isNewbieGuiding)
			{
				Debug.Log("正在新手引导中");
				return false;
			}
			this.m_WakeupOpenId = wakeupOpenId;
			if (Singleton<LobbyLogic>.get_instance().isLogin)
			{
				return this.UnPackSNSDataStr(data);
			}
			this.m_ShareStr = data;
			return true;
		}

		private bool IsSupport3rdAPP(string tag)
		{
			for (int i = 0; i < ShareSys.m_Support3rdAppList.Length; i++)
			{
				if (tag.Equals(ShareSys.m_Support3rdAppList[i]))
				{
					return true;
				}
			}
			return false;
		}

		private bool UnPackSNSDataStr(string data)
		{
			string[] array = data.Split(new char[]
			{
				'_'
			});
			if (array != null && array[0] == "ShareRoom")
			{
				return this.UnpackRoomData(data);
			}
			if (array != null && array[0] == "ShareTeam")
			{
				return this.UnpackTeamData(data);
			}
			if (array == null || !this.IsSupport3rdAPP(array[0]))
			{
				return false;
			}
			if (!this.UnPackQQGameTeamData(data))
			{
				this.m_QQGameTeamStr = string.Empty;
				this.m_WakeupOpenId = string.Empty;
				return false;
			}
			return true;
		}

		public bool UnpackRoomData(string data)
		{
			Debug.Log("UnpackRoomData");
			string[] array = data.Split(new char[]
			{
				'_'
			});
			if (array == null || array.Length != 9 || array[0] != "ShareRoom")
			{
				return false;
			}
			if (!this.CheckEnterShareTeamLimit(ref array[7], ref array[8], true, false))
			{
				return false;
			}
			this.m_ShareRoom = new CSPKG_JOINMULTIGAMEREQ();
			if (int.TryParse(array[1], ref this.m_ShareRoom.iRoomEntity) && uint.TryParse(array[2], ref this.m_ShareRoom.dwRoomID) && uint.TryParse(array[3], ref this.m_ShareRoom.dwRoomSeq) && byte.TryParse(array[4], ref this.m_ShareRoom.bMapType) && uint.TryParse(array[5], ref this.m_ShareRoom.dwMapId) && ulong.TryParse(array[6], ref this.m_ShareRoom.ullFeature))
			{
				if (Singleton<LobbyLogic>.get_instance().isLogin)
				{
					this.SendRoomDataMsg(true);
				}
				return true;
			}
			return false;
		}

		public string PackTeamData(ulong uuid, uint dwTeamId, uint dwTeamSeq, int iTeamEntity, ulong ullTeamFeature, byte bInviterGradeOfRank, byte bGameMode, byte bPkAI, byte bMapType, uint dwMapId, byte bAILevel, byte bMaxTeamNum, byte bTeamGradeOfRank)
		{
			string text = string.Format("ShareTeam_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}_{12}_{13}_{14}", new object[]
			{
				uuid,
				dwTeamId,
				dwTeamSeq,
				iTeamEntity,
				ullTeamFeature,
				bInviterGradeOfRank,
				bGameMode,
				bPkAI,
				bMapType,
				dwMapId,
				bAILevel,
				(int)Application.platform,
				ApolloConfig.platform,
				bMaxTeamNum,
				bTeamGradeOfRank
			});
			Debug.Log(text);
			return text;
		}

		public bool UnpackTeamData(string data)
		{
			Debug.Log("UnpackTeamData");
			string[] array = data.Split(new char[]
			{
				'_'
			});
			if (array == null || array.Length != 16 || array[0] != "ShareTeam")
			{
				return false;
			}
			if (!this.CheckEnterShareTeamLimit(ref array[12], ref array[13], true, false))
			{
				return false;
			}
			this.m_ShareTeam = new COMDT_INVITE_JOIN_INFO();
			this.m_ShareTeam.iInviteType = 2;
			this.m_ShareTeam.stInviteDetail.set_stInviteJoinTeam(new COMDT_INVITE_TEAM_DETAIL());
			try
			{
				if (ulong.TryParse(array[1], ref this.m_ShareTeam.stInviterInfo.ullUid) && uint.TryParse(array[2], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().dwTeamId) && uint.TryParse(array[3], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().dwTeamSeq) && int.TryParse(array[4], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().iTeamEntity) && ulong.TryParse(array[5], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().ullTeamFeature) && byte.TryParse(array[6], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().bGradeOfInviter) && byte.TryParse(array[7], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().stTeamInfo.bGameMode) && byte.TryParse(array[8], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().stTeamInfo.bPkAI) && byte.TryParse(array[9], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().stTeamInfo.bMapType) && uint.TryParse(array[10], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().stTeamInfo.dwMapId) && byte.TryParse(array[11], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().stTeamInfo.bAILevel) && byte.TryParse(array[14], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().stTeamInfo.bMaxTeamNum) && byte.TryParse(array[15], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().stTeamInfo.bGradeOfRank) && Singleton<LobbyLogic>.get_instance().isLogin)
				{
					this.SendTeamDataMsg(true);
				}
			}
			catch (Exception ex)
			{
				Debug.Log(ex.ToString());
			}
			return false;
		}

		private void SendTeamDataMsg(bool clearData = true)
		{
			if (this.m_ShareTeam != null)
			{
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5210u);
				cSPkg.stPkgData.get_stJoinTeamReq().stInviteJoinInfo = this.m_ShareTeam;
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
				Debug.Log("share teamdata msg");
			}
			if (clearData)
			{
				this.ClearTeamDataMsg();
			}
		}

		private void ClearTeamDataMsg()
		{
			this.m_ShareTeam = null;
			this.m_ShareStr = string.Empty;
		}

		public string PackRecruitFriendInfo()
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				return string.Empty;
			}
			ApolloAccountInfo accountInfo = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false);
			if (accountInfo == null)
			{
				return string.Empty;
			}
			return string.Format("mqq_{0}_{1}_{2}_{3}", new object[]
			{
				masterRoleInfo.playerUllUID,
				MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID,
				accountInfo.get_OpenId(),
				CUICommonSystem.GetPlatformArea()
			});
		}

		public bool UnpackRecruitFriendInfo(string data)
		{
			string[] array = data.Split(new char[]
			{
				'_'
			});
			if (array == null || array.Length != 5 || array[0] != "mqq")
			{
				return false;
			}
			Debug.Log("UnpackRecruitFriendInfo " + data);
			this.m_RecruitSource = new COMDT_RECRUITMENT_SOURCE();
			uint dwLogicWorldId = 0u;
			uint.TryParse(array[2], ref dwLogicWorldId);
			this.m_RecruitSource.stUin.dwLogicWorldId = dwLogicWorldId;
			uint num = 0u;
			uint.TryParse(array[1], ref num);
			this.m_RecruitSource.stUin.ullUid = (ulong)num;
			int iPlatId = 0;
			int.TryParse(array[4], ref iPlatId);
			this.m_RecruitSource.iPlatId = iPlatId;
			Utility.StringToByteArray(array[3], ref this.m_RecruitSource.szOpenID);
			return true;
		}

		public void ShareRecruitFriend(string title, string desc)
		{
			Singleton<ApolloHelper>.GetInstance().ShareRecruitFriend(title, desc, this.PackRecruitFriendInfo());
		}

		public void SendShareDataMsg()
		{
			if (!string.IsNullOrEmpty(this.m_ShareStr))
			{
				this.UnPackSNSDataStr(this.m_ShareStr);
				this.m_ShareStr = string.Empty;
			}
			else
			{
				if (this.m_ShareRoom != null)
				{
					this.SendRoomDataMsg(string.IsNullOrEmpty(this.m_RoomModeId));
				}
				if (this.m_ShareTeam != null)
				{
					this.SendTeamDataMsg(string.IsNullOrEmpty(this.m_RoomModeId));
				}
				if (!string.IsNullOrEmpty(this.m_RoomModeId))
				{
					this.SendQQGameTeamCreateMsg(this.m_RoomModeId);
				}
				this.m_ShareStr = string.Empty;
			}
		}

		public void ClearShareDataMsg()
		{
			this.ClearTeamDataMsg();
			this.ClearRoomData();
		}

		private void SendRoomDataMsg(bool clearData = true)
		{
			if (this.m_ShareRoom != null)
			{
				CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(5208u);
				cSPkg.stPkgData.set_stJoinMultiGameReq(this.m_ShareRoom);
				Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
				Singleton<WatchController>.GetInstance().Stop();
				Debug.Log("share roomdata msg");
			}
			if (clearData)
			{
				this.ClearRoomData();
			}
		}

		private void ClearRoomData()
		{
			this.m_ShareRoom = null;
			this.m_ShareStr = string.Empty;
		}

		public string PackQQGameTeamData(int iRoomEntity, uint dwRoomID, uint dwRoomSeq, ulong ullFeature)
		{
			if (string.IsNullOrEmpty(this.m_QQGameTeamStr))
			{
				return string.Empty;
			}
			string text = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", new object[]
			{
				iRoomEntity,
				dwRoomID,
				dwRoomSeq,
				ullFeature,
				(int)Application.platform,
				ApolloConfig.platform
			});
			Debug.Log(text);
			return text;
		}

		public string PackQQGameTeamData(ulong uuid, uint dwTeamId, uint dwTeamSeq, int iTeamEntity, ulong ullTeamFeature, byte bInviterGradeOfRank, byte bGameMode, byte bPkAI, byte bAILevel, int maxTeamerNum)
		{
			if (string.IsNullOrEmpty(this.m_QQGameTeamStr))
			{
				return string.Empty;
			}
			string text = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}_{9}_{10}_{11}", new object[]
			{
				uuid,
				dwTeamId,
				dwTeamSeq,
				iTeamEntity,
				ullTeamFeature,
				bInviterGradeOfRank,
				bGameMode,
				bPkAI,
				bAILevel,
				maxTeamerNum,
				(int)Application.platform,
				ApolloConfig.platform
			});
			Debug.Log(text);
			return text;
		}

		public bool UnPackQQGameTeamData(string data)
		{
			Debug.Log("UnpackQQGameTeamData");
			ApolloAccountInfo accountInfo = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false);
			if (accountInfo != null && accountInfo.get_OpenId() != this.m_WakeupOpenId)
			{
				Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(Singleton<CTextManager>.GetInstance().GetText("Common_Login_Different_Account_Tip"), enUIEventID.Login_Change_Account_Yes, enUIEventID.Login_Change_Account_No, false);
				Singleton<ApolloHelper>.GetInstance().IsWXGameCenter = false;
				Singleton<ApolloHelper>.GetInstance().IsQQGameCenter = false;
				return true;
			}
			this.m_QQGameTeamStr = data;
			this.m_bIsQQGameTeamOwner = false;
			string[] array = data.Split(new char[]
			{
				'_'
			});
			if (array == null)
			{
				return false;
			}
			int num = array.Length;
			if (num > 0)
			{
				this.m_MarkStr = array[0];
			}
			if (array == null || (num != 4 && num != 10 && array.Length != 16))
			{
				return false;
			}
			if (!this.IsSupport3rdAPP(array[0]))
			{
				return false;
			}
			if (!this.CheckEnterShareTeamLimit(ref array[num - 2], ref array[num - 1], num != 4, false))
			{
				return false;
			}
			this.m_RoomModeId = array[1];
			string[] array2 = this.m_RoomModeId.Split(new char[]
			{
				'-'
			});
			if (array2 == null || array2.Length != 3)
			{
				return false;
			}
			COM_ROOM_TYPE cOM_ROOM_TYPE = Convert.ToInt32(array2[0]);
			COM_BATTLE_MAP_TYPE cOM_BATTLE_MAP_TYPE = Convert.ToInt32(array2[1]);
			uint dwMapId = Convert.ToUInt32(array2[2]);
			if (num == 4)
			{
				this.SendQQGameTeamCreateMsg(this.m_RoomModeId);
				this.m_bIsQQGameTeamOwner = true;
				return true;
			}
			if (cOM_ROOM_TYPE == 1)
			{
				this.m_ShareTeam = new COMDT_INVITE_JOIN_INFO();
				this.m_ShareTeam.iInviteType = 2;
				this.m_ShareTeam.stInviteDetail.set_stInviteJoinTeam(new COMDT_INVITE_TEAM_DETAIL());
				if (ulong.TryParse(array[4], ref this.m_ShareTeam.stInviterInfo.ullUid) && uint.TryParse(array[5], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().dwTeamId) && uint.TryParse(array[6], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().dwTeamSeq) && int.TryParse(array[7], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().iTeamEntity) && ulong.TryParse(array[8], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().ullTeamFeature) && byte.TryParse(array[9], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().bGradeOfInviter) && byte.TryParse(array[10], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().stTeamInfo.bGameMode) && byte.TryParse(array[11], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().stTeamInfo.bPkAI) && byte.TryParse(array[12], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().stTeamInfo.bAILevel) && byte.TryParse(array[13], ref this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().stTeamInfo.bMaxTeamNum))
				{
					this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().stTeamInfo.bMapType = cOM_BATTLE_MAP_TYPE;
					this.m_ShareTeam.stInviteDetail.get_stInviteJoinTeam().stTeamInfo.dwMapId = dwMapId;
					if (Singleton<LobbyLogic>.get_instance().isLogin)
					{
						this.SendTeamDataMsg(false);
					}
					return true;
				}
			}
			else if (cOM_ROOM_TYPE == 2)
			{
				this.m_ShareRoom = new CSPKG_JOINMULTIGAMEREQ();
				if (int.TryParse(array[4], ref this.m_ShareRoom.iRoomEntity) && uint.TryParse(array[5], ref this.m_ShareRoom.dwRoomID) && uint.TryParse(array[6], ref this.m_ShareRoom.dwRoomSeq) && ulong.TryParse(array[7], ref this.m_ShareRoom.ullFeature))
				{
					this.m_ShareRoom.bMapType = cOM_BATTLE_MAP_TYPE;
					this.m_ShareRoom.dwMapId = dwMapId;
					if (Singleton<LobbyLogic>.get_instance().isLogin)
					{
						this.SendRoomDataMsg(false);
					}
					return true;
				}
			}
			return false;
		}

		private void SendQQGameTeamCreateMsg(string roomInfoStr)
		{
			Debug.Log("QQGameTeamCreate");
			if (string.IsNullOrEmpty(roomInfoStr))
			{
				return;
			}
			string[] array = roomInfoStr.Split(new char[]
			{
				'-'
			});
			if (array == null || array.Length != 3)
			{
				return;
			}
			COM_ROOM_TYPE cOM_ROOM_TYPE = Convert.ToInt32(array[0]);
			COM_BATTLE_MAP_TYPE cOM_BATTLE_MAP_TYPE = Convert.ToInt32(array[1]);
			uint mapId = Convert.ToUInt32(array[2]);
			if (cOM_ROOM_TYPE == 1)
			{
				if (Singleton<LobbyLogic>.get_instance().isLogin)
				{
					ResDT_LevelCommonInfo pvpMapCommonInfo = CLevelCfgLogicManager.GetPvpMapCommonInfo(cOM_BATTLE_MAP_TYPE, mapId);
					CMatchingSystem.ReqCreateTeam(mapId, false, cOM_BATTLE_MAP_TYPE, (int)(pvpMapCommonInfo.bMaxAcntNum / 2), 2, false);
				}
				return;
			}
			if (cOM_ROOM_TYPE == 2)
			{
				if (Singleton<LobbyLogic>.get_instance().isLogin)
				{
					CRoomSystem.ReqCreateRoom(mapId, cOM_BATTLE_MAP_TYPE, false);
				}
				return;
			}
		}

		public void SendQQGameTeamStateChgMsg(ShareSys.QQGameTeamEventType EventType, COM_ROOM_TYPE roomType = 0, byte mapType = 0, uint mapID = 0u, string roomStr = "")
		{
			Debug.Log("QQGameTeamStateChg" + EventType);
			if (string.IsNullOrEmpty(this.m_QQGameTeamStr))
			{
				return;
			}
			if (EventType == ShareSys.QQGameTeamEventType.join)
			{
				if (!this.CheckQQGameTeamInfo(roomType, mapType, mapID))
				{
					this.m_QQGameTeamStr = string.Empty;
					this.m_WakeupOpenId = string.Empty;
					return;
				}
				this.m_RoomInfoStr = roomStr;
				if (this.m_bIsQQGameTeamOwner)
				{
					Singleton<CUIManager>.GetInstance().OpenTips("QQGameTeam_Tips1", true, 1.5f, null, new object[0]);
					this.m_bIsQQGameTeamOwner = false;
				}
			}
			if (this.m_RoomInfoStr == null)
			{
				return;
			}
			ApolloAccountInfo accountInfo = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false);
			if (accountInfo == null)
			{
				return;
			}
			string text = string.Empty;
			if (this.m_MarkStr == "PenguinEsport")
			{
				text = string.Format("http://game.egame.qq.com/cgi-bin/game_notify?event={0}&openid={1}&openkey={2}&gamedata={3}&appid={4}", new object[]
				{
					EventType.ToString(),
					accountInfo.get_OpenId(),
					accountInfo.GetToken(1).get_Value(),
					this.m_QQGameTeamStr + "_" + this.m_RoomInfoStr,
					ApolloConfig.GetAppID()
				});
			}
			else if (this.m_MarkStr == "GameHelper")
			{
				text = string.Format("http://api.helper.qq.com/play/smobanotify?event={0}&openid={1}&openkey={2}&gamedata={3}", new object[]
				{
					EventType.ToString(),
					accountInfo.get_OpenId(),
					accountInfo.GetToken(1).get_Value(),
					this.m_QQGameTeamStr + "_" + this.m_RoomInfoStr
				});
			}
			else
			{
				text = string.Format("http://openmobile.qq.com/gameteam/game_notify?event={0}&openid={1}&openkey={2}&gamedata={3}", new object[]
				{
					EventType.ToString(),
					accountInfo.get_OpenId(),
					accountInfo.GetToken(1).get_Value(),
					this.m_QQGameTeamStr + "_" + this.m_RoomInfoStr
				});
			}
			Debug.Log("QQGameTeamStateChg:" + text);
			base.StartCoroutine(this.QQGameTeamStateChgGet(text));
			if (EventType == ShareSys.QQGameTeamEventType.end)
			{
				this.m_QQGameTeamStr = string.Empty;
				this.m_WakeupOpenId = string.Empty;
				this.m_RoomInfoStr = string.Empty;
			}
			this.ClearQQGameCreateInfo();
		}

		[DebuggerHidden]
		private IEnumerator QQGameTeamStateChgGet(string url)
		{
			ShareSys.<QQGameTeamStateChgGet>c__Iterator2E <QQGameTeamStateChgGet>c__Iterator2E = new ShareSys.<QQGameTeamStateChgGet>c__Iterator2E();
			<QQGameTeamStateChgGet>c__Iterator2E.url = url;
			<QQGameTeamStateChgGet>c__Iterator2E.<$>url = url;
			return <QQGameTeamStateChgGet>c__Iterator2E;
		}

		private bool CheckQQGameTeamInfo(COM_ROOM_TYPE roomType, byte mapType, uint mapID)
		{
			string[] array = this.m_QQGameTeamStr.Split(new char[]
			{
				'_'
			});
			if (array == null)
			{
				return false;
			}
			int num = array.Length;
			if (array == null || (num != 4 && num != 10 && array.Length != 16))
			{
				return false;
			}
			if (!this.IsSupport3rdAPP(array[0]))
			{
				return false;
			}
			string text = array[1];
			string[] array2 = text.Split(new char[]
			{
				'-'
			});
			return array2 != null && array2.Length == 3 && Convert.ToUInt32(array2[0]) == roomType && Convert.ToByte(array2[1]) == mapType && Convert.ToUInt32(array2[2]) == mapID;
		}

		private void ClearQQGameCreateInfo()
		{
			this.m_ShareStr = string.Empty;
			this.m_RoomModeId = string.Empty;
			this.m_ShareRoom = null;
			this.m_ShareTeam = null;
		}

		public bool IsQQGameTeamCreate()
		{
			return !string.IsNullOrEmpty(this.m_QQGameTeamStr);
		}

		public void AddshareReportInfo(uint dwType, uint dwSubType)
		{
			bool flag = false;
			for (int i = 0; i < this.m_ShareReportInfoList.get_Count(); i++)
			{
				CSDT_SHARE_TLOG_INFO cSDT_SHARE_TLOG_INFO = this.m_ShareReportInfoList.get_Item(i);
				if (cSDT_SHARE_TLOG_INFO.dwType == dwType && cSDT_SHARE_TLOG_INFO.dwSubType == dwSubType)
				{
					cSDT_SHARE_TLOG_INFO.dwCnt += 1u;
					flag = true;
				}
			}
			if (!flag)
			{
				CSDT_SHARE_TLOG_INFO cSDT_SHARE_TLOG_INFO2 = new CSDT_SHARE_TLOG_INFO();
				cSDT_SHARE_TLOG_INFO2.dwCnt = 1u;
				cSDT_SHARE_TLOG_INFO2.dwType = dwType;
				cSDT_SHARE_TLOG_INFO2.dwSubType = dwSubType;
				this.m_ShareReportInfoList.Add(cSDT_SHARE_TLOG_INFO2);
			}
		}

		private void ReportShareInfo()
		{
			CSDT_TRANK_TLOG_INFO[] uiTlog = Singleton<RankingSystem>.get_instance().GetUiTlog();
			if (uiTlog.Length == 0 && this.m_ShareReportInfoList.get_Count() == 0)
			{
				return;
			}
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(4307u);
			int num = this.m_ShareReportInfoList.get_Count();
			cSPkg.stPkgData.get_stShareTLogReq().bNum = (byte)num;
			for (int i = 0; i < num; i++)
			{
				cSPkg.stPkgData.get_stShareTLogReq().astShareDetail[i].dwType = this.m_ShareReportInfoList.get_Item(i).dwType;
				cSPkg.stPkgData.get_stShareTLogReq().astShareDetail[i].dwSubType = this.m_ShareReportInfoList.get_Item(i).dwSubType;
				cSPkg.stPkgData.get_stShareTLogReq().astShareDetail[i].dwCnt = this.m_ShareReportInfoList.get_Item(i).dwCnt;
			}
			num = uiTlog.Length;
			cSPkg.stPkgData.get_stShareTLogReq().dwTrankNum = (uint)num;
			for (int j = 0; j < num; j++)
			{
				cSPkg.stPkgData.get_stShareTLogReq().astTrankDetail[j].dwType = uiTlog[j].dwType;
				cSPkg.stPkgData.get_stShareTLogReq().astTrankDetail[j].dwCnt = uiTlog[j].dwCnt;
			}
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			this.m_ShareReportInfoList.Clear();
			Singleton<RankingSystem>.get_instance().ClearUiTlog();
		}

		[MessageHandler(4308)]
		public static void OnShareReport(CSPkg msg)
		{
			Debug.Log("share report " + msg.stPkgData.get_stShareTLogRsp().iErrCode);
		}
	}
}
