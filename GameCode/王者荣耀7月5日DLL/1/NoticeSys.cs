using Apollo;
using Assets.Scripts.Framework;
using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NoticeSys : MonoSingleton<NoticeSys>
{
	public enum NOTICE_STATE
	{
		LOGIN_Before,
		LOGIN_After
	}

	public enum BTN_DOSOMTHING
	{
		BTN_DOSOMTHING_NONE,
		BTN_DOSOMTHING_URL,
		BTN_DOSOMTHING_GAME,
		BTN_DOSOMTHING_NOTSHOW
	}

	public static class UrlX
	{
		private static bool IsSafe(char ch)
		{
			if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
			{
				return true;
			}
			switch (ch)
			{
			case '!':
			case '\'':
			case '(':
			case ')':
			case '*':
			case '-':
			case '.':
				return true;
			case '"':
			case '#':
			case '$':
			case '%':
			case '&':
			case '+':
			case ',':
				IL_75:
				if (ch != '_')
				{
					return false;
				}
				return true;
			}
			goto IL_75;
		}

		private static char IntToHex(int n)
		{
			if (n <= 9)
			{
				return (char)(n + 48);
			}
			return (char)(n - 10 + 97);
		}

		public static byte[] UrlEncodeBytesToBytes(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				char c = (char)bytes[offset + i];
				if (c == ' ')
				{
					num++;
				}
				else if (!NoticeSys.UrlX.IsSafe(c))
				{
					num2++;
				}
			}
			if (!alwaysCreateReturnValue && num == 0 && num2 == 0)
			{
				return bytes;
			}
			byte[] array = new byte[count + num2 * 2];
			int num3 = 0;
			for (int j = 0; j < count; j++)
			{
				byte b = bytes[offset + j];
				char c2 = (char)b;
				if (NoticeSys.UrlX.IsSafe(c2))
				{
					array[num3++] = b;
				}
				else if (c2 == ' ')
				{
					array[num3++] = 43;
				}
				else
				{
					array[num3++] = 37;
					array[num3++] = (byte)NoticeSys.UrlX.IntToHex(b >> 4 & 15);
					array[num3++] = (byte)NoticeSys.UrlX.IntToHex((int)(b & 15));
				}
			}
			return array;
		}

		public static byte[] UrlEncodeToBytes(string str)
		{
			if (str == null)
			{
				return null;
			}
			return NoticeSys.UrlX.UrlEncodeToBytes(str, Encoding.get_UTF8());
		}

		public static byte[] UrlEncodeToBytes(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			byte[] bytes = e.GetBytes(str);
			return NoticeSys.UrlX.UrlEncodeToBytes(bytes);
		}

		public static byte[] UrlEncodeToBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			return NoticeSys.UrlX.UrlEncodeBytesToBytes(bytes, 0, bytes.Length, false);
		}

		public static string UrlEncode(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			return Encoding.get_ASCII().GetString(NoticeSys.UrlX.UrlEncodeToBytes(str, e));
		}

		public static string UrlEncode(string str)
		{
			if (str == null)
			{
				return null;
			}
			return NoticeSys.UrlX.UrlEncode(str, Encoding.get_UTF8());
		}

		public static string UrlEncode(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			return Encoding.get_ASCII().GetString(NoticeSys.UrlX.UrlEncodeToBytes(bytes));
		}
	}

	public static string s_formNoticeLoginPath = CUIUtility.s_IDIP_Form_Dir + "Form_NoticeLoginBefore.prefab";

	private CUIFormScript m_Form;

	private Image m_ImageContent;

	private Image m_ImageDefault;

	public static bool m_bShowLoginBefore = false;

	private bool m_bShow;

	private Text m_TextContent;

	private Text m_Title;

	private GameObject m_TitleBoard;

	private GameObject m_ImageTop;

	private Image m_PanelImage;

	private ListView<ApolloNoticeData> m_NoticeDataList = new ListView<ApolloNoticeData>();

	private NoticeSys.NOTICE_STATE m_CurState;

	private bool m_bLoadImage;

	private NoticeSys.BTN_DOSOMTHING m_btnDoSth;

	private string m_btnUrl = string.Empty;

	private string m_ImageModleTitle = string.Empty;

	private bool m_bGoto;

	private UrlAction m_urlAction;

	protected override void Init()
	{
		Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.MSDK_NOTICE_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnCloseIDIPForm));
		Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.MSDK_NOTICE_Btn_Complete, new CUIEventManager.OnUIEventHandler(this.OnBtnComplete));
		Singleton<CTimerManager>.GetInstance().AddTimer(3000, 1, new CTimer.OnTimeUpHandler(this.OnTimeEnd));
	}

	private void OnTimeEnd(int timersequence)
	{
		this.PreLoadImage();
	}

	public void PreLoadImage()
	{
		List<string> noticeUrl = Singleton<ApolloHelper>.GetInstance().GetNoticeUrl(0, "1");
		for (int i = 0; i < noticeUrl.get_Count(); i++)
		{
			base.StartCoroutine(MonoSingleton<IDIPSys>.GetInstance().DownloadImage(noticeUrl.get_Item(i), delegate(Texture2D text2)
			{
			}, 0));
		}
		List<string> noticeUrl2 = Singleton<ApolloHelper>.GetInstance().GetNoticeUrl(0, "2");
		for (int j = 0; j < noticeUrl2.get_Count(); j++)
		{
			base.StartCoroutine(MonoSingleton<IDIPSys>.GetInstance().DownloadImage(noticeUrl2.get_Item(j), delegate(Texture2D text2)
			{
			}, 0));
		}
	}

	public void OnOpenForm(ApolloNoticeInfo noticeInfo, NoticeSys.NOTICE_STATE noticeState)
	{
		this.m_CurState = noticeState;
		this.m_NoticeDataList = new ListView<ApolloNoticeData>();
		int count = noticeInfo.get_DataList().get_Count();
		for (int i = 0; i < count; i++)
		{
			ApolloNoticeData apolloNoticeData = noticeInfo.get_DataList().get_Item(i);
			if (apolloNoticeData.get_MsgType() == null)
			{
				this.m_NoticeDataList.Add(apolloNoticeData);
			}
		}
		if (count > 0)
		{
			this.ShowNoticeWindow(0);
		}
		else if (this.m_CurState == NoticeSys.NOTICE_STATE.LOGIN_After)
		{
			this.ShowOtherTips();
		}
	}

	public void DelayShowNoticeWindow()
	{
		this.ShowNoticeWindow(0);
	}

	private void ShowNoticeWindow(int idx)
	{
		if (this.m_NoticeDataList != null && idx >= 0 && idx < this.m_NoticeDataList.get_Count())
		{
			ApolloNoticeData apolloNoticeData = this.m_NoticeDataList.get_Item(idx);
			this.m_NoticeDataList.Remove(apolloNoticeData);
			this.ProcessShowNoticeWindown(apolloNoticeData);
		}
	}

	private void ProcessShowNoticeWindown(ApolloNoticeData noticeData)
	{
		this.m_bGoto = false;
		this.m_bLoadImage = false;
		string msgID = noticeData.get_MsgID();
		string openID = noticeData.get_OpenID();
		string text = noticeData.get_MsgUrl();
		ListView<UrlAction> listView = UrlAction.ParseFromText(noticeData.get_ContentUrl(), null);
		if (listView.get_Count() > 0)
		{
			this.m_urlAction = listView.get_Item(0);
		}
		else
		{
			this.m_urlAction = null;
		}
		if (text == null)
		{
			text = string.Empty;
		}
		APOLLO_NOTICETYPE msgType = noticeData.get_MsgType();
		string startTime = noticeData.get_StartTime();
		APOLLO_NOTICE_CONTENTTYPE contentType = noticeData.get_ContentType();
		string msgTitle = noticeData.get_MsgTitle();
		string msgContent = noticeData.get_MsgContent();
		Debug.Log(string.Concat(new object[]
		{
			"noticesysy onopenform MsgUrl",
			text,
			"msgtitle = ",
			msgTitle,
			" content ",
			msgContent,
			" openid= ",
			openID,
			" MsgType  = ",
			msgType,
			"Msg Scene",
			noticeData.get_MsgScene()
		}));
		uint num = 0u;
		if (this.m_CurState == NoticeSys.NOTICE_STATE.LOGIN_After)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				num = masterRoleInfo.PvpLevel;
			}
		}
		else
		{
			num = 0u;
		}
		this.m_btnUrl = string.Empty;
		this.m_btnDoSth = NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_NONE;
		if (msgType == null)
		{
			bool flag = false;
			bool flag2 = this.CheckIsBtnUrl(text, ref flag, ref this.m_ImageModleTitle, ref this.m_btnDoSth, ref this.m_btnUrl);
			if (flag2)
			{
				if (flag)
				{
					bool flag3 = false;
					flag2 = this.CheckIsBtnUrl("#" + this.m_btnUrl + "&end", ref flag3, ref this.m_ImageModleTitle, ref this.m_btnDoSth, ref this.m_btnUrl);
				}
				if (this.m_btnDoSth == NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_NOTSHOW && this.m_btnUrl != MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.logicWorldID.ToString())
				{
					Debug.Log("noticesys not show " + this.m_btnUrl);
					return;
				}
				Debug.Log("find url " + this.m_btnUrl + " ori = " + text);
			}
			else
			{
				Debug.Log("find url   ori = " + text);
			}
			if (this.m_CurState == NoticeSys.NOTICE_STATE.LOGIN_After && num <= 5u)
			{
				this.m_btnDoSth = NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_NONE;
				this.m_btnUrl = string.Empty;
			}
			if (this.m_Form == null)
			{
				this.m_Form = Singleton<CUIManager>.GetInstance().OpenForm(NoticeSys.s_formNoticeLoginPath, false, true);
			}
			Transform transform = this.m_Form.gameObject.transform.Find("Panel/BtnGroup/Button_Complte");
			if (this.m_btnUrl != string.Empty && this.m_btnDoSth != NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_NONE)
			{
				this.m_bGoto = true;
			}
			else
			{
				this.m_bGoto = false;
			}
			this.m_bShow = true;
			this.m_TextContent = Utility.GetComponetInChild<Text>(this.m_Form.gameObject, "Panel/ScrollRect/Content/Text");
			this.m_ImageContent = Utility.GetComponetInChild<Image>(this.m_Form.gameObject, "Panel/Image");
			this.m_ImageDefault = Utility.GetComponetInChild<Image>(this.m_Form.gameObject, "Panel/ImageDefalut");
			this.m_Title = Utility.GetComponetInChild<Text>(this.m_Form.gameObject, "Panel/GameObject/Title/ContentTitle");
			this.m_TitleBoard = this.m_Form.gameObject.transform.Find("Panel/GameObject/Title").gameObject;
			this.m_TextContent.gameObject.CustomSetActive(false);
			this.m_ImageContent.gameObject.CustomSetActive(false);
			if (this.m_ImageDefault)
			{
				this.m_ImageDefault.gameObject.CustomSetActive(false);
			}
			this.m_Title.text = msgTitle;
			if (contentType != 1)
			{
				if (contentType == null)
				{
					this.m_TextContent.gameObject.CustomSetActive(true);
					this.m_TextContent.text = msgContent;
					this.m_TitleBoard.CustomSetActive(true);
					RectTransform component = this.m_TextContent.transform.parent.gameObject.GetComponent<RectTransform>();
					if (component)
					{
						component.sizeDelta = new Vector2(0f, this.m_TextContent.preferredHeight + 50f);
					}
				}
				else if (contentType == 2 && this.m_urlAction != null)
				{
					this.m_bShow = true;
					this.m_TitleBoard.CustomSetActive(true);
					this.m_Title.text = this.m_ImageModleTitle;
					if (this.m_ImageDefault)
					{
						this.m_ImageDefault.gameObject.CustomSetActive(true);
					}
					this.m_ImageContent.gameObject.CustomSetActive(false);
					base.StartCoroutine(MonoSingleton<IDIPSys>.GetInstance().DownloadImage(this.m_urlAction.target, delegate(Texture2D text2)
					{
						if (this.m_bShow && this.m_ImageContent != null)
						{
							this.m_ImageContent.gameObject.CustomSetActive(true);
							if (this.m_ImageDefault)
							{
								this.m_ImageDefault.gameObject.CustomSetActive(false);
							}
							this.m_bLoadImage = true;
							this.m_ImageContent.SetSprite(Sprite.Create(text2, new Rect(0f, 0f, (float)text2.width, (float)text2.height), new Vector2(0.5f, 0.5f)), ImageAlphaTexLayout.None);
							BugLocateLogSys.Log("noticesysy contenturl " + this.m_urlAction.target);
						}
					}, 0));
				}
			}
		}
	}

	private void ProcessButton(uint nLevel, bool isGoto)
	{
		if (this.m_Form == null)
		{
			return;
		}
		Transform transform = this.m_Form.gameObject.transform.Find("Panel/Image/Button_Complte");
		if (nLevel <= 5u)
		{
			transform.gameObject.CustomSetActive(false);
		}
		else if (isGoto)
		{
			transform.gameObject.CustomSetActive(true);
		}
		else
		{
			transform.gameObject.CustomSetActive(false);
		}
	}

	private bool CheckIsBtnUrl(string msgUrl, ref bool bTitle, ref string sTitle, ref NoticeSys.BTN_DOSOMTHING btnDoSth, ref string url)
	{
		if (msgUrl == null)
		{
			url = string.Empty;
			btnDoSth = NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_NONE;
			bTitle = false;
			return false;
		}
		string text = "#";
		string text2 = "&end";
		string text3 = "&";
		int num = msgUrl.IndexOf(text);
		if (num < 0)
		{
			url = string.Empty;
			btnDoSth = NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_NONE;
			bTitle = false;
			return false;
		}
		int num2 = msgUrl.IndexOf(text2);
		if (num2 < 0)
		{
			url = string.Empty;
			btnDoSth = NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_NONE;
			bTitle = false;
			return false;
		}
		int num3 = msgUrl.IndexOf(text3);
		if (num3 < 0)
		{
			url = string.Empty;
			btnDoSth = NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_NONE;
			bTitle = false;
			return false;
		}
		string text4 = msgUrl.Substring(num + text.get_Length(), num3 - num - text.get_Length());
		if (text4.Contains("title="))
		{
			string text5 = text4.Substring("title=".get_Length());
			sTitle = Uri.UnescapeDataString(text5);
			bTitle = true;
		}
		else if (text4.Contains("button=0"))
		{
			btnDoSth = NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_URL;
		}
		else if (text4.Contains("button=1"))
		{
			btnDoSth = NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_GAME;
		}
		else if (text4.Contains("button=2"))
		{
			btnDoSth = NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_NOTSHOW;
		}
		string text6 = string.Empty;
		if (num2 - num3 - text3.get_Length() > 0)
		{
			text6 = msgUrl.Substring(num3 + text3.get_Length(), num2 - num3 - text3.get_Length());
		}
		if (bTitle)
		{
			url = text6;
			btnDoSth = NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_NONE;
			return true;
		}
		if (text6.Contains("url="))
		{
			url = text6.Substring("url=".get_Length());
			return true;
		}
		url = string.Empty;
		btnDoSth = NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_NONE;
		bTitle = false;
		return false;
	}

	private void OnBtnComplete(CUIEvent ciEvent)
	{
		if (this.m_urlAction != null && this.m_urlAction.Execute())
		{
			this.OnCloseIDIPForm(null);
		}
		else if (this.m_bGoto && this.m_bShow && this.m_btnUrl != string.Empty)
		{
			NoticeSys.BTN_DOSOMTHING btnDoSth = this.m_btnDoSth;
			string text = this.m_btnUrl;
			this.OnCloseIDIPForm(null);
			if (btnDoSth == NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_URL)
			{
				CUICommonSystem.OpenUrl(text, true, 0);
			}
			else if (btnDoSth == NoticeSys.BTN_DOSOMTHING.BTN_DOSOMTHING_GAME)
			{
				int num = 0;
				int.TryParse(text, ref num);
				if (num > 0)
				{
					CUICommonSystem.JumpForm(num, 0, 0);
				}
			}
			text = string.Empty;
		}
	}

	private void OnCloseIDIPForm(CUIEvent uiEvent)
	{
		if (this.m_Form != null)
		{
			Singleton<CUIManager>.GetInstance().CloseForm(this.m_Form);
			this.m_Form = null;
		}
		this.m_bShow = false;
		if (this.m_ImageContent && this.m_ImageContent.sprite && this.m_bLoadImage)
		{
			Object.Destroy(this.m_ImageContent.sprite);
		}
		if (this.m_NoticeDataList.get_Count() > 0)
		{
			this.ShowNoticeWindow(0);
		}
		else
		{
			this.ShowOtherTips();
		}
	}

	private void ShowOtherTips()
	{
		if (Singleton<GameStateCtrl>.get_instance().GetCurrentState() is LobbyState)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null && ActivitySys.NeedShowWhenLogin() && !MonoSingleton<NewbieGuideManager>.GetInstance().isNewbieGuiding)
			{
				ActivitySys.UpdateLoginShowCnt();
				Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Activity_OpenForm);
			}
			else
			{
				MonoSingleton<NobeSys>.GetInstance().ShowDelayNobeLoseTipsInfo();
				MonoSingleton<PandroaSys>.GetInstance().ShowPopNews();
			}
		}
	}
}
