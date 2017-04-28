using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.UI;
using CSProtocol;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class MinimapSys
	{
		public enum ElementType
		{
			None,
			Tower,
			Base,
			Hero,
			Dragon_5_big,
			Dragon_5_small,
			Dragon_3,
			Eye
		}

		public enum EMapType
		{
			None,
			Mini,
			Big,
			Skill
		}

		public class ElementInMap
		{
			public VInt3 pos;

			public GameObject smallElement;

			public GameObject bigElement;

			public ElementInMap(VInt3 pos, GameObject smallElement, GameObject bigElement)
			{
				this.pos = pos;
				this.smallElement = smallElement;
				this.bigElement = bigElement;
			}
		}

		public struct BornRecord
		{
			public VInt3 pos;

			public uint cfgId;
		}

		public static float DEPTH = 30f;

		public static string self_tower = "UGUI/Sprite/Battle/Img_Map_Tower_Green";

		public static string self_base = "UGUI/Sprite/Battle/Img_Map_Base_Green";

		public static string enemy_tower = "UGUI/Sprite/Battle/Img_Map_Tower_Red";

		public static string enemy_base = "UGUI/Sprite/Battle/Img_Map_Base_Red";

		public static string self_tower_outline = "UGUI/Sprite/Battle/Img_Map_Tower_Green_outline";

		public static string self_base_outline = "UGUI/Sprite/Battle/Img_Map_Base_Green_outline";

		public static string enemy_tower_outline = "UGUI/Sprite/Battle/Img_Map_Tower_Red_outline";

		public static string enemy_base_outline = "UGUI/Sprite/Battle/Img_Map_Base_Red_outline";

		public static string self_Eye = "UGUI/Sprite/Battle/Img_Map_BlueEye";

		public static string enemy_Eye = "UGUI/Sprite/Battle/Img_Map_RedEye";

		private List<MinimapSys.ElementInMap> m_mapElements = new List<MinimapSys.ElementInMap>();

		private List<MinimapSys.BornRecord> m_bornRecords = new List<MinimapSys.BornRecord>();

		private MinimapSys.EMapType curMapType;

		private CUIFormScript _ownerForm;

		private DragonIcon m_dragonIcon;

		public MiniMapCameraFrame_3DUI MMiniMapCameraFrame_3Dui;

		public BackCityCom_3DUI MMiniMapBackCityCom_3Dui;

		public MiniMapTrack_3DUI MMiniMapTrack_3Dui;

		public MinimapSkillIndicator_3DUI MMinimapSkillIndicator_3Dui;

		public UI3DEventMgr UI3DEventMgr = new UI3DEventMgr();

		private COM_PLAYERCAMP m_playerCamp = 3;

		private int m_elementIndex;

		private SkillSlotType m_CurSelectedSlotType = SkillSlotType.SLOT_SKILL_COUNT;

		private Vector2 m_mmStandardScreenPos;

		private Vector2 m_bmStandardScreenPos;

		private Vector2 m_mmFinalScreenPos;

		private Vector2 m_bmFinalScreenPos;

		private string miniMapPrefabPath = "UI3D/Battle/MiniMap/Mini_Map.prefab";

		private Vector3 cachePos = Vector3.zero;

		public GameObject mmRoot
		{
			get;
			private set;
		}

		public GameObject mmUGUIRoot
		{
			get;
			private set;
		}

		public GameObject mmpcAlies
		{
			get;
			private set;
		}

		public GameObject mmpcHero
		{
			get;
			private set;
		}

		public GameObject mmpcRedBuff
		{
			get;
			private set;
		}

		public GameObject mmpcBlueBuff
		{
			get;
			private set;
		}

		public GameObject mmpcJungle
		{
			get;
			private set;
		}

		public GameObject mmpcEnemy
		{
			get;
			private set;
		}

		public GameObject mmpcOrgan
		{
			get;
			private set;
		}

		public GameObject mmpcSignal
		{
			get;
			private set;
		}

		public GameObject mmBGMap_3dui
		{
			get;
			private set;
		}

		public GameObject mmpcDragon_ugui
		{
			get;
			private set;
		}

		public GameObject mmpcDragon_3dui
		{
			get;
			private set;
		}

		public GameObject mmpcEffect
		{
			get;
			private set;
		}

		public GameObject mmpcEye
		{
			get;
			private set;
		}

		public GameObject mmpcTrack
		{
			get;
			private set;
		}

		public GameObject mmHeroIconNode
		{
			get;
			private set;
		}

		public GameObject mmHeroIconNode_Self
		{
			get;
			private set;
		}

		public GameObject mmHeroIconNode_Friend
		{
			get;
			private set;
		}

		public GameObject mmHeroIconNode_Enemy
		{
			get;
			private set;
		}

		public GameObject bmRoot
		{
			get;
			private set;
		}

		public GameObject bmUGUIRoot
		{
			get;
			private set;
		}

		public GameObject bmpcAlies
		{
			get;
			private set;
		}

		public GameObject bmpcHero
		{
			get;
			private set;
		}

		public GameObject bmpcRedBuff
		{
			get;
			private set;
		}

		public GameObject bmpcBlueBuff
		{
			get;
			private set;
		}

		public GameObject bmpcJungle
		{
			get;
			private set;
		}

		public GameObject bmpcEnemy
		{
			get;
			private set;
		}

		public GameObject bmpcOrgan
		{
			get;
			private set;
		}

		public GameObject bmpcSignal
		{
			get;
			private set;
		}

		public GameObject bmBGMap_3dui
		{
			get;
			private set;
		}

		public GameObject bmpcDragon_ugui
		{
			get;
			private set;
		}

		public GameObject bmpcDragon_3dui
		{
			get;
			private set;
		}

		public GameObject bmpcEffect
		{
			get;
			private set;
		}

		public GameObject bmpcEye
		{
			get;
			private set;
		}

		public GameObject bmpcTrack
		{
			get;
			private set;
		}

		public GameObject bmHeroIconNode
		{
			get;
			private set;
		}

		public GameObject bmHeroIconNode_Self
		{
			get;
			private set;
		}

		public GameObject bmHeroIconNode_Friend
		{
			get;
			private set;
		}

		public GameObject bmHeroIconNode_Enemy
		{
			get;
			private set;
		}

		public Vector2 mmFinalScreenSize
		{
			get;
			private set;
		}

		public Vector2 bmFinalScreenSize
		{
			get;
			private set;
		}

		public Vector2 GetMMStandardScreenPos()
		{
			return this.m_mmStandardScreenPos;
		}

		public Vector2 GetBMStandardScreenPos()
		{
			return this.m_bmStandardScreenPos;
		}

		public Vector2 GetMMFianlScreenPos()
		{
			return this.m_mmFinalScreenPos;
		}

		public Vector2 GetBMFianlScreenPos()
		{
			return this.m_bmFinalScreenPos;
		}

		private void GetCacheObj()
		{
			this.mmpcAlies = Utility.FindChild(this.mmRoot, "Container_MiniMapPointer_Alies");
			this.mmpcHero = Utility.FindChild(this.mmRoot, "Container_MiniMapPointer_Hero");
			this.mmpcRedBuff = Utility.FindChild(this.mmRoot, "Container_MiniMapPointer_RedBuff");
			this.mmpcBlueBuff = Utility.FindChild(this.mmRoot, "Container_MiniMapPointer_BlueBuff");
			this.mmpcJungle = Utility.FindChild(this.mmRoot, "Container_MiniMapPointer_Jungle");
			this.mmpcEnemy = Utility.FindChild(this.mmRoot, "Container_MiniMapPointer_Enemy");
			this.mmpcOrgan = Utility.FindChild(this.mmRoot, "Container_MiniMapPointer_Organ");
			this.mmpcSignal = Utility.FindChild(this.mmRoot, "Container_MiniMapPointer_Signal");
			this.mmpcDragon_ugui = Utility.FindChild(this._ownerForm.gameObject, "MapPanel/Mini/Container_MiniMapPointer_Dragon");
			this.mmpcDragon_ugui.CustomSetActive(true);
			this.mmpcDragon_3dui = Utility.FindChild(this.mmRoot, "Container_MiniMapPointer_Dragon");
			this.mmpcEffect = Utility.FindChild(this.mmRoot, "BigMapEffectRoot");
			this.mmpcEye = Utility.FindChild(this.mmRoot, "Container_MiniMapPointer_Eye");
			this.mmpcTrack = Utility.FindChild(this.mmRoot, "Container_MiniMapPointer_Track");
			this.mmHeroIconNode = Utility.FindChild(this.mmRoot, "HeroIconNode");
			this.mmHeroIconNode_Self = Utility.FindChild(this.mmRoot, "HeroIconNode/self");
			this.mmHeroIconNode_Friend = Utility.FindChild(this.mmRoot, "HeroIconNode/friend");
			this.mmHeroIconNode_Enemy = Utility.FindChild(this.mmRoot, "HeroIconNode/enemy");
			this.bmpcAlies = Utility.FindChild(this.bmRoot, "Container_BigMapPointer_Alies");
			this.bmpcHero = Utility.FindChild(this.bmRoot, "Container_BigMapPointer_Hero");
			this.bmpcRedBuff = Utility.FindChild(this.bmRoot, "Container_BigMapPointer_RedBuff");
			this.bmpcBlueBuff = Utility.FindChild(this.bmRoot, "Container_BigMapPointer_BlueBuff");
			this.bmpcJungle = Utility.FindChild(this.bmRoot, "Container_BigMapPointer_Jungle");
			this.bmpcEnemy = Utility.FindChild(this.bmRoot, "Container_BigMapPointer_Enemy");
			this.bmpcOrgan = Utility.FindChild(this.bmRoot, "Container_BigMapPointer_Organ");
			this.bmpcSignal = Utility.FindChild(this.bmRoot, "Container_BigMapPointer_Signal");
			this.bmpcDragon_ugui = Utility.FindChild(this._ownerForm.gameObject, "MapPanel/Big/Container_BigMapPointer_Dragon");
			this.bmpcDragon_ugui.CustomSetActive(true);
			this.bmpcDragon_3dui = Utility.FindChild(this.bmRoot, "Container_BigMapPointer_Dragon");
			this.mmpcEffect = Utility.FindChild(this.bmRoot, "BigMapEffectRoot");
			this.bmpcEye = Utility.FindChild(this.bmRoot, "Container_BigMapPointer_Eye");
			this.bmpcTrack = Utility.FindChild(this.bmRoot, "Container_BigMapPointer_Track");
			this.bmHeroIconNode = Utility.FindChild(this.bmRoot, "HeroIconNode");
			this.bmHeroIconNode_Self = Utility.FindChild(this.bmRoot, "HeroIconNode/self");
			this.bmHeroIconNode_Friend = Utility.FindChild(this.bmRoot, "HeroIconNode/friend");
			this.bmHeroIconNode_Enemy = Utility.FindChild(this.bmRoot, "HeroIconNode/enemy");
		}

		public void Init(CUIFormScript formObj, SLevelContext levelContext)
		{
			if (formObj == null)
			{
				return;
			}
			this.m_playerCamp = Singleton<GamePlayerCenter>.get_instance().hostPlayerCamp;
			this._ownerForm = formObj;
			this.mmUGUIRoot = Utility.FindChild(formObj.gameObject, "MapPanel/Mini");
			this.bmUGUIRoot = Utility.FindChild(formObj.gameObject, "MapPanel/Big");
			if (!levelContext.IsMobaMode())
			{
				this.mmUGUIRoot.CustomSetActive(false);
				this.bmUGUIRoot.CustomSetActive(false);
				return;
			}
			GameObject gameObject = Singleton<CGameObjectPool>.GetInstance().GetGameObject(this.miniMapPrefabPath, 0);
			DebugHelper.Assert(gameObject != null, "---minimap3DUI is null...");
			gameObject.name = "Mini_Map";
			Camera currentCamera = Singleton<Camera_UI3D>.GetInstance().GetCurrentCamera();
			if (currentCamera == null)
			{
				return;
			}
			gameObject.transform.parent = currentCamera.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, MinimapSys.DEPTH);
			this.mmRoot = Utility.FindChild(gameObject.gameObject, "Mini");
			this.bmRoot = Utility.FindChild(gameObject.gameObject, "Big");
			if (this.mmRoot == null || this.bmRoot == null)
			{
				return;
			}
			this.mmRoot.CustomSetActive(true);
			this.bmRoot.CustomSetActive(false);
			string text = CUIUtility.s_Sprite_Dynamic_Map_Dir + levelContext.m_miniMapPath;
			string text2 = CUIUtility.s_Sprite_Dynamic_Map_Dir + levelContext.m_bigMapPath;
			this.mmBGMap_3dui = Singleton<CGameObjectPool>.GetInstance().GetGameObject(text, 5);
			this.bmBGMap_3dui = Singleton<CGameObjectPool>.GetInstance().GetGameObject(text2, 5);
			this.mmBGMap_3dui.transform.SetParent(this.mmRoot.transform, true);
			this.mmBGMap_3dui.transform.localScale = new Vector3(1f, 1f, 1f);
			this.mmBGMap_3dui.transform.localRotation = Quaternion.identity;
			this.bmBGMap_3dui.transform.SetParent(this.bmRoot.transform, true);
			this.bmBGMap_3dui.transform.localScale = new Vector3(1f, 1f, 1f);
			this.bmBGMap_3dui.transform.localRotation = Quaternion.identity;
			this.mmBGMap_3dui.transform.SetAsFirstSibling();
			this.bmBGMap_3dui.transform.SetAsFirstSibling();
			Singleton<Camera_UI3D>.GetInstance().GetCurrentCanvas().RefreshLayout(null);
			MiniMapSysUT.NativeSizeLize(gameObject);
			if (!levelContext.IsMobaMode())
			{
				this.mmRoot.SetActive(false);
				this.bmRoot.SetActive(false);
				return;
			}
			if (levelContext == null)
			{
				return;
			}
			this.regEvent();
			this.GetCacheObj();
			Sprite3D component = this.mmBGMap_3dui.GetComponent<Sprite3D>();
			Sprite3D component2 = this.bmBGMap_3dui.GetComponent<Sprite3D>();
			if (levelContext.IsMobaMode())
			{
				this.Switch(MinimapSys.EMapType.Mini);
				if (component != null)
				{
					this.initWorldUITransformFactor(new Vector2((float)component.textureWidth, (float)component.textureHeight), levelContext, true, out Singleton<CBattleSystem>.get_instance().world_UI_Factor_Small, out Singleton<CBattleSystem>.get_instance().UI_world_Factor_Small, component);
				}
				if (component2 != null)
				{
					this.initWorldUITransformFactor(new Vector2((float)component2.textureWidth, (float)component2.textureHeight), levelContext, false, out Singleton<CBattleSystem>.get_instance().world_UI_Factor_Big, out Singleton<CBattleSystem>.get_instance().UI_world_Factor_Big, component2);
				}
				if (component != null)
				{
					this.m_mmFinalScreenPos.x = (float)component.textureWidth * 0.5f * Sprite3D.Ratio();
					this.m_mmFinalScreenPos.y = (float)Screen.height - (float)component.textureHeight * 0.5f * Sprite3D.Ratio();
					component.transform.position = this.Get3DUIObj_WorldPos(this.m_mmFinalScreenPos.x, this.m_mmFinalScreenPos.y);
					this.mmFinalScreenSize = new Vector2((float)component.textureWidth * Sprite3D.Ratio(), (float)component.textureHeight * Sprite3D.Ratio());
				}
				if (component2 != null)
				{
					this.m_bmFinalScreenPos.x = (float)component2.textureWidth * 0.5f * Sprite3D.Ratio();
					this.m_bmFinalScreenPos.y = (float)Screen.height - (float)component2.textureHeight * 0.5f * Sprite3D.Ratio();
					component2.transform.position = this.Get3DUIObj_WorldPos(this.m_bmFinalScreenPos.x, this.m_bmFinalScreenPos.y);
					this.bmFinalScreenSize = new Vector2((float)component2.textureWidth * Sprite3D.Ratio(), (float)component2.textureHeight * Sprite3D.Ratio());
				}
				if (component != null)
				{
					this.m_mmStandardScreenPos.x = (float)component.textureWidth * 0.5f;
					this.m_mmStandardScreenPos.y = (float)component.textureHeight * 0.5f;
				}
				if (component2 != null)
				{
					this.m_bmStandardScreenPos.x = (float)component2.textureWidth * 0.5f;
					this.m_bmStandardScreenPos.y = (float)component2.textureHeight * 0.5f;
				}
				RectTransform rectTransform = this.mmUGUIRoot.transform as RectTransform;
				rectTransform.sizeDelta = new Vector2((float)component.textureWidth, (float)component.textureHeight);
				rectTransform.anchoredPosition = new Vector2((float)component.textureWidth * 0.5f, (float)(-(float)component.textureHeight) * 0.5f);
				RectTransform rectTransform2 = this.bmUGUIRoot.transform as RectTransform;
				rectTransform2.sizeDelta = new Vector2((float)component2.textureWidth, (float)component2.textureHeight);
				rectTransform2.anchoredPosition = new Vector2((float)component2.textureWidth * 0.5f, (float)(-(float)component2.textureHeight) * 0.5f);
				MiniMapSysUT.RefreshMapPointerBig(this.mmUGUIRoot);
				MiniMapSysUT.RefreshMapPointerBig(this.bmUGUIRoot);
				if (levelContext.m_pvpPlayerNum == 6)
				{
					GameObject gameObject2 = Utility.FindChild(this._ownerForm.gameObject, "MapPanel/DragonInfo");
					if (gameObject2)
					{
						RectTransform rectTransform3 = gameObject2.gameObject.transform as RectTransform;
						rectTransform3.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform3.anchoredPosition.y);
					}
				}
			}
			else
			{
				this.Switch(MinimapSys.EMapType.None);
			}
			this.curMapType = MinimapSys.EMapType.Mini;
			bool flag = false;
			bool b5V = false;
			if (levelContext.m_pveLevelType == 4)
			{
				int mapID = levelContext.m_mapID;
				if (mapID == 2)
				{
					flag = true;
					b5V = false;
				}
				if (mapID == 3 || mapID == 6 || mapID == 7)
				{
					flag = true;
					b5V = true;
				}
			}
			else if (levelContext.m_pvpPlayerNum == 6 || levelContext.m_pvpPlayerNum == 10)
			{
				flag = true;
				b5V = (levelContext.m_pvpPlayerNum == 10);
			}
			if (flag && this.mmpcDragon_ugui)
			{
				this.m_dragonIcon = new DragonIcon();
				this.m_dragonIcon.Init(this.mmpcDragon_ugui, this.bmpcDragon_ugui, this.mmpcDragon_3dui, this.bmpcDragon_3dui, b5V);
				this.mmpcDragon_ugui.CustomSetActive(true);
				this.bmpcDragon_ugui.CustomSetActive(true);
			}
			else
			{
				this.mmpcDragon_ugui.CustomSetActive(false);
				this.bmpcDragon_ugui.CustomSetActive(false);
			}
			GameObject gameObject3 = this.mmUGUIRoot.transform.Find("CameraFrame").gameObject;
			if (gameObject3 == null)
			{
				return;
			}
			this.MMiniMapCameraFrame_3Dui = new MiniMapCameraFrame_3DUI(gameObject3, (float)component.textureWidth, (float)component.textureHeight);
			this.MMiniMapCameraFrame_3Dui.SetFrameSize((CameraHeightType)GameSettings.CameraHeight);
			this.MMiniMapBackCityCom_3Dui = new BackCityCom_3DUI();
			this.MMiniMapTrack_3Dui = new MiniMapTrack_3DUI();
			this.MMinimapSkillIndicator_3Dui = new MinimapSkillIndicator_3DUI();
			GameObject miniTrackNode = Utility.FindChild(this.mmUGUIRoot, "Track");
			GameObject bigTrackNode = Utility.FindChild(this.bmUGUIRoot, "Track");
			this.MMinimapSkillIndicator_3Dui.Init(miniTrackNode, bigTrackNode);
			this.ChangeBigMapSide(true);
		}

		private Vector3 Get3DUIObj_WorldPos(float finalScreenX, float finalScreenY)
		{
			Camera currentCamera = Singleton<Camera_UI3D>.GetInstance().GetCurrentCamera();
			if (currentCamera == null)
			{
				return Vector3.zero;
			}
			Vector3 position = new Vector3(finalScreenX, finalScreenY, MinimapSys.DEPTH);
			return currentCamera.ScreenToWorldPoint(position);
		}

		public void Clear()
		{
			this.cachePos = Vector3.zero;
			this.unRegEvent();
			if (this.MMiniMapTrack_3Dui != null)
			{
				this.MMiniMapTrack_3Dui.Clear();
				this.MMiniMapTrack_3Dui = null;
			}
			if (this.m_dragonIcon != null)
			{
				this.m_dragonIcon.Clear();
				this.m_dragonIcon = null;
			}
			if (this.MMiniMapCameraFrame_3Dui != null)
			{
				this.MMiniMapCameraFrame_3Dui.Clear();
				this.MMiniMapCameraFrame_3Dui = null;
			}
			if (this.MMinimapSkillIndicator_3Dui != null)
			{
				this.MMinimapSkillIndicator_3Dui.Clear();
				this.MMinimapSkillIndicator_3Dui = null;
			}
			this.mmRoot = null;
			this.bmRoot = null;
			this.mmBGMap_3dui = null;
			this.mmpcAlies = null;
			this.mmpcHero = null;
			this.mmpcRedBuff = null;
			this.mmpcBlueBuff = null;
			this.mmpcJungle = null;
			this.mmpcEnemy = null;
			this.mmpcOrgan = null;
			this.mmpcSignal = null;
			this.mmpcDragon_ugui = null;
			this.mmpcDragon_3dui = null;
			this.mmpcEffect = null;
			this.mmpcEye = null;
			this.mmpcTrack = null;
			this.mmHeroIconNode = null;
			this.mmHeroIconNode_Self = null;
			this.mmHeroIconNode_Friend = null;
			this.mmHeroIconNode_Enemy = null;
			this.bmBGMap_3dui = null;
			this.bmpcAlies = null;
			this.bmpcHero = null;
			this.bmpcRedBuff = null;
			this.bmpcBlueBuff = null;
			this.bmpcJungle = null;
			this.bmpcEnemy = null;
			this.bmpcOrgan = null;
			this.bmpcSignal = null;
			this.bmpcDragon_ugui = null;
			this.bmpcDragon_3dui = null;
			this.mmpcEffect = null;
			this.bmpcEye = null;
			this.bmpcTrack = null;
			this.bmHeroIconNode = null;
			this.bmHeroIconNode_Self = null;
			this.bmHeroIconNode_Friend = null;
			this.bmHeroIconNode_Enemy = null;
			this._ownerForm = null;
			this.m_playerCamp = 3;
			this.m_elementIndex = 0;
			for (int i = 0; i < this.m_mapElements.get_Count(); i++)
			{
				if (this.m_mapElements.get_Item(i).smallElement != null)
				{
					MiniMapSysUT.RecycleMapGameObject(this.m_mapElements.get_Item(i).smallElement);
					this.m_mapElements.get_Item(i).smallElement = null;
				}
				if (this.m_mapElements.get_Item(i).bigElement != null)
				{
					MiniMapSysUT.RecycleMapGameObject(this.m_mapElements.get_Item(i).bigElement);
					this.m_mapElements.get_Item(i).bigElement = null;
				}
			}
			this.m_mapElements.Clear();
			this.m_bornRecords.Clear();
		}

		public MinimapSys.EMapType CurMapType()
		{
			return this.curMapType;
		}

		public void Switch(MinimapSys.EMapType type)
		{
			this.curMapType = type;
			if (this._ownerForm == null)
			{
				return;
			}
			GameObject widget = this._ownerForm.GetWidget(45);
			if (this.curMapType == MinimapSys.EMapType.Mini)
			{
				CUICommonSystem.SetObjActive(this.mmRoot, true);
				CUICommonSystem.SetObjActive(this.mmUGUIRoot, true);
				CUICommonSystem.SetObjActive(this.bmRoot, false);
				CUICommonSystem.SetObjActive(this.bmUGUIRoot, false);
				widget.CustomSetActive(true);
				SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
				if (curLvelContext != null && curLvelContext.m_isShowTrainingHelper)
				{
					CTrainingHelper instance = Singleton<CTrainingHelper>.GetInstance();
					instance.SetButtonActive(!instance.IsPanelActive());
				}
				this.SetSkillBtnActive(true);
			}
			else if (this.curMapType == MinimapSys.EMapType.Big)
			{
				CUICommonSystem.SetObjActive(this.mmRoot, false);
				CUICommonSystem.SetObjActive(this.mmUGUIRoot, false);
				CUICommonSystem.SetObjActive(this.bmRoot, true);
				CUICommonSystem.SetObjActive(this.bmUGUIRoot, true);
				widget.CustomSetActive(false);
				SLevelContext curLvelContext2 = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
				if (curLvelContext2 != null && curLvelContext2.m_isShowTrainingHelper)
				{
					CTrainingHelper instance2 = Singleton<CTrainingHelper>.GetInstance();
					if (instance2.IsOpenBtnActive())
					{
						instance2.SetButtonActive(false);
					}
				}
				this.ChangeBigMapSide(true);
				this.ChangeMapPointerDepth(this.curMapType);
				this.SetSkillBtnActive(true);
			}
			else if (this.curMapType == MinimapSys.EMapType.Skill)
			{
				CUICommonSystem.SetObjActive(this.mmRoot, false);
				CUICommonSystem.SetObjActive(this.mmUGUIRoot, false);
				CUICommonSystem.SetObjActive(this.bmRoot, true);
				CUICommonSystem.SetObjActive(this.bmUGUIRoot, true);
				widget.CustomSetActive(false);
				this.ChangeBigMapSide(false);
				this.ChangeMapPointerDepth(this.curMapType);
				this.SetSkillBtnActive(false);
			}
			else
			{
				this.mmRoot.CustomSetActive(false);
				CUICommonSystem.SetObjActive(this.mmUGUIRoot, false);
				this.bmRoot.CustomSetActive(false);
				CUICommonSystem.SetObjActive(this.bmUGUIRoot, false);
				this.SetSkillBtnActive(true);
			}
			this.m_CurSelectedSlotType = SkillSlotType.SLOT_SKILL_COUNT;
			if (this.MMinimapSkillIndicator_3Dui != null)
			{
				this.MMinimapSkillIndicator_3Dui.ForceUpdate();
			}
		}

		public void Switch(MinimapSys.EMapType type, SkillSlotType slotType)
		{
			this.Switch(type);
			this.m_CurSelectedSlotType = slotType;
		}

		public void ChangeMapPointerDepth(MinimapSys.EMapType mapType)
		{
			if (mapType == MinimapSys.EMapType.Skill)
			{
				this.bmpcOrgan.transform.localPosition = new Vector3(0f, 0f, -20f);
				this.bmpcEye.transform.localPosition = new Vector3(0f, 0f, -21f);
			}
			else
			{
				this.bmpcOrgan.transform.localPosition = Vector3.zero;
				this.bmpcEye.transform.localPosition = Vector3.zero;
			}
		}

		public void ChangeBigMapSide(bool bLeft)
		{
			if (Singleton<WatchController>.get_instance().IsWatching)
			{
				return;
			}
			if (this.bmBGMap_3dui != null && this.bmRoot != null && this.bmUGUIRoot != null)
			{
				Sprite3D component = this.bmBGMap_3dui.GetComponent<Sprite3D>();
				if (component == null)
				{
				}
				this.m_bmFinalScreenPos.x = ((!bLeft) ? ((float)Screen.width - (float)component.textureWidth * 0.5f * Sprite3D.Ratio()) : ((float)component.textureWidth * 0.5f * Sprite3D.Ratio()));
				this.m_bmFinalScreenPos.y = (float)Screen.height - (float)component.textureHeight * 0.5f * Sprite3D.Ratio();
				if (bLeft)
				{
					if (this.cachePos == Vector3.zero)
					{
						this.cachePos = this.Get3DUIObj_WorldPos(this.m_bmFinalScreenPos.x, this.m_bmFinalScreenPos.y);
					}
					component.transform.position = this.cachePos;
				}
				if (!bLeft)
				{
					component.transform.position = this.Get3DUIObj_WorldPos(this.m_bmFinalScreenPos.x, this.m_bmFinalScreenPos.y);
				}
				List<PoolObjHandle<ActorRoot>> gameActors = Singleton<GameObjMgr>.GetInstance().GameActors;
				for (int i = 0; i < gameActors.get_Count(); i++)
				{
					PoolObjHandle<ActorRoot> poolObjHandle = gameActors.get_Item(i);
					if (poolObjHandle && poolObjHandle.get_handle() != null && poolObjHandle.get_handle().HudControl != null)
					{
						poolObjHandle.get_handle().HudControl.AddForceUpdateFlag();
					}
				}
				if (this._ownerForm != null)
				{
					RectTransform rectTransform = this.bmUGUIRoot.transform as RectTransform;
					rectTransform.sizeDelta = new Vector2((float)component.textureWidth, (float)component.textureHeight);
					float x = (!bLeft) ? (this._ownerForm.ChangeScreenValueToForm((float)Screen.width) - (float)component.textureWidth * 0.5f) : ((float)component.textureWidth * 0.5f);
					rectTransform.anchoredPosition = new Vector2(x, (float)(-(float)component.textureHeight) * 0.5f);
				}
				if (this.m_dragonIcon != null)
				{
					this.m_dragonIcon.RefreshDragNode(bLeft, false);
				}
				MiniMapSysUT.RefreshMapPointerBig(this.bmUGUIRoot);
			}
		}

		public void SetSkillBtnActive(bool active)
		{
			if (Singleton<WatchController>.get_instance().IsWatching)
			{
				return;
			}
			if (Singleton<CBattleSystem>.get_instance().FightForm != null)
			{
				CSkillButtonManager skillButtonManager = Singleton<CBattleSystem>.get_instance().FightForm.m_skillButtonManager;
				if (skillButtonManager != null)
				{
					skillButtonManager.SetSkillButtuonActive(SkillSlotType.SLOT_SKILL_2, active);
					skillButtonManager.SetSkillButtuonActive(SkillSlotType.SLOT_SKILL_3, active);
					if (active)
					{
						Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
						if (hostPlayer != null && hostPlayer.Captain && hostPlayer.Captain.get_handle().EquipComponent.GetEquipActiveSkillShowFlag(SkillSlotType.SLOT_SKILL_9))
						{
							skillButtonManager.SetSkillButtuonActive(SkillSlotType.SLOT_SKILL_9, true);
						}
					}
					else
					{
						skillButtonManager.SetSkillButtuonActive(SkillSlotType.SLOT_SKILL_9, false);
					}
				}
			}
			this.SetSkillMapDragonEventActive(active);
		}

		public void SetSkillMapDragonEventActive(bool bIsActive)
		{
			if (Singleton<WatchController>.get_instance().IsWatching)
			{
				return;
			}
			if (Singleton<CBattleSystem>.get_instance().FightForm != null)
			{
				GameObject bigMapDragonContainer = Singleton<CBattleSystem>.get_instance().FightForm.GetBigMapDragonContainer();
				if (bigMapDragonContainer && bigMapDragonContainer.transform)
				{
					int childCount = bigMapDragonContainer.transform.childCount;
					for (int i = 0; i < childCount; i++)
					{
						Transform child = bigMapDragonContainer.transform.GetChild(i);
						if (child)
						{
							CUIEventScript component = child.GetComponent<CUIEventScript>();
							if (component)
							{
								component.enabled = bIsActive;
							}
						}
					}
				}
			}
		}

		public void ClearMapSkillStatus()
		{
			if (this.curMapType == MinimapSys.EMapType.Skill)
			{
				this.Switch(MinimapSys.EMapType.Mini);
			}
		}

		private void initWorldUITransformFactor(Vector2 mapImgSize, SLevelContext levelContext, bool bMiniMap, out Vector2 world_UI_Factor, out Vector2 UI_world_Factor, Sprite3D sprite)
		{
			int num = (!bMiniMap) ? levelContext.m_bigMapWidth : levelContext.m_mapWidth;
			int num2 = (!bMiniMap) ? levelContext.m_bigMapHeight : levelContext.m_mapHeight;
			float num3 = mapImgSize.x / (float)num;
			float num4 = mapImgSize.y / (float)num2;
			world_UI_Factor = new Vector2(num3, num4);
			float num5 = (float)num / mapImgSize.x;
			float num6 = (float)num2 / mapImgSize.y;
			UI_world_Factor = new Vector2(num5, num6);
			if (levelContext.m_isCameraFlip)
			{
				world_UI_Factor = new Vector2(-num3, -num4);
				UI_world_Factor = new Vector2(-num5, -num6);
			}
			if (null != sprite)
			{
				float x = (!bMiniMap) ? levelContext.m_bigMapFowScale : levelContext.m_mapFowScale;
				float y = (!levelContext.m_isCameraFlip) ? 1f : 0f;
				sprite.atlas.get_material().SetVector("_FowParams", new Vector4(x, y, 1f, 1f));
			}
		}

		private void regEvent()
		{
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.BigMap_Open_BigMap, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Open_BigMap));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Battle_CloseBigMap, new CUIEventManager.OnUIEventHandler(this.OnCloseBigMap));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.BigMap_Click_5_Dalong, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_5_Dalong));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.BigMap_Click_5_Xiaolong, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_5_Xiaolong));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.BigMap_Click_3_long, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_3_long));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.BigMap_Click_Organ, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_Organ));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.BigMap_Click_Hero, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_Hero));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.BigMap_Click_Map, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_Map));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.BigMap_Click_Eye, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_Eye));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Battle_Down_MiniMap, new CUIEventManager.OnUIEventHandler(this.OnMiniMap_Click_Down));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Battle_Click_MiniMap_Up, new CUIEventManager.OnUIEventHandler(this.OnMiniMap_Click_Up));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Battle_Drag_SignalPanel, new CUIEventManager.OnUIEventHandler(this.OnDragMiniMap));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Battle_Drag_SignalPanel_End, new CUIEventManager.OnUIEventHandler(this.OnDragMiniMapEnd));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_ActorBeAttack, new RefAction<DefaultGameEventParam>(this.OnBuildingAttacked));
			Singleton<GameEventSys>.get_instance().AddEventHandler<GameDeadEventParam>(GameEventDef.Event_MonsterGroupDead, new RefAction<GameDeadEventParam>(this.OnMonsterGroupDead));
			Singleton<GameEventSys>.GetInstance().AddEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.OnActorDead));
		}

		private void OnMonsterGroupDead(ref GameDeadEventParam evtParam)
		{
			if (FogOfWar.enable && !Singleton<WatchController>.get_instance().IsWatching)
			{
				SpawnGroup x = evtParam.spawnPoint as SpawnGroup;
				if (evtParam.src && evtParam.orignalAtker && x != null)
				{
					bool flag = evtParam.src.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Monster && evtParam.src.get_handle().ActorControl.GetActorSubType() == 2;
					byte actorSubSoliderType = evtParam.src.get_handle().ActorControl.GetActorSubSoliderType();
					bool flag2 = actorSubSoliderType == 7 || actorSubSoliderType == 8 || actorSubSoliderType == 9;
					if (flag && !flag2)
					{
						Player hostPlayer = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer();
						if (hostPlayer.PlayerCamp != evtParam.orignalAtker.get_handle().TheActorMeta.ActorCamp)
						{
							GameObject mapPointerSmall = evtParam.src.get_handle().HudControl.MapPointerSmall;
							GameObject mapPointerBig = evtParam.src.get_handle().HudControl.MapPointerBig;
							if (!(mapPointerSmall != null) || mapPointerBig != null)
							{
							}
						}
					}
				}
			}
		}

		private void OnBuildingAttacked(ref DefaultGameEventParam evtParam)
		{
			if (!evtParam.src)
			{
				return;
			}
			ActorRoot handle = evtParam.src.get_handle();
			if (handle == null)
			{
				return;
			}
			bool flag = HudUT.IsTower(handle);
			if (flag)
			{
				HudComponent3D hudControl = handle.HudControl;
				if (hudControl == null)
				{
					return;
				}
				GameObject mapPointerSmall = hudControl.MapPointerSmall;
				TowerHitMgr towerHitMgr = Singleton<CBattleSystem>.GetInstance().TowerHitMgr;
				if (mapPointerSmall != null && towerHitMgr != null)
				{
					towerHitMgr.TryActive(handle.ObjID, mapPointerSmall);
				}
				Sprite3D bigTower_Spt3D = hudControl.GetBigTower_Spt3D();
				Sprite3D smallTower_Spt3D = hudControl.GetSmallTower_Spt3D();
				if (bigTower_Spt3D != null && smallTower_Spt3D != null && handle.ValueComponent != null)
				{
					Sprite3D arg_CB_0 = smallTower_Spt3D;
					float single = handle.ValueComponent.GetHpRate().get_single();
					bigTower_Spt3D.fillAmount = single;
					arg_CB_0.fillAmount = single;
				}
			}
		}

		private void OnActorDead(ref GameDeadEventParam prm)
		{
			if (ActorHelper.IsHostCtrlActor(ref prm.src) && this.curMapType == MinimapSys.EMapType.Skill)
			{
				this.Switch(MinimapSys.EMapType.Mini);
			}
		}

		private void unRegEvent()
		{
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.BigMap_Open_BigMap, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Open_BigMap));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Battle_CloseBigMap, new CUIEventManager.OnUIEventHandler(this.OnCloseBigMap));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.BigMap_Click_5_Dalong, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_5_Dalong));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.BigMap_Click_5_Xiaolong, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_5_Xiaolong));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.BigMap_Click_3_long, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_3_long));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.BigMap_Click_Organ, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_Organ));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.BigMap_Click_Hero, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_Hero));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.BigMap_Click_Map, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_Map));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.BigMap_Click_Eye, new CUIEventManager.OnUIEventHandler(this.OnBigMap_Click_Eye));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Battle_Down_MiniMap, new CUIEventManager.OnUIEventHandler(this.OnMiniMap_Click_Down));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Battle_Click_MiniMap_Up, new CUIEventManager.OnUIEventHandler(this.OnMiniMap_Click_Up));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Battle_Drag_SignalPanel, new CUIEventManager.OnUIEventHandler(this.OnDragMiniMap));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Battle_Drag_SignalPanel_End, new CUIEventManager.OnUIEventHandler(this.OnDragMiniMapEnd));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_ActorBeAttack, new RefAction<DefaultGameEventParam>(this.OnBuildingAttacked));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_MonsterGroupDead, new RefAction<GameDeadEventParam>(this.OnMonsterGroupDead));
			Singleton<GameEventSys>.GetInstance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.OnActorDead));
		}

		private void OnBigMap_Open_BigMap(CUIEvent uievent)
		{
			CPlayerBehaviorStat.Plus(CPlayerBehaviorStat.BehaviorType.Battle_OpenBigMap);
			this.Switch(MinimapSys.EMapType.Big);
			this.RefreshMapPointers();
		}

		public void RefreshMapPointers()
		{
			List<PoolObjHandle<ActorRoot>> heroActors = Singleton<GameObjMgr>.GetInstance().HeroActors;
			for (int i = 0; i < heroActors.get_Count(); i++)
			{
				PoolObjHandle<ActorRoot> poolObjHandle = heroActors.get_Item(i);
				if (poolObjHandle && poolObjHandle.get_handle() != null && poolObjHandle.get_handle().HudControl != null)
				{
					poolObjHandle.get_handle().HudControl.RefreshMapPointerBig();
				}
			}
		}

		private void OnCloseBigMap(CUIEvent uiEvent)
		{
			if (this.curMapType == MinimapSys.EMapType.Big)
			{
				this.Switch(MinimapSys.EMapType.Mini);
			}
		}

		private void OnMiniMap_Click_Down(CUIEvent uievent)
		{
			SignalPanel theSignalPanel = Singleton<CBattleSystem>.GetInstance().TheSignalPanel;
			if (theSignalPanel == null)
			{
				this.Move_CameraToClickPosition(uievent);
			}
			else if (!theSignalPanel.IsUseSingalButton())
			{
				this.Move_CameraToClickPosition(uievent);
			}
		}

		public void Move_CameraToClickPosition(CUIEvent uiEvent)
		{
			if (MonoSingleton<CameraSystem>.GetInstance().enableLockedCamera)
			{
				MonoSingleton<CameraSystem>.get_instance().ToggleFreeDragCamera(true);
				if (Singleton<CBattleSystem>.GetInstance().WatchForm != null)
				{
					Singleton<CBattleSystem>.GetInstance().WatchForm.OnCamerFreed();
				}
			}
			CUIFormScript srcFormScript = uiEvent.m_srcFormScript;
			if (srcFormScript != null && uiEvent.m_srcWidget != null && uiEvent.m_pointerEventData != null)
			{
				Vector2 position = uiEvent.m_pointerEventData.position;
				MinimapSys theMinimapSys = Singleton<CBattleSystem>.GetInstance().TheMinimapSys;
				if (theMinimapSys == null)
				{
					return;
				}
				Vector2 mMFianlScreenPos = theMinimapSys.GetMMFianlScreenPos();
				float num = position.x - mMFianlScreenPos.x;
				float num2 = position.y - mMFianlScreenPos.y;
				num = uiEvent.m_srcFormScript.ChangeScreenValueToForm(num);
				num2 = uiEvent.m_srcFormScript.ChangeScreenValueToForm(num2);
				float x = num * Singleton<CBattleSystem>.get_instance().UI_world_Factor_Small.x;
				float z = num2 * Singleton<CBattleSystem>.get_instance().UI_world_Factor_Small.y;
				if (MonoSingleton<CameraSystem>.get_instance().MobaCamera != null)
				{
					MonoSingleton<CameraSystem>.get_instance().MobaCamera.SetAbsoluteLockLocation(new Vector3(x, 1f, z));
					if (this.MMiniMapCameraFrame_3Dui != null)
					{
						if (!this.MMiniMapCameraFrame_3Dui.IsCameraFrameShow)
						{
							this.MMiniMapCameraFrame_3Dui.Show();
							this.MMiniMapCameraFrame_3Dui.ShowNormal();
						}
						this.MMiniMapCameraFrame_3Dui.SetPos(num, num2);
					}
				}
			}
		}

		private void OnMiniMap_Click_Up(CUIEvent uievent)
		{
			if (this.MMiniMapCameraFrame_3Dui != null)
			{
				this.MMiniMapCameraFrame_3Dui.Hide();
			}
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (!Singleton<WatchController>.GetInstance().IsWatching && hostPlayer != null && hostPlayer.Captain && hostPlayer.Captain.get_handle().ActorControl != null && (!hostPlayer.Captain.get_handle().ActorControl.IsDeadState || hostPlayer.Captain.get_handle().TheStaticData.TheBaseAttribute.DeadControl))
			{
				MonoSingleton<CameraSystem>.get_instance().ToggleFreeDragCamera(false);
			}
		}

		public void OnActorDamage(ref HurtEventResultInfo hri)
		{
			if (this.MMiniMapCameraFrame_3Dui == null)
			{
				return;
			}
			if (!this.MMiniMapCameraFrame_3Dui.IsCameraFrameShow)
			{
				return;
			}
			if (hri.hurtInfo.hurtType != HurtTypeDef.Therapic)
			{
				Player hostPlayer = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer();
				if (hri.src && hostPlayer != null && hostPlayer.Captain && hostPlayer.Captain == hri.src)
				{
					this.MMiniMapCameraFrame_3Dui.ShowRed();
				}
			}
		}

		public void Update()
		{
			if (this.MMiniMapCameraFrame_3Dui != null)
			{
				this.MMiniMapCameraFrame_3Dui.Update();
			}
			if (this.MMinimapSkillIndicator_3Dui != null)
			{
				this.MMinimapSkillIndicator_3Dui.Update();
			}
			if (FogOfWar.enable && !Singleton<WatchController>.get_instance().IsWatching)
			{
				if (this.m_elementIndex >= 0 && this.m_elementIndex < this.m_mapElements.get_Count())
				{
					MinimapSys.ElementInMap elementInMap = this.m_mapElements.get_Item(this.m_elementIndex);
					VInt3 worldLoc = new VInt3(elementInMap.pos.x, elementInMap.pos.z, 0);
					if (Singleton<GameFowManager>.get_instance().IsSurfaceCellVisible(worldLoc, this.m_playerCamp))
					{
						if (elementInMap.smallElement != null)
						{
							MiniMapSysUT.RecycleMapGameObject(elementInMap.smallElement);
							elementInMap.smallElement = null;
						}
						if (elementInMap.bigElement != null)
						{
							MiniMapSysUT.RecycleMapGameObject(elementInMap.bigElement);
							elementInMap.bigElement = null;
						}
						this.m_mapElements.RemoveAt(this.m_elementIndex);
					}
					this.m_elementIndex++;
				}
				else
				{
					this.m_elementIndex = 0;
				}
			}
		}

		private void OnDragMiniMap(CUIEvent uiEvent)
		{
			CUIFormScript srcFormScript = uiEvent.m_srcFormScript;
			if (uiEvent == null || srcFormScript == null || uiEvent.m_pointerEventData == null || uiEvent.m_srcWidget == null)
			{
				return;
			}
			if (MonoSingleton<CameraSystem>.GetInstance().enableLockedCamera)
			{
				MonoSingleton<CameraSystem>.get_instance().ToggleFreeDragCamera(true);
				if (Singleton<CBattleSystem>.GetInstance().WatchForm != null)
				{
					Singleton<CBattleSystem>.GetInstance().WatchForm.OnCamerFreed();
				}
			}
			Vector2 position = uiEvent.m_pointerEventData.position;
			MinimapSys theMinimapSys = Singleton<CBattleSystem>.GetInstance().TheMinimapSys;
			if (theMinimapSys == null)
			{
				return;
			}
			Vector2 mMFianlScreenPos = theMinimapSys.GetMMFianlScreenPos();
			float num = position.x - mMFianlScreenPos.x;
			float num2 = position.y - mMFianlScreenPos.y;
			num = uiEvent.m_srcFormScript.ChangeScreenValueToForm(num);
			num2 = uiEvent.m_srcFormScript.ChangeScreenValueToForm(num2);
			float x = num * Singleton<CBattleSystem>.get_instance().UI_world_Factor_Small.x;
			float z = num2 * Singleton<CBattleSystem>.get_instance().UI_world_Factor_Small.y;
			if (MonoSingleton<CameraSystem>.get_instance().MobaCamera != null)
			{
				MonoSingleton<CameraSystem>.get_instance().MobaCamera.SetAbsoluteLockLocation(new Vector3(x, 1f, z));
			}
			if (this.MMiniMapCameraFrame_3Dui != null)
			{
				if (!this.MMiniMapCameraFrame_3Dui.IsCameraFrameShow)
				{
					this.MMiniMapCameraFrame_3Dui.Show();
					this.MMiniMapCameraFrame_3Dui.ShowNormal();
				}
				this.MMiniMapCameraFrame_3Dui.SetPos(num, num2);
			}
		}

		private void OnDragMiniMapEnd(CUIEvent uievent)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (!Singleton<WatchController>.GetInstance().IsWatching && hostPlayer != null && hostPlayer.Captain && hostPlayer.Captain.get_handle() != null && hostPlayer.Captain.get_handle().ActorControl != null && !hostPlayer.Captain.get_handle().ActorControl.IsDeadState)
			{
				MonoSingleton<CameraSystem>.get_instance().ToggleFreeDragCamera(false);
				if (this.MMiniMapCameraFrame_3Dui != null)
				{
					this.MMiniMapCameraFrame_3Dui.Hide();
				}
			}
		}

		private void OnBigMap_Click_5_Dalong(CUIEvent uievent)
		{
			this.send_signal(uievent, this.bmRoot, MinimapSys.ElementType.Dragon_5_big, 0);
		}

		private void OnBigMap_Click_5_Xiaolong(CUIEvent uievent)
		{
			this.send_signal(uievent, this.bmRoot, MinimapSys.ElementType.Dragon_5_small, 0);
		}

		private void OnBigMap_Click_3_long(CUIEvent uievent)
		{
			this.send_signal(uievent, this.bmRoot, MinimapSys.ElementType.Dragon_3, 0);
		}

		private void OnBigMap_Click_Organ(CUIEvent uievent)
		{
			this.send_signal(uievent, this.bmRoot, MinimapSys.ElementType.None, 0);
		}

		private void OnBigMap_Click_Hero(CUIEvent uievent)
		{
			this.send_signal(uievent, this.bmRoot, MinimapSys.ElementType.None, 0);
		}

		private void OnBigMap_Click_Map(CUIEvent uievent)
		{
			if (this.curMapType == MinimapSys.EMapType.Skill)
			{
				if (!this.UI3DEventMgr.HandleSkillClickEvent(uievent.m_pointerEventData))
				{
					this.send_signal(uievent, this.bmRoot, MinimapSys.ElementType.None, 1);
				}
			}
			else if (!this.UI3DEventMgr.HandleClickEvent(uievent.m_pointerEventData))
			{
				this.send_signal(uievent, this.bmRoot, MinimapSys.ElementType.None, 1);
			}
		}

		private void OnBigMap_Click_Eye(CUIEvent uievent)
		{
			this.send_signal(uievent, this.bmRoot, MinimapSys.ElementType.None, 0);
		}

		private void send_signal(CUIEvent uiEvent, GameObject img, MinimapSys.ElementType elementType = MinimapSys.ElementType.None, int signal_id = 0)
		{
			if (uiEvent == null || img == null)
			{
				return;
			}
			byte b = (byte)uiEvent.m_eventParams.tag2;
			uint tagUInt = uiEvent.m_eventParams.tagUInt;
			if (signal_id == 0)
			{
				signal_id = uiEvent.m_eventParams.tag3;
			}
			MinimapSys.EMapType eMapType = this.curMapType;
			SkillSlotType curSelectedSlotType = this.m_CurSelectedSlotType;
			if (eMapType != MinimapSys.EMapType.Skill)
			{
				this.Switch(MinimapSys.EMapType.Mini);
			}
			SignalPanel signalPanel = (Singleton<CBattleSystem>.GetInstance().FightForm == null) ? null : Singleton<CBattleSystem>.get_instance().FightForm.GetSignalPanel();
			if (signalPanel != null)
			{
				if (eMapType == MinimapSys.EMapType.Skill)
				{
					Singleton<CBattleSystem>.GetInstance().FightForm.m_skillButtonManager.SelectedMapTarget(curSelectedSlotType, tagUInt);
					if (tagUInt != 0u)
					{
						Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
						if (hostPlayer != null && hostPlayer.Captain)
						{
							SkillSlot skillSlot = hostPlayer.Captain.get_handle().SkillControl.GetSkillSlot(curSelectedSlotType);
							if (skillSlot != null)
							{
								PoolObjHandle<ActorRoot> actor = Singleton<GameObjMgr>.GetInstance().GetActor(tagUInt);
								if (actor && skillSlot.IsValidSkillTarget(ref actor))
								{
									this.m_CurSelectedSlotType = SkillSlotType.SLOT_SKILL_COUNT;
									this.Switch(MinimapSys.EMapType.Mini);
								}
							}
						}
					}
					return;
				}
				if (b == 3 || b == 1 || b == 2)
				{
					signalPanel.SendCommand_SignalMiniMap_Target((byte)signal_id, b, tagUInt);
					return;
				}
				if (elementType == MinimapSys.ElementType.Dragon_3 || elementType == MinimapSys.ElementType.Dragon_5_big || elementType == MinimapSys.ElementType.Dragon_5_small)
				{
					this.Send_Position_Signal(uiEvent, img, 2, elementType, true);
					return;
				}
				this.Send_Position_Signal(uiEvent, img, (byte)signal_id, MinimapSys.ElementType.None, true);
			}
		}

		private void Send_Position_Signal(CUIEvent uiEvent, GameObject img, byte signal_id, MinimapSys.ElementType type, bool bBigMap = true)
		{
			SignalPanel signalPanel = (Singleton<CBattleSystem>.GetInstance().FightForm == null) ? null : Singleton<CBattleSystem>.get_instance().FightForm.GetSignalPanel();
			if (signalPanel == null)
			{
				return;
			}
			MinimapSys theMinimapSys = Singleton<CBattleSystem>.GetInstance().TheMinimapSys;
			if (theMinimapSys == null)
			{
				return;
			}
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer == null)
			{
				return;
			}
			ActorRoot actorRoot = (hostPlayer == null || !hostPlayer.Captain) ? null : hostPlayer.Captain.get_handle();
			if (actorRoot == null)
			{
				return;
			}
			Vector2 vector = (!bBigMap) ? theMinimapSys.GetMMFianlScreenPos() : theMinimapSys.GetBMFianlScreenPos();
			float num = uiEvent.m_pointerEventData.position.x - vector.x;
			float num2 = uiEvent.m_pointerEventData.position.y - vector.y;
			num = uiEvent.m_srcFormScript.ChangeScreenValueToForm(num);
			num2 = uiEvent.m_srcFormScript.ChangeScreenValueToForm(num2);
			VInt3 zero = VInt3.zero;
			zero.x = (int)(num * Singleton<CBattleSystem>.GetInstance().UI_world_Factor_Big.x);
			zero.y = (int)((actorRoot == null) ? 0.15f : ((Vector3)actorRoot.location).y);
			zero.z = (int)(num2 * Singleton<CBattleSystem>.GetInstance().UI_world_Factor_Big.y);
			signalPanel.SendCommand_SignalMiniMap_Position(signal_id, zero, type);
		}

		public static Image SetTower_Image(bool bAlie, int value, GameObject mapPointer, CUIFormScript formScript)
		{
			if (mapPointer == null || formScript == null)
			{
				return null;
			}
			Image component = mapPointer.GetComponent<Image>();
			Image component2 = mapPointer.transform.Find("front").GetComponent<Image>();
			if (component == null || component2 == null)
			{
				return null;
			}
			if (value == 2)
			{
				component.SetSprite((!bAlie) ? MinimapSys.enemy_base : MinimapSys.self_base, formScript, true, false, false, false);
				component2.SetSprite((!bAlie) ? MinimapSys.enemy_base_outline : MinimapSys.self_base_outline, formScript, true, false, false, false);
			}
			else if (value == 1 || value == 4)
			{
				component.SetSprite((!bAlie) ? MinimapSys.enemy_tower : MinimapSys.self_tower, formScript, true, false, false, false);
				component2.SetSprite((!bAlie) ? MinimapSys.enemy_tower_outline : MinimapSys.self_tower_outline, formScript, true, false, false, false);
			}
			return component;
		}
	}
}
