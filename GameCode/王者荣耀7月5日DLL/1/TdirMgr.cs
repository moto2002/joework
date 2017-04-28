using Apollo;
using Assets.Scripts.Framework;
using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class TdirMgr : MonoSingleton<TdirMgr>
{
	public delegate void TdirManagerEvent();

	private const int RootNodeID = 0;

	private const int PrivateNodeID = 1;

	private const int TestNodeID = 2;

	private const int MaxSyncTime = 3;

	private const string LoginTimeKey = "LoginTimeKey";

	private const string LoginFailTimesKey = "LoginFailTimesKey";

	private const int FailTimesLimit1 = 3;

	private const int FailTimesLimit2 = 6;

	private const int FailTimesDuration1 = 10;

	private const int FailTimesDuration2 = 300;

	public const int RegetTime = 200;

	public TdirMgr.TdirManagerEvent SvrListLoaded;

	private static RootNodeType SNodeType;

	public static bool s_maintainBlock = true;

	private static bool mIsCheckVersion;

	private int mCurrentNodeIDFirst = 1;

	private ApolloTdir mTdir;

	private List<TdirTreeNode> mTreeNodes;

	private int recvTimeout = 10;

	private int apolloTimeout = 10000;

	private int syncTimeOut = 12;

	private List<TdirSvrGroup> mSvrListGroup;

	private TdirSvrGroup mRecommondUrlList;

	private TdirSvrGroup mOwnRoleList;

	private TdirSvrGroup mPrivateSvrList;

	private TdirSvrGroup mTestSvrList;

	private AsyncStatu mAsyncFinished;

	private ApolloPlatform mPlatform;

	private int mLastLoginNodeID;

	private TdirUrl mLastLoginUrl;

	private TdirUrl mSelectedTdir;

	private TdirEnv mTdirEnv = TdirEnv.Release;

	public bool EnableMarkLastLogin = true;

	private int mSyncTime;

	private int mFailTimes;

	private DictionaryView<string, List<TdirTreeNode>> mOpenIDTreeNodeDic = new DictionaryView<string, List<TdirTreeNode>>();

	public float m_regetTime;

	public string[] m_iplist;

	public int[] m_portlist;

	private IAsyncResult getAddressResult;

	private ApolloAccountInfo m_info;

	public int m_connectIndex = -1;

	public bool isPreEnabled;

	public string preVersion;

	private float m_GetTdirTime;

	private TdirResult m_GetinnerResult = 211;

	private static int InternalNodeID
	{
		get
		{
			return (!TdirMgr.mIsCheckVersion) ? 1 : 1;
		}
	}

	private static int QQNodeID
	{
		get
		{
			if (TdirMgr.SNodeType == RootNodeType.Normal)
			{
				return (!TdirMgr.mIsCheckVersion) ? 2 : 5;
			}
			if (TdirMgr.SNodeType == RootNodeType.TestFlight)
			{
				return 8;
			}
			if (TdirMgr.SNodeType == RootNodeType.TestSpecial)
			{
				return 11;
			}
			return (!TdirMgr.mIsCheckVersion) ? 2 : 5;
		}
	}

	private static int WeixinNodeID
	{
		get
		{
			if (TdirMgr.SNodeType == RootNodeType.Normal)
			{
				return (!TdirMgr.mIsCheckVersion) ? 3 : 6;
			}
			if (TdirMgr.SNodeType == RootNodeType.TestFlight)
			{
				return 9;
			}
			if (TdirMgr.SNodeType == RootNodeType.TestSpecial)
			{
				return 12;
			}
			return (!TdirMgr.mIsCheckVersion) ? 3 : 6;
		}
	}

	private static int GuestNodeID
	{
		get
		{
			if (TdirMgr.SNodeType == RootNodeType.Normal)
			{
				return (!TdirMgr.mIsCheckVersion) ? 4 : 7;
			}
			if (TdirMgr.SNodeType == RootNodeType.TestFlight)
			{
				return 10;
			}
			if (TdirMgr.SNodeType == RootNodeType.TestSpecial)
			{
				return 13;
			}
			return (!TdirMgr.mIsCheckVersion) ? 4 : 7;
		}
	}

	public static bool IsCheckVersion
	{
		get
		{
			return TdirMgr.mIsCheckVersion;
		}
	}

	public List<TdirSvrGroup> SvrUrlList
	{
		get
		{
			return this.mSvrListGroup;
		}
	}

	public TdirSvrGroup RecommondUrlList
	{
		get
		{
			return this.mRecommondUrlList;
		}
	}

	public TdirSvrGroup OwnRoleList
	{
		get
		{
			return this.mOwnRoleList;
		}
	}

	public AsyncStatu AsyncFinished
	{
		get
		{
			return this.mAsyncFinished;
		}
	}

	public TdirUrl LastLoginUrl
	{
		get
		{
			return this.mLastLoginUrl;
		}
	}

	public TdirUrl SelectedTdir
	{
		get
		{
			return this.mSelectedTdir;
		}
	}

	private int FailTimes
	{
		get
		{
			return this.mFailTimes;
		}
		set
		{
			PlayerPrefs.SetInt("LoginFailTimesKey", value);
			this.mFailTimes = value;
		}
	}

	protected override void Init()
	{
		this.mTdir = new ApolloTdir();
		if (PlayerPrefs.HasKey("LoginFailTimesKey"))
		{
			this.FailTimes = PlayerPrefs.GetInt("LoginFailTimesKey");
		}
		this.apolloTimeout = PlayerPrefs.GetInt(TdirNodeAttrType.tdirTimeOut.ToString(), this.apolloTimeout);
		this.syncTimeOut = PlayerPrefs.GetInt(TdirNodeAttrType.tdirSyncTimeOut.ToString(), this.syncTimeOut);
		Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.TDir_QuitGame, new CUIEventManager.OnUIEventHandler(this.OnQuitGame));
		Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.TDir_TryAgain, new CUIEventManager.OnUIEventHandler(this.OnTryAgain));
	}

	public void Dispose()
	{
		Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.TDir_QuitGame, new CUIEventManager.OnUIEventHandler(this.OnQuitGame));
		Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.TDir_TryAgain, new CUIEventManager.OnUIEventHandler(this.OnTryAgain));
	}

	private void OnQuitGame(CUIEvent uiEvent)
	{
		SGameApplication.Quit();
	}

	private void OnTryAgain(CUIEvent uiEvent)
	{
		this.TdirAsync(this.m_info, null, null, true);
	}

	public void TdirAsync(ApolloAccountInfo info, Action successCallBack, Action failCallBack, bool reasync)
	{
		this.preVersion = null;
		this.isPreEnabled = false;
		this.m_info = info;
		this.mSyncTime = 0;
		this.SetIpAndPort();
		this.m_GetTdirTime = Time.time;
		base.StartCoroutine(this.QueryTdirAsync(info, successCallBack, failCallBack, reasync));
	}

	public void SetIpAndPort()
	{
		this.m_iplist = TdirConfig.GetTdirIPList();
		this.m_portlist = TdirConfig.GetTdirPortList();
	}

	private void InitTdir(ApolloPlatform platform)
	{
		this.mPlatform = platform;
		this.InitSvrList();
	}

	public void EnterGame(TdirUrl tdirUrl)
	{
		this.mSelectedTdir = tdirUrl;
		this.SetConfigAtClickTdir(this.mSelectedTdir);
	}

	private void InitSvrList()
	{
		this.m_connectIndex = -1;
		if (this.mPlatform == 2)
		{
			this.mCurrentNodeIDFirst = TdirMgr.QQNodeID;
		}
		else if (this.mPlatform == 1)
		{
			this.mCurrentNodeIDFirst = TdirMgr.WeixinNodeID;
		}
		else if (this.mPlatform == 5)
		{
			this.mCurrentNodeIDFirst = TdirMgr.GuestNodeID;
		}
		this.ResetLastLoginUrl();
		this.ResetSelectedUrl();
		this.mOwnRoleList = default(TdirSvrGroup);
		this.mOwnRoleList.name = Singleton<CTextManager>.get_instance().GetText("Tdir_My_Svr");
		this.mOwnRoleList.tdirUrls = new List<TdirUrl>();
		this.mRecommondUrlList = default(TdirSvrGroup);
		this.mRecommondUrlList.name = Singleton<CTextManager>.get_instance().GetText("Tdir_Rcmd_Svr");
		this.mRecommondUrlList.tdirUrls = new List<TdirUrl>();
		this.mPrivateSvrList = default(TdirSvrGroup);
		this.mPrivateSvrList.name = "PrivateSvrList";
		this.mPrivateSvrList.nodeID = this.GetNodeIDByPos(this.mCurrentNodeIDFirst, 1, 0, 0);
		this.mPrivateSvrList.tdirUrls = new List<TdirUrl>();
		this.mTestSvrList = default(TdirSvrGroup);
		this.mTestSvrList.name = "TestSvrList";
		this.mTestSvrList.nodeID = this.GetNodeIDByPos(this.mCurrentNodeIDFirst, 2, 0, 0);
		this.mTestSvrList.tdirUrls = new List<TdirUrl>();
		this.mSvrListGroup = new List<TdirSvrGroup>();
		this.mSvrListGroup.Add(this.mOwnRoleList);
		this.mSvrListGroup.Add(this.mRecommondUrlList);
		if (this.mTdirEnv == TdirEnv.Test)
		{
			this.mSvrListGroup.Add(this.mPrivateSvrList);
			this.mSvrListGroup.Add(this.mTestSvrList);
		}
		this.ParseNodeInfo();
		this.mOwnRoleList.tdirUrls.Sort(new Comparison<TdirUrl>(this.SortTdirUrl));
		if (this.mLastLoginUrl.nodeID > 0 && this.LastLoginUrl.name != null && this.LastLoginUrl.name.get_Length() != 0)
		{
			this.mSelectedTdir = this.mLastLoginUrl;
		}
		else if (this.mRecommondUrlList.tdirUrls.get_Count() > 0)
		{
			this.mSelectedTdir = this.mRecommondUrlList.tdirUrls.get_Item(this.mRecommondUrlList.tdirUrls.get_Count() - 1);
		}
		else if (this.mSvrListGroup.get_Count() > 0 && this.mSvrListGroup.get_Item(this.mSvrListGroup.get_Count() - 1).tdirUrls.get_Count() > 0)
		{
			int count = this.mSvrListGroup.get_Item(this.mSvrListGroup.get_Count() - 1).tdirUrls.get_Count();
			this.mSelectedTdir = this.mSvrListGroup.get_Item(this.mSvrListGroup.get_Count() - 1).tdirUrls.get_Item(count - 1);
		}
		if (this.SvrListLoaded != null)
		{
			this.SvrListLoaded();
		}
	}

	public TdirUrl GetDefaultTdirUrl()
	{
		if (this.CheckTdirUrlValid(this.mLastLoginUrl))
		{
			return this.mLastLoginUrl;
		}
		if (this.RecommondUrlList.tdirUrls != null)
		{
			for (int i = 0; i < this.RecommondUrlList.tdirUrls.get_Count(); i++)
			{
				if (this.CheckTdirUrlValid(this.RecommondUrlList.tdirUrls.get_Item(i)))
				{
					return this.RecommondUrlList.tdirUrls.get_Item(i);
				}
			}
		}
		if (this.mSvrListGroup != null)
		{
			for (int j = 0; j < this.mSvrListGroup.get_Count(); j++)
			{
				if (this.mSvrListGroup.get_Item(j).tdirUrls != null)
				{
					for (int k = 0; k < this.mSvrListGroup.get_Item(j).tdirUrls.get_Count(); k++)
					{
						if (this.CheckTdirUrlValid(this.mSvrListGroup.get_Item(j).tdirUrls.get_Item(k)))
						{
							return this.mSvrListGroup.get_Item(j).tdirUrls.get_Item(k);
						}
					}
				}
			}
		}
		return default(TdirUrl);
	}

	public bool CheckTdirUrlValid(TdirUrl url)
	{
		return url.nodeID != 0 && url.addrs != null;
	}

	private int SortTdirUrl(TdirUrl url1, TdirUrl url2)
	{
		int nodeID = url1.nodeID;
		int nodeID2 = url2.nodeID;
		if (nodeID > nodeID2)
		{
			return 1;
		}
		if (nodeID == nodeID2)
		{
			return 0;
		}
		return -1;
	}

	private bool CheckTreeNodeValid(TdirTreeNode node)
	{
		if (node.staticInfo.cltAttr1 == 0)
		{
			return false;
		}
		object obj = new object();
		IPAddress iPAddress;
		if (IPAddress.TryParse(GameFramework.AppVersion, ref iPAddress))
		{
			int num = BitConverter.ToInt32(iPAddress.GetAddressBytes(), 0);
			return (!this.GetTreeNodeAttr(node, TdirNodeAttrType.versionDown, ref obj) || (int)obj <= num) && (!this.GetTreeNodeAttr(node, TdirNodeAttrType.versionUp, ref obj) || (int)obj >= num);
		}
		return true;
	}

	private bool CheckEnterTdirUrl(TdirUrl tdirUrl, bool tips = false)
	{
		if (tdirUrl.nodeID == 0 || tdirUrl.statu == TdirSvrStatu.UNAVAILABLE || tdirUrl.addrs == null)
		{
			if (tips)
			{
			}
			return false;
		}
		return true;
	}

	private bool GetLastLoginNode(int nodeID, ref TdirUrl url)
	{
		if (this.mSvrListGroup == null || nodeID == 0 || (this.mPlatform == 2 && this.GetIPPosByNodeID(nodeID, 0) != TdirMgr.QQNodeID) || (this.mPlatform == 1 && this.GetIPPosByNodeID(nodeID, 0) != TdirMgr.WeixinNodeID) || (this.mPlatform == 5 && this.GetIPPosByNodeID(nodeID, 0) != TdirMgr.GuestNodeID))
		{
			url.nodeID = 0;
			return false;
		}
		for (int i = 0; i < this.mSvrListGroup.get_Count(); i++)
		{
			List<TdirUrl> tdirUrls = this.mSvrListGroup.get_Item(i).tdirUrls;
			for (int j = 0; j < tdirUrls.get_Count(); j++)
			{
				if (tdirUrls.get_Item(j).nodeID == nodeID)
				{
					return this.CreateSvrUrl(tdirUrls.get_Item(j), ref url);
				}
			}
		}
		return false;
	}

	[DebuggerHidden]
	private IEnumerator QueryTdirAsync(ApolloAccountInfo info, Action successCallBack, Action failCallBack, bool reasync)
	{
		TdirMgr.<QueryTdirAsync>c__Iterator2F <QueryTdirAsync>c__Iterator2F = new TdirMgr.<QueryTdirAsync>c__Iterator2F();
		<QueryTdirAsync>c__Iterator2F.info = info;
		<QueryTdirAsync>c__Iterator2F.failCallBack = failCallBack;
		<QueryTdirAsync>c__Iterator2F.reasync = reasync;
		<QueryTdirAsync>c__Iterator2F.successCallBack = successCallBack;
		<QueryTdirAsync>c__Iterator2F.<$>info = info;
		<QueryTdirAsync>c__Iterator2F.<$>failCallBack = failCallBack;
		<QueryTdirAsync>c__Iterator2F.<$>reasync = reasync;
		<QueryTdirAsync>c__Iterator2F.<$>successCallBack = successCallBack;
		<QueryTdirAsync>c__Iterator2F.<>f__this = this;
		return <QueryTdirAsync>c__Iterator2F;
	}

	private void CheckAuditVersion()
	{
		IPAddress iPAddress;
		if (IPAddress.TryParse(GameFramework.AppVersion, ref iPAddress))
		{
			int num = BitConverter.ToInt32(iPAddress.GetAddressBytes(), 0);
			object obj = 0;
			for (int i = 0; i < this.mTreeNodes.get_Count(); i++)
			{
				if (this.ParseNodeAppAttr(this.mTreeNodes.get_Item(i).staticInfo.appAttr, TdirNodeAttrType.versionOnlyExcept, ref obj) && (int)obj == num)
				{
					TdirMgr.mIsCheckVersion = true;
					break;
				}
			}
		}
	}

	private void CheckPreEnable()
	{
		for (int i = 0; i < this.mTreeNodes.get_Count(); i++)
		{
			object obj = null;
			if (this.ParseNodeAppAttr(this.mTreeNodes.get_Item(i).staticInfo.appAttr, TdirNodeAttrType.PreEnable, ref obj) && !string.IsNullOrEmpty((string)obj))
			{
				this.preVersion = (string)obj;
				this.isPreEnabled = true;
				break;
			}
		}
	}

	private void ParseNodeInfo()
	{
		if (this.mTreeNodes == null)
		{
			return;
		}
		object obj = new object();
		for (int i = 0; i < this.mTreeNodes.get_Count(); i++)
		{
			if (this.GetIPPosByNodeID(this.mTreeNodes.get_Item(i).nodeID, 0) == this.mCurrentNodeIDFirst)
			{
				TdirUrl tdirUrl = default(TdirUrl);
				if (this.CreateSvrUrl(this.mTreeNodes.get_Item(i), ref tdirUrl))
				{
					if (tdirUrl.roleCount != 0u)
					{
						this.mOwnRoleList.tdirUrls.Add(tdirUrl);
					}
					if (tdirUrl.flag == 2 || (this.ParseNodeAppAttr(this.mTreeNodes.get_Item(i).staticInfo.appAttr, TdirNodeAttrType.recommond, ref obj) && (int)obj != 0))
					{
						this.mRecommondUrlList.tdirUrls.Add(tdirUrl);
					}
					if (tdirUrl.roleCount > 0u && this.mLastLoginUrl.lastLoginTime < tdirUrl.lastLoginTime)
					{
						this.mLastLoginUrl = tdirUrl;
					}
					if (this.mTreeNodes.get_Item(i).parentID != 0)
					{
						if (this.mTreeNodes.get_Item(i).nodeType == 0)
						{
							TdirSvrGroup tdirSvrGroup = default(TdirSvrGroup);
							if (this.CreateSvrGroup(this.mTreeNodes.get_Item(i), ref tdirSvrGroup))
							{
								this.mSvrListGroup.Add(tdirSvrGroup);
							}
						}
						else
						{
							bool flag = false;
							for (int j = 0; j < this.mSvrListGroup.get_Count(); j++)
							{
								if (this.mTreeNodes.get_Item(i).parentID == this.mSvrListGroup.get_Item(j).nodeID)
								{
									this.mSvrListGroup.get_Item(j).tdirUrls.Add(tdirUrl);
									flag = true;
								}
							}
							if (!flag)
							{
								for (int k = 0; k < this.mTreeNodes.get_Count(); k++)
								{
									if (this.mTreeNodes.get_Item(i).parentID == this.mTreeNodes.get_Item(k).nodeID)
									{
										TdirSvrGroup tdirSvrGroup2 = default(TdirSvrGroup);
										if (this.CreateSvrGroup(this.mTreeNodes.get_Item(k), ref tdirSvrGroup2))
										{
											tdirSvrGroup2.tdirUrls.Add(tdirUrl);
											this.mSvrListGroup.Add(tdirSvrGroup2);
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	private void ResetLastLoginUrl()
	{
		this.mLastLoginUrl.logicWorldID = 0;
		this.mLastLoginUrl.nodeID = 0;
		this.mLastLoginUrl.lastLoginTime = 0u;
		this.mLastLoginUrl.name = null;
		this.mLastLoginUrl.flag = 0;
		this.mLastLoginUrl.attr = null;
		this.mLastLoginUrl.statu = TdirSvrStatu.UNAVAILABLE;
		this.mLastLoginUrl.addrs = null;
	}

	private void ResetSelectedUrl()
	{
		this.mSelectedTdir.logicWorldID = 0;
		this.mSelectedTdir.nodeID = 0;
		this.mSelectedTdir.lastLoginTime = 0u;
		this.mSelectedTdir.name = null;
		this.mSelectedTdir.flag = 0;
		this.mSelectedTdir.attr = null;
		this.mSelectedTdir.statu = TdirSvrStatu.UNAVAILABLE;
		this.mSelectedTdir.addrs = null;
	}

	private bool CreateSvrGroup(TdirTreeNode node, ref TdirSvrGroup group)
	{
		if (!this.CheckTreeNodeValid(node))
		{
			return false;
		}
		group.nodeID = node.nodeID;
		group.name = node.name;
		group.appAttr = node.staticInfo.appAttr;
		group.tdirUrls = new List<TdirUrl>();
		return true;
	}

	private bool CreateSvrUrl(TdirTreeNode node, ref TdirUrl tdirUrl)
	{
		string connectUrl = node.dynamicInfo.connectUrl;
		if (string.IsNullOrEmpty(connectUrl))
		{
			return false;
		}
		if (!this.CheckTreeNodeValid(node))
		{
			return false;
		}
		tdirUrl.nodeID = node.nodeID;
		tdirUrl.name = node.name;
		tdirUrl.statu = (TdirSvrStatu)node.status;
		if (tdirUrl.statu == TdirSvrStatu.FINE || tdirUrl.statu == TdirSvrStatu.HEAVY)
		{
			tdirUrl.flag = node.svrFlag;
		}
		else
		{
			tdirUrl.flag = 0;
		}
		tdirUrl.mask = node.staticInfo.bitmapMask;
		tdirUrl.addrs = new List<IPAddrInfo>();
		tdirUrl.attr = node.staticInfo.appAttr;
		if (node.userRoleInfo != null)
		{
			tdirUrl.roleCount = (uint)node.userRoleInfo.get_Count();
			uint num = 0u;
			for (int i = 0; i < node.userRoleInfo.get_Count(); i++)
			{
				if (node.userRoleInfo.get_Item(i).lastLoginTime > num)
				{
					num = node.userRoleInfo.get_Item(i).lastLoginTime;
				}
			}
			tdirUrl.lastLoginTime = num;
		}
		else
		{
			tdirUrl.roleCount = 0u;
			tdirUrl.lastLoginTime = 0u;
		}
		string[] array = connectUrl.Split(new char[]
		{
			';'
		});
		if (array == null)
		{
			return false;
		}
		for (int j = 0; j < array.Length; j++)
		{
			string[] array2 = array[j].Split(new char[]
			{
				':'
			});
			for (int k = 1; k < array2.Length; k++)
			{
				if (array2[k].IndexOf('-') >= 0)
				{
					string[] array3 = array2[k].Split(new char[]
					{
						'-'
					});
					if (array3.Length == 2)
					{
						int num2;
						if (int.TryParse(array3[0], ref num2))
						{
							int num3;
							if (int.TryParse(array3[1], ref num3))
							{
								if (num2 <= num3)
								{
									int num4 = num3 - num2 + 1;
									for (int l = 0; l < num4; l++)
									{
										int num5 = num2 + l;
										IPAddrInfo iPAddrInfo = default(IPAddrInfo);
										iPAddrInfo.ip = array2[0];
										iPAddrInfo.port = num5.ToString();
										tdirUrl.addrs.Add(iPAddrInfo);
									}
								}
							}
						}
					}
				}
				else
				{
					IPAddrInfo iPAddrInfo2 = default(IPAddrInfo);
					iPAddrInfo2.ip = array2[0];
					iPAddrInfo2.port = array2[k];
					tdirUrl.addrs.Add(iPAddrInfo2);
				}
			}
		}
		object obj = null;
		if (this.ParseNodeAppAttr(tdirUrl.attr, TdirNodeAttrType.LogicWorldID, ref obj))
		{
			tdirUrl.logicWorldID = (int)obj;
		}
		else
		{
			tdirUrl.logicWorldID = 0;
		}
		object obj2 = null;
		if (this.ParseNodeAppAttr(tdirUrl.attr, TdirNodeAttrType.UseNetAcc, ref obj2))
		{
			tdirUrl.useNetAcc = (bool)obj2;
		}
		else
		{
			tdirUrl.useNetAcc = false;
		}
		return true;
	}

	private bool CreateSvrUrl(TdirUrl srcUrl, ref TdirUrl tdirUrl)
	{
		if (!this.CheckTdirUrlValid(srcUrl))
		{
			tdirUrl.nodeID = 0;
			return false;
		}
		tdirUrl.nodeID = srcUrl.nodeID;
		tdirUrl.name = srcUrl.name;
		tdirUrl.statu = srcUrl.statu;
		tdirUrl.flag = srcUrl.flag;
		tdirUrl.mask = srcUrl.mask;
		tdirUrl.roleCount = srcUrl.roleCount;
		tdirUrl.attr = srcUrl.attr;
		tdirUrl.addrs = new List<IPAddrInfo>(srcUrl.addrs);
		tdirUrl.logicWorldID = srcUrl.logicWorldID;
		return true;
	}

	public bool GetTreeNodeAttr(TdirTreeNode node, TdirNodeAttrType attrType, ref object param)
	{
		return this.ParseNodeAppAttr(node.staticInfo.appAttr, attrType, ref param);
	}

	public bool ParseNodeAppAttr(string attr, TdirNodeAttrType attrType, ref object param)
	{
		if (!string.IsNullOrEmpty(attr))
		{
			attr = attr.ToLower();
			string[] array = attr.Split(new char[]
			{
				';'
			});
			if (array == null)
			{
				return false;
			}
			string text = attrType.ToString().ToLower();
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					':'
				});
				if (array2 != null && array2.Length == 2 && !string.IsNullOrEmpty(array2[0]) && !string.IsNullOrEmpty(array2[1]) && array2[0].ToLower() == text)
				{
					switch (attrType)
					{
					case TdirNodeAttrType.recommond:
					case TdirNodeAttrType.enableTSS:
					case TdirNodeAttrType.backTime:
					case TdirNodeAttrType.msdkLogMem:
					case TdirNodeAttrType.tdirTimeOut:
					case TdirNodeAttrType.tdirSyncTimeOut:
					case TdirNodeAttrType.waitHurtActionDone:
					case TdirNodeAttrType.unloadValidCnt:
					case TdirNodeAttrType.checkdevice:
					case TdirNodeAttrType.authorParam:
					case TdirNodeAttrType.autoReplaceActorParam:
					case TdirNodeAttrType.socketRecvBuffer:
					case TdirNodeAttrType.reconnectMaxCnt:
					case TdirNodeAttrType.LogicWorldID:
					case TdirNodeAttrType.UseDeviceIDChooseSvr:
					case TdirNodeAttrType.IOSX:
					case TdirNodeAttrType.UseTongcai:
					case TdirNodeAttrType.ShowTCBtn:
					{
						int num = 0;
						if (int.TryParse(array2[1], ref num))
						{
							param = num;
							return true;
						}
						break;
					}
					case TdirNodeAttrType.ISPChoose:
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>();
						string[] array3 = array2[1].Split(new char[]
						{
							'*'
						});
						for (int j = 0; j < array3.Length; j++)
						{
							string[] array4 = array3[j].Split(new char[]
							{
								'$'
							});
							DebugHelper.Assert(array4.Length == 2, "后台大爷把用于ISP解析的字符串配错了");
							int num2 = 0;
							int.TryParse(array4[0], ref num2);
							IPAddress iPAddress;
							if (!IPAddress.TryParse(array4[1], ref iPAddress))
							{
								DebugHelper.Assert(false, "后台大爷把用于ISP解析的字符串配错了,ip解析不了");
							}
							dictionary.Add(array4[1], num2);
						}
						param = dictionary;
						return true;
					}
					case TdirNodeAttrType.versionDown:
					case TdirNodeAttrType.versionUp:
					case TdirNodeAttrType.versionOnlyExcept:
					{
						IPAddress iPAddress2;
						if (IPAddress.TryParse(array2[1], ref iPAddress2))
						{
							param = BitConverter.ToInt32(iPAddress2.GetAddressBytes(), 0);
							return true;
						}
						break;
					}
					case TdirNodeAttrType.UseNetAcc:
					{
						int num3 = 0;
						if (int.TryParse(array2[1], ref num3))
						{
							if (num3 != 0)
							{
								param = true;
							}
							else
							{
								param = false;
							}
							return true;
						}
						break;
					}
					case TdirNodeAttrType.PreEnable:
						param = array2[1];
						return true;
					}
				}
			}
		}
		return false;
	}

	private void SetGlobalConfig()
	{
		if (this.mTreeNodes == null || this.mTreeNodes.get_Count() == 0)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		for (int i = 0; i < this.mTreeNodes.get_Count(); i++)
		{
			object obj = new object();
			if (this.GetTreeNodeAttr(this.mTreeNodes.get_Item(i), TdirNodeAttrType.enableTSS, ref obj))
			{
				PlayerPrefs.SetInt("EnableTSS", (int)obj);
				flag = true;
			}
			if (!flag2 && this.GetTreeNodeAttr(this.mTreeNodes.get_Item(i), TdirNodeAttrType.tdirTimeOut, ref obj))
			{
				PlayerPrefs.SetInt(TdirNodeAttrType.tdirTimeOut.ToString(), (int)obj);
				flag2 = true;
				this.apolloTimeout = (int)obj;
			}
			if (this.GetTreeNodeAttr(this.mTreeNodes.get_Item(i), TdirNodeAttrType.tdirSyncTimeOut, ref obj))
			{
				PlayerPrefs.SetInt(TdirNodeAttrType.tdirSyncTimeOut.ToString(), (int)obj);
				flag3 = true;
				this.syncTimeOut = (int)obj;
			}
			if (flag && flag2 && flag3)
			{
				return;
			}
		}
	}

	private void SetConfigAtClickTdir(TdirUrl tdirUrl)
	{
		if (tdirUrl.nodeID != 0)
		{
			object obj = null;
			if (!this.ParseNodeAppAttr(tdirUrl.attr, TdirNodeAttrType.reconnectMaxCnt, ref obj) || (int)obj >= 0)
			{
			}
		}
	}

	private int GetIPPosByNodeID(int nodeID, int pos)
	{
		return nodeID & 255 << pos * 8;
	}

	private int GetNodeIDByPos(int first = 0, int second = 0, int third = 0, int forth = 0)
	{
		if (first == 0 && second == 0 && third == 0 && forth == 0)
		{
			return 0;
		}
		return first | second << 8 | third << 16 | forth << 24;
	}

	public void ChooseGameServer(TdirUrl tdirurl)
	{
		this.m_connectIndex = -1;
		if (this.mCurrentNodeIDFirst != TdirMgr.InternalNodeID && tdirurl.statu == TdirSvrStatu.UNAVAILABLE && TdirMgr.s_maintainBlock)
		{
			Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("Maintaining"), false);
			return;
		}
		this.mSelectedTdir = tdirurl;
		if (tdirurl.addrs == null || tdirurl.addrs.get_Count() <= 0)
		{
			DebugHelper.Assert(false, "{0} 后台大爷给这个区配个gameserver的url呗", new object[]
			{
				tdirurl.name
			});
			return;
		}
		MonoSingleton<CTongCaiSys>.get_instance().isCanUseTongCai = true;
		object obj = null;
		if (this.ParseNodeAppAttr(this.SelectedTdir.attr, TdirNodeAttrType.UseTongcai, ref obj) && (int)obj == 0)
		{
			MonoSingleton<CTongCaiSys>.get_instance().isCanUseTongCai = false;
		}
		Singleton<ReconnectIpSelect>.get_instance().Reset();
		this.ConnectLobby();
	}

	public void StartGetHostAddress(TdirUrl tdirurl)
	{
		bool flag = false;
		IPAddress iPAddress;
		if (IPAddress.TryParse(tdirurl.addrs.get_Item(0).ip, ref iPAddress))
		{
			long num = (long)BitConverter.ToInt32(iPAddress.GetAddressBytes(), 0);
			if (num > 0L)
			{
				flag = true;
			}
		}
		ApolloConfig.ISPType = 0;
		if (!flag)
		{
			if (this.getAddressResult == null)
			{
				this.getAddressResult = Dns.BeginGetHostAddresses(tdirurl.addrs.get_Item(0).ip, null, null);
			}
			if (this.getAddressResult == null)
			{
				object obj = 0;
				if (this.ParseNodeAppAttr(tdirurl.attr, TdirNodeAttrType.ISPChoose, ref obj))
				{
					Dictionary<string, int> dictionary = (Dictionary<string, int>)obj;
					if (dictionary != null)
					{
						using (Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, int> current = enumerator.get_Current();
								if (current.get_Value() == this.GetISP())
								{
									ApolloConfig.loginUrl = string.Format("tcp://{0}:{1}", current.get_Key(), tdirurl.addrs.get_Item(0).port);
									ApolloConfig.loginOnlyIpOrUrl = current.get_Key();
									ApolloConfig.loginOnlyVPort = ushort.Parse(tdirurl.addrs.get_Item(0).port);
									ApolloConfig.ISPType = this.GetISP();
									break;
								}
							}
						}
					}
				}
			}
		}
		else
		{
			ApolloConfig.loginUrl = string.Format("tcp://{0}:{1}", tdirurl.addrs.get_Item(0).ip, tdirurl.addrs.get_Item(0).port);
			ApolloConfig.loginOnlyIpOrUrl = tdirurl.addrs.get_Item(0).ip;
			ApolloConfig.loginOnlyVPort = ushort.Parse(tdirurl.addrs.get_Item(0).port);
			ApolloConfig.ISPType = 1;
		}
	}

	public void ConnectLobby()
	{
		if (this.m_connectIndex < 0)
		{
			int num = 1;
			object obj = null;
			if (this.ParseNodeAppAttr(this.SelectedTdir.attr, TdirNodeAttrType.UseDeviceIDChooseSvr, ref obj))
			{
				num = (int)obj;
			}
			if (num == 1 && !string.IsNullOrEmpty(SystemInfo.deviceUniqueIdentifier))
			{
				byte[] bytes = Encoding.get_ASCII().GetBytes(SystemInfo.deviceUniqueIdentifier);
				MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
				mD5CryptoServiceProvider.Initialize();
				mD5CryptoServiceProvider.TransformFinalBlock(bytes, 0, bytes.Length);
				ulong num2 = (ulong)BitConverter.ToInt64(mD5CryptoServiceProvider.get_Hash(), 0);
				ulong num3 = (ulong)BitConverter.ToInt64(mD5CryptoServiceProvider.get_Hash(), 8);
				this.m_connectIndex = (int)(num2 ^ num3);
				if (this.m_connectIndex < 0)
				{
					this.m_connectIndex *= -1;
				}
			}
			else
			{
				this.m_connectIndex = Random.Range(0, 10000);
			}
			this.m_connectIndex %= MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.addrs.get_Count();
			Singleton<LobbySvrMgr>.GetInstance().ConnectServerWithTdirDefault(this.m_connectIndex, ChooseSvrPolicy.DeviceID);
		}
		else
		{
			this.m_connectIndex++;
			this.m_connectIndex %= MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.addrs.get_Count();
			Singleton<LobbySvrMgr>.GetInstance().ConnectServerWithTdirDefault(this.m_connectIndex, Singleton<LobbySvrMgr>.GetInstance().chooseSvrPol);
		}
	}

	public void ConnectLobbyWithSnsNameChooseSvr()
	{
		if (!string.IsNullOrEmpty(Singleton<CLoginSystem>.GetInstance().m_nickName))
		{
			byte[] bytes = Encoding.get_ASCII().GetBytes(Singleton<CLoginSystem>.GetInstance().m_nickName);
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			mD5CryptoServiceProvider.Initialize();
			mD5CryptoServiceProvider.TransformFinalBlock(bytes, 0, bytes.Length);
			ulong num = (ulong)BitConverter.ToInt64(mD5CryptoServiceProvider.get_Hash(), 0);
			ulong num2 = (ulong)BitConverter.ToInt64(mD5CryptoServiceProvider.get_Hash(), 8);
			this.m_connectIndex = (int)(num ^ num2);
			if (this.m_connectIndex < 0)
			{
				this.m_connectIndex *= -1;
			}
		}
		else
		{
			this.m_connectIndex = Random.Range(0, 10000);
		}
		this.m_connectIndex %= MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.addrs.get_Count();
		Singleton<LobbySvrMgr>.GetInstance().ConnectServerWithTdirDefault(this.m_connectIndex, ChooseSvrPolicy.NickName);
	}

	public void ConnectLobbyRandomChooseSvr(ChooseSvrPolicy policy)
	{
		this.m_connectIndex = Random.Range(0, 10000);
		this.m_connectIndex %= MonoSingleton<TdirMgr>.GetInstance().SelectedTdir.addrs.get_Count();
		Singleton<LobbySvrMgr>.GetInstance().ConnectServerWithTdirDefault(this.m_connectIndex, policy);
	}

	private void Update()
	{
	}

	public bool ShowBuyTongCaiBtn()
	{
		object obj = null;
		if (this.ParseNodeAppAttr(this.SelectedTdir.attr, TdirNodeAttrType.ShowTCBtn, ref obj))
		{
			int num = (int)obj;
			if (num == 1)
			{
				return true;
			}
		}
		return false;
	}

	public int GetISP()
	{
		if (this.mTdir == null)
		{
			return 0;
		}
		TreeCommonData treeCommonData = this.mTdir.GetTreeCommonData();
		if (treeCommonData.ispCode == 0)
		{
			return 0;
		}
		if (treeCommonData.ispCode == 1)
		{
			return 1;
		}
		if (treeCommonData.ispCode == 2)
		{
			return 2;
		}
		if (treeCommonData.ispCode == 5)
		{
			return 3;
		}
		return 4;
	}

	public string GetDianXingIP()
	{
		return this.GetIpByIspCode(1);
	}

	public string GetLianTongIP()
	{
		return this.GetIpByIspCode(2);
	}

	public string GetYiDongIP()
	{
		return this.GetIpByIspCode(3);
	}

	public string GetIpByIspCode(int code)
	{
		object obj = 0;
		if (this.ParseNodeAppAttr(this.SelectedTdir.attr, TdirNodeAttrType.ISPChoose, ref obj))
		{
			Dictionary<string, int> dictionary = (Dictionary<string, int>)obj;
			if (dictionary != null)
			{
				using (Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, int> current = enumerator.get_Current();
						if (current.get_Value() == code)
						{
							return current.get_Key();
						}
					}
				}
			}
		}
		return null;
	}

	public enIIPSServerType GetOfficialIIPSServerType()
	{
		enIIPSServerType result = enIIPSServerType.Official;
		if (this.isPreEnabled)
		{
			string[] array = GameFramework.AppVersion.Split(new char[]
			{
				'.'
			});
			if (array.Length > 2 && this.preVersion.Equals(array[1]))
			{
				result = enIIPSServerType.PreviousOfficial;
			}
		}
		return result;
	}

	public VerUpdateInfo CheckPreviousVersionUpdate()
	{
		VerUpdateInfo result;
		if (this.isPreEnabled)
		{
			string[] array = GameFramework.AppVersion.Split(new char[]
			{
				'.'
			});
			int num = 0;
			if (array.Length > 2)
			{
				int.TryParse(array[1], ref num);
			}
			int num2 = 0;
			int.TryParse(this.preVersion, ref num2);
			if (this.SelectedTdir.isPreSvr)
			{
				if (num == num2)
				{
					result.svrInfo = LoginSvrInfo.Login_valid;
					result.svrType = enIIPSServerType.PreviousOfficial;
				}
				else if (num < num2)
				{
					result.svrInfo = LoginSvrInfo.Login_needUpdate;
					result.svrType = enIIPSServerType.PreviousOfficial;
				}
				else
				{
					result.svrInfo = LoginSvrInfo.Login_invalid;
					result.svrType = enIIPSServerType.PreviousOfficial;
				}
			}
			else
			{
				if (num >= num2)
				{
					result.svrInfo = LoginSvrInfo.Login_invalid;
					result.svrType = enIIPSServerType.Official;
				}
				result.svrInfo = LoginSvrInfo.Login_valid;
				result.svrType = enIIPSServerType.Official;
			}
		}
		else
		{
			result.svrInfo = LoginSvrInfo.Login_valid;
			result.svrType = enIIPSServerType.Official;
		}
		return result;
	}
}
