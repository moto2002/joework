using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.Sound;
using Assets.Scripts.UI;
using ResData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	public class CSkillButtonManager
	{
		private const float c_skillIndicatorRespondMinRadius = 15f;

		private const float c_skillIndicatorMoveRadius = 30f;

		private const float c_directionalSkillIndicatorRespondMinRadius = 30f;

		private const float c_skillICancleRadius = 270f;

		private const float c_skillIndicatorRadius = 110f;

		private const string c_skillJoystickTargetNormalBorderPath = "UGUI/Sprite/Battle/Battle_skillFrameBg_new.prefab";

		private const string c_skillJoystickTargetSelectedBorderPath = "UGUI/Sprite/Battle/Battle_ComboCD.prefab";

		private const string c_skillBtnFormPath = "UGUI/Form/Battle/Part/Form_Battle_Part_SkillBtn.prefab";

		private const string m_strKillNotifyBlueRingPath = "UGUI/Sprite/Battle/LockEnemy/Battle_KillNotify_Blue_ring";

		private const string m_strKillNotifyRedRingPath = "UGUI/Sprite/Battle/LockEnemy/Battle_KillNotify_Red_ring";

		private static enSkillButtonFormWidget[] s_skillButtons = new enSkillButtonFormWidget[]
		{
			enSkillButtonFormWidget.Button_Attack,
			enSkillButtonFormWidget.Button_Skill_1,
			enSkillButtonFormWidget.Button_Skill_2,
			enSkillButtonFormWidget.Button_Skill_3,
			enSkillButtonFormWidget.Button_Recover,
			enSkillButtonFormWidget.Button_Talent,
			enSkillButtonFormWidget.Button_Skill_6,
			enSkillButtonFormWidget.Button_Skill_Ornament,
			enSkillButtonFormWidget.Button_Skill_9,
			enSkillButtonFormWidget.Button_Skill_10,
			enSkillButtonFormWidget.None
		};

		private static enSkillButtonFormWidget[] s_skillCDTexts = new enSkillButtonFormWidget[]
		{
			enSkillButtonFormWidget.None,
			enSkillButtonFormWidget.Text_Skill_1_CD,
			enSkillButtonFormWidget.Text_Skill_2_CD,
			enSkillButtonFormWidget.Text_Skill_3_CD,
			enSkillButtonFormWidget.Text_Skill_4_CD,
			enSkillButtonFormWidget.Text_Skill_5_CD,
			enSkillButtonFormWidget.Text_Skill_6_CD,
			enSkillButtonFormWidget.Text_Skill_Ornament_CD,
			enSkillButtonFormWidget.Text_Skill_9_CD,
			enSkillButtonFormWidget.Text_Skill_10_CD,
			enSkillButtonFormWidget.None
		};

		private static enSkillButtonFormWidget[] s_skillBeanTexts = new enSkillButtonFormWidget[]
		{
			enSkillButtonFormWidget.None,
			enSkillButtonFormWidget.Text_Skill_1_Bean,
			enSkillButtonFormWidget.Text_Skill_2_Bean,
			enSkillButtonFormWidget.Text_Skill_3_Bean,
			enSkillButtonFormWidget.Text_Skill_4_Bean,
			enSkillButtonFormWidget.Text_Skill_5_Bean,
			enSkillButtonFormWidget.Text_Skill_6_Bean,
			enSkillButtonFormWidget.Text_Skill_7_Bean,
			enSkillButtonFormWidget.Text_Skill_9_Bean,
			enSkillButtonFormWidget.Text_Skill_10_Bean,
			enSkillButtonFormWidget.None
		};

		private SkillButton[] _skillButtons;

		private enSkillIndicatorMode m_skillIndicatorMode = enSkillIndicatorMode.FixedPosition;

		private bool m_currentSkillIndicatorResponed;

		private bool m_currentSkillTipsResponed;

		private bool m_commonAtkSlide;

		private int m_currentSkillJoystickSelectedIndex = -1;

		private stCampHeroInfo[] m_campHeroInfos = new stCampHeroInfo[0];

		private stCampHeroInfo[] m_targetInfos = new stCampHeroInfo[0];

		private uint m_CurTargtNum;

		private static int MaxTargetNum = 4;

		private static Color s_skillCursorBGColorBlue = new Color(0.168627456f, 0.7607843f, 1f, 1f);

		private static Color s_skillCursorBGColorRed = new Color(0.972549f, 0.184313729f, 0.184313729f, 1f);

		private SkillSlotType m_currentSkillSlotType = SkillSlotType.SLOT_SKILL_COUNT;

		private Vector2 m_currentSkillDownScreenPosition = Vector2.zero;

		private bool m_currentSkillIndicatorEnabled;

		private bool m_currentSkillIndicatorJoystickEnabled;

		private enSkillJoystickMode m_currentSkillJoystickMode;

		private bool m_currentSkillIndicatorInCancelArea;

		private Vector2 m_currentSkillIndicatorScreenPosition = Vector2.zero;

		private static byte s_maxButtonCount = 10;

		public bool CurrentSkillTipsResponed
		{
			get
			{
				return this.m_currentSkillTipsResponed;
			}
		}

		public bool CurrentSkillIndicatorResponed
		{
			get
			{
				return this.m_currentSkillIndicatorResponed;
			}
		}

		public CSkillButtonManager()
		{
			this._skillButtons = new SkillButton[(int)CSkillButtonManager.s_maxButtonCount];
			Singleton<GameEventSys>.get_instance().AddEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_ActorRevive, new RefAction<DefaultGameEventParam>(this.onActorRevive));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_CaptainSwitch, new RefAction<DefaultGameEventParam>(this.OnCaptainSwitched));
			Singleton<EventRouter>.GetInstance().AddEventHandler<PoolObjHandle<ActorRoot>, int, int>("HeroHpChange", new Action<PoolObjHandle<ActorRoot>, int, int>(this.OnHeroHpChange));
		}

		~CSkillButtonManager()
		{
		}

		public void Initialise(PoolObjHandle<ActorRoot> actor, bool bInitSpecifiedButton = false, SkillSlotType specifiedType = SkillSlotType.SLOT_SKILL_COUNT)
		{
			if (!actor || actor.get_handle().SkillControl == null || actor.get_handle().SkillControl.SkillSlotArray == null)
			{
				return;
			}
			SkillSlot[] skillSlotArray = actor.get_handle().SkillControl.SkillSlotArray;
			for (int i = 0; i < (int)CSkillButtonManager.s_maxButtonCount; i++)
			{
				SkillSlotType skillSlotType = (SkillSlotType)i;
				if (!bInitSpecifiedButton || skillSlotType == specifiedType)
				{
					SkillButton button = this.GetButton(skillSlotType);
					SkillSlot skillSlot = skillSlotArray[i];
					if (!MinimapSkillIndicator_3DUI.IsIndicatorInited() && skillSlotType == SkillSlotType.SLOT_SKILL_3 && !Singleton<WatchController>.GetInstance().IsWatching)
					{
						MinimapSkillIndicator_3DUI.InitIndicator(skillSlot.SkillObj.cfgData.szMapIndicatorNormalPrefab, skillSlot.SkillObj.cfgData.szMapIndicatorRedPrefab, (float)skillSlot.SkillObj.cfgData.iSmallMapIndicatorHeight, (float)skillSlot.SkillObj.cfgData.iBigMapIndicatorHeight);
					}
					DebugHelper.Assert(button != null);
					if (button != null)
					{
						button.bDisableFlag = (skillSlot != null && !skillSlot.EnableButtonFlag);
						if (actor.get_handle().SkillControl.IsIngnoreDisableSkill((SkillSlotType)i) && actor.get_handle().SkillControl.ForceDisableSkill[i] == 0)
						{
							button.bLimitedFlag = false;
						}
						else
						{
							button.bLimitedFlag = (actor.get_handle().SkillControl.DisableSkill[i] >= 1);
						}
						SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
						if (skillSlotType == SkillSlotType.SLOT_SKILL_6)
						{
							if (curLvelContext.IsGameTypeGuide())
							{
								if (!CBattleGuideManager.Is5v5GuideLevel(curLvelContext.m_mapID) || skillSlot == null || skillSlot.SkillObj == null || skillSlot.SkillObj.cfgData == null)
								{
									button.m_button.CustomSetActive(false);
									goto IL_8AC;
								}
							}
							else if (!curLvelContext.IsMobaModeWithOutGuide() || curLvelContext.m_pvpPlayerNum != 10 || skillSlot == null || skillSlot.SkillObj == null || skillSlot.SkillObj.cfgData == null)
							{
								button.m_button.CustomSetActive(false);
								goto IL_8AC;
							}
						}
						if (skillSlotType == SkillSlotType.SLOT_SKILL_7 && !curLvelContext.m_bEnableOrnamentSlot)
						{
							button.m_button.CustomSetActive(false);
							button.m_cdText.CustomSetActive(false);
						}
						else
						{
							if (Singleton<BattleLogic>.get_instance().GetCurLvelContext().IsGameTypeBurning() || Singleton<BattleLogic>.get_instance().GetCurLvelContext().IsGameTypeArena() || Singleton<BattleLogic>.get_instance().GetCurLvelContext().IsGameTypeAdventure())
							{
								if ((Singleton<BattleLogic>.get_instance().GetCurLvelContext().IsGameTypeBurning() || Singleton<BattleLogic>.get_instance().GetCurLvelContext().IsGameTypeArena()) && (skillSlotType == SkillSlotType.SLOT_SKILL_4 || skillSlotType == SkillSlotType.SLOT_SKILL_5))
								{
									if (button.m_button != null && button.m_cdText != null)
									{
										button.m_button.CustomSetActive(false);
										button.m_cdText.CustomSetActive(false);
									}
									goto IL_8AC;
								}
								if (skillSlotType >= SkillSlotType.SLOT_SKILL_1 && skillSlotType <= SkillSlotType.SLOT_SKILL_3)
								{
									if (button.m_button != null)
									{
										GameObject skillLvlFrameImg = button.GetSkillLvlFrameImg(true);
										if (skillLvlFrameImg != null)
										{
											skillLvlFrameImg.CustomSetActive(false);
										}
										GameObject skillLvlFrameImg2 = button.GetSkillLvlFrameImg(false);
										if (skillLvlFrameImg2 != null)
										{
											skillLvlFrameImg2.CustomSetActive(false);
										}
										GameObject skillFrameImg = button.GetSkillFrameImg();
										if (skillFrameImg != null)
										{
											skillFrameImg.CustomSetActive(true);
										}
									}
									if (skillSlot != null)
									{
										int skillLevel = 0;
										if (skillSlotType == SkillSlotType.SLOT_SKILL_1 || skillSlotType == SkillSlotType.SLOT_SKILL_2)
										{
											ResGlobalInfo dataByKey = GameDataMgr.globalInfoDatabin.GetDataByKey(142u);
											skillLevel = (int)dataByKey.dwConfValue;
										}
										else if (skillSlotType == SkillSlotType.SLOT_SKILL_3)
										{
											ResGlobalInfo dataByKey2 = GameDataMgr.globalInfoDatabin.GetDataByKey(143u);
											skillLevel = (int)dataByKey2.dwConfValue;
										}
										skillSlot.SetSkillLevel(skillLevel);
									}
								}
							}
							if (!(button.m_button == null))
							{
								CUIEventScript component = button.m_button.GetComponent<CUIEventScript>();
								CUIEventScript component2 = button.GetDisableButton().GetComponent<CUIEventScript>();
								if (skillSlot != null)
								{
									component.enabled = true;
									component2.enabled = true;
									if (skillSlotType == SkillSlotType.SLOT_SKILL_1 || skillSlotType == SkillSlotType.SLOT_SKILL_2 || skillSlotType == SkillSlotType.SLOT_SKILL_3)
									{
										if (skillSlot.EnableButtonFlag)
										{
											component.enabled = true;
										}
										else
										{
											component.enabled = false;
										}
									}
									if (button.GetDisableButton() != null)
									{
										if (skillSlot.GetSkillLevel() > 0)
										{
											this.SetEnableButton(skillSlotType);
											if ((actor.get_handle().ActorControl != null && actor.get_handle().ActorControl.IsDeadState) || (skillSlot.SlotType != SkillSlotType.SLOT_SKILL_0 && !skillSlot.IsCDReady))
											{
												this.SetDisableButton(skillSlotType);
											}
										}
										else
										{
											this.SetDisableButton(skillSlotType);
										}
									}
									if (button.m_button != null)
									{
										button.m_button.CustomSetActive(true);
									}
									if (button.m_cdText != null)
									{
										button.m_cdText.CustomSetActive(true);
									}
									if (skillSlotType != SkillSlotType.SLOT_SKILL_0)
									{
										Image component3 = button.m_button.transform.Find("Present/SkillImg").GetComponent<Image>();
										component3.gameObject.CustomSetActive(true);
										component3.SetSprite(CUIUtility.s_Sprite_Dynamic_Skill_Dir + skillSlot.SkillObj.IconName, Singleton<CBattleSystem>.GetInstance().FightFormScript, true, false, false, false);
										if (skillSlotType == SkillSlotType.SLOT_SKILL_4 || skillSlotType == SkillSlotType.SLOT_SKILL_5 || skillSlotType == SkillSlotType.SLOT_SKILL_6 || skillSlotType == SkillSlotType.SLOT_SKILL_7)
										{
											Transform transform = button.m_button.transform.Find("lblName");
											if (transform != null)
											{
												if (skillSlot.SkillObj.cfgData != null)
												{
													transform.GetComponent<Text>().text = skillSlot.SkillObj.cfgData.szSkillName;
												}
												transform.gameObject.CustomSetActive(true);
											}
										}
										if (actor.get_handle().SkillControl.IsDisableSkillSlot(skillSlotType))
										{
											this.SetLimitButton(skillSlotType);
										}
										else if (skillSlot.GetSkillLevel() > 0 && skillSlot.IsEnergyEnough)
										{
											this.CancelLimitButton(skillSlotType);
										}
										if (skillSlot.GetSkillLevel() > 0)
										{
											this.UpdateButtonCD(skillSlotType, skillSlot.CurSkillCD);
										}
										else if (button.m_cdText)
										{
											button.m_cdText.CustomSetActive(false);
										}
									}
									component.m_onDownEventParams.m_skillSlotType = skillSlotType;
									component.m_onUpEventParams.m_skillSlotType = skillSlotType;
									component.m_onHoldEventParams.m_skillSlotType = skillSlotType;
									component.m_onHoldEndEventParams.m_skillSlotType = skillSlotType;
									component.m_onDragStartEventParams.m_skillSlotType = skillSlotType;
									component.m_onDragEventParams.m_skillSlotType = skillSlotType;
									component2.m_onClickEventParams.m_skillSlotType = skillSlotType;
									if (skillSlot.skillChangeEvent.IsDisplayUI())
									{
										this.SetComboEffect(skillSlotType, skillSlot.skillChangeEvent.GetEffectTIme(), skillSlot.skillChangeEvent.GetEffectMaxTime());
									}
									else if (!curLvelContext.IsMobaMode())
									{
										this.SetComboEffect(skillSlotType, skillSlot.skillChangeEvent.GetEffectTIme(), skillSlot.skillChangeEvent.GetEffectMaxTime());
									}
								}
								else
								{
									component.enabled = false;
									component2.enabled = false;
									if (button.GetDisableButton() != null)
									{
										this.SetDisableButton(skillSlotType);
									}
									if (button.m_cdText != null)
									{
										button.m_cdText.CustomSetActive(false);
									}
									if (skillSlotType == SkillSlotType.SLOT_SKILL_7)
									{
										Transform transform2 = button.m_button.transform.Find("Present/SkillImg");
										if (transform2 != null)
										{
											transform2.gameObject.CustomSetActive(false);
										}
										Transform transform3 = button.m_button.transform.Find("lblName");
										if (transform3 != null)
										{
											transform3.gameObject.CustomSetActive(false);
										}
									}
								}
								if (button.m_beanText != null)
								{
									if (skillSlotType >= SkillSlotType.SLOT_SKILL_1 && skillSlotType <= SkillSlotType.SLOT_SKILL_3 && skillSlot != null && skillSlot.IsUnLock() && skillSlot.bConsumeBean)
									{
										Text component4 = button.m_beanText.GetComponent<Text>();
										component4.text = skillSlot.skillBeanAmount.ToString();
										button.m_beanText.CustomSetActive(true);
									}
									else
									{
										button.m_beanText.CustomSetActive(false);
									}
									if (button.m_button != null)
									{
										button.m_beanText.transform.position = button.m_button.transform.position;
									}
								}
								if ((skillSlotType == SkillSlotType.SLOT_SKILL_9 || skillSlotType == SkillSlotType.SLOT_SKILL_10) && !bInitSpecifiedButton)
								{
									button.m_button.CustomSetActive(false);
									button.m_cdText.CustomSetActive(false);
								}
							}
						}
					}
				}
				IL_8AC:;
			}
		}

		public void Clear()
		{
			if (this._skillButtons != null)
			{
				for (int i = 0; i < this._skillButtons.Length; i++)
				{
					if (this._skillButtons[i] != null)
					{
						this._skillButtons[i].Clear();
					}
				}
			}
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_ActorRevive, new RefAction<DefaultGameEventParam>(this.onActorRevive));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_CaptainSwitch, new RefAction<DefaultGameEventParam>(this.OnCaptainSwitched));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<PoolObjHandle<ActorRoot>, int, int>("HeroHpChange", new Action<PoolObjHandle<ActorRoot>, int, int>(this.OnHeroHpChange));
		}

		public void InitializeCampHeroInfo(CUIFormScript battleFormScript)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer == null)
			{
				return;
			}
			SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
			if (curLvelContext == null)
			{
				return;
			}
			int num = 0;
			if (curLvelContext.IsMobaMode())
			{
				List<Player> allCampPlayers = Singleton<GamePlayerCenter>.GetInstance().GetAllCampPlayers(hostPlayer.PlayerCamp);
				if (allCampPlayers != null)
				{
					this.m_campHeroInfos = new stCampHeroInfo[allCampPlayers.get_Count() - 1];
					for (int i = 0; i < allCampPlayers.get_Count(); i++)
					{
						if (allCampPlayers.get_Item(i) != hostPlayer)
						{
							this.m_campHeroInfos[num].m_headIconPath = CUIUtility.s_Sprite_Dynamic_BustCircle_Dir + CSkinInfo.GetHeroSkinPic((uint)allCampPlayers.get_Item(i).Captain.get_handle().TheActorMeta.ConfigId, 0u);
							this.m_campHeroInfos[num].m_objectID = allCampPlayers.get_Item(i).Captain.get_handle().ObjID;
							num++;
						}
					}
				}
			}
			else
			{
				ReadonlyContext<PoolObjHandle<ActorRoot>> allHeroes = hostPlayer.GetAllHeroes();
				int count = allHeroes.get_Count();
				if (count > 0)
				{
					this.m_campHeroInfos = new stCampHeroInfo[count - 1];
					for (int j = 0; j < count; j++)
					{
						if (!(allHeroes.get_Item(j) == hostPlayer.Captain))
						{
							this.m_campHeroInfos[num].m_headIconPath = CUIUtility.s_Sprite_Dynamic_BustCircle_Dir + CSkinInfo.GetHeroSkinPic((uint)allHeroes.get_Item(j).get_handle().TheActorMeta.ConfigId, 0u);
							this.m_campHeroInfos[num].m_objectID = allHeroes.get_Item(j).get_handle().ObjID;
							num++;
						}
					}
				}
			}
			this.m_currentSkillJoystickSelectedIndex = -1;
			if (battleFormScript == null)
			{
				return;
			}
			GameObject skillCursor = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCursor(enSkillJoystickMode.SelectTarget);
			CUIFormScript skillCursorFormScript = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCursorFormScript();
			if (skillCursorFormScript == null)
			{
				return;
			}
			if (skillCursor != null)
			{
				CUIComponent component = skillCursor.GetComponent<CUIComponent>();
				if (component != null && component.m_widgets != null)
				{
					CSkillButtonManager.MaxTargetNum = component.m_widgets.Length;
					this.m_targetInfos = new stCampHeroInfo[CSkillButtonManager.MaxTargetNum];
					for (int k = 0; k < component.m_widgets.Length; k++)
					{
						GameObject gameObject = component.m_widgets[k];
						if (gameObject != null)
						{
							gameObject.CustomSetActive(true);
							gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
							CSkillButtonManager.GetJoystickHeadAreaInScreen(ref this.m_targetInfos[k].m_headAreaInScreen, battleFormScript, skillCursor.transform as RectTransform, gameObject.transform as RectTransform);
							CSkillButtonManager.GetJoystickHeadAreaFan(ref this.m_targetInfos[k].m_headAreaFan, gameObject, (k != 0) ? component.m_widgets[k - 1] : null, (k != component.m_widgets.Length - 1) ? component.m_widgets[k + 1] : null);
						}
					}
				}
				skillCursor.CustomSetActive(false);
			}
		}

		public void ResetSkillTargetJoyStickHeadToCampHero()
		{
			GameObject skillCursor = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCursor(enSkillJoystickMode.SelectTarget);
			CUIFormScript skillCursorFormScript = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCursorFormScript();
			if (skillCursorFormScript == null)
			{
				return;
			}
			if (skillCursor != null)
			{
				CUIComponent component = skillCursor.GetComponent<CUIComponent>();
				DebugHelper.Assert(CSkillButtonManager.MaxTargetNum >= this.m_campHeroInfos.Length, "目标数大于轮盘支持的最大英雄数!");
				this.m_CurTargtNum = (uint)Mathf.Min(CSkillButtonManager.MaxTargetNum, this.m_campHeroInfos.Length);
				if (component != null && component.m_widgets != null)
				{
					for (int i = 0; i < component.m_widgets.Length; i++)
					{
						GameObject gameObject = component.m_widgets[i];
						if (gameObject != null)
						{
							if (i >= this.m_campHeroInfos.Length)
							{
								this.m_targetInfos[i].m_objectID = 0u;
								gameObject.CustomSetActive(false);
							}
							else
							{
								this.m_targetInfos[i].m_objectID = this.m_campHeroInfos[i].m_objectID;
								gameObject.CustomSetActive(true);
								gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
								CUIComponent component2 = gameObject.GetComponent<CUIComponent>();
								if (component2 != null && component2.m_widgets != null && component2.m_widgets.Length >= 2)
								{
									GameObject gameObject2 = component2.m_widgets[0];
									if (gameObject2 != null)
									{
										Image component3 = gameObject2.GetComponent<Image>();
										if (component3 != null)
										{
											component3.SetSprite(this.m_campHeroInfos[i].m_headIconPath, skillCursorFormScript, true, false, false, false);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		public void ResetSkillTargetJoyStickHeadToTargets(ListValueView<uint> targetIDs)
		{
			GameObject skillCursor = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCursor(enSkillJoystickMode.SelectTarget);
			CUIFormScript skillCursorFormScript = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCursorFormScript();
			if (skillCursorFormScript == null)
			{
				return;
			}
			if (skillCursor != null)
			{
				CUIComponent component = skillCursor.GetComponent<CUIComponent>();
				DebugHelper.Assert(CSkillButtonManager.MaxTargetNum >= targetIDs.get_Count(), "目标数大于轮盘支持的最大英雄数!");
				this.m_CurTargtNum = (uint)Mathf.Min(CSkillButtonManager.MaxTargetNum, targetIDs.get_Count());
				if (component != null && component.m_widgets != null)
				{
					for (int i = 0; i < component.m_widgets.Length; i++)
					{
						GameObject gameObject = component.m_widgets[i];
						if (gameObject != null)
						{
							if (i >= targetIDs.get_Count())
							{
								this.m_targetInfos[i].m_objectID = 0u;
								gameObject.CustomSetActive(false);
							}
							else
							{
								this.m_targetInfos[i].m_objectID = targetIDs.get_Item(i);
								gameObject.CustomSetActive(true);
								gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
								CUIComponent component2 = gameObject.GetComponent<CUIComponent>();
								if (component2 != null && component2.m_widgets != null && component2.m_widgets.Length >= 2)
								{
									PoolObjHandle<ActorRoot> actor = Singleton<GameObjMgr>.GetInstance().GetActor(targetIDs.get_Item(i));
									string prefabPath = CUIUtility.s_Sprite_Dynamic_BustCircle_Dir + CSkinInfo.GetHeroSkinPic((uint)actor.get_handle().TheActorMeta.ConfigId, 0u);
									GameObject gameObject2 = component2.m_widgets[0];
									if (gameObject2 != null)
									{
										Image component3 = gameObject2.GetComponent<Image>();
										if (component3 != null)
										{
											component3.SetSprite(prefabPath, skillCursorFormScript, true, false, false, false);
										}
									}
									GameObject gameObject3 = component2.m_widgets[1];
									if (gameObject3 != null)
									{
										Image component4 = gameObject3.GetComponent<Image>();
										if (component4 != null)
										{
											component4.SetSprite("UGUI/Sprite/Battle/Battle_skillFrameBg_new.prefab", skillCursorFormScript, true, false, false, false);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		public static Color GetCursorBGColor(bool cancel)
		{
			return (!cancel) ? CSkillButtonManager.s_skillCursorBGColorBlue : CSkillButtonManager.s_skillCursorBGColorRed;
		}

		private static void GetJoystickHeadAreaInScreen(ref Rect targetArea, CUIFormScript formScript, RectTransform joystickRectTransform, RectTransform targetRectTransform)
		{
			if (joystickRectTransform == null || targetRectTransform == null)
			{
				targetArea = new Rect(0f, 0f, 0f, 0f);
				return;
			}
			Vector2 vector = new Vector2(formScript.ChangeFormValueToScreen(targetRectTransform.anchoredPosition.x), formScript.ChangeFormValueToScreen(targetRectTransform.anchoredPosition.y));
			float num = formScript.ChangeFormValueToScreen(targetRectTransform.rect.width);
			float num2 = formScript.ChangeFormValueToScreen(targetRectTransform.rect.height);
			float x = vector.x - num / 2f;
			float y = vector.y - num2 / 2f;
			targetArea = new Rect(x, y, num, num2);
		}

		private static void GetJoystickHeadAreaFan(ref stFan headAreaFan, GameObject head, GameObject preHead, GameObject backHead)
		{
			if (preHead == null && backHead == null)
			{
				headAreaFan.m_minRadian = 0f;
				headAreaFan.m_maxRadian = 0f;
			}
			float radian = CSkillButtonManager.GetRadian((head.transform as RectTransform).anchoredPosition);
			if (preHead != null)
			{
				headAreaFan.m_minRadian = CSkillButtonManager.GetRadian(((head.transform as RectTransform).anchoredPosition + (preHead.transform as RectTransform).anchoredPosition) / 2f);
				if (backHead == null)
				{
					headAreaFan.m_maxRadian = radian + (radian - headAreaFan.m_minRadian);
					return;
				}
			}
			if (backHead != null)
			{
				headAreaFan.m_maxRadian = CSkillButtonManager.GetRadian(((head.transform as RectTransform).anchoredPosition + (backHead.transform as RectTransform).anchoredPosition) / 2f);
				if (preHead == null)
				{
					headAreaFan.m_minRadian = radian - (headAreaFan.m_maxRadian - radian);
					return;
				}
			}
		}

		private static float GetRadian(Vector2 point)
		{
			float num = Mathf.Atan2(point.y, point.x);
			if (num < 0f)
			{
				num += 6.28318548f;
			}
			return num;
		}

		public static void Preload(ref ActorPreloadTab result)
		{
		}

		public SkillButton GetButton(SkillSlotType skillSlotType)
		{
			if (skillSlotType < SkillSlotType.SLOT_SKILL_0 || skillSlotType >= (SkillSlotType)this._skillButtons.Length)
			{
				return null;
			}
			SkillButton skillButton = this._skillButtons[(int)skillSlotType];
			if (skillButton == null)
			{
				FightForm fightForm = Singleton<CBattleSystem>.GetInstance().FightForm;
				if (fightForm != null)
				{
					CUIFormScript skillBtnFormScript = fightForm.GetSkillBtnFormScript();
					skillButton = new SkillButton();
					skillButton.m_button = ((!(skillBtnFormScript == null)) ? skillBtnFormScript.GetWidget((int)CSkillButtonManager.s_skillButtons[(int)skillSlotType]) : null);
					skillButton.m_cdText = ((!(skillBtnFormScript == null)) ? skillBtnFormScript.GetWidget((int)CSkillButtonManager.s_skillCDTexts[(int)skillSlotType]) : null);
					skillButton.m_beanText = ((!(skillBtnFormScript == null)) ? skillBtnFormScript.GetWidget((int)CSkillButtonManager.s_skillBeanTexts[(int)skillSlotType]) : null);
					this._skillButtons[(int)skillSlotType] = skillButton;
				}
				if (skillButton.m_button != null)
				{
					Transform transform = skillButton.m_button.transform.FindChild("IndicatorPosition");
					if (transform != null)
					{
						skillButton.m_skillIndicatorFixedPosition = transform.position;
					}
				}
			}
			return skillButton;
		}

		public void SetLimitButton(SkillSlotType skillSlotType)
		{
			CUIFormScript fightFormScript = Singleton<CBattleSystem>.GetInstance().FightFormScript;
			if (fightFormScript == null)
			{
				return;
			}
			if (this.m_currentSkillSlotType == skillSlotType)
			{
				this.SkillButtonUp(fightFormScript, skillSlotType, false, default(Vector2));
			}
			SkillButton button = this.GetButton(skillSlotType);
			DebugHelper.Assert(button != null);
			if (button != null)
			{
				button.bLimitedFlag = true;
				if (skillSlotType != SkillSlotType.SLOT_SKILL_0)
				{
					if (button.m_button != null)
					{
						CUIEventScript component = button.m_button.GetComponent<CUIEventScript>();
						if (component)
						{
							if (component.ClearInputStatus())
							{
								Singleton<CBattleSystem>.GetInstance().FightForm.HideSkillDescInfo();
							}
							component.enabled = false;
						}
					}
					GameObject gameObject = button.GetAnimationPresent().transform.Find("disableFrame").gameObject;
					DebugHelper.Assert(gameObject != null);
					if (gameObject != null)
					{
						gameObject.CustomSetActive(true);
					}
					GameObject disableButton = button.GetDisableButton();
					if (disableButton)
					{
						disableButton.CustomSetActive(true);
					}
					GameObject animationPresent = button.GetAnimationPresent();
					CUICommonSystem.PlayAnimation(animationPresent, enSkillButtonAnimationName.disable.ToString(), false);
				}
			}
		}

		public void CancelLimitButton(SkillSlotType skillSlotType)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			SkillButton button = this.GetButton(skillSlotType);
			DebugHelper.Assert(button != null);
			if (button != null)
			{
				button.bLimitedFlag = false;
				if (skillSlotType == SkillSlotType.SLOT_SKILL_0)
				{
					if (!button.bDisableFlag)
					{
						GameObject animationPresent = button.GetAnimationPresent();
						if (animationPresent != null)
						{
							Image component = animationPresent.GetComponent<Image>();
							if (component != null)
							{
								component.color = CUIUtility.s_Color_Full;
							}
						}
						GameObject disableButton = button.GetDisableButton();
						if (disableButton)
						{
							disableButton.CustomSetActive(false);
						}
						this.SetSelectTargetBtnState(false);
					}
				}
				else
				{
					if (button.m_button != null)
					{
						CUIEventScript component2 = button.m_button.GetComponent<CUIEventScript>();
						SkillSlot skillSlot;
						if (component2 && hostPlayer != null && hostPlayer.Captain && hostPlayer.Captain.get_handle().SkillControl.TryGetSkillSlot(skillSlotType, out skillSlot))
						{
							if (skillSlot.EnableButtonFlag)
							{
								component2.enabled = true;
							}
							else
							{
								component2.enabled = false;
							}
						}
					}
					GameObject gameObject = button.GetAnimationPresent().transform.Find("disableFrame").gameObject;
					DebugHelper.Assert(gameObject != null);
					if (gameObject != null)
					{
						gameObject.CustomSetActive(false);
					}
					if (!button.bDisableFlag)
					{
						GameObject disableButton2 = button.GetDisableButton();
						if (disableButton2)
						{
							CUIEventScript component3 = disableButton2.GetComponent<CUIEventScript>();
							if (component3 && component3.ClearInputStatus())
							{
								Singleton<CBattleSystem>.GetInstance().FightForm.HideSkillDescInfo();
							}
							disableButton2.CustomSetActive(false);
						}
						GameObject animationPresent2 = button.GetAnimationPresent();
						CUICommonSystem.PlayAnimation(animationPresent2, enSkillButtonAnimationName.normal.ToString(), false);
					}
				}
			}
		}

		public void SetDisableButton(SkillSlotType skillSlotType)
		{
			CUIFormScript fightFormScript = Singleton<CBattleSystem>.GetInstance().FightFormScript;
			if (fightFormScript == null)
			{
				return;
			}
			if (this.m_currentSkillSlotType == skillSlotType)
			{
				this.SkillButtonUp(fightFormScript, skillSlotType, false, default(Vector2));
			}
			SkillButton button = this.GetButton(skillSlotType);
			if (button != null)
			{
				if (button.m_button != null)
				{
					CUIEventScript component = button.m_button.GetComponent<CUIEventScript>();
					if (component)
					{
						if (component.ClearInputStatus())
						{
							Singleton<CBattleSystem>.GetInstance().FightForm.HideSkillDescInfo();
						}
						component.enabled = false;
					}
				}
				button.bDisableFlag = true;
				if (skillSlotType != SkillSlotType.SLOT_SKILL_0)
				{
					GameObject animationPresent = button.GetAnimationPresent();
					CUICommonSystem.PlayAnimation(animationPresent, enSkillButtonAnimationName.disable.ToString(), false);
				}
				else
				{
					GameObject animationPresent2 = button.GetAnimationPresent();
					if (animationPresent2 != null)
					{
						Image component2 = animationPresent2.GetComponent<Image>();
						if (component2 != null)
						{
							component2.color = CUIUtility.s_Color_DisableGray;
						}
					}
					GameObject disableButton = button.GetDisableButton();
					if (disableButton)
					{
						disableButton.CustomSetActive(true);
					}
					this.SetSelectTargetBtnState(true);
					this.EnableLastHitBtn(false);
				}
				GameObject disableButton2 = button.GetDisableButton();
				if (disableButton2 != null)
				{
					disableButton2.CustomSetActive(true);
				}
			}
		}

		public void SetEnergyDisableButton(SkillSlotType skillSlotType)
		{
			CUIFormScript fightFormScript = Singleton<CBattleSystem>.GetInstance().FightFormScript;
			if (fightFormScript == null)
			{
				return;
			}
			SkillButton button = this.GetButton(skillSlotType);
			if (button != null)
			{
				if (button.m_button != null)
				{
					CUIEventScript component = button.m_button.GetComponent<CUIEventScript>();
					if (component)
					{
						component.enabled = false;
					}
				}
				button.bDisableFlag = true;
				GameObject animationPresent = button.GetAnimationPresent();
				CUICommonSystem.PlayAnimation(animationPresent, enSkillButtonAnimationName.disable.ToString(), false);
				GameObject disableButton = button.GetDisableButton();
				if (disableButton != null)
				{
					disableButton.CustomSetActive(true);
				}
			}
		}

		public bool SetEnableButton(SkillSlotType skillSlotType)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer != null && hostPlayer.Captain)
			{
				SkillSlot skillSlot;
				if (!hostPlayer.Captain.get_handle().SkillControl.TryGetSkillSlot(skillSlotType, out skillSlot))
				{
					return false;
				}
				if (skillSlotType == SkillSlotType.SLOT_SKILL_0)
				{
					if (hostPlayer.Captain.get_handle().ActorControl.IsDeadState)
					{
						return false;
					}
				}
				else if (!skillSlot.EnableButtonFlag)
				{
					return false;
				}
			}
			SkillButton button = this.GetButton(skillSlotType);
			if (button != null)
			{
				button.bDisableFlag = false;
				if (button.bLimitedFlag)
				{
					return false;
				}
				if (button.m_button != null)
				{
					CUIEventScript component = button.m_button.GetComponent<CUIEventScript>();
					if (component)
					{
						if (component.ClearInputStatus())
						{
							Singleton<CBattleSystem>.GetInstance().FightForm.HideSkillDescInfo();
						}
						component.enabled = true;
					}
				}
				if (skillSlotType != SkillSlotType.SLOT_SKILL_0)
				{
					GameObject animationPresent = button.GetAnimationPresent();
					CUICommonSystem.PlayAnimation(animationPresent, enSkillButtonAnimationName.normal.ToString(), false);
				}
				else if (skillSlotType == SkillSlotType.SLOT_SKILL_0)
				{
					GameObject animationPresent2 = button.GetAnimationPresent();
					if (animationPresent2 != null)
					{
						Image component2 = animationPresent2.GetComponent<Image>();
						if (component2 != null)
						{
							component2.color = CUIUtility.s_Color_Full;
						}
					}
					this.SetSelectTargetBtnState(false);
					this.EnableLastHitBtn(true);
				}
				GameObject disableButton = button.GetDisableButton();
				if (disableButton != null)
				{
					CUIEventScript component3 = disableButton.GetComponent<CUIEventScript>();
					if (component3)
					{
						if (component3.ClearInputStatus())
						{
							Singleton<CBattleSystem>.GetInstance().FightForm.HideSkillDescInfo();
						}
						component3.enabled = true;
					}
					disableButton.CustomSetActive(false);
				}
			}
			return true;
		}

		public bool ClearButtonInput(SkillSlotType CurSlotType)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer != null && hostPlayer.Captain)
			{
				for (int i = 0; i < 10; i++)
				{
					if (CurSlotType != (SkillSlotType)i)
					{
						SkillSlot skillSlot;
						if (!hostPlayer.Captain.get_handle().SkillControl.TryGetSkillSlot((SkillSlotType)i, out skillSlot))
						{
							return false;
						}
						SkillButton button = this.GetButton((SkillSlotType)i);
						if (button != null)
						{
							if (button.m_button != null)
							{
								CUIEventScript component = button.m_button.GetComponent<CUIEventScript>();
								if (component)
								{
									component.ClearInputStatus();
								}
							}
							GameObject disableButton = button.GetDisableButton();
							if (disableButton != null)
							{
								CUIEventScript component2 = disableButton.GetComponent<CUIEventScript>();
								if (component2)
								{
									component2.ClearInputStatus();
								}
							}
						}
					}
				}
			}
			return true;
		}

		public void LastHitButtonDown(CUIFormScript formScript)
		{
			this.SendUseCommonAttack(2, 0u);
		}

		public void LastHitButtonUp(CUIFormScript formScript)
		{
			this.SendUseCommonAttack(0, 0u);
		}

		public void SetLastHitBtnState(LastHitMode mode)
		{
			CUIFormScript cUIFormScript = (Singleton<CBattleSystem>.GetInstance().FightForm != null) ? Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillBtnFormScript() : null;
			if (cUIFormScript == null)
			{
				return;
			}
			GameObject widget = cUIFormScript.GetWidget(25);
			if (widget != null)
			{
				widget.CustomSetActive(mode == LastHitMode.LastHit);
			}
		}

		public void SkillButtonDown(CUIFormScript formScript, SkillSlotType skillSlotType, Vector2 downScreenPosition)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer != null && hostPlayer.Captain)
			{
				int num = 0;
				if (hostPlayer.Captain.get_handle().SkillControl != null && skillSlotType >= SkillSlotType.SLOT_SKILL_0 && skillSlotType < SkillSlotType.SLOT_SKILL_COUNT && hostPlayer.Captain.get_handle().SkillControl.SkillSlotArray[(int)skillSlotType] != null)
				{
					num = hostPlayer.Captain.get_handle().SkillControl.SkillSlotArray[(int)skillSlotType].GetSkillLevel();
				}
				if (num <= 0)
				{
					return;
				}
			}
			if (this.m_currentSkillSlotType != SkillSlotType.SLOT_SKILL_COUNT)
			{
				this.SkillButtonUp(formScript, this.m_currentSkillSlotType, false, default(Vector2));
			}
			this.SetSkillBtnDragFlag(skillSlotType, false);
			Singleton<CBattleSystem>.get_instance().TheMinimapSys.ClearMapSkillStatus();
			this.m_currentSkillSlotType = skillSlotType;
			this.m_currentSkillDownScreenPosition = downScreenPosition;
			this.m_currentSkillIndicatorEnabled = false;
			this.m_currentSkillIndicatorJoystickEnabled = false;
			this.m_currentSkillIndicatorInCancelArea = false;
			this.m_currentSkillJoystickMode = this.GetSkillJoystickMode(skillSlotType);
			this.m_commonAtkSlide = false;
			GameObject animationPresent = this.GetButton(skillSlotType).GetAnimationPresent();
			if (hostPlayer == null)
			{
				return;
			}
			if (skillSlotType == SkillSlotType.SLOT_SKILL_0)
			{
				this.SendUseCommonAttack(1, 0u);
				Singleton<CUIParticleSystem>.GetInstance().AddParticle(CUIParticleSystem.s_particleSkillBtnEffect_Path, 0.5f, animationPresent, formScript);
			}
			else
			{
				if (skillSlotType != SkillSlotType.SLOT_SKILL_0)
				{
					CUICommonSystem.PlayAnimation(animationPresent, enSkillButtonAnimationName.pressDown.ToString(), false);
				}
				this.ReadyUseSkillSlot(skillSlotType);
				this.EnableSkillCursor(formScript, ref downScreenPosition, this.IsUseSkillCursorJoystick(skillSlotType), skillSlotType, hostPlayer.Captain.get_handle().SkillControl.SkillSlotArray[(int)skillSlotType], skillSlotType != SkillSlotType.SLOT_SKILL_0);
				if (skillSlotType == SkillSlotType.SLOT_SKILL_3 && hostPlayer != null && hostPlayer.Captain && hostPlayer.Captain.get_handle().SkillControl != null)
				{
					GameObject prefabEffectObj = hostPlayer.Captain.get_handle().SkillControl.GetPrefabEffectObj(skillSlotType);
					if (prefabEffectObj != null)
					{
						Vector3 forward = prefabEffectObj.transform.forward;
						MinimapSkillIndicator_3DUI.SetIndicator(ref forward, true);
						MinimapSkillIndicator_3DUI.SetIndicatorColor(!this.m_currentSkillIndicatorInCancelArea);
					}
				}
			}
		}

		public void SkillButtonUp(CUIFormScript formScript, SkillSlotType skillSlotType, bool isTriggeredActively, Vector2 screenPosition = default(Vector2))
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer == null || this.m_currentSkillSlotType != skillSlotType || formScript == null)
			{
				return;
			}
			if (hostPlayer.Captain)
			{
				int num = 0;
				if (hostPlayer.Captain.get_handle().SkillControl != null && skillSlotType >= SkillSlotType.SLOT_SKILL_0 && skillSlotType < SkillSlotType.SLOT_SKILL_COUNT && hostPlayer.Captain.get_handle().SkillControl.SkillSlotArray[(int)skillSlotType] != null)
				{
					num = hostPlayer.Captain.get_handle().SkillControl.SkillSlotArray[(int)skillSlotType].GetSkillLevel();
				}
				if (num <= 0)
				{
					return;
				}
			}
			SkillButton button = this.GetButton(skillSlotType);
			if (button == null)
			{
				return;
			}
			GameObject animationPresent = button.GetAnimationPresent();
			if (skillSlotType == SkillSlotType.SLOT_SKILL_0)
			{
				if (this.m_commonAtkSlide)
				{
					this.CommonAtkSlide(formScript, screenPosition);
					this.m_commonAtkSlide = false;
				}
				this.SendUseCommonAttack(0, 0u);
			}
			else
			{
				if (skillSlotType != SkillSlotType.SLOT_SKILL_0)
				{
					CUICommonSystem.PlayAnimation(animationPresent, enSkillButtonAnimationName.pressUp.ToString(), false);
				}
				if (isTriggeredActively && !this.m_currentSkillIndicatorInCancelArea)
				{
					if (skillSlotType != SkillSlotType.SLOT_SKILL_0)
					{
						enSkillJoystickMode skillJoystickMode = this.GetSkillJoystickMode(skillSlotType);
						if (skillJoystickMode != enSkillJoystickMode.MapSelect && skillJoystickMode != enSkillJoystickMode.MapSelectOther)
						{
							uint num2 = 0u;
							SkillSlot skillSlot;
							if (GameSettings.TheCastType == CastType.LunPanCast && GameSettings.ShowEnemyHeroHeadBtnMode && GameSettings.LunPanLockEnemyHeroMode && hostPlayer.Captain && hostPlayer.Captain.get_handle().SkillControl != null && hostPlayer.Captain.get_handle().SkillControl.TryGetSkillSlot(skillSlotType, out skillSlot))
							{
								Skill skill = (skillSlot.NextSkillObj == null) ? skillSlot.SkillObj : skillSlot.NextSkillObj;
								if (skill != null && skill.cfgData != null && skill.cfgData.bRangeAppointType == 1 && skillSlot.CanUseSkillWithEnemyHeroSelectMode() && skillJoystickMode == enSkillJoystickMode.General && (num2 = Singleton<CBattleSystem>.get_instance().FightForm.m_enemyHeroAtkBtn.GetLockedEnemyHeroObjId()) > 0u)
								{
									Singleton<CBattleSystem>.get_instance().FightForm.m_enemyHeroAtkBtn.OnSkillBtnUp(skillSlotType, this.m_currentSkillJoystickMode);
								}
							}
							if (num2 == 0u)
							{
								if (this.m_currentSkillJoystickSelectedIndex != -1)
								{
									num2 = this.m_targetInfos[this.m_currentSkillJoystickSelectedIndex].m_objectID;
								}
								this.RequestUseSkillSlot(skillSlotType, this.m_currentSkillJoystickMode, num2);
							}
						}
					}
				}
				else
				{
					this.CancelUseSkillSlot(skillSlotType, this.m_currentSkillJoystickMode);
				}
				if (this.m_currentSkillIndicatorEnabled)
				{
					this.DisableSkillCursor(formScript, skillSlotType);
				}
				if (this.m_currentSkillSlotType == SkillSlotType.SLOT_SKILL_3)
				{
					MinimapSkillIndicator_3DUI.CancelIndicator();
				}
			}
			if (CSkillButtonManager.IsSelectedTargetJoyStick(this.m_currentSkillJoystickMode) && this.m_currentSkillJoystickSelectedIndex >= 0)
			{
				this.m_currentSkillJoystickSelectedIndex = -1;
				this.SetSkillIndicatorSelectedTarget(this.m_currentSkillJoystickSelectedIndex);
			}
			this.m_currentSkillSlotType = SkillSlotType.SLOT_SKILL_COUNT;
			this.m_currentSkillDownScreenPosition = Vector2.zero;
		}

		public void SkillButtonUp(CUIFormScript formScript)
		{
			if (this.m_currentSkillSlotType == SkillSlotType.SLOT_SKILL_COUNT || formScript == null)
			{
				return;
			}
			this.SkillButtonUp(formScript, this.m_currentSkillSlotType, false, default(Vector2));
		}

		public void SelectedMapTarget(SkillSlotType skillSlotType, uint targetObjId)
		{
			this.RequestUseSkillSlot(skillSlotType, enSkillJoystickMode.MapSelect, targetObjId);
		}

		public void SelectedMapTarget(uint targetObjId)
		{
			SkillSlotType skillSlotType;
			if (this.HasMapSlectTargetSkill(out skillSlotType))
			{
				this.RequestUseSkillSlot(skillSlotType, enSkillJoystickMode.MapSelect, targetObjId);
			}
		}

		public void CommonAtkSlide(CUIFormScript battleFormScript, Vector2 screenPosition)
		{
			if (GameSettings.TheCommonAttackType == CommonAttactType.Type2)
			{
				SkillButton button = this.GetButton(SkillSlotType.SLOT_SKILL_0);
				CUIFormScript skillBtnFormScript = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillBtnFormScript();
				if (button == null || button.bDisableFlag || skillBtnFormScript == null)
				{
					return;
				}
				GameObject widget = skillBtnFormScript.GetWidget(24);
				GameObject widget2 = skillBtnFormScript.GetWidget(9);
				if (this.IsSkillCursorInTargetArea(battleFormScript, ref screenPosition, widget))
				{
					Singleton<LockModeKeySelector>.GetInstance().OnHandleClickSelectTargetBtn(AttackTargetType.ATTACK_TARGET_HERO);
					Singleton<CUIParticleSystem>.GetInstance().AddParticle(CUIParticleSystem.s_particleSkillBtnEffect_Path, 0.5f, widget, battleFormScript);
					Singleton<CSoundManager>.GetInstance().PostEvent("UI_signal_Change_hero", null);
				}
				else if (this.IsSkillCursorInTargetArea(battleFormScript, ref screenPosition, widget2))
				{
					Singleton<LockModeKeySelector>.GetInstance().OnHandleClickSelectTargetBtn(AttackTargetType.ATTACK_TARGET_SOLDIER);
					Singleton<CUIParticleSystem>.GetInstance().AddParticle(CUIParticleSystem.s_particleSkillBtnEffect_Path, 0.5f, widget2, battleFormScript);
					Singleton<CSoundManager>.GetInstance().PostEvent("UI_signal_Change_xiaobing", null);
				}
			}
		}

		public void DragSkillButton(CUIFormScript formScript, SkillSlotType skillSlotType, Vector2 dragScreenPosition)
		{
			if (skillSlotType == SkillSlotType.SLOT_SKILL_0)
			{
				this.m_commonAtkSlide = true;
			}
			if (this.m_currentSkillSlotType != skillSlotType || !this.m_currentSkillIndicatorEnabled)
			{
				return;
			}
			bool currentSkillIndicatorInCancelArea = this.m_currentSkillIndicatorInCancelArea;
			if (formScript == null)
			{
				return;
			}
			this.SetSkillBtnDragFlag(skillSlotType, true);
			GameObject skillCursor = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCursor(this.m_currentSkillJoystickMode);
			Vector2 vector = this.MoveSkillCursor(formScript, skillCursor, skillSlotType, this.m_currentSkillJoystickMode, ref dragScreenPosition, out this.m_currentSkillIndicatorInCancelArea);
			if (currentSkillIndicatorInCancelArea != this.m_currentSkillIndicatorInCancelArea)
			{
				this.ChangeSkillCursorBGSprite(formScript, skillCursor, this.m_currentSkillIndicatorInCancelArea);
			}
			if (this.m_currentSkillJoystickMode == enSkillJoystickMode.General)
			{
				if (GameSettings.TheCastType == CastType.LunPanCast && GameSettings.ShowEnemyHeroHeadBtnMode && GameSettings.LunPanLockEnemyHeroMode)
				{
					Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
					SkillSlot skillSlot;
					if (hostPlayer.Captain.get_handle().SkillControl.TryGetSkillSlot(skillSlotType, out skillSlot))
					{
						Skill skill = (skillSlot.NextSkillObj == null) ? skillSlot.SkillObj : skillSlot.NextSkillObj;
						if (skill != null && skill.cfgData != null && skill.cfgData.bRangeAppointType == 1 && skillSlot.CanUseSkillWithEnemyHeroSelectMode() && Singleton<CBattleSystem>.get_instance().FightForm != null && Singleton<CBattleSystem>.get_instance().FightForm.m_enemyHeroAtkBtn != null)
						{
							Singleton<CBattleSystem>.get_instance().FightForm.m_enemyHeroAtkBtn.OnSkillBtnDrag(formScript, skillSlotType, dragScreenPosition, this.m_currentSkillIndicatorInCancelArea);
							return;
						}
					}
				}
				this.MoveSkillCursorInScene(skillSlotType, ref vector, this.m_currentSkillIndicatorInCancelArea);
				if (skillSlotType == SkillSlotType.SLOT_SKILL_3)
				{
					MinimapSkillIndicator_3DUI.UpdateIndicator(ref vector, true);
					MinimapSkillIndicator_3DUI.SetIndicatorColor(!this.m_currentSkillIndicatorInCancelArea);
				}
			}
			else if (this.m_currentSkillJoystickMode == enSkillJoystickMode.SelectTarget && this.m_currentSkillJoystickSelectedIndex != -1)
			{
				uint objectID = this.m_targetInfos[this.m_currentSkillJoystickSelectedIndex].m_objectID;
				PoolObjHandle<ActorRoot> actor = Singleton<GameObjMgr>.GetInstance().GetActor(objectID);
				if (actor)
				{
					MonoSingleton<CameraSystem>.GetInstance().SetFocusActor(actor);
				}
			}
		}

		public void SendUseCommonAttack(sbyte Start, uint uiRealObjID = 0u)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer != null && hostPlayer.Captain)
			{
				if (hostPlayer.Captain.get_handle().ActorControl.IsDeadState)
				{
					return;
				}
				FrameCommand<UseCommonAttackCommand> frameCommand = FrameCommandFactory.CreateCSSyncFrameCommand<UseCommonAttackCommand>();
				frameCommand.cmdData.Start = Start;
				frameCommand.cmdData.uiRealObjID = uiRealObjID;
				frameCommand.Send();
			}
		}

		public void ReadyUseSkillSlot(SkillSlotType skillSlotType)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer != null && hostPlayer.Captain)
			{
				hostPlayer.Captain.get_handle().SkillControl.ReadyUseSkillSlot(skillSlotType);
			}
		}

		public void OnBattleSkillDisableAlert(SkillSlotType skillSlotType)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			SkillSlot skillSlot;
			if (hostPlayer != null && hostPlayer.Captain && hostPlayer.Captain.get_handle().SkillControl.TryGetSkillSlot(skillSlotType, out skillSlot) && skillSlot.IsUnLock())
			{
				if (!skillSlot.IsCDReady)
				{
					skillSlot.SendSkillCooldownEvent();
				}
				else if (!skillSlot.IsEnergyEnough)
				{
					skillSlot.SendSkillShortageEvent();
				}
				else if (!skillSlot.IsSkillBeanEnough)
				{
					skillSlot.SendSkillBeanShortageEvent();
				}
			}
		}

		public void RequestUseSkillSlot(SkillSlotType skillSlotType, enSkillJoystickMode mode = enSkillJoystickMode.General, uint objID = 0u)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer != null && hostPlayer.Captain)
			{
				hostPlayer.Captain.get_handle().SkillControl.RequestUseSkillSlot(skillSlotType, mode, objID);
			}
		}

		public void CancelUseSkillSlot(SkillSlotType skillSlotType, enSkillJoystickMode mode = enSkillJoystickMode.General)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer != null && hostPlayer.Captain)
			{
				hostPlayer.Captain.get_handle().SkillControl.HostCancelUseSkillSlot(skillSlotType, mode);
			}
			if ((mode == enSkillJoystickMode.MapSelect || mode == enSkillJoystickMode.MapSelectOther) && Singleton<CBattleSystem>.get_instance().TheMinimapSys.CurMapType() == MinimapSys.EMapType.Skill)
			{
				Singleton<CBattleSystem>.get_instance().TheMinimapSys.Switch(MinimapSys.EMapType.Mini);
			}
		}

		public bool IsUseSkillCursorJoystick(SkillSlotType skillSlotType)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			return hostPlayer != null && hostPlayer.Captain && hostPlayer.Captain.get_handle().SkillControl.IsUseSkillJoystick(skillSlotType);
		}

		public enSkillJoystickMode GetSkillJoystickMode(SkillSlotType skillSlotType)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer != null && hostPlayer.Captain)
			{
				SkillSlot skillSlot = null;
				if (hostPlayer.Captain.get_handle().SkillControl.TryGetSkillSlot(skillSlotType, out skillSlot) && skillSlot != null)
				{
					Skill skill = (skillSlot.NextSkillObj == null) ? skillSlot.SkillObj : skillSlot.NextSkillObj;
					if (skill != null && skill.cfgData != null)
					{
						return (enSkillJoystickMode)skill.cfgData.bWheelType;
					}
				}
			}
			return enSkillJoystickMode.General;
		}

		public void EnableSkillCursor(CUIFormScript battleFormScript, ref Vector2 screenPosition, bool enableSkillCursorJoystick, SkillSlotType skillSlotType, SkillSlot useSlot, bool isSkillCanBeCancled)
		{
			this.m_currentSkillIndicatorEnabled = true;
			this.m_currentSkillIndicatorResponed = false;
			this.m_currentSkillTipsResponed = false;
			if (enableSkillCursorJoystick)
			{
				this.m_currentSkillIndicatorJoystickEnabled = true;
				if (battleFormScript != null)
				{
					if (this.m_currentSkillJoystickMode == enSkillJoystickMode.General)
					{
						this.DisableSkillJoystick(battleFormScript, enSkillJoystickMode.SelectTarget);
						GameObject skillCursor = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCursor(enSkillJoystickMode.General);
						if (skillCursor != null)
						{
							skillCursor.CustomSetActive(true);
							Vector3 skillIndicatorFixedPosition = this.GetButton(skillSlotType).m_skillIndicatorFixedPosition;
							if (this.m_skillIndicatorMode == enSkillIndicatorMode.General || skillIndicatorFixedPosition == Vector3.zero)
							{
								skillCursor.transform.position = CUIUtility.ScreenToWorldPoint(battleFormScript.GetCamera(), screenPosition, skillCursor.transform.position.z);
								this.m_currentSkillIndicatorScreenPosition = screenPosition;
							}
							else
							{
								skillCursor.transform.position = skillIndicatorFixedPosition;
								this.m_currentSkillIndicatorScreenPosition = CUIUtility.WorldToScreenPoint(battleFormScript.GetCamera(), skillIndicatorFixedPosition);
							}
						}
						this.ChangeSkillCursorBGSprite(battleFormScript, skillCursor, this.m_currentSkillIndicatorInCancelArea);
					}
					else if (CSkillButtonManager.IsSelectedTargetJoyStick(this.m_currentSkillJoystickMode))
					{
						this.DisableSkillJoystick(battleFormScript, enSkillJoystickMode.General);
						if (this.m_currentSkillJoystickMode == enSkillJoystickMode.SelectTarget)
						{
							this.ResetSkillTargetJoyStickHeadToCampHero();
						}
						else if (this.m_currentSkillJoystickMode == enSkillJoystickMode.SelectNextSkillTarget)
						{
							this.ResetSkillTargetJoyStickHeadToTargets(useSlot.NextSkillTargetIDs);
						}
						GameObject skillCursor2 = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCursor(enSkillJoystickMode.SelectTarget);
						if (skillCursor2 != null)
						{
							skillCursor2.CustomSetActive(true);
							skillCursor2.transform.position = this.GetButton(skillSlotType).m_button.transform.position;
							this.m_currentSkillIndicatorScreenPosition = CUIUtility.WorldToScreenPoint(battleFormScript.GetCamera(), skillCursor2.transform.position);
							CUIAnimationScript component = skillCursor2.GetComponent<CUIAnimationScript>();
							if (component != null)
							{
								component.PlayAnimation("Head_In2", true);
							}
							this.ResetSkillJoystickSelectedTarget(battleFormScript);
							this.ChangeSkillCursorBGSprite(battleFormScript, skillCursor2, this.m_currentSkillIndicatorInCancelArea);
						}
					}
					else if (this.m_currentSkillJoystickMode == enSkillJoystickMode.MapSelect)
					{
						if (Singleton<TeleportTargetSelector>.GetInstance().m_ClickDownStatus)
						{
							MonoSingleton<CameraSystem>.get_instance().ToggleFreeDragCamera(false);
						}
						this.DisableSkillJoystick(battleFormScript, enSkillJoystickMode.General);
						this.DisableSkillJoystick(battleFormScript, enSkillJoystickMode.SelectTarget);
						Singleton<CBattleSystem>.get_instance().TheMinimapSys.Switch(MinimapSys.EMapType.Skill, skillSlotType);
					}
					else if (this.m_currentSkillJoystickMode == enSkillJoystickMode.MapSelectOther)
					{
						this.DisableSkillJoystick(battleFormScript, enSkillJoystickMode.General);
						this.DisableSkillJoystick(battleFormScript, enSkillJoystickMode.SelectTarget);
						if (Singleton<TeleportTargetSelector>.GetInstance().m_ClickDownStatus)
						{
							Singleton<TeleportTargetSelector>.GetInstance().m_bConfirmed = true;
						}
						else
						{
							Singleton<CBattleSystem>.get_instance().TheMinimapSys.Switch(MinimapSys.EMapType.Skill, skillSlotType);
						}
					}
				}
			}
			if (battleFormScript != null && GameSettings.TheSkillCancleType == SkillCancleType.AreaCancle)
			{
				GameObject gameObject;
				if (skillSlotType != SkillSlotType.SLOT_SKILL_9)
				{
					gameObject = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCancleArea();
				}
				else
				{
					gameObject = Singleton<CBattleSystem>.GetInstance().FightForm.GetEquipSkillCancleArea();
				}
				if (gameObject != null)
				{
					gameObject.CustomSetActive(isSkillCanBeCancled);
				}
			}
		}

		public void DisableSkillCursor(CUIFormScript battleFormScript, SkillSlotType skillSlotType)
		{
			this.m_currentSkillIndicatorEnabled = false;
			this.m_currentSkillIndicatorJoystickEnabled = false;
			this.m_currentSkillIndicatorResponed = false;
			this.m_currentSkillTipsResponed = false;
			this.m_currentSkillIndicatorInCancelArea = false;
			DebugHelper.Assert(battleFormScript != null);
			if (battleFormScript != null)
			{
				this.DisableSkillJoystick(battleFormScript, this.m_currentSkillJoystickMode);
				if (GameSettings.TheSkillCancleType == SkillCancleType.AreaCancle)
				{
					GameObject gameObject;
					if (skillSlotType == SkillSlotType.SLOT_SKILL_9)
					{
						gameObject = Singleton<CBattleSystem>.GetInstance().FightForm.GetEquipSkillCancleArea();
					}
					else
					{
						gameObject = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCancleArea();
					}
					if (gameObject != null)
					{
						gameObject.CustomSetActive(false);
					}
				}
			}
		}

		public Vector2 MoveSkillCursor(CUIFormScript battleFormScript, GameObject skillJoystick, SkillSlotType skillSlotType, enSkillJoystickMode skillJoystickMode, ref Vector2 screenPosition, out bool skillCanceled)
		{
			skillCanceled = this.IsSkillCursorInCanceledArea(battleFormScript, ref screenPosition, skillSlotType);
			if (!this.m_currentSkillIndicatorJoystickEnabled)
			{
				return Vector2.zero;
			}
			if (!this.m_currentSkillIndicatorResponed && battleFormScript.ChangeScreenValueToForm((screenPosition - this.m_currentSkillDownScreenPosition).magnitude) > 15f)
			{
				this.m_currentSkillIndicatorResponed = true;
			}
			if (!this.m_currentSkillTipsResponed && battleFormScript.ChangeScreenValueToForm((screenPosition - this.m_currentSkillDownScreenPosition).magnitude) > 30f)
			{
				this.m_currentSkillTipsResponed = true;
			}
			if (!this.m_currentSkillIndicatorResponed)
			{
				return Vector2.zero;
			}
			Vector2 vector = screenPosition - this.m_currentSkillIndicatorScreenPosition;
			Vector2 vector2 = vector;
			float magnitude = vector.magnitude;
			float num = battleFormScript.ChangeScreenValueToForm(magnitude);
			vector2.x = battleFormScript.ChangeScreenValueToForm(vector.x);
			vector2.y = battleFormScript.ChangeScreenValueToForm(vector.y);
			if (num > 110f)
			{
				vector2 = vector2.normalized * 110f;
			}
			if (skillJoystick != null)
			{
				Transform transform = skillJoystick.transform.FindChild("Cursor");
				if (transform != null)
				{
					(transform as RectTransform).anchoredPosition = vector2;
				}
			}
			if (skillJoystickMode == enSkillJoystickMode.General)
			{
				Player hostPlayer = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer();
				if (hostPlayer != null && hostPlayer.Captain && hostPlayer.Captain.get_handle().SkillControl.SkillSlotArray[(int)skillSlotType].SkillObj.cfgData.bRangeAppointType == 3 && num < 30f)
				{
					return Vector2.zero;
				}
			}
			else if (CSkillButtonManager.IsSelectedTargetJoyStick(skillJoystickMode))
			{
				int selectedIndex = this.SkillJoystickSelectTarget(battleFormScript, skillJoystick, ref screenPosition);
				this.ChangeSkillJoystickSelectedTarget(battleFormScript, skillJoystick, selectedIndex);
			}
			return vector2 / 110f;
		}

		private void DisableSkillJoystick(CUIFormScript battleFormScript, enSkillJoystickMode skillJoystickMode)
		{
			if (battleFormScript == null)
			{
				return;
			}
			if (skillJoystickMode == enSkillJoystickMode.General)
			{
				GameObject skillCursor = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCursor(enSkillJoystickMode.General);
				if (skillCursor != null && skillCursor.activeSelf)
				{
					skillCursor.CustomSetActive(false);
					RectTransform rectTransform = skillCursor.transform.FindChild("Cursor") as RectTransform;
					if (rectTransform != null)
					{
						rectTransform.anchoredPosition = Vector2.zero;
					}
				}
			}
			else if (CSkillButtonManager.IsSelectedTargetJoyStick(skillJoystickMode))
			{
				GameObject skillCursor2 = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCursor(enSkillJoystickMode.SelectTarget);
				if (skillCursor2 != null && skillCursor2.activeSelf)
				{
					skillCursor2.CustomSetActive(false);
					RectTransform rectTransform2 = skillCursor2.transform.FindChild("Cursor") as RectTransform;
					if (rectTransform2 != null)
					{
						rectTransform2.anchoredPosition = Vector2.zero;
					}
				}
			}
		}

		private bool IsSkillCursorInCanceledArea(CUIFormScript battleFormScript, ref Vector2 screenPosition, SkillSlotType skillSlotType)
		{
			if (GameSettings.TheSkillCancleType == SkillCancleType.AreaCancle)
			{
				GameObject targetObj;
				if (skillSlotType != SkillSlotType.SLOT_SKILL_9)
				{
					targetObj = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCancleArea();
				}
				else
				{
					targetObj = Singleton<CBattleSystem>.GetInstance().FightForm.GetEquipSkillCancleArea();
				}
				return this.IsSkillCursorInTargetArea(battleFormScript, ref screenPosition, targetObj);
			}
			return battleFormScript.ChangeScreenValueToForm((screenPosition - this.m_currentSkillDownScreenPosition).magnitude) > 270f;
		}

		public bool IsSkillCursorInTargetArea(CUIFormScript battleFormScript, ref Vector2 screenPosition, GameObject targetObj)
		{
			DebugHelper.Assert(battleFormScript != null, "battleFormScript!=null");
			if (battleFormScript != null)
			{
				DebugHelper.Assert(targetObj != null && targetObj.transform is RectTransform, "targetObj != null && targetObj.transform is RectTransform");
				if (targetObj != null && targetObj.activeInHierarchy && targetObj.transform is RectTransform)
				{
					Vector2 vector = CUIUtility.WorldToScreenPoint(battleFormScript.GetCamera(), targetObj.transform.position);
					float num = battleFormScript.ChangeFormValueToScreen((targetObj.transform as RectTransform).sizeDelta.x);
					float num2 = battleFormScript.ChangeFormValueToScreen((targetObj.transform as RectTransform).sizeDelta.y);
					Rect rect = new Rect(vector.x - num / 2f, vector.y - num2 / 2f, num, num2);
					return rect.Contains(screenPosition);
				}
			}
			return false;
		}

		private int SkillJoystickSelectTarget(CUIFormScript battleFormScript, GameObject skillJoystick, ref Vector2 screenPosition)
		{
			Vector2 point = screenPosition - this.m_currentSkillIndicatorScreenPosition;
			if (battleFormScript == null || battleFormScript.ChangeScreenValueToForm(point.magnitude) > 270f)
			{
				return -1;
			}
			float radian = CSkillButtonManager.GetRadian(point);
			if (battleFormScript != null && skillJoystick != null)
			{
				CUIComponent component = skillJoystick.GetComponent<CUIComponent>();
				if (component != null && component.m_widgets != null && component.m_widgets.Length >= this.m_targetInfos.Length)
				{
					for (int i = 0; i < this.m_targetInfos.Length; i++)
					{
						GameObject gameObject = component.m_widgets[i];
						if (gameObject != null && gameObject.activeSelf && (false || (radian >= this.m_targetInfos[i].m_headAreaFan.m_minRadian && radian <= this.m_targetInfos[i].m_headAreaFan.m_maxRadian)))
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		private int GetCampHeroInfosIndexByObjId(uint uiObjId)
		{
			int result = -1;
			for (int i = 0; i < this.m_campHeroInfos.Length; i++)
			{
				if (this.m_campHeroInfos[i].m_objectID == uiObjId)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		public void MoveSkillCursorInScene(SkillSlotType skillSlotType, ref Vector2 cursor, bool isSkillCursorInCancelArea)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer != null && hostPlayer.Captain)
			{
				hostPlayer.Captain.get_handle().SkillControl.SelectSkillTarget(skillSlotType, cursor, isSkillCursorInCancelArea);
			}
		}

		private void ChangeSkillCursorBGSprite(CUIFormScript battleFormScript, GameObject skillJoystick, bool isSkillCursorInCancelArea)
		{
			if (skillJoystick != null)
			{
				Image component = skillJoystick.GetComponent<Image>();
				if (component != null)
				{
					component.color = CSkillButtonManager.GetCursorBGColor(isSkillCursorInCancelArea);
				}
			}
		}

		private void ResetSkillJoystickSelectedTarget(CUIFormScript battleFormScript)
		{
			this.m_currentSkillJoystickSelectedIndex = -1;
			this.SetSkillIndicatorSelectedTarget(this.m_currentSkillJoystickSelectedIndex);
			if (battleFormScript == null)
			{
				return;
			}
			GameObject skillCursor = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCursor(enSkillJoystickMode.SelectTarget);
			if (skillCursor != null)
			{
				CUIComponent component = skillCursor.GetComponent<CUIComponent>();
				if (component != null && component.m_widgets != null)
				{
					int num = 0;
					while ((long)num < (long)((ulong)this.m_CurTargtNum))
					{
						this.SetSkillJoystickTargetHead(battleFormScript, component.m_widgets[num], false);
						PoolObjHandle<ActorRoot> actor = Singleton<GameObjMgr>.GetInstance().GetActor(this.m_targetInfos[num].m_objectID);
						if (actor && actor.get_handle().ValueComponent != null)
						{
							float fHpRate = (float)actor.get_handle().ValueComponent.actorHp / (float)actor.get_handle().ValueComponent.actorHpTotal;
							this.SetJoystickHeroHpFill(component.m_widgets[num], fHpRate);
						}
						num++;
					}
				}
				Transform transform = skillCursor.transform.FindChild("HighLight");
				if (transform != null)
				{
					transform.gameObject.CustomSetActive(false);
				}
			}
		}

		private void ChangeSkillJoystickSelectedTarget(CUIFormScript battleFormScript, GameObject skillJoystick, int selectedIndex)
		{
			if (this.m_currentSkillJoystickSelectedIndex == selectedIndex)
			{
				return;
			}
			int currentSkillJoystickSelectedIndex = this.m_currentSkillJoystickSelectedIndex;
			this.m_currentSkillJoystickSelectedIndex = selectedIndex;
			this.SetSkillIndicatorSelectedTarget(this.m_currentSkillJoystickSelectedIndex);
			if (battleFormScript == null || skillJoystick == null)
			{
				return;
			}
			CUIComponent component = skillJoystick.GetComponent<CUIComponent>();
			if (component != null && component.m_widgets != null)
			{
				if (this.m_currentSkillJoystickSelectedIndex >= 0 && (long)this.m_currentSkillJoystickSelectedIndex < (long)((ulong)this.m_CurTargtNum))
				{
					this.SetSkillJoystickTargetHead(battleFormScript, component.m_widgets[this.m_currentSkillJoystickSelectedIndex], true);
					Transform transform = skillJoystick.transform.FindChild("HighLight");
					if (transform != null)
					{
						transform.gameObject.CustomSetActive(true);
						transform.eulerAngles = new Vector3(0f, 0f, (float)(45 * this.m_currentSkillJoystickSelectedIndex));
					}
				}
				else
				{
					Transform transform2 = skillJoystick.transform.FindChild("HighLight");
					if (transform2 != null)
					{
						transform2.gameObject.CustomSetActive(false);
					}
				}
				if (currentSkillJoystickSelectedIndex >= 0 && (long)currentSkillJoystickSelectedIndex < (long)((ulong)this.m_CurTargtNum))
				{
					this.SetSkillJoystickTargetHead(battleFormScript, component.m_widgets[currentSkillJoystickSelectedIndex], false);
				}
			}
		}

		private void SetSkillJoystickTargetHead(CUIFormScript battleFormScript, GameObject head, bool selected)
		{
			if (head != null)
			{
				head.transform.localScale = new Vector3((!selected) ? 1f : 1.3f, (!selected) ? 1f : 1.3f, (!selected) ? 1f : 1.3f);
			}
		}

		private void SetSkillIndicatorSelectedTarget(int index)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer != null && hostPlayer.Captain)
			{
				SkillSlot skillSlot = hostPlayer.Captain.get_handle().SkillControl.GetSkillSlot(this.m_currentSkillSlotType);
				if (skillSlot != null && skillSlot.skillIndicator != null)
				{
					if (index < 0 || index >= this.m_targetInfos.Length || this.m_targetInfos[index].m_objectID == 0u)
					{
						skillSlot.skillIndicator.SetUseSkillTarget(null);
					}
					else
					{
						PoolObjHandle<ActorRoot> actor = Singleton<GameObjMgr>.GetInstance().GetActor(this.m_targetInfos[this.m_currentSkillJoystickSelectedIndex].m_objectID);
						if (actor)
						{
							skillSlot.skillIndicator.SetUseSkillTarget(actor.get_handle());
						}
						else
						{
							skillSlot.skillIndicator.SetUseSkillTarget(null);
						}
					}
				}
			}
		}

		private void SetSkillBtnDragFlag(SkillSlotType slotType, bool bDrag)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			if (hostPlayer != null && hostPlayer.Captain)
			{
				SkillSlot skillSlot = hostPlayer.Captain.get_handle().SkillControl.GetSkillSlot(slotType);
				if (skillSlot != null && skillSlot.skillIndicator != null)
				{
					skillSlot.skillIndicator.SetSkillBtnDrag(bDrag);
				}
			}
		}

		public void ChangeSkill(SkillSlotType skillSlotType, ref ChangeSkillEventParam skillParam)
		{
			if (skillSlotType > SkillSlotType.SLOT_SKILL_0 && skillParam.skillID > 0)
			{
				SkillButton skillButton = this._skillButtons[(int)skillSlotType];
				if (skillButton != null)
				{
					skillButton.ChangeSkillIcon(skillParam.skillID);
				}
				this.SetComboEffect(skillSlotType, skillParam.changeTime, skillParam.changeTime);
			}
		}

		public void RecoverSkill(SkillSlotType skillSlotType, ref DefaultSkillEventParam skillParam)
		{
			if (skillSlotType > SkillSlotType.SLOT_SKILL_0 && skillParam.param > 0)
			{
				SkillButton skillButton = this._skillButtons[(int)skillSlotType];
				if (skillButton != null)
				{
					skillButton.ChangeSkillIcon(skillParam.param);
				}
				this.SetComboEffect(skillSlotType, 0, 0);
			}
		}

		private void SetComboEffect(SkillSlotType skillSlotType, int leftTime, int totalTime)
		{
			SkillButton button = this.GetButton(skillSlotType);
			if (button == null || null == button.m_button)
			{
				return;
			}
			button.effectTimeTotal = totalTime;
			button.effectTimeLeft = leftTime;
			GameObject gameObject = Utility.FindChildSafe(button.m_button, "Present/comboCD");
			if (gameObject)
			{
				if (button.effectTimeLeft > 0 && button.effectTimeTotal > 0)
				{
					gameObject.CustomSetActive(true);
					button.effectTimeImage = gameObject.GetComponent<Image>();
				}
				else
				{
					gameObject.CustomSetActive(false);
					button.effectTimeImage = null;
				}
			}
		}

		public void SetButtonCDStart(SkillSlotType skillSlotType)
		{
			if (skillSlotType == SkillSlotType.SLOT_SKILL_0)
			{
				return;
			}
			this.SetDisableButton(skillSlotType);
			SkillButton button = this.GetButton(skillSlotType);
			GameObject target = (button == null) ? null : button.GetAnimationCD();
			CUICommonSystem.PlayAnimation(target, enSkillButtonAnimationName.CD_Star.ToString(), false);
		}

		public void SetButtonCDOver(SkillSlotType skillSlotType, bool isPlayMusic = true)
		{
			if (skillSlotType == SkillSlotType.SLOT_SKILL_0)
			{
				return;
			}
			if (!this.SetEnableButton(skillSlotType))
			{
				return;
			}
			SkillButton button = this.GetButton(skillSlotType);
			GameObject target = (button == null) ? null : button.GetAnimationCD();
			CUICommonSystem.PlayAnimation(target, enSkillButtonAnimationName.CD_End.ToString(), false);
			if (isPlayMusic)
			{
				Singleton<CSoundManager>.GetInstance().PlayBattleSound2D("UI_prompt_jineng");
			}
		}

		public void UpdateButtonBeanNum(SkillSlotType skillSlotType, int value)
		{
			SkillButton button = this.GetButton(skillSlotType);
			if (value > 0)
			{
				this.SetEnableButton(skillSlotType);
			}
			else
			{
				this.SetEnergyDisableButton(skillSlotType);
			}
			if (button.m_beanText != null)
			{
				Text component = button.m_beanText.GetComponent<Text>();
				if (component != null)
				{
					component.text = SimpleNumericString.GetNumeric(value);
				}
				button.m_beanText.CustomSetActive(true);
			}
		}

		public void UpdateButtonCD(SkillSlotType skillSlotType, int cd)
		{
			SkillButton button = this.GetButton(skillSlotType);
			if (cd <= 0)
			{
				this.SetEnableButton(skillSlotType);
			}
			this.UpdateButtonCDText((button == null) ? null : button.m_button, (button == null) ? null : button.m_cdText, cd);
		}

		private void UpdateButtonCDText(GameObject button, GameObject cdText, int cd)
		{
			if (cdText != null)
			{
				if (cd <= 0)
				{
					cdText.CustomSetActive(false);
				}
				else
				{
					if (button && button.activeSelf)
					{
						cdText.CustomSetActive(true);
					}
					Text component = cdText.GetComponent<Text>();
					if (component != null)
					{
						component.text = SimpleNumericString.GetNumeric(Mathf.CeilToInt((float)(cd / 1000)) + 1);
					}
				}
			}
			if (button != null && cdText != null)
			{
				cdText.transform.position = button.transform.position;
			}
		}

		public void SetButtonHighLight(SkillSlotType skillSlotType, bool highLight)
		{
			SkillButton button = this.GetButton(skillSlotType);
			if (button != null && button.m_button != null)
			{
				this.SetButtonHighLight(button.m_button, highLight);
			}
		}

		public void SetButtonHighLight(GameObject button, bool highLight)
		{
			Transform transform = button.transform.FindChild("Present/highlighter");
			if (transform != null)
			{
				transform.gameObject.CustomSetActive(highLight);
			}
		}

		public void SetlearnBtnHighLight(GameObject learnBtn, bool highLight)
		{
			Transform transform = learnBtn.transform.FindChild("highlighter");
			if (transform != null)
			{
				transform.gameObject.CustomSetActive(highLight);
			}
		}

		public void SetButtonFlowLight(GameObject button, bool highLight)
		{
			Transform transform = button.transform.FindChild("Present/highlighter");
			if (transform != null)
			{
				transform.gameObject.CustomSetActive(highLight);
			}
		}

		public SkillSlotType GetCurSkillSlotType()
		{
			return this.m_currentSkillSlotType;
		}

		public void SetSkillIndicatorMode(enSkillIndicatorMode indicaMode)
		{
			this.m_skillIndicatorMode = indicaMode;
		}

		public void UpdateLogic(int delta)
		{
			for (int i = 0; i < this._skillButtons.Length; i++)
			{
				SkillButton skillButton = this._skillButtons[i];
				if (skillButton != null && null != skillButton.effectTimeImage)
				{
					skillButton.effectTimeLeft -= delta;
					if (skillButton.effectTimeLeft < 0)
					{
						skillButton.effectTimeLeft = 0;
					}
					skillButton.effectTimeImage.CustomFillAmount((float)skillButton.effectTimeLeft / (float)skillButton.effectTimeTotal);
					if (skillButton.effectTimeLeft <= 0)
					{
						skillButton.effectTimeTotal = 0;
						skillButton.effectTimeImage.gameObject.CustomSetActive(false);
						skillButton.effectTimeImage = null;
					}
				}
			}
		}

		public bool IsIndicatorInCancelArea()
		{
			return this.m_currentSkillIndicatorInCancelArea;
		}

		private void onActorDead(ref GameDeadEventParam prm)
		{
			PoolObjHandle<ActorRoot> captain = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain;
			if (captain == prm.src && (!captain.get_handle().TheStaticData.TheBaseAttribute.DeadControl || captain.get_handle().ActorControl.IsEnableReviveContext()))
			{
				for (int i = 0; i < 10; i++)
				{
					this.SetDisableButton((SkillSlotType)i);
				}
			}
		}

		private void onActorRevive(ref DefaultGameEventParam prm)
		{
			PoolObjHandle<ActorRoot> captain = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain;
			if (captain == prm.src && (!captain.get_handle().TheStaticData.TheBaseAttribute.DeadControl || captain.get_handle().ActorControl.IsEnableReviveContext()))
			{
				for (int i = 0; i < 10; i++)
				{
					this.SetEnableButton((SkillSlotType)i);
				}
			}
		}

		private void OnCaptainSwitched(ref DefaultGameEventParam prm)
		{
			Singleton<CBattleSystem>.GetInstance().FightForm.ResetSkillButtonManager(prm.src, false, SkillSlotType.SLOT_SKILL_COUNT);
			this.ResetPickHeroInfo(prm.src);
		}

		private void ResetPickHeroInfo(PoolObjHandle<ActorRoot> actor)
		{
			Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
			SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
			if (hostPlayer == null || curLvelContext == null)
			{
				return;
			}
			if (!curLvelContext.IsMobaMode())
			{
				ReadonlyContext<PoolObjHandle<ActorRoot>> allHeroes = hostPlayer.GetAllHeroes();
				int count = allHeroes.get_Count();
				int num = 0;
				if (count > 0)
				{
					for (int i = 0; i < count; i++)
					{
						if (!(allHeroes.get_Item(i) == actor))
						{
							this.m_campHeroInfos[num].m_headIconPath = CUIUtility.s_Sprite_Dynamic_BustCircle_Dir + CSkinInfo.GetHeroSkinPic((uint)allHeroes.get_Item(i).get_handle().TheActorMeta.ConfigId, 0u);
							this.m_campHeroInfos[num].m_objectID = allHeroes.get_Item(i).get_handle().ObjID;
							num++;
						}
					}
				}
			}
			this.m_currentSkillJoystickSelectedIndex = -1;
			this.ResetSkillTargetJoyStickHeadToCampHero();
		}

		public void SetCommonAtkBtnState(CommonAttactType byAtkType)
		{
			CUIFormScript cUIFormScript = (Singleton<CBattleSystem>.GetInstance().FightForm != null) ? Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillBtnFormScript() : null;
			if (cUIFormScript == null)
			{
				return;
			}
			GameObject widget = cUIFormScript.GetWidget(24);
			GameObject widget2 = cUIFormScript.GetWidget(9);
			if (widget == null || widget2 == null)
			{
				return;
			}
			CUIEventScript component = widget2.GetComponent<CUIEventScript>();
			if (component == null)
			{
				return;
			}
			if (byAtkType == CommonAttactType.Type1)
			{
				widget.CustomSetActive(false);
				widget2.CustomSetActive(false);
			}
			else if (byAtkType == CommonAttactType.Type2)
			{
				widget.CustomSetActive(true);
				widget2.CustomSetActive(true);
				bool selectTargetBtnState = false;
				SkillButton button = this.GetButton(SkillSlotType.SLOT_SKILL_0);
				if (button != null)
				{
					GameObject disableButton = button.GetDisableButton();
					if (disableButton != null)
					{
						selectTargetBtnState = disableButton.activeSelf;
					}
				}
				this.SetSelectTargetBtnState(selectTargetBtnState);
			}
			Singleton<EventRouter>.GetInstance().BroadCastEvent("CommonAttack_Type_Changed");
		}

		private void SetSelectTargetBtnState(bool bActive)
		{
			if (GameSettings.TheCommonAttackType != CommonAttactType.Type2)
			{
				return;
			}
			CUIFormScript cUIFormScript = (Singleton<CBattleSystem>.GetInstance().FightForm != null) ? Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillBtnFormScript() : null;
			if (cUIFormScript == null)
			{
				return;
			}
			GameObject widget = cUIFormScript.GetWidget(24);
			GameObject widget2 = cUIFormScript.GetWidget(9);
			if (widget == null || widget2 == null)
			{
				return;
			}
			Color color = CUIUtility.s_Color_Full;
			if (bActive)
			{
				color = CUIUtility.s_Color_DisableGray;
			}
			GameObject gameObject = widget2.transform.FindChild("disable").gameObject;
			if (gameObject)
			{
				gameObject.CustomSetActive(bActive);
			}
			GameObject gameObject2 = widget2.transform.FindChild("Present").gameObject;
			if (gameObject2 != null)
			{
				Image component = gameObject2.GetComponent<Image>();
				if (component != null)
				{
					component.color = color;
				}
			}
			gameObject = widget.transform.FindChild("disable").gameObject;
			if (gameObject)
			{
				gameObject.CustomSetActive(bActive);
			}
			gameObject2 = widget.transform.FindChild("Present").gameObject;
			if (gameObject2 != null)
			{
				Image component2 = gameObject2.GetComponent<Image>();
				if (component2 != null)
				{
					component2.color = color;
				}
			}
		}

		private void EnableLastHitBtn(bool _bEnable)
		{
			CUIFormScript skillBtnFormScript = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillBtnFormScript();
			if (skillBtnFormScript == null)
			{
				return;
			}
			GameObject widget = skillBtnFormScript.GetWidget(25);
			if (widget == null)
			{
				return;
			}
			Color color = (!_bEnable) ? CUIUtility.s_Color_DisableGray : CUIUtility.s_Color_Full;
			GameObject gameObject = Utility.FindChild(widget, "disable");
			if (gameObject != null)
			{
				gameObject.CustomSetActive(!_bEnable);
			}
			GameObject gameObject2 = Utility.FindChild(widget, "Present");
			if (gameObject2 != null)
			{
				Image component = gameObject2.GetComponent<Image>();
				if (component != null)
				{
					component.color = color;
				}
			}
		}

		public bool HasMapSlectTargetSkill(out SkillSlotType slotType)
		{
			for (int i = 0; i < (int)CSkillButtonManager.s_maxButtonCount; i++)
			{
				SkillSlotType skillSlotType = (SkillSlotType)i;
				enSkillJoystickMode skillJoystickMode = this.GetSkillJoystickMode(skillSlotType);
				if (skillJoystickMode == enSkillJoystickMode.MapSelect || skillJoystickMode == enSkillJoystickMode.MapSelectOther)
				{
					slotType = skillSlotType;
					return true;
				}
			}
			slotType = SkillSlotType.SLOT_SKILL_COUNT;
			return false;
		}

		private void SetJoystickHeroHpFill(GameObject head, float fHpRate)
		{
			if (head != null)
			{
				CUIComponent component = head.GetComponent<CUIComponent>();
				if (component != null && component.m_widgets != null && component.m_widgets.Length >= 2)
				{
					GameObject gameObject = component.m_widgets[0];
					GameObject gameObject2 = component.m_widgets[1];
					if (gameObject2 != null && gameObject != null)
					{
						Image component2 = gameObject.GetComponent<Image>();
						Image component3 = gameObject2.GetComponent<Image>();
						if (component3 != null)
						{
							float fillAmount = component3.fillAmount;
							if ((double)fHpRate < 0.3 && (double)fillAmount >= 0.3)
							{
								component3.SetSprite("UGUI/Sprite/Battle/LockEnemy/Battle_KillNotify_Red_ring", Singleton<CBattleSystem>.GetInstance().FormScript, true, false, false, false);
								if (component2)
								{
									component2.color = CUIUtility.s_Color_EnemyHero_Button_PINK;
								}
							}
							else if ((double)fHpRate >= 0.3 && (double)fillAmount < 0.3)
							{
								component3.SetSprite("UGUI/Sprite/Battle/LockEnemy/Battle_KillNotify_Blue_ring", Singleton<CBattleSystem>.GetInstance().FormScript, true, false, false, false);
								if (component2)
								{
									component2.color = CUIUtility.s_Color_White;
								}
							}
							else if (fHpRate <= 0f && fillAmount > 0f && component2)
							{
								component2.color = CUIUtility.s_Color_DisableGray;
							}
							component3.CustomFillAmount(fHpRate);
						}
					}
				}
			}
		}

		private void SetJoystickHeroHpFill(PoolObjHandle<ActorRoot> hero, float fHpRate)
		{
			if (!hero)
			{
				return;
			}
			GameObject skillCursor = Singleton<CBattleSystem>.GetInstance().FightForm.GetSkillCursor(enSkillJoystickMode.SelectTarget);
			if (skillCursor != null)
			{
				CUIComponent component = skillCursor.GetComponent<CUIComponent>();
				if (component != null && component.m_widgets != null && component.m_widgets.Length >= this.m_targetInfos.Length)
				{
					for (int i = 0; i < component.m_widgets.Length; i++)
					{
						if (i >= this.m_targetInfos.Length)
						{
							break;
						}
						if (this.m_targetInfos[i].m_objectID == hero.get_handle().ObjID)
						{
							this.SetJoystickHeroHpFill(component.m_widgets[i], fHpRate);
							break;
						}
					}
				}
			}
		}

		private void OnHeroHpChange(PoolObjHandle<ActorRoot> hero, int iCurVal, int iMaxVal)
		{
			if (!this.m_currentSkillIndicatorJoystickEnabled)
			{
				return;
			}
			if (!CSkillButtonManager.IsSelectedTargetJoyStick(this.m_currentSkillJoystickMode))
			{
				return;
			}
			if (!hero || !ActorHelper.IsCaptainActor(ref hero))
			{
				return;
			}
			float fHpRate = (float)iCurVal / (float)iMaxVal;
			this.SetJoystickHeroHpFill(hero, fHpRate);
		}

		private static bool IsSelectedTargetJoyStick(enSkillJoystickMode mode)
		{
			return mode == enSkillJoystickMode.SelectTarget || mode == enSkillJoystickMode.SelectNextSkillTarget;
		}

		public void SetSkillButtuonActive(SkillSlotType skillSlotType, bool active)
		{
			SkillButton skillButton = this._skillButtons[(int)skillSlotType];
			if (skillButton != null)
			{
				skillButton.m_button.CustomSetActive(active);
				skillButton.m_cdText.GetComponent<Text>().enabled = active;
			}
		}
	}
}
