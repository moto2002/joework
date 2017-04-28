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
	[MessageHandlerClass]
	internal class CRoomSystem : Singleton<CRoomSystem>
	{
		public const int MAX_NUM_PER_TEAM = 5;

		public static string PATH_CREATE_ROOM = "UGUI/Form/System/PvP/Room/Form_CreateRoom.prefab";

		public static string PATH_ROOM = "UGUI/Form/System/PvP/Room/Form_Room.prefab";

		public static string PATH_ROOM_SWAP = "UGUI/Form/System/PvP/Room/Form_RoomSwapMessageBox.prefab";

		private static ulong NpcUlId = 1uL;

		private bool bInRoom;

		private ListView<RoomMapInfo> mapList;

		public RoomInfo roomInfo;

		private uint mapId;

		private byte mapType = 1;

		private uint MeleeMapId;

		public int m_roomType;

		public int RoomType
		{
			get
			{
				return this.m_roomType;
			}
		}

		public bool IsInRoom
		{
			get
			{
				return this.bInRoom;
			}
		}

		public bool IsSelfRoomOwner
		{
			get
			{
				return this.roomInfo.selfInfo.ullUid == this.roomInfo.roomOwner.ullUid && this.roomInfo.selfInfo.iGameEntity == this.roomInfo.roomOwner.iGameEntity;
			}
		}

		public override void Init()
		{
			base.Init();
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_OpenCreateForm, new CUIEventManager.OnUIEventHandler(this.OnRoom_OpenCreateForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_CreateRoom, new CUIEventManager.OnUIEventHandler(this.OnRoom_CreateRoom));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnRoom_CloseForm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_SelectMap, new CUIEventManager.OnUIEventHandler(this.OnRoom_SelectMap));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_OpenInvite, new CUIEventManager.OnUIEventHandler(this.OnRoom_OpenInvite));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_StartGame, new CUIEventManager.OnUIEventHandler(this.OnRoom_StartGame));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_AddRobot, new CUIEventManager.OnUIEventHandler(this.OnRoom_AddRobot));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_ChangePos, new CUIEventManager.OnUIEventHandler(this.OnRoom_ChangePos));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_KickPlayer, new CUIEventManager.OnUIEventHandler(this.OnRoom_KickPlayer));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_LeaveRoom, new CUIEventManager.OnUIEventHandler(this.OnRoom_LeaveRoom));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_AddFriend, new CUIEventManager.OnUIEventHandler(this.OnRoom_AddFriend));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_ShareRoom, new CUIEventManager.OnUIEventHandler(this.OnRoom_ShareFriend));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_ChangePos_TimeUp, new CUIEventManager.OnUIEventHandler(this.OnRoomChangePosTimeUp));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_ChangePos_Confirm, new CUIEventManager.OnUIEventHandler(this.OnRoomChangePosConfirm));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_ChangePos_Refuse, new CUIEventManager.OnUIEventHandler(this.OnRoomChangePosRefuse));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_ChangePos_Box_TimerChange, new CUIEventManager.OnUIEventHandler(this.OnRoomChangePosBoxTimerChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Room_On_Close, new CUIEventManager.OnUIEventHandler(this.OnRoomClose));
			CRoomObserve.RegisterEvents();
		}

		public override void UnInit()
		{
			this.roomInfo = null;
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_OpenCreateForm, new CUIEventManager.OnUIEventHandler(this.OnRoom_OpenCreateForm));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_CreateRoom, new CUIEventManager.OnUIEventHandler(this.OnRoom_CreateRoom));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnRoom_CloseForm));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_SelectMap, new CUIEventManager.OnUIEventHandler(this.OnRoom_SelectMap));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_OpenInvite, new CUIEventManager.OnUIEventHandler(this.OnRoom_OpenInvite));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_StartGame, new CUIEventManager.OnUIEventHandler(this.OnRoom_StartGame));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_AddRobot, new CUIEventManager.OnUIEventHandler(this.OnRoom_AddRobot));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_ChangePos, new CUIEventManager.OnUIEventHandler(this.OnRoom_ChangePos));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_KickPlayer, new CUIEventManager.OnUIEventHandler(this.OnRoom_KickPlayer));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_LeaveRoom, new CUIEventManager.OnUIEventHandler(this.OnRoom_LeaveRoom));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_AddFriend, new CUIEventManager.OnUIEventHandler(this.OnRoom_AddFriend));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_ShareRoom, new CUIEventManager.OnUIEventHandler(this.OnRoom_ShareFriend));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_ChangePos_TimeUp, new CUIEventManager.OnUIEventHandler(this.OnRoomChangePosTimeUp));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_ChangePos_Confirm, new CUIEventManager.OnUIEventHandler(this.OnRoomChangePosConfirm));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_ChangePos_Refuse, new CUIEventManager.OnUIEventHandler(this.OnRoomChangePosRefuse));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_ChangePos_Box_TimerChange, new CUIEventManager.OnUIEventHandler(this.OnRoomChangePosBoxTimerChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Room_On_Close, new CUIEventManager.OnUIEventHandler(this.OnRoomClose));
			CRoomObserve.UnRegisterEvents();
			base.UnInit();
		}

		public void Clear()
		{
			this.bInRoom = false;
			this.roomInfo = null;
			this.m_roomType = 0;
		}

		public void CloseRoom()
		{
			Singleton<CUIManager>.GetInstance().CloseForm(CRoomSystem.PATH_CREATE_ROOM);
			Singleton<CUIManager>.GetInstance().CloseForm(CRoomSystem.PATH_ROOM);
			Singleton<CTopLobbyEntry>.GetInstance().CloseForm();
			Singleton<CInviteSystem>.GetInstance().CloseInviteForm();
			CChatUT.LeaveRoom();
			this.bInRoom = false;
		}

		private void InitMaps(CUIFormScript rootFormScript)
		{
			this.mapList = new ListView<RoomMapInfo>();
			uint[] array = new uint[10];
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapID_Room_1"), ref array[0]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapID_Room_2"), ref array[1]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapID_Room_3"), ref array[2]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapID_Room_4"), ref array[3]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapID_Room_5"), ref array[4]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapID_Room_6"), ref array[5]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapID_Room_7"), ref array[6]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapID_Room_8"), ref array[7]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapID_Room_9"), ref array[8]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapID_Room_10"), ref array[9]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapID_PVP_MELEE"), ref this.MeleeMapId);
			uint[] array2 = new uint[10];
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapType_Room_1"), ref array2[0]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapType_Room_2"), ref array2[1]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapType_Room_3"), ref array2[2]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapType_Room_4"), ref array2[3]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapType_Room_5"), ref array2[4]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapType_Room_6"), ref array2[5]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapType_Room_7"), ref array2[6]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapType_Room_8"), ref array2[7]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapType_Room_9"), ref array2[8]);
			uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapType_Room_10"), ref array2[9]);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != 0u)
				{
					ResDT_LevelCommonInfo pvpMapCommonInfo = CLevelCfgLogicManager.GetPvpMapCommonInfo((byte)array2[i], array[i]);
					if (pvpMapCommonInfo != null)
					{
						RoomMapInfo roomMapInfo = new RoomMapInfo();
						roomMapInfo.mapType = (byte)array2[i];
						roomMapInfo.mapID = array[i];
						this.mapList.Add(roomMapInfo);
					}
				}
			}
			GameObject gameObject = rootFormScript.transform.Find("Panel_Main/List").gameObject;
			CUIListScript component = gameObject.GetComponent<CUIListScript>();
			component.SetElementAmount(this.mapList.get_Count());
			for (int j = 0; j < component.m_elementAmount; j++)
			{
				CUIListElementScript elemenet = component.GetElemenet(j);
				Image component2 = elemenet.transform.GetComponent<Image>();
				string prefabPath = CUIUtility.s_Sprite_Dynamic_PvpEntry_Dir + this.mapList.get_Item(j).mapID;
				component2.SetSprite(prefabPath, rootFormScript, true, false, false, false);
				uint num = 0u;
				uint.TryParse(Singleton<CTextManager>.get_instance().GetText("MapID_PVP_5V5Miwu"), ref num);
				if (this.mapList.get_Item(j).mapID == num)
				{
					elemenet.transform.FindChild("Flag").gameObject.CustomSetActive(true);
				}
			}
			component.SelectElement(-1, true);
			if (CSysDynamicBlock.bLobbyEntryBlocked)
			{
				Transform transform = rootFormScript.transform.Find("panelGroupBottom");
				if (transform)
				{
					transform.gameObject.CustomSetActive(false);
				}
			}
		}

		private void OnRoom_OpenCreateForm(CUIEvent uiEvent)
		{
			if (!Singleton<SCModuleControl>.get_instance().GetActiveModule(512))
			{
				Singleton<CUIManager>.get_instance().OpenMessageBox(Singleton<SCModuleControl>.get_instance().PvpAndPvpOffTips, false);
				return;
			}
			CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CRoomSystem.PATH_CREATE_ROOM, false, true);
			this.InitMaps(cUIFormScript);
			this.ShowBonusImage(cUIFormScript);
			this.entertainmentAddLock(cUIFormScript);
		}

		private void OnRoom_CreateRoom(CUIEvent uiEvent)
		{
			if (this.mapId > 0u)
			{
				Singleton<CMatchingSystem>.get_instance().cacheMathingInfo.uiEventId = uiEvent.m_eventID;
				Singleton<CMatchingSystem>.get_instance().cacheMathingInfo.mapType = this.mapType;
				Singleton<CMatchingSystem>.get_instance().cacheMathingInfo.mapId = this.mapId;
				CRoomSystem.ReqCreateRoom(this.mapId, this.mapType, false);
			}
		}

		private void OnRoom_CloseForm(CUIEvent uiEvent)
		{
			CRoomSystem.ReqLeaveRoom();
		}

		private void OnRoom_SelectMap(CUIEvent uiEvent)
		{
			int selectedIndex = uiEvent.m_srcWidget.GetComponent<CUIListScript>().GetSelectedIndex();
			if (selectedIndex < 0 || selectedIndex >= this.mapList.get_Count())
			{
				return;
			}
			RoomMapInfo roomMapInfo = this.mapList.get_Item(selectedIndex);
			this.mapId = roomMapInfo.mapID;
			this.mapType = roomMapInfo.mapType;
			if (this.mapId == this.MeleeMapId && !Singleton<CFunctionUnlockSys>.get_instance().FucIsUnlock(25))
			{
				ResSpecialFucUnlock dataByKey = GameDataMgr.specialFunUnlockDatabin.GetDataByKey(25u);
				Singleton<CUIManager>.GetInstance().OpenTips(dataByKey.szLockedTip, false, 1.5f, null, new object[0]);
				return;
			}
			Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.Room_CreateRoom);
		}

		private void OnRoom_OpenInvite(CUIEvent uiEvent)
		{
		}

		private void OnRoom_StartGame(CUIEvent uiEvent)
		{
			if (this.IsSelfRoomOwner)
			{
				Button component = uiEvent.m_srcWidget.GetComponent<Button>();
				if (component.interactable)
				{
					CRoomSystem.ReqStartGame();
				}
			}
			else
			{
				DebugHelper.Assert(false);
			}
		}

		private void OnRoom_AddRobot(CUIEvent uiEvent)
		{
			if (this.IsSelfRoomOwner)
			{
				COM_PLAYERCAMP camp = 2;
				if (uiEvent.m_eventParams.tag == 1)
				{
					camp = 1;
				}
				else if (uiEvent.m_eventParams.tag == 2)
				{
					camp = 2;
				}
				CRoomSystem.ReqAddRobot(camp);
			}
			else
			{
				DebugHelper.Assert(false);
			}
		}

		private void OnRoom_ChangePos(CUIEvent uiEvent)
		{
			COM_PLAYERCAMP tag = uiEvent.m_eventParams.tag;
			int tag2 = uiEvent.m_eventParams.tag2;
			COM_CHGROOMPOS_TYPE tag3 = uiEvent.m_eventParams.tag3;
			CRoomSystem.ReqChangeCamp(tag, tag2, tag3);
		}

		private void OnRoom_KickPlayer(CUIEvent uiEvent)
		{
			if (this.IsSelfRoomOwner)
			{
				GameObject gameObject = uiEvent.m_srcWidget.transform.parent.parent.gameObject;
				COM_PLAYERCAMP camp = 1;
				int pos = 0;
				this.GetMemberPosInfo(gameObject, out camp, out pos);
				CRoomSystem.ReqKickPlayer(camp, pos);
			}
			else
			{
				DebugHelper.Assert(false, "Not Room Owner!");
			}
		}

		private void OnRoom_LeaveRoom(CUIEvent uiEvent)
		{
			CRoomSystem.ReqLeaveRoom();
		}

		private void OnRoom_AddFriend(CUIEvent uiEvent)
		{
			GameObject gameObject = uiEvent.m_srcWidget.transform.parent.parent.gameObject;
			COM_PLAYERCAMP cOM_PLAYERCAMP = 1;
			int num = 0;
			this.GetMemberPosInfo(gameObject, out cOM_PLAYERCAMP, out num);
			if (this.roomInfo != null)
			{
				MemberInfo memberInfo = this.roomInfo.GetMemberInfo(cOM_PLAYERCAMP, num);
				DebugHelper.Assert(memberInfo != null, "Room member info is NULL!! Camp -- {0}, Pos -- {1}", new object[]
				{
					cOM_PLAYERCAMP,
					num
				});
				if (memberInfo != null)
				{
					Singleton<CFriendContoller>.get_instance().Open_Friend_Verify(memberInfo.ullUid, (uint)memberInfo.iFromGameEntity, false, 0, -1, true);
				}
			}
		}

		private void OnRoom_ShareFriend(CUIEvent uiEvent)
		{
			if (Singleton<CRoomSystem>.GetInstance().IsInRoom)
			{
				this.OnRoom_ShareFriend_Room(uiEvent);
			}
			else if (Singleton<CMatchingSystem>.GetInstance().IsInMatchingTeam)
			{
				Singleton<CMatchingSystem>.GetInstance().OnTeam_ShareFriend_Team(uiEvent);
			}
		}

		private void OnRoomChangePosTimeUp(CUIEvent uiEvent)
		{
			this.RestMasterSwapInfo();
		}

		private void RestMasterSwapInfo()
		{
			CRoomView.ResetSwapView();
			MemberInfo masterMemberInfo = Singleton<CRoomSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			if (masterMemberInfo == null)
			{
				return;
			}
			masterMemberInfo.swapSeq = 0u;
			masterMemberInfo.swapStatus = 0;
			masterMemberInfo.swapUid = 0uL;
		}

		private void OnRoomChangePosConfirm(CUIEvent uiEvent)
		{
			if (Singleton<CRoomSystem>.get_instance().roomInfo == null)
			{
				return;
			}
			MemberInfo masterMemberInfo = Singleton<CRoomSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2037u);
			cSPkg.stPkgData.get_stChgRoomPosConfirmReq().bIsAccept = 1;
			cSPkg.stPkgData.get_stChgRoomPosConfirmReq().dwChgSeq = masterMemberInfo.swapSeq;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			Singleton<CRoomSystem>.get_instance().RestMasterSwapInfo();
		}

		private void OnRoomChangePosRefuse(CUIEvent uiEvent)
		{
			if (Singleton<CRoomSystem>.get_instance().roomInfo == null)
			{
				return;
			}
			MemberInfo masterMemberInfo = Singleton<CRoomSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2037u);
			cSPkg.stPkgData.get_stChgRoomPosConfirmReq().bIsAccept = 0;
			cSPkg.stPkgData.get_stChgRoomPosConfirmReq().dwChgSeq = masterMemberInfo.swapSeq;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
			Singleton<CRoomSystem>.get_instance().RestMasterSwapInfo();
		}

		private void OnRoomChangePosBoxTimerChange(CUIEvent uiEvent)
		{
			CRoomView.UpdateSwapBox(uiEvent.m_eventParams.tag2, uiEvent.m_eventParams.tag);
		}

		private void OnRoomClose(CUIEvent uiEvent)
		{
			Singleton<CUIManager>.get_instance().CloseForm(CRoomSystem.PATH_ROOM_SWAP);
		}

		private void OnRoom_ShareFriend_Room(CUIEvent uiEvent)
		{
			if (this.roomInfo == null || this.roomInfo.roomAttrib == null)
			{
				return;
			}
			uint dwMapId = this.roomInfo.roomAttrib.dwMapId;
			int bMapType = (int)this.roomInfo.roomAttrib.bMapType;
			string text = string.Empty;
			string text2 = string.Empty;
			ResDT_LevelCommonInfo pvpMapCommonInfo = CLevelCfgLogicManager.GetPvpMapCommonInfo((byte)bMapType, dwMapId);
			int bMaxAcntNum = (int)pvpMapCommonInfo.bMaxAcntNum;
			text = pvpMapCommonInfo.szName;
			if (bMapType == 3)
			{
				text2 = Singleton<CTextManager>.get_instance().GetText("Common_Team_Player_Type_6");
			}
			else
			{
				text2 = Singleton<CTextManager>.get_instance().GetText(string.Format("Common_Team_Player_Type_{0}", bMaxAcntNum / 2));
			}
			string text3 = Singleton<CTextManager>.GetInstance().GetText("Share_Room_Info_Title");
			string text4 = Singleton<CTextManager>.get_instance().GetText("Share_Room_Info_Desc", new string[]
			{
				text2,
				text
			});
			string text5 = MonoSingleton<ShareSys>.GetInstance().PackRoomData(this.roomInfo.iRoomEntity, this.roomInfo.dwRoomID, this.roomInfo.dwRoomSeq, this.roomInfo.roomAttrib.bMapType, this.roomInfo.roomAttrib.dwMapId, this.roomInfo.roomAttrib.ullFeature);
			Singleton<ApolloHelper>.GetInstance().InviteFriendToRoom(text3, text4, text5);
		}

		private void GetMemberPosInfo(GameObject go, out COM_PLAYERCAMP Camp, out int Pos)
		{
			Camp = 1;
			if (go.name.StartsWith("Left"))
			{
				Camp = 1;
			}
			else if (go.name.StartsWith("Right"))
			{
				Camp = 2;
			}
			Pos = 0;
			if (go.name.EndsWith("1"))
			{
				Pos = 0;
			}
			else if (go.name.EndsWith("2"))
			{
				Pos = 1;
			}
			else if (go.name.EndsWith("3"))
			{
				Pos = 2;
			}
			else if (go.name.EndsWith("4"))
			{
				Pos = 3;
			}
			else if (go.name.EndsWith("5"))
			{
				Pos = 4;
			}
		}

		private void ShowBonusImage(CUIFormScript form)
		{
			if (form == null)
			{
				return;
			}
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo();
			GameObject gameObject = form.transform.FindChild("panelGroupBottom/ButtonTrain/ImageBonus").gameObject;
			if (masterRoleInfo != null && masterRoleInfo.IsTrainingLevelFin())
			{
				gameObject.CustomSetActive(false);
			}
			else
			{
				gameObject.CustomSetActive(true);
			}
		}

		private void entertainmentAddLock(CUIFormScript form)
		{
			CUIListScript component = form.transform.FindChild("Panel_Main/List").GetComponent<CUIListScript>();
			for (int i = 0; i < this.mapList.get_Count(); i++)
			{
				if (this.mapList.get_Item(i) == null)
				{
					break;
				}
				if (this.mapList.get_Item(i).mapID == this.MeleeMapId)
				{
					Transform transform = component.GetElemenet(i).transform;
					if (transform != null)
					{
						if (!Singleton<CFunctionUnlockSys>.get_instance().FucIsUnlock(25))
						{
							transform.GetComponent<Image>().color = CUIUtility.s_Color_Button_Disable;
							transform.FindChild("Lock").gameObject.CustomSetActive(true);
							ResSpecialFucUnlock dataByKey = GameDataMgr.specialFunUnlockDatabin.GetDataByKey(25u);
							transform.FindChild("Lock/Text").GetComponent<Text>().text = Utility.UTF8Convert(dataByKey.szLockedTip);
						}
						else
						{
							transform.GetComponent<Image>().color = CUIUtility.s_Color_White;
							transform.FindChild("Lock").gameObject.CustomSetActive(false);
						}
					}
					break;
				}
			}
		}

		public void SetRoomType(int roomType)
		{
			this.m_roomType = roomType;
		}

		private MemberInfo CreateMemInfo(ref COMDT_ROOMMEMBER_DT memberDT, COM_PLAYERCAMP camp, bool bWarmBattle)
		{
			MemberInfo memberInfo = new MemberInfo();
			memberInfo.RoomMemberType = memberDT.dwRoomMemberType;
			memberInfo.dwPosOfCamp = memberDT.dwPosOfCamp;
			memberInfo.camp = camp;
			if (memberDT.dwRoomMemberType == 1u)
			{
				memberInfo.ullUid = memberDT.stMemberDetail.get_stMemberOfAcnt().ullUid;
				memberInfo.iFromGameEntity = memberDT.stMemberDetail.get_stMemberOfAcnt().iFromGameEntity;
				memberInfo.iLogicWorldID = memberDT.stMemberDetail.get_stMemberOfAcnt().iLogicWorldID;
				memberInfo.MemberName = StringHelper.UTF8BytesToString(ref memberDT.stMemberDetail.get_stMemberOfAcnt().szMemberName);
				memberInfo.dwMemberLevel = memberDT.stMemberDetail.get_stMemberOfAcnt().dwMemberLevel;
				memberInfo.dwMemberPvpLevel = memberDT.stMemberDetail.get_stMemberOfAcnt().dwMemberPvpLevel;
				memberInfo.dwMemberHeadId = memberDT.stMemberDetail.get_stMemberOfAcnt().dwMemberHeadId;
				memberInfo.MemberHeadUrl = StringHelper.UTF8BytesToString(ref memberDT.stMemberDetail.get_stMemberOfAcnt().szMemberHeadUrl);
				memberInfo.ChoiceHero = new COMDT_CHOICEHERO[1];
				memberInfo.ChoiceHero[0] = new COMDT_CHOICEHERO();
				memberInfo.recentUsedHero = new COMDT_RECENT_USED_HERO();
				memberInfo.canUseHero = new uint[0];
				memberInfo.isPrepare = false;
				memberInfo.dwObjId = 0u;
			}
			else if (memberDT.dwRoomMemberType == 2u)
			{
				MemberInfo arg_155_0 = memberInfo;
				ulong expr_14C = CRoomSystem.NpcUlId;
				CRoomSystem.NpcUlId = expr_14C + 1uL;
				arg_155_0.ullUid = expr_14C;
				memberInfo.iFromGameEntity = 0;
				memberInfo.MemberName = Singleton<CTextManager>.GetInstance().GetText("PVP_NPC");
				memberInfo.dwMemberLevel = (uint)memberDT.stMemberDetail.get_stMemberOfNpc().bLevel;
				memberInfo.dwMemberHeadId = 1u;
				memberInfo.ChoiceHero = new COMDT_CHOICEHERO[1];
				memberInfo.ChoiceHero[0] = new COMDT_CHOICEHERO();
				memberInfo.recentUsedHero = new COMDT_RECENT_USED_HERO();
				memberInfo.canUseHero = new uint[0];
				memberInfo.isPrepare = true;
				memberInfo.dwObjId = 0u;
				memberInfo.WarmNpc = memberDT.stMemberDetail.get_stMemberOfNpc().stDetail;
				if (bWarmBattle)
				{
					memberInfo.ullUid = memberInfo.WarmNpc.ullUid;
					memberInfo.dwMemberPvpLevel = memberInfo.WarmNpc.dwAcntPvpLevel;
					memberInfo.MemberName = StringHelper.UTF8BytesToString(ref memberInfo.WarmNpc.szUserName);
					memberInfo.MemberHeadUrl = StringHelper.UTF8BytesToString(ref memberInfo.WarmNpc.szUserHeadUrl);
					memberInfo.isPrepare = false;
				}
			}
			return memberInfo;
		}

		public void BuildRoomInfo(COMDT_MATCH_SUCC_DETAIL roomData)
		{
			this.roomInfo = new RoomInfo();
			this.roomInfo.iRoomEntity = roomData.iRoomEntity;
			this.roomInfo.dwRoomID = roomData.dwRoomID;
			this.roomInfo.dwRoomSeq = roomData.dwRoomSeq;
			this.roomInfo.roomAttrib.bGameMode = roomData.stRoomInfo.bGameMode;
			this.roomInfo.roomAttrib.bPkAI = roomData.stRoomInfo.bPkAI;
			this.roomInfo.roomAttrib.bMapType = roomData.stRoomInfo.bMapType;
			this.roomInfo.roomAttrib.dwMapId = roomData.stRoomInfo.dwMapId;
			this.roomInfo.roomAttrib.bWarmBattle = Convert.ToBoolean(roomData.stRoomInfo.bIsWarmBattle);
			this.roomInfo.roomAttrib.npcAILevel = roomData.stRoomInfo.bAILevel;
			this.roomInfo.roomAttrib.ullFeature = roomData.stRoomInfo.ullFeature;
			ResDT_LevelCommonInfo pvpMapCommonInfo = CLevelCfgLogicManager.GetPvpMapCommonInfo(this.roomInfo.roomAttrib.bMapType, this.roomInfo.roomAttrib.dwMapId);
			this.roomInfo.roomAttrib.judgeNum = (int)((pvpMapCommonInfo == null) ? 0u : pvpMapCommonInfo.dwJudgeNum);
			this.roomInfo.selfInfo.ullUid = Singleton<CRoleInfoManager>.get_instance().GetMasterRoleInfo().playerUllUID;
			for (int i = 0; i < 3; i++)
			{
				COM_PLAYERCAMP camp = i;
				ListView<MemberInfo> listView = this.roomInfo[camp];
				listView.Clear();
				int num = 0;
				while ((long)num < (long)((ulong)roomData.stMemInfo.astCampMem[i].dwMemNum))
				{
					COMDT_ROOMMEMBER_DT cOMDT_ROOMMEMBER_DT = roomData.stMemInfo.astCampMem[i].astMemInfo[num];
					MemberInfo memberInfo = this.CreateMemInfo(ref cOMDT_ROOMMEMBER_DT, camp, this.roomInfo.roomAttrib.bWarmBattle);
					listView.Add(memberInfo);
					num++;
				}
				this.roomInfo.SortCampMemList(camp);
			}
		}

		public void BuildRoomInfo(COMDT_JOINMULTGAMERSP_SUCC roomData)
		{
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo == null)
			{
				return;
			}
			this.roomInfo = new RoomInfo();
			this.roomInfo.iRoomEntity = roomData.iRoomEntity;
			this.roomInfo.dwRoomID = roomData.dwRoomID;
			this.roomInfo.dwRoomSeq = roomData.dwRoomSeq;
			this.roomInfo.roomAttrib.bGameMode = roomData.stRoomInfo.bGameMode;
			this.roomInfo.roomAttrib.bPkAI = 0;
			this.roomInfo.roomAttrib.bMapType = roomData.stRoomInfo.bMapType;
			this.roomInfo.roomAttrib.dwMapId = roomData.stRoomInfo.dwMapId;
			this.roomInfo.roomAttrib.ullFeature = roomData.stRoomInfo.ullFeature;
			this.roomInfo.roomAttrib.bWarmBattle = false;
			this.roomInfo.roomAttrib.npcAILevel = 2;
			ResDT_LevelCommonInfo pvpMapCommonInfo = CLevelCfgLogicManager.GetPvpMapCommonInfo(this.roomInfo.roomAttrib.bMapType, this.roomInfo.roomAttrib.dwMapId);
			this.roomInfo.roomAttrib.judgeNum = (int)((pvpMapCommonInfo == null) ? 0u : pvpMapCommonInfo.dwJudgeNum);
			this.roomInfo.selfInfo.ullUid = roomData.ullSelfUid;
			this.roomInfo.selfInfo.iGameEntity = roomData.iSelfGameEntity;
			this.roomInfo.roomOwner.ullUid = roomData.stRoomMaster.ullMasterUid;
			this.roomInfo.roomOwner.iGameEntity = roomData.stRoomMaster.iMasterGameEntity;
			for (int i = 0; i < 3; i++)
			{
				COM_PLAYERCAMP camp = i;
				ListView<MemberInfo> listView = this.roomInfo[camp];
				listView.Clear();
				int num = 0;
				while ((long)num < (long)((ulong)roomData.stMemInfo.astCampMem[i].dwMemNum))
				{
					COMDT_ROOMMEMBER_DT cOMDT_ROOMMEMBER_DT = roomData.stMemInfo.astCampMem[i].astMemInfo[num];
					MemberInfo memberInfo = this.CreateMemInfo(ref cOMDT_ROOMMEMBER_DT, camp, this.roomInfo.roomAttrib.bWarmBattle);
					if (memberInfo.ullUid == masterRoleInfo.playerUllUID)
					{
						this.roomInfo.selfObjID = memberInfo.dwObjId;
					}
					listView.Add(memberInfo);
					num++;
				}
				this.roomInfo.SortCampMemList(camp);
			}
		}

		public void UpdateRoomInfoReconnectPick(COMDT_DESKINFO inDeskInfo, CSDT_RECONN_CAMPPICKINFO[] inCampInfo)
		{
			this.roomInfo = new RoomInfo();
			this.roomInfo.roomAttrib.bGameMode = inDeskInfo.bGameMode;
			this.roomInfo.roomAttrib.bPkAI = 0;
			this.roomInfo.roomAttrib.bMapType = inDeskInfo.bMapType;
			this.roomInfo.roomAttrib.dwMapId = inDeskInfo.dwMapId;
			this.roomInfo.roomAttrib.bWarmBattle = Convert.ToBoolean(inDeskInfo.bIsWarmBattle);
			this.roomInfo.roomAttrib.npcAILevel = inDeskInfo.bAILevel;
			ResDT_LevelCommonInfo pvpMapCommonInfo = CLevelCfgLogicManager.GetPvpMapCommonInfo(this.roomInfo.roomAttrib.bMapType, this.roomInfo.roomAttrib.dwMapId);
			this.roomInfo.roomAttrib.judgeNum = (int)((pvpMapCommonInfo == null) ? 0u : pvpMapCommonInfo.dwJudgeNum);
			for (int i = 0; i < inCampInfo.Length; i++)
			{
				COM_PLAYERCAMP camp = i + 1;
				CSDT_RECONN_CAMPPICKINFO cSDT_RECONN_CAMPPICKINFO = inCampInfo[i];
				ListView<MemberInfo> listView = this.roomInfo[camp];
				listView.Clear();
				int num = 0;
				while ((long)num < (long)((ulong)cSDT_RECONN_CAMPPICKINFO.dwPlayerNum))
				{
					MemberInfo memberInfo = new MemberInfo();
					COMDT_PLAYERINFO stPlayerInfo = cSDT_RECONN_CAMPPICKINFO.astPlayerInfo[num].stPickHeroInfo.stPlayerInfo;
					COMDT_ACNT_USABLE_HERO stUsableHero = cSDT_RECONN_CAMPPICKINFO.astPlayerInfo[num].stPickHeroInfo.stUsableHero;
					memberInfo.isPrepare = (cSDT_RECONN_CAMPPICKINFO.astPlayerInfo[num].bIsPickOK > 0);
					memberInfo.RoomMemberType = (uint)stPlayerInfo.bObjType;
					memberInfo.dwPosOfCamp = (uint)stPlayerInfo.bPosOfCamp;
					memberInfo.camp = camp;
					memberInfo.dwMemberLevel = stPlayerInfo.dwLevel;
					if (memberInfo.RoomMemberType == 1u)
					{
						memberInfo.ullUid = stPlayerInfo.stDetail.get_stPlayerOfAcnt().ullUid;
						memberInfo.dwMemberPvpLevel = stPlayerInfo.stDetail.get_stPlayerOfAcnt().dwPvpLevel;
					}
					memberInfo.dwObjId = stPlayerInfo.dwObjId;
					memberInfo.MemberName = StringHelper.UTF8BytesToString(ref stPlayerInfo.szName);
					memberInfo.ChoiceHero = stPlayerInfo.astChoiceHero;
					memberInfo.canUseHero = stUsableHero.HeroDetail;
					memberInfo.recentUsedHero = cSDT_RECONN_CAMPPICKINFO.astPlayerInfo[num].stPickHeroInfo.stRecentUsedHero;
					memberInfo.isGM = (cSDT_RECONN_CAMPPICKINFO.astPlayerInfo[num].stPickHeroInfo.bIsGM > 0);
					if (stPlayerInfo.bObjType == 1)
					{
						memberInfo.dwMemberHeadId = stPlayerInfo.stDetail.get_stPlayerOfAcnt().dwHeadId;
						if (stPlayerInfo.stDetail.get_stPlayerOfAcnt().ullUid == Singleton<CRoleInfoManager>.get_instance().masterUUID)
						{
							this.roomInfo.selfObjID = stPlayerInfo.dwObjId;
							Singleton<CHeroSelectBaseSystem>.get_instance().ResetRandHeroLeftCount((int)cSDT_RECONN_CAMPPICKINFO.astPlayerInfo[num].stPickHeroInfo.dwRandomHeroCnt);
						}
						memberInfo.ullUid = stPlayerInfo.stDetail.get_stPlayerOfAcnt().ullUid;
					}
					else if (stPlayerInfo.bObjType == 2)
					{
						memberInfo.dwMemberHeadId = 1u;
						memberInfo.ullUid = stPlayerInfo.stDetail.get_stPlayerOfNpc().ullFakeUid;
					}
					listView.Add(memberInfo);
					num++;
				}
				this.roomInfo.SortCampMemList(camp);
			}
		}

		public void UpdateRoomInfo(COMDT_DESKINFO inDeskInfo, CSDT_CAMPINFO[] inCampInfo)
		{
			uint selfObjID = 0u;
			if (this.roomInfo == null)
			{
				this.roomInfo = new RoomInfo();
				this.roomInfo.roomAttrib.bGameMode = inDeskInfo.bGameMode;
				this.roomInfo.roomAttrib.bPkAI = 0;
				this.roomInfo.roomAttrib.bMapType = inDeskInfo.bMapType;
				this.roomInfo.roomAttrib.dwMapId = inDeskInfo.dwMapId;
				this.roomInfo.roomAttrib.bWarmBattle = Convert.ToBoolean(inDeskInfo.bIsWarmBattle);
				this.roomInfo.roomAttrib.npcAILevel = inDeskInfo.bAILevel;
				ResDT_LevelCommonInfo pvpMapCommonInfo = CLevelCfgLogicManager.GetPvpMapCommonInfo(this.roomInfo.roomAttrib.bMapType, this.roomInfo.roomAttrib.dwMapId);
				this.roomInfo.roomAttrib.judgeNum = (int)((pvpMapCommonInfo == null) ? 0u : pvpMapCommonInfo.dwJudgeNum);
				for (int i = 0; i < inCampInfo.Length; i++)
				{
					COM_PLAYERCAMP camp = i + 1;
					CSDT_CAMPINFO cSDT_CAMPINFO = inCampInfo[i];
					ListView<MemberInfo> listView = this.roomInfo[camp];
					listView.Clear();
					int num = 0;
					while ((long)num < (long)((ulong)cSDT_CAMPINFO.dwPlayerNum))
					{
						MemberInfo memberInfo = new MemberInfo();
						COMDT_PLAYERINFO stPlayerInfo = cSDT_CAMPINFO.astCampPlayerInfo[num].stPlayerInfo;
						COMDT_ACNT_USABLE_HERO stUsableHero = cSDT_CAMPINFO.astCampPlayerInfo[num].stUsableHero;
						memberInfo.RoomMemberType = (uint)stPlayerInfo.bObjType;
						memberInfo.dwPosOfCamp = (uint)stPlayerInfo.bPosOfCamp;
						memberInfo.camp = camp;
						memberInfo.dwMemberLevel = stPlayerInfo.dwLevel;
						if (memberInfo.RoomMemberType == 1u)
						{
							memberInfo.dwMemberPvpLevel = stPlayerInfo.stDetail.get_stPlayerOfAcnt().dwPvpLevel;
						}
						memberInfo.dwObjId = stPlayerInfo.dwObjId;
						memberInfo.MemberName = StringHelper.UTF8BytesToString(ref stPlayerInfo.szName);
						memberInfo.ChoiceHero = stPlayerInfo.astChoiceHero;
						memberInfo.canUseHero = stUsableHero.HeroDetail;
						memberInfo.recentUsedHero = cSDT_CAMPINFO.astCampPlayerInfo[num].stRecentUsedHero;
						if (stPlayerInfo.bObjType == 1)
						{
							memberInfo.dwMemberHeadId = stPlayerInfo.stDetail.get_stPlayerOfAcnt().dwHeadId;
							if (stPlayerInfo.stDetail.get_stPlayerOfAcnt().ullUid == Singleton<CRoleInfoManager>.get_instance().masterUUID)
							{
								selfObjID = stPlayerInfo.dwObjId;
								Singleton<CHeroSelectBaseSystem>.get_instance().ResetRandHeroLeftCount((int)cSDT_CAMPINFO.astCampPlayerInfo[num].dwRandomHeroCnt);
							}
							memberInfo.ullUid = stPlayerInfo.stDetail.get_stPlayerOfAcnt().ullUid;
						}
						else if (stPlayerInfo.bObjType == 2)
						{
							memberInfo.dwMemberHeadId = 1u;
							memberInfo.ullUid = stPlayerInfo.stDetail.get_stPlayerOfNpc().ullFakeUid;
						}
						listView.Add(memberInfo);
						num++;
					}
					this.roomInfo.SortCampMemList(camp);
				}
			}
			else
			{
				this.roomInfo.roomAttrib.bGameMode = inDeskInfo.bGameMode;
				this.roomInfo.roomAttrib.bPkAI = 0;
				this.roomInfo.roomAttrib.bMapType = inDeskInfo.bMapType;
				this.roomInfo.roomAttrib.dwMapId = inDeskInfo.dwMapId;
				for (int j = 0; j < inCampInfo.Length; j++)
				{
					COM_PLAYERCAMP camp2 = j + 1;
					CSDT_CAMPINFO cSDT_CAMPINFO2 = inCampInfo[j];
					ListView<MemberInfo> listView2 = this.roomInfo[camp2];
					int num2 = 0;
					while ((long)num2 < (long)((ulong)cSDT_CAMPINFO2.dwPlayerNum))
					{
						COMDT_PLAYERINFO stPlayerInfo2 = cSDT_CAMPINFO2.astCampPlayerInfo[num2].stPlayerInfo;
						COMDT_ACNT_USABLE_HERO stUsableHero2 = cSDT_CAMPINFO2.astCampPlayerInfo[num2].stUsableHero;
						MemberInfo memberInfo2 = this.roomInfo.GetMemberInfo(camp2, (int)stPlayerInfo2.bPosOfCamp);
						if (memberInfo2 != null)
						{
							memberInfo2.dwObjId = stPlayerInfo2.dwObjId;
							memberInfo2.camp = camp2;
							memberInfo2.ChoiceHero = stPlayerInfo2.astChoiceHero;
							memberInfo2.canUseHero = stUsableHero2.HeroDetail;
							memberInfo2.recentUsedHero = cSDT_CAMPINFO2.astCampPlayerInfo[num2].stRecentUsedHero;
							if (stPlayerInfo2.bObjType == 1)
							{
								memberInfo2.dwMemberHeadId = stPlayerInfo2.stDetail.get_stPlayerOfAcnt().dwHeadId;
								if (stPlayerInfo2.stDetail.get_stPlayerOfAcnt().ullUid == Singleton<CRoleInfoManager>.get_instance().masterUUID)
								{
									selfObjID = stPlayerInfo2.dwObjId;
									Singleton<CHeroSelectBaseSystem>.get_instance().ResetRandHeroLeftCount((int)cSDT_CAMPINFO2.astCampPlayerInfo[num2].dwRandomHeroCnt);
								}
								memberInfo2.ullUid = stPlayerInfo2.stDetail.get_stPlayerOfAcnt().ullUid;
							}
							else if (stPlayerInfo2.bObjType == 2)
							{
								memberInfo2.dwMemberHeadId = 1u;
								memberInfo2.ullUid = stPlayerInfo2.stDetail.get_stPlayerOfNpc().ullFakeUid;
							}
						}
						num2++;
					}
					this.roomInfo.SortCampMemList(camp2);
				}
			}
			this.roomInfo.selfObjID = selfObjID;
		}

		public static void ReqCreateRoom(uint MapId, byte mapType, bool isInviteFriendImmediately = false)
		{
			CInviteSystem.s_isInviteFriendImmidiately = isInviteFriendImmediately;
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1020u);
			StringHelper.StringToUTF8Bytes("testRoom", ref cSPkg.stPkgData.get_stCreateMultGameReq().szRoomName);
			cSPkg.stPkgData.get_stCreateMultGameReq().bMapType = mapType;
			cSPkg.stPkgData.get_stCreateMultGameReq().dwMapId = MapId;
			cSPkg.stPkgData.get_stCreateMultGameReq().bGameMode = 1;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public static void ReqCreateRoomAndInvite(uint mapId, COM_BATTLE_MAP_TYPE mapType, CInviteSystem.stInviteInfo inviteInfo)
		{
			Singleton<CInviteSystem>.GetInstance().InviteInfo = inviteInfo;
			CRoomSystem.ReqCreateRoom(mapId, mapType, true);
		}

		public static void ReqLeaveRoom()
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1023u);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public static void ReqAddRobot(COM_PLAYERCAMP Camp)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2015u);
			cSPkg.stPkgData.get_stAddNpcReq().stNpcInfo.iCamp = Camp;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public static void ReqKickPlayer(COM_PLAYERCAMP Camp, int Pos)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2019u);
			cSPkg.stPkgData.get_stKickoutRoomMemberReq().stKickMemberInfo.iCamp = Camp;
			cSPkg.stPkgData.get_stKickoutRoomMemberReq().stKickMemberInfo.bPosOfCamp = (byte)Pos;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public static void ReqChangeCamp(COM_PLAYERCAMP Camp, int Pos, COM_CHGROOMPOS_TYPE type)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2033u);
			cSPkg.stPkgData.get_stChgMemberPosReq().bCamp = Camp;
			cSPkg.stPkgData.get_stChgMemberPosReq().bPosOfCamp = (byte)Pos;
			cSPkg.stPkgData.get_stChgMemberPosReq().bChgType = type;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		public static void ReqStartGame()
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(2013u);
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, true);
		}

		[MessageHandler(1022)]
		public static void OnPlayerJoinRoom(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stJoinMultGameRsp().iErrCode == 0)
			{
				Singleton<GameBuilder>.get_instance().EndGame();
				CRoomSystem instance = Singleton<CRoomSystem>.GetInstance();
				instance.bInRoom = true;
				instance.BuildRoomInfo(msg.stPkgData.get_stJoinMultGameRsp().stInfo.get_stOfSucc());
				CUIFormScript cUIFormScript = Singleton<CUIManager>.GetInstance().OpenForm(CRoomSystem.PATH_ROOM, false, true);
				Singleton<CTopLobbyEntry>.GetInstance().OpenForm();
				Singleton<CInviteSystem>.GetInstance().OpenInviteForm(1);
				CChatUT.EnterRoom();
				CRoomView.SetRoomData(cUIFormScript.gameObject, instance.roomInfo);
				CRoomObserve.SetObservers(Utility.FindChild(cUIFormScript.gameObject, "Panel_Main/Observers"), instance.roomInfo.roomAttrib.judgeNum, instance.roomInfo[0], instance.roomInfo.GetMasterMemberInfo());
				Singleton<CRoomSystem>.get_instance().RestMasterSwapInfo();
				Singleton<CMatchingSystem>.get_instance().cacheMathingInfo.CanGameAgain = instance.IsSelfRoomOwner;
				if (!instance.IsSelfRoomOwner)
				{
					MonoSingleton<NewbieGuideManager>.get_instance().StopCurrentGuide();
				}
				if (MonoSingleton<ShareSys>.get_instance().IsQQGameTeamCreate())
				{
					string roomStr = MonoSingleton<ShareSys>.get_instance().PackQQGameTeamData(instance.roomInfo.iRoomEntity, instance.roomInfo.dwRoomID, instance.roomInfo.dwRoomSeq, instance.roomInfo.roomAttrib.ullFeature);
					MonoSingleton<ShareSys>.get_instance().SendQQGameTeamStateChgMsg(ShareSys.QQGameTeamEventType.join, 2, instance.roomInfo.roomAttrib.bMapType, instance.roomInfo.roomAttrib.dwMapId, roomStr);
				}
				CMatchingSystem.CloseExcludeForm();
			}
			else if (msg.stPkgData.get_stJoinMultGameRsp().iErrCode == 26)
			{
				DateTime banTime = MonoSingleton<IDIPSys>.GetInstance().GetBanTime(9);
				string strContent = string.Format("您被禁止竞技！截止时间为{0}年{1}月{2}日{3}时{4}分", new object[]
				{
					banTime.get_Year(),
					banTime.get_Month(),
					banTime.get_Day(),
					banTime.get_Hour(),
					banTime.get_Minute()
				});
				Singleton<CUIManager>.GetInstance().OpenMessageBox(strContent, false);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips("PVP_Enter_Room_Error", true, 1f, null, new object[]
				{
					Utility.ProtErrCodeToStr(1022, msg.stPkgData.get_stJoinMultGameRsp().iErrCode)
				});
			}
		}

		[MessageHandler(2038)]
		public static void OnRoomChgPosNtf(CSPkg msg)
		{
			COM_CHGROOMPOS_RESULT bResult = msg.stPkgData.get_stChgRoomPosNtf().bResult;
			COMDT_ROOMCHGPOS_DATA stChgPosData = msg.stPkgData.get_stChgRoomPosNtf().stChgPosData;
			if (Singleton<CRoomSystem>.get_instance().roomInfo == null)
			{
				return;
			}
			MemberInfo masterMemberInfo = Singleton<CRoomSystem>.get_instance().roomInfo.GetMasterMemberInfo();
			if (masterMemberInfo == null)
			{
				return;
			}
			switch (bResult)
			{
			case 1:
				if (RoomInfo.IsSameMemeber(masterMemberInfo, stChgPosData.get_stMemberInfo().stSender.bCamp, (int)stChgPosData.get_stMemberInfo().stSender.bPosOfCamp))
				{
					MemberInfo memberInfo = Singleton<CRoomSystem>.get_instance().roomInfo.GetMemberInfo(stChgPosData.get_stMemberInfo().stReceiver.bCamp, (int)stChgPosData.get_stMemberInfo().stReceiver.bPosOfCamp);
					if (memberInfo == null)
					{
						return;
					}
					CRoomView.SetChgEnable(false);
					CRoomView.SetSwapTimer((int)stChgPosData.get_stMemberInfo().dwTimeOutSec, stChgPosData.get_stMemberInfo().stReceiver.bCamp, (int)stChgPosData.get_stMemberInfo().stReceiver.bPosOfCamp);
					CRoomView.ShowSwapMsg(0, 1, 0);
					masterMemberInfo.swapSeq = stChgPosData.get_stMemberInfo().dwChgPosSeq;
					masterMemberInfo.swapStatus = 1;
					masterMemberInfo.swapUid = memberInfo.ullUid;
					Singleton<CUIManager>.get_instance().CloseSendMsgAlert();
				}
				else if (RoomInfo.IsSameMemeber(masterMemberInfo, stChgPosData.get_stMemberInfo().stReceiver.bCamp, (int)stChgPosData.get_stMemberInfo().stReceiver.bPosOfCamp))
				{
					MemberInfo memberInfo2 = Singleton<CRoomSystem>.get_instance().roomInfo.GetMemberInfo(stChgPosData.get_stMemberInfo().stSender.bCamp, (int)stChgPosData.get_stMemberInfo().stSender.bPosOfCamp);
					if (memberInfo2 == null)
					{
						return;
					}
					CRoomView.SetChgEnable(false, stChgPosData.get_stMemberInfo().stSender.bCamp, (int)stChgPosData.get_stMemberInfo().stSender.bPosOfCamp);
					CRoomView.SetSwapTimer(0, 1, 0);
					CRoomView.ShowSwapMsg((int)stChgPosData.get_stMemberInfo().dwTimeOutSec, stChgPosData.get_stMemberInfo().stSender.bCamp, (int)stChgPosData.get_stMemberInfo().stSender.bPosOfCamp);
					masterMemberInfo.swapSeq = stChgPosData.get_stMemberInfo().dwChgPosSeq;
					masterMemberInfo.swapStatus = 2;
					masterMemberInfo.swapUid = memberInfo2.ullUid;
				}
				break;
			case 2:
				Singleton<CUIManager>.get_instance().OpenTips("Room_Change_Pos_Tip_4", true, 1.5f, null, new object[0]);
				Singleton<CUIManager>.get_instance().CloseSendMsgAlert();
				break;
			case 3:
				Singleton<CUIManager>.get_instance().OpenTips("Room_Change_Pos_Tip_5", true, 1.5f, null, new object[0]);
				Singleton<CUIManager>.get_instance().CloseSendMsgAlert();
				break;
			case 4:
				Singleton<CUIManager>.get_instance().OpenTips("Room_Change_Pos_Tip_6", true, 1.5f, null, new object[0]);
				Singleton<CRoomSystem>.get_instance().RestMasterSwapInfo();
				break;
			case 5:
				Singleton<CUIManager>.get_instance().OpenTips("Room_Change_Pos_Tip_7", true, 1.5f, null, new object[0]);
				Singleton<CRoomSystem>.get_instance().RestMasterSwapInfo();
				break;
			case 6:
				Singleton<CUIManager>.get_instance().OpenTips("Room_Change_Pos_Tip_8", true, 1.5f, null, new object[0]);
				Singleton<CRoomSystem>.get_instance().RestMasterSwapInfo();
				break;
			}
		}

		[MessageHandler(1024)]
		public static void OnLeaveRoom(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stQuitMultGameRsp().iErrCode == 0)
			{
				if (msg.stPkgData.get_stQuitMultGameRsp().bLevelFromType == 1)
				{
					Singleton<CRoomSystem>.GetInstance().bInRoom = false;
					Singleton<CUIManager>.GetInstance().CloseForm(CRoomSystem.PATH_ROOM);
					Singleton<CTopLobbyEntry>.GetInstance().CloseForm();
					Singleton<CInviteSystem>.GetInstance().CloseInviteForm();
					CChatUT.LeaveRoom();
				}
				else if (msg.stPkgData.get_stQuitMultGameRsp().bLevelFromType == 2)
				{
					CMatchingSystem.OnPlayerLeaveMatching();
				}
				MonoSingleton<ShareSys>.GetInstance().SendQQGameTeamStateChgMsg(ShareSys.QQGameTeamEventType.leave, 0, 0, 0u, string.Empty);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips("PVP_Exit_Room_Error", true, 1f, null, new object[]
				{
					Utility.ProtErrCodeToStr(1024, msg.stPkgData.get_stQuitMultGameRsp().iErrCode)
				});
			}
		}

		[MessageHandler(2014)]
		public static void OnRoomStarted(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			if (msg.stPkgData.get_stStartMultiGameRsp().bErrcode == 0)
			{
				Singleton<CRoomSystem>.get_instance().SetRoomType(2);
				MonoSingleton<ShareSys>.GetInstance().SendQQGameTeamStateChgMsg(ShareSys.QQGameTeamEventType.start, 0, 0, 0u, string.Empty);
			}
			else
			{
				Singleton<CUIManager>.GetInstance().OpenTips("PVP_Start_Game_Error", true, 1f, null, new object[]
				{
					Utility.ProtErrCodeToStr(2014, (int)msg.stPkgData.get_stStartMultiGameRsp().bErrcode)
				});
			}
		}

		[MessageHandler(1025)]
		public static void OnRoomChange(CSPkg msg)
		{
			Singleton<CUIManager>.GetInstance().CloseSendMsgAlert();
			RoomInfo roomInfo = Singleton<CRoomSystem>.GetInstance().roomInfo;
			if (roomInfo == null)
			{
				DebugHelper.Assert(false, "Room Info is NULL!!!");
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.iChgType == 0)
			{
				COM_PLAYERCAMP iCamp = msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.stChgInfo.get_stPlayerAdd().iCamp;
				MemberInfo memberInfo = Singleton<CRoomSystem>.GetInstance().CreateMemInfo(ref msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.stChgInfo.get_stPlayerAdd().stMemInfo, iCamp, roomInfo.roomAttrib.bWarmBattle);
				roomInfo[iCamp].Add(memberInfo);
				flag = true;
			}
			else if (msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.iChgType == 1)
			{
				COM_PLAYERCAMP iCamp2 = msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.stChgInfo.get_stPlayerLeave().iCamp;
				int bPos = (int)msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.stChgInfo.get_stPlayerLeave().bPos;
				ListView<MemberInfo> listView = roomInfo[iCamp2];
				for (int i = 0; i < listView.get_Count(); i++)
				{
					if ((ulong)listView.get_Item(i).dwPosOfCamp == (ulong)((long)bPos))
					{
						listView.RemoveAt(i);
						break;
					}
				}
				flag = true;
			}
			else if (msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.iChgType == 2)
			{
				Singleton<CRoomSystem>.GetInstance().bInRoom = false;
				Singleton<CUIManager>.GetInstance().CloseForm(CRoomSystem.PATH_CREATE_ROOM);
				Singleton<CUIManager>.GetInstance().CloseForm(CRoomSystem.PATH_ROOM);
				Singleton<CTopLobbyEntry>.GetInstance().CloseForm();
				Singleton<CInviteSystem>.GetInstance().CloseInviteForm();
				CChatUT.LeaveRoom();
				Singleton<CChatController>.get_instance().ShowPanel(false, false);
				Singleton<CUIManager>.GetInstance().OpenTips("PVP_Room_Kick_Tip", true, 1.5f, null, new object[0]);
				MonoSingleton<ShareSys>.GetInstance().SendQQGameTeamStateChgMsg(ShareSys.QQGameTeamEventType.leave, 0, 0, 0u, string.Empty);
			}
			else if (msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.iChgType == 4)
			{
				roomInfo.roomOwner.ullUid = msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.stChgInfo.get_stMasterChg().stNewMaster.ullMasterUid;
				roomInfo.roomOwner.iGameEntity = msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.stChgInfo.get_stMasterChg().stNewMaster.iMasterGameEntity;
				flag = true;
			}
			else if (msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.iChgType == 5)
			{
				COMDT_ROOMCHG_CHGMEMBERPOS stChgMemberPos = msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.stChgInfo.get_stChgMemberPos();
				ListView<MemberInfo> listView2 = roomInfo[stChgMemberPos.bOldCamp];
				ListView<MemberInfo> listView3 = roomInfo[stChgMemberPos.bNewCamp];
				MemberInfo memberInfo2 = roomInfo.GetMemberInfo(stChgMemberPos.ullMemberUid);
				if (memberInfo2 == null)
				{
					return;
				}
				if (memberInfo2.camp == stChgMemberPos.bNewCamp && memberInfo2.dwPosOfCamp == (uint)stChgMemberPos.bNewPosOfCamp)
				{
					return;
				}
				listView2.Remove(memberInfo2);
				MemberInfo memberInfo3 = roomInfo.GetMemberInfo(stChgMemberPos.bNewCamp, (int)stChgMemberPos.bNewPosOfCamp);
				DebugHelper.Assert(memberInfo2 != null, "srcMemberInfo is NULL!!");
				memberInfo2.camp = stChgMemberPos.bNewCamp;
				memberInfo2.dwPosOfCamp = (uint)stChgMemberPos.bNewPosOfCamp;
				listView3.Add(memberInfo2);
				if (memberInfo3 != null)
				{
					listView3.Remove(memberInfo3);
					memberInfo3.camp = stChgMemberPos.bOldCamp;
					memberInfo3.dwPosOfCamp = (uint)stChgMemberPos.bOldPosOfCamp;
					listView2.Add(memberInfo3);
				}
				if (roomInfo.GetMasterMemberInfo().ullUid == stChgMemberPos.ullMemberUid)
				{
					flag2 = true;
				}
				flag = true;
			}
			else if (msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.iChgType == 3)
			{
				enRoomState bOldState = (enRoomState)msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.stChgInfo.get_stStateChg().bOldState;
				enRoomState bNewState = (enRoomState)msg.stPkgData.get_stRoomChgNtf().stRoomChgInfo.stChgInfo.get_stStateChg().bNewState;
				if (bOldState == enRoomState.E_ROOM_PREPARE && bNewState == enRoomState.E_ROOM_WAIT)
				{
					Singleton<LobbyLogic>.GetInstance().inMultiRoom = false;
					Singleton<CHeroSelectBaseSystem>.get_instance().CloseForm();
					Singleton<CUIManager>.GetInstance().OpenForm(CRoomSystem.PATH_ROOM, false, true);
					CChatUT.EnterRoom();
				}
				if (bOldState == enRoomState.E_ROOM_WAIT && bNewState == enRoomState.E_ROOM_CONFIRM)
				{
					CUIEvent cUIEvent = new CUIEvent();
					cUIEvent.m_eventID = enUIEventID.Matching_OpenConfirmBox;
					cUIEvent.m_eventParams.tag = (int)roomInfo.roomAttrib.bPkAI;
					Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(cUIEvent);
					if (roomInfo.roomAttrib.bWarmBattle)
					{
						CFakePvPHelper.SetConfirmFakeData();
					}
				}
			}
			for (int j = 0; j < 3; j++)
			{
				roomInfo[j].Sort(new Comparison<MemberInfo>(CRoomSystem.SortMemeberFun));
			}
			CUIFormScript form = Singleton<CUIManager>.GetInstance().GetForm(CRoomSystem.PATH_ROOM);
			if (form != null)
			{
				if (flag)
				{
					CRoomView.SetRoomData(form.gameObject, roomInfo);
					CRoomObserve.SetObservers(Utility.FindChild(form.gameObject, "Panel_Main/Observers"), roomInfo.roomAttrib.judgeNum, roomInfo[0], roomInfo.GetMasterMemberInfo());
				}
				if (flag2)
				{
					Singleton<CRoomSystem>.get_instance().RestMasterSwapInfo();
				}
			}
		}

		private static int SortMemeberFun(MemberInfo left, MemberInfo right)
		{
			return (int)(left.dwPosOfCamp - right.dwPosOfCamp);
		}
	}
}
