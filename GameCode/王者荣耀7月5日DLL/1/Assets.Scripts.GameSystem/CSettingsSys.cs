using Apollo;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic;
using Assets.Scripts.UI;
using CSProtocol;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameSystem
{
	internal class CSettingsSys : Singleton<CSettingsSys>
	{
		protected enum SettingType
		{
			Basic,
			Operation,
			SelectHero,
			VoiceSetting,
			NetAcc,
			KingTime,
			SecurePwd,
			ReplayKit,
			TypeCount
		}

		public static string SETTING_FORM = "UGUI/Form/System/Settings/Form_Settings.prefab";

		private readonly string PLAYCEHOLDER = "---";

		private CSettingsSys.SettingType[] _availableSettingTypes;

		private int _availableSettingTypesCnt;

		private CUIFormScript _form;

		private CUIListScript _tabList;

		private static Color s_sliderFillColor = new Color(0f, 0.784313738f, 1f, 1f);

		public override void Init()
		{
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OpenForm, new CUIEventManager.OnUIEventHandler(this.onOpenSetting));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_ReqLogout, new CUIEventManager.OnUIEventHandler(this.onReqLogout));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_ConfirmLogout, new CUIEventManager.OnUIEventHandler(this.onConfirmLogout));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_SettingTypeChange, new CUIEventManager.OnUIEventHandler(this.OnSettingTabChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnCloseSetting));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OpenNetworkAccelerator, new CUIEventManager.OnUIEventHandler(this.OnNetAccChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_AutomaticOpenNetworkAccelerator, new CUIEventManager.OnUIEventHandler(this.OnAutoNetAccChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_PrivacyPolicy, new CUIEventManager.OnUIEventHandler(this.OnClickPrivacyPolicy));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_TermOfService, new CUIEventManager.OnUIEventHandler(this.OnClickTermOfService));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Contract, new CUIEventManager.OnUIEventHandler(this.OnClickContract));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_UpdateTimer, new CUIEventManager.OnUIEventHandler(this.OnUpdateTimer));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_SurrenderCDReady, new CUIEventManager.OnUIEventHandler(this.OnSurrenderCDReady));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_ConfirmQuality_Accept, new CUIEventManager.OnUIEventHandler(this.onQualitySettingAccept));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_ConfirmQuality_Cancel, new CUIEventManager.OnUIEventHandler(this.onQualitySettingCancel));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_ClickMoveCameraGuide, new CUIEventManager.OnUIEventHandler(this.onClickMoveCameraGuide));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_ClickSkillCancleTypeHelp, new CUIEventManager.OnUIEventHandler(this.onClickSkillCancleTypeHelp));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnClickHeroInfoBarHelp, new CUIEventManager.OnUIEventHandler(this.onClickHeroInfoBarHelp));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnClickLastHitModeHelp, new CUIEventManager.OnUIEventHandler(this.onClickLastHitModeHelp));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnClickLockEnemyHeroHelp, new CUIEventManager.OnUIEventHandler(this.onClickLockEnemyHeroHelp));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnClickShowEnemyHeroHelp, new CUIEventManager.OnUIEventHandler(this.onClickShowEnemyHeroHelp));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnClickBPHeroSelectHelpBtn, new CUIEventManager.OnUIEventHandler(this.OnClickBPHeroSelectHelpBtn));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnSmartCastChange, new CUIEventManager.OnUIEventHandler(this.OnSmartCastChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnLunPanCastChange, new CUIEventManager.OnUIEventHandler(this.OnLunPanCastChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnPickNearestChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnPickNearestChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnPickMinHpChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnPickMinHpChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnCommonAttackType1Change, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnCommonAttackType1Change));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnCommonAttackType2Change, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnCommonAttackType2Change));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnSkillCanleType1Change, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnSkillCanleType1Change));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnSkillCanleType2Change, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnSkillCanleType2Change));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnJoyStickMoveChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnJoyStickMoveChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnJoyStickNoMoveChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnJoyStickNoMoveChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnRightJoyStickBtnLocChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnRightJoyStickBtnLocChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnRightJoyStickFingerLocChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnRightJoyStickFingerLocChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onLunpanSensitivityChange, new CUIEventManager.OnUIEventHandler(this.OnSensitivityChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Toggle_OnLockEnemyHeroChange, new CUIEventManager.OnUIEventHandler(this.OnLockEnemyHeroChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Toggle_OnShowEnemyHeroChange, new CUIEventManager.OnUIEventHandler(this.OnShowEnemyHeroChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnNoneLastHitModeChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnNoneLastHitModeChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnLastHitModeChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnLastHitModeChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Toggle_OnNotLockEnemyHeroChange, new CUIEventManager.OnUIEventHandler(this.OnNotLockEnemyHeroChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Toggle_OnNotShowEnemyHeroChange, new CUIEventManager.OnUIEventHandler(this.OnNotShowEnemyHeroChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_On3DTouchBarChange, new CUIEventManager.OnUIEventHandler(this.On3DTouchBarChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onModelLODChange, new CUIEventManager.OnUIEventHandler(this.OnModeLODChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onParticleLODChange, new CUIEventManager.OnUIEventHandler(this.OnParticleLODChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onSkillTipChange, new CUIEventManager.OnUIEventHandler(this.OnSkillTipChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onFpsChange, new CUIEventManager.OnUIEventHandler(this.OnFpsChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onInputChatChange, new CUIEventManager.OnUIEventHandler(this.OnInputChatChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onMoveCameraChange, new CUIEventManager.OnUIEventHandler(this.OnMoveCameraChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onHDBarChange, new CUIEventManager.OnUIEventHandler(this.OnHDBarChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_CameraHeight, new CUIEventManager.OnUIEventHandler(this.OnCameraHeightChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onCameraSensitivityChange, new CUIEventManager.OnUIEventHandler(this.OnCameraSensitivityChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_OnHeroInfoBarChange, new CUIEventManager.OnUIEventHandler(this.OnHeroInfoBarChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Battle_PauseMultiGame, new CUIEventManager.OnUIEventHandler(this.OnPauseMultiGame));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Toggle_OnClickHeroSelectSortTypeDefault, new CUIEventManager.OnUIEventHandler(this.OnClickHeroSelectSortTypeDefault));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Toggle_OnClickHeroSelectSortTypeProficiency, new CUIEventManager.OnUIEventHandler(this.OnClickHeroSelectSortTypeProficiency));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_3DTouchChange, new CUIEventManager.OnUIEventHandler(this.On3DTouchChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onMusicChange, new CUIEventManager.OnUIEventHandler(this.OnMiusicChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onSoundEffectChange, new CUIEventManager.OnUIEventHandler(this.OnSoundEffectChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onVoiceChange, new CUIEventManager.OnUIEventHandler(this.OnVoiceChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onVibrateChange, new CUIEventManager.OnUIEventHandler(this.OnVibrateChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onMusicSensitivityChange, new CUIEventManager.OnUIEventHandler(this.OnSensitivityChangeMusic));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onSoundSensitivityChange, new CUIEventManager.OnUIEventHandler(this.OnSensitivityChangeSound));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onVoiceSensitivityChange, new CUIEventManager.OnUIEventHandler(this.OnSensitivityChangeVoice));
			this._availableSettingTypes = new CSettingsSys.SettingType[8];
			this._availableSettingTypesCnt = 0;
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_ReplayKitCourse, new CUIEventManager.OnUIEventHandler(this.onClickReplayKitCourse));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_OnClickReplayKitHelp, new CUIEventManager.OnUIEventHandler(this.onClickReplayKitHelp));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onReplayKitEnableChange, new CUIEventManager.OnUIEventHandler(this.OnReplayKitEnableChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onReplayKitEnableAutoModeChange, new CUIEventManager.OnUIEventHandler(this.OnReplayKitEnableAutoModeChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_KingTimeCourse, new CUIEventManager.OnUIEventHandler(this.OnClickOBFormOpen));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onRecordKingTimeEnableChange, new CUIEventManager.OnUIEventHandler(this.OnRecordKingTimeEnableChange));
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Settings_Slider_onSecurePwdEnableChange, new CUIEventManager.OnUIEventHandler(this.OnSecurePwdEnableChange));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.SECURE_PWD_STATUS_CHANGE, new Action(this.OnSecurePwdStatusChange));
			Singleton<EventRouter>.GetInstance().AddEventHandler(EventID.SECURE_PWD_OP_CANCEL, new Action(this.OnSecurePwdStatusChange));
		}

		public override void UnInit()
		{
			base.UnInit();
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OpenForm, new CUIEventManager.OnUIEventHandler(this.onOpenSetting));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_ReqLogout, new CUIEventManager.OnUIEventHandler(this.onReqLogout));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_ConfirmLogout, new CUIEventManager.OnUIEventHandler(this.onConfirmLogout));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_SettingTypeChange, new CUIEventManager.OnUIEventHandler(this.OnSettingTabChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_CloseForm, new CUIEventManager.OnUIEventHandler(this.OnCloseSetting));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OpenNetworkAccelerator, new CUIEventManager.OnUIEventHandler(this.OnNetAccChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_AutomaticOpenNetworkAccelerator, new CUIEventManager.OnUIEventHandler(this.OnAutoNetAccChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_PrivacyPolicy, new CUIEventManager.OnUIEventHandler(this.OnClickPrivacyPolicy));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_TermOfService, new CUIEventManager.OnUIEventHandler(this.OnClickTermOfService));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Contract, new CUIEventManager.OnUIEventHandler(this.OnClickContract));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_UpdateTimer, new CUIEventManager.OnUIEventHandler(this.OnUpdateTimer));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_SurrenderCDReady, new CUIEventManager.OnUIEventHandler(this.OnSurrenderCDReady));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_ConfirmQuality_Accept, new CUIEventManager.OnUIEventHandler(this.onQualitySettingAccept));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_ConfirmQuality_Cancel, new CUIEventManager.OnUIEventHandler(this.onQualitySettingCancel));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_ClickMoveCameraGuide, new CUIEventManager.OnUIEventHandler(this.onClickMoveCameraGuide));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_ClickSkillCancleTypeHelp, new CUIEventManager.OnUIEventHandler(this.onClickSkillCancleTypeHelp));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnClickHeroInfoBarHelp, new CUIEventManager.OnUIEventHandler(this.onClickHeroInfoBarHelp));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnClickLastHitModeHelp, new CUIEventManager.OnUIEventHandler(this.onClickLastHitModeHelp));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnClickLockEnemyHeroHelp, new CUIEventManager.OnUIEventHandler(this.onClickLockEnemyHeroHelp));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnClickShowEnemyHeroHelp, new CUIEventManager.OnUIEventHandler(this.onClickShowEnemyHeroHelp));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnClickBPHeroSelectHelpBtn, new CUIEventManager.OnUIEventHandler(this.OnClickBPHeroSelectHelpBtn));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnSmartCastChange, new CUIEventManager.OnUIEventHandler(this.OnSmartCastChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnLunPanCastChange, new CUIEventManager.OnUIEventHandler(this.OnLunPanCastChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnPickNearestChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnPickNearestChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnPickMinHpChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnPickMinHpChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnCommonAttackType1Change, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnCommonAttackType1Change));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnCommonAttackType2Change, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnCommonAttackType2Change));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnSkillCanleType1Change, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnSkillCanleType1Change));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnSkillCanleType2Change, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnSkillCanleType2Change));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnJoyStickMoveChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnJoyStickMoveChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnJoyStickNoMoveChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnJoyStickNoMoveChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnRightJoyStickBtnLocChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnRightJoyStickBtnLocChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnRightJoyStickFingerLocChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnRightJoyStickFingerLocChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onLunpanSensitivityChange, new CUIEventManager.OnUIEventHandler(this.OnSensitivityChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Toggle_OnLockEnemyHeroChange, new CUIEventManager.OnUIEventHandler(this.OnLockEnemyHeroChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Toggle_OnShowEnemyHeroChange, new CUIEventManager.OnUIEventHandler(this.OnShowEnemyHeroChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnNoneLastHitModeChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnNoneLastHitModeChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnLastHitModeChange, new CUIEventManager.OnUIEventHandler(CSettingsSys.OnLastHitModeChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Toggle_OnNotLockEnemyHeroChange, new CUIEventManager.OnUIEventHandler(this.OnNotLockEnemyHeroChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Toggle_OnNotShowEnemyHeroChange, new CUIEventManager.OnUIEventHandler(this.OnNotShowEnemyHeroChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_On3DTouchBarChange, new CUIEventManager.OnUIEventHandler(this.On3DTouchBarChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onModelLODChange, new CUIEventManager.OnUIEventHandler(this.OnModeLODChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onParticleLODChange, new CUIEventManager.OnUIEventHandler(this.OnParticleLODChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onSkillTipChange, new CUIEventManager.OnUIEventHandler(this.OnSkillTipChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onFpsChange, new CUIEventManager.OnUIEventHandler(this.OnFpsChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onInputChatChange, new CUIEventManager.OnUIEventHandler(this.OnInputChatChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onMoveCameraChange, new CUIEventManager.OnUIEventHandler(this.OnMoveCameraChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onHDBarChange, new CUIEventManager.OnUIEventHandler(this.OnHDBarChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_CameraHeight, new CUIEventManager.OnUIEventHandler(this.OnCameraHeightChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onCameraSensitivityChange, new CUIEventManager.OnUIEventHandler(this.OnCameraSensitivityChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_OnHeroInfoBarChange, new CUIEventManager.OnUIEventHandler(this.OnHeroInfoBarChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Battle_PauseMultiGame, new CUIEventManager.OnUIEventHandler(this.OnPauseMultiGame));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Toggle_OnClickHeroSelectSortTypeDefault, new CUIEventManager.OnUIEventHandler(this.OnClickHeroSelectSortTypeDefault));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Toggle_OnClickHeroSelectSortTypeProficiency, new CUIEventManager.OnUIEventHandler(this.OnClickHeroSelectSortTypeProficiency));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_3DTouchChange, new CUIEventManager.OnUIEventHandler(this.On3DTouchChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onMusicChange, new CUIEventManager.OnUIEventHandler(this.OnMiusicChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onSoundEffectChange, new CUIEventManager.OnUIEventHandler(this.OnSoundEffectChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onVoiceChange, new CUIEventManager.OnUIEventHandler(this.OnVoiceChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onVibrateChange, new CUIEventManager.OnUIEventHandler(this.OnVibrateChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onMusicSensitivityChange, new CUIEventManager.OnUIEventHandler(this.OnSensitivityChangeMusic));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onSoundSensitivityChange, new CUIEventManager.OnUIEventHandler(this.OnSensitivityChangeSound));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onVoiceSensitivityChange, new CUIEventManager.OnUIEventHandler(this.OnSensitivityChangeVoice));
			this._availableSettingTypes = null;
			this._availableSettingTypesCnt = 0;
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_ReplayKitCourse, new CUIEventManager.OnUIEventHandler(this.onClickReplayKitCourse));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_OnClickReplayKitHelp, new CUIEventManager.OnUIEventHandler(this.onClickReplayKitHelp));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onReplayKitEnableChange, new CUIEventManager.OnUIEventHandler(this.OnReplayKitEnableChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onReplayKitEnableAutoModeChange, new CUIEventManager.OnUIEventHandler(this.OnReplayKitEnableAutoModeChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_KingTimeCourse, new CUIEventManager.OnUIEventHandler(this.OnClickOBFormOpen));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onRecordKingTimeEnableChange, new CUIEventManager.OnUIEventHandler(this.OnRecordKingTimeEnableChange));
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Settings_Slider_onSecurePwdEnableChange, new CUIEventManager.OnUIEventHandler(this.OnSecurePwdEnableChange));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler(EventID.SECURE_PWD_STATUS_CHANGE, new Action(this.OnSecurePwdStatusChange));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler(EventID.SECURE_PWD_OP_CANCEL, new Action(this.OnSecurePwdStatusChange));
		}

		private void InitWidget(string formPath)
		{
			this._form = Singleton<CUIManager>.GetInstance().OpenForm(formPath, false, true);
			bool isRuning = Singleton<BattleLogic>.get_instance().isRuning;
			this._form.m_formWidgets[55].CustomSetActive(!isRuning);
			this._form.m_formWidgets[56].CustomSetActive(isRuning);
			if (isRuning)
			{
				this._tabList = this._form.m_formWidgets[18].GetComponent<CUIListScript>();
			}
			else
			{
				this._tabList = this._form.m_formWidgets[3].GetComponent<CUIListScript>();
			}
			DebugHelper.Assert(this._tabList != null);
			this.SetAvailableTabs();
			DebugHelper.Assert(this._availableSettingTypesCnt != 0, "Available Setting Type Array's Length Is 0 ?!");
			this._tabList.SetElementAmount(this._availableSettingTypesCnt);
			for (int i = 0; i < this._availableSettingTypesCnt; i++)
			{
				CSettingsSys.SettingType settingType = this._availableSettingTypes[i];
				CUIListElementScript elemenet = this._tabList.GetElemenet(i);
				switch (settingType)
				{
				case CSettingsSys.SettingType.Basic:
					this.ChangeText(elemenet, "基础设置");
					Singleton<CUINewFlagSystem>.GetInstance().AddNewFlag(elemenet.gameObject, enNewFlagKey.New_BasicSettingTab_V2, enNewFlagPos.enTopRight, 1f, 0f, 0f, enNewFlagType.enRedDot);
					break;
				case CSettingsSys.SettingType.Operation:
					this.ChangeText(elemenet, "操作设置");
					Singleton<CUINewFlagSystem>.GetInstance().AddNewFlag(elemenet.gameObject, enNewFlagKey.New_OperationTab_V1, enNewFlagPos.enTopRight, 1f, 0f, 0f, enNewFlagType.enRedDot);
					break;
				case CSettingsSys.SettingType.SelectHero:
					this.ChangeText(elemenet, "选将设置");
					break;
				case CSettingsSys.SettingType.VoiceSetting:
					this.ChangeText(elemenet, "音效设置");
					break;
				case CSettingsSys.SettingType.NetAcc:
					this.ChangeText(elemenet, "网络加速");
					break;
				case CSettingsSys.SettingType.KingTime:
					this.ChangeText(elemenet, "录像设置");
					Singleton<CUINewFlagSystem>.GetInstance().AddNewFlag(elemenet.gameObject, enNewFlagKey.New_KingTimeTab_V2, enNewFlagPos.enTopRight, 1f, 0f, 0f, enNewFlagType.enRedDot);
					break;
				case CSettingsSys.SettingType.SecurePwd:
					this.ChangeText(elemenet, "二级密码");
					break;
				case CSettingsSys.SettingType.ReplayKit:
					this.ChangeText(elemenet, Singleton<CTextManager>.GetInstance().GetText("ReplayKit_Label_Name"));
					break;
				}
			}
			this._tabList.SelectElement(0, true);
			if (isRuning)
			{
				this._form.m_formWidgets[54].CustomSetActive(false);
				this._form.m_formWidgets[53].CustomSetActive(true);
				this.GetSliderBarScript(this._form.m_formWidgets[22]).value = (float)GameSettings.FpsShowType;
				this.GetSliderBarScript(this._form.m_formWidgets[42]).value = (float)GameSettings.TheCameraMoveType;
				this.GetSliderBarScript(this._form.m_formWidgets[66]).value = (float)((!GameSettings.EnableHeroInfo) ? 0 : 1);
				Singleton<CUINewFlagSystem>.GetInstance().AddNewFlag(this._form.m_formWidgets[66].transform.FindChild("Text").gameObject, enNewFlagKey.New_SettingShowTargetInfo_V1, enNewFlagPos.enTopRight, 0.8f, 6.6f, 9.5f, enNewFlagType.enNewFlag);
				this.UpdateCameraSensitivitySlider();
				SLevelContext curLvelContext = Singleton<BattleLogic>.get_instance().GetCurLvelContext();
				GameObject gameObject = this._form.m_formWidgets[0];
				if (gameObject != null && curLvelContext != null && curLvelContext.IsMultilModeWithWarmBattle())
				{
					gameObject.CustomSetActive(false);
				}
				else
				{
					gameObject.CustomSetActive(true);
				}
				this.ValidatePauseState();
				GameObject widget = this._form.GetWidget(26);
				if (widget == null)
				{
					return;
				}
				if (Singleton<CSurrenderSystem>.get_instance().CanSurrender())
				{
					widget.CustomSetActive(true);
					GameObject gameObject2 = widget;
					if (gameObject2 == null)
					{
						return;
					}
					Button component = gameObject2.GetComponent<Button>();
					if (component == null)
					{
						return;
					}
					GameObject gameObject3 = Utility.FindChild(gameObject2, "CountDown");
					if (gameObject3 == null)
					{
						return;
					}
					CUITimerScript componetInChild = Utility.GetComponetInChild<CUITimerScript>(gameObject3, "timerSurrender");
					if (componetInChild == null)
					{
						return;
					}
					uint num = 0u;
					if (Singleton<CSurrenderSystem>.get_instance().InSurrenderCD(out num))
					{
						gameObject3.CustomSetActive(true);
						CUICommonSystem.SetButtonEnable(component, false, false, true);
						componetInChild.SetTotalTime(num);
						componetInChild.ReStartTimer();
					}
					else
					{
						gameObject3.CustomSetActive(false);
						CUICommonSystem.SetButtonEnable(component, true, true, true);
					}
				}
				else
				{
					widget.CustomSetActive(false);
				}
			}
			else
			{
				this._form.m_formWidgets[54].CustomSetActive(true);
				this._form.m_formWidgets[53].CustomSetActive(false);
				this._form.m_formWidgets[69].CustomSetActive(false);
				this.SetHDBarShow();
				CUISliderEventScript sliderBarScript = this.GetSliderBarScript(this._form.m_formWidgets[6]);
				CUISliderEventScript sliderBarScript2 = this.GetSliderBarScript(this._form.m_formWidgets[7]);
				sliderBarScript.value = (float)(sliderBarScript.MaxValue - GameSettings.ModelLOD);
				sliderBarScript2.value = (float)(sliderBarScript2.MaxValue - GameSettings.ParticleLOD);
				this.GetSliderBarScript(this._form.m_formWidgets[49]).value = (float)((!GameSettings.EnableHDMode) ? 0 : 1);
				if (GameSettings.HeroSelectHeroViewSortType == CMallSortHelper.HeroViewSortType.Name)
				{
					this._form.m_formWidgets[70].GetComponent<Toggle>().isOn = true;
					this._form.m_formWidgets[71].GetComponent<Toggle>().isOn = false;
				}
				else
				{
					this._form.m_formWidgets[70].GetComponent<Toggle>().isOn = false;
					this._form.m_formWidgets[71].GetComponent<Toggle>().isOn = true;
				}
				Text component2 = this._form.m_formWidgets[9].transform.FindChild("Text").gameObject.GetComponent<Text>();
				ApolloAccountInfo accountInfo = Singleton<ApolloHelper>.GetInstance().GetAccountInfo(false);
				if (accountInfo != null)
				{
					if (accountInfo.get_Platform() == 2)
					{
						component2.text = Singleton<CTextManager>.GetInstance().GetText("Common_Login_QQ");
					}
					else if (accountInfo.get_Platform() == 1)
					{
						component2.text = Singleton<CTextManager>.GetInstance().GetText("Common_Login_Weixin");
					}
					else if (accountInfo.get_Platform() == 3)
					{
						component2.text = Singleton<CTextManager>.GetInstance().GetText("Common_Login_PC");
					}
					else if (accountInfo.get_Platform() == 5)
					{
						component2.text = Singleton<CTextManager>.GetInstance().GetText("Common_Login_Guest");
					}
				}
			}
			this.GetSliderBarScript(this._form.m_formWidgets[16]).value = (float)GameSettings.CameraHeight;
			this.GetSliderBarScript(this._form.m_formWidgets[8]).value = (float)((!GameSettings.EnableOutline) ? 0 : 1);
			this.GetSliderBarScript(this._form.m_formWidgets[38]).value = (float)GameSettings.InBattleInputChatEnable;
			this.GetSliderBarScript(this._form.m_formWidgets[82]).value = (float)((!GameSettings.Unity3DTouchEnable) ? 0 : 1);
			this.Set3DTouchBarShow();
			Singleton<CUINewFlagSystem>.GetInstance().AddNewFlag(this._form.m_formWidgets[80].transform.FindChild("Text").gameObject, enNewFlagKey.New_Setting_HeroSelectHeroSortType_V1, enNewFlagPos.enTopCenter, 0.8f, 80f, 10f, enNewFlagType.enNewFlag);
			if (GameSettings.TheCastType == CastType.LunPanCast)
			{
				this._form.m_formWidgets[11].GetComponent<Toggle>().isOn = false;
				this._form.m_formWidgets[12].GetComponent<Toggle>().isOn = true;
				this.LunPanSettingsStatusChange(true);
			}
			else
			{
				this._form.m_formWidgets[11].GetComponent<Toggle>().isOn = true;
				this._form.m_formWidgets[12].GetComponent<Toggle>().isOn = false;
				this.LunPanSettingsStatusChange(false);
			}
			this._form.m_formWidgets[14].GetComponent<Toggle>().isOn = (GameSettings.TheSelectType == SelectEnemyType.SelectNearest);
			this._form.m_formWidgets[15].GetComponent<Toggle>().isOn = (GameSettings.TheSelectType == SelectEnemyType.SelectLowHp);
			this._form.m_formWidgets[24].GetComponent<Toggle>().isOn = (GameSettings.TheCommonAttackType == CommonAttactType.Type1);
			this._form.m_formWidgets[25].GetComponent<Toggle>().isOn = (GameSettings.TheCommonAttackType == CommonAttactType.Type2);
			this._form.m_formWidgets[73].GetComponent<Toggle>().isOn = (GameSettings.TheLastHitMode == LastHitMode.None);
			this._form.m_formWidgets[74].GetComponent<Toggle>().isOn = (GameSettings.TheLastHitMode == LastHitMode.LastHit);
			this._form.m_formWidgets[20].GetComponent<Toggle>().isOn = (GameSettings.JoyStickShowType == 0);
			this._form.m_formWidgets[21].GetComponent<Toggle>().isOn = (GameSettings.JoyStickShowType == 1);
			this._form.m_formWidgets[39].GetComponent<Toggle>().isOn = (GameSettings.TheSkillCancleType == SkillCancleType.AreaCancle);
			this._form.m_formWidgets[40].GetComponent<Toggle>().isOn = (GameSettings.TheSkillCancleType == SkillCancleType.DisitanceCancle);
			this.GetSliderBarScript(this._form.m_formWidgets[13]).value = GameSettings.LunPanSensitivity;
			this._form.m_formWidgets[67].GetComponent<Toggle>().isOn = GameSettings.LunPanLockEnemyHeroMode;
			this._form.m_formWidgets[68].GetComponent<Toggle>().isOn = !GameSettings.LunPanLockEnemyHeroMode;
			this._form.m_formWidgets[75].GetComponent<Toggle>().isOn = GameSettings.ShowEnemyHeroHeadBtnMode;
			this._form.m_formWidgets[76].GetComponent<Toggle>().isOn = !GameSettings.ShowEnemyHeroHeadBtnMode;
			Singleton<CUINewFlagSystem>.GetInstance().AddNewFlag(this._form.m_formWidgets[79].transform.FindChild("Text").gameObject, enNewFlagKey.New_Setting_LockEnemyHero_V1, enNewFlagPos.enTopLeft, 0.8f, 122.5f, 6.6f, enNewFlagType.enNewFlag);
			Singleton<CUINewFlagSystem>.GetInstance().AddNewFlag(this._form.m_formWidgets[77].transform.FindChild("Text").gameObject, enNewFlagKey.New_Setting_ShowEnemyHeroBtn_V1, enNewFlagPos.enTopCenter, 0.8f, 83f, 6.6f, enNewFlagType.enNewFlag);
			Singleton<CUINewFlagSystem>.GetInstance().AddNewFlag(this._form.m_formWidgets[78].transform.FindChild("Text").gameObject, enNewFlagKey.New_Setting_LastHitMode_V1, enNewFlagPos.enTopCenter, 0.8f, 83f, 6.6f, enNewFlagType.enNewFlag);
			this.GetSliderBarScript(this._form.m_formWidgets[28]).value = GameSettings.MusicEffectLevel * 0.01f;
			this.GetSliderBarScript(this._form.m_formWidgets[29]).value = GameSettings.SoundEffectLevel * 0.01f;
			this.GetSliderBarScript(this._form.m_formWidgets[30]).value = GameSettings.VoiceEffectLevel * 0.01f;
			this.GetSliderBarScript(this._form.m_formWidgets[4]).value = (float)((!GameSettings.EnableMusic) ? 0 : 1);
			this.GetSliderBarScript(this._form.m_formWidgets[5]).value = (float)((!GameSettings.EnableSound) ? 0 : 1);
			this.GetSliderBarScript(this._form.m_formWidgets[23]).value = (float)((!GameSettings.EnableVoice) ? 0 : 1);
			this.GetSliderBarScript(this._form.m_formWidgets[41]).value = (float)((!GameSettings.EnableVibrate) ? 0 : 1);
			this.InitVoiceSetting();
			this.GetSliderBarScript(this._form.m_formWidgets[35]).value = (float)((!NetworkAccelerator.IsNetAccConfigOpen()) ? 0 : 1);
			this.GetSliderBarScript(this._form.m_formWidgets[36]).value = (float)((!NetworkAccelerator.IsAutoNetAccConfigOpen()) ? 0 : 1);
			CRoleInfo masterRoleInfo = Singleton<CRoleInfoManager>.GetInstance().GetMasterRoleInfo();
			if (masterRoleInfo != null)
			{
				this._form.m_formWidgets[88].GetComponent<Toggle>().isOn = !CLadderSystem.IsRecentUsedHeroMaskSet(ref masterRoleInfo.recentUseHero.dwCtrlMask, 1);
				this._form.m_formWidgets[89].GetComponent<Toggle>().isOn = CLadderSystem.IsRecentUsedHeroMaskSet(ref masterRoleInfo.recentUseHero.dwCtrlMask, 1);
			}
			if (CSysDynamicBlock.bLobbyEntryBlocked)
			{
				Transform transform = this._form.transform.FindChild("BasicSetting/BasicLobbyOnlyWidgets/Layout/Panelelem2/HelpMe");
				if (transform)
				{
					transform.gameObject.CustomSetActive(false);
				}
			}
		}

		private void UnInitWidget()
		{
			if (this._tabList != null)
			{
				Singleton<CUINewFlagSystem>.GetInstance().DelNewFlag(this._tabList.GetElemenet(0).gameObject, enNewFlagKey.New_BasicSettingTab_V2, true);
			}
			this._tabList = null;
			this._form = null;
		}

		private void InitRecorderWidget()
		{
			GameObject gameObject = this._form.m_formWidgets[57];
			if (gameObject != null)
			{
				if (!Singleton<CRecordUseSDK>.get_instance().GetRecorderGlobalCfgEnableFlag())
				{
					gameObject.CustomSetActive(false);
				}
				else
				{
					Transform transform = gameObject.transform.FindChild("Text");
					if (transform != null)
					{
						Text component = transform.gameObject.GetComponent<Text>();
						if (component != null)
						{
							component.text = Singleton<CTextManager>.GetInstance().GetText("RecordKingTimeName");
						}
					}
					Transform transform2 = gameObject.transform.FindChild("Desc");
					if (transform2 != null)
					{
						Text component2 = transform2.gameObject.GetComponent<Text>();
						if (component2 != null)
						{
							component2.text = Singleton<CTextManager>.GetInstance().GetText("RecordKingTimeDesc");
						}
					}
					CUISliderEventScript sliderBarScript = this.GetSliderBarScript(gameObject);
					int num = (!GameSettings.EnableKingTimeMode) ? 0 : 1;
					if (sliderBarScript != null && (int)sliderBarScript.value != num)
					{
						sliderBarScript.value = (float)num;
					}
					gameObject.CustomSetActive(true);
				}
			}
		}

		private void SetAvailableTabs()
		{
			this._availableSettingTypesCnt = 0;
			int num = 8;
			for (int i = 0; i < num; i++)
			{
				CSettingsSys.SettingType settingType = (CSettingsSys.SettingType)i;
				switch (settingType)
				{
				case CSettingsSys.SettingType.Basic:
				case CSettingsSys.SettingType.Operation:
				case CSettingsSys.SettingType.VoiceSetting:
					this._availableSettingTypes[this._availableSettingTypesCnt++] = settingType;
					break;
				case CSettingsSys.SettingType.SelectHero:
				case CSettingsSys.SettingType.SecurePwd:
					if (!Singleton<BattleLogic>.get_instance().isRuning)
					{
						this._availableSettingTypes[this._availableSettingTypesCnt++] = settingType;
					}
					break;
				case CSettingsSys.SettingType.NetAcc:
					if (NetworkAccelerator.SettingUIEnbaled)
					{
						this._availableSettingTypes[this._availableSettingTypesCnt++] = settingType;
					}
					break;
				case CSettingsSys.SettingType.KingTime:
					if (!Singleton<BattleLogic>.get_instance().isRuning && Singleton<CRecordUseSDK>.get_instance().GetRecorderGlobalCfgEnableFlag() && !CSysDynamicBlock.bLobbyEntryBlocked)
					{
						this._availableSettingTypes[this._availableSettingTypesCnt++] = settingType;
					}
					break;
				}
			}
		}

		protected void OnSettingTabChange(CUIEvent uiEvent)
		{
			if (this._form != null && this._tabList != null)
			{
				int selectedIndex = this._tabList.GetSelectedIndex();
				if (selectedIndex < 0 || selectedIndex >= this._availableSettingTypesCnt)
				{
					return;
				}
				CSettingsSys.SettingType settingType = this._availableSettingTypes[selectedIndex];
				if (settingType == CSettingsSys.SettingType.Basic)
				{
					this._form.m_formWidgets[1].CustomSetActive(true);
					this._form.m_formWidgets[2].CustomSetActive(false);
					this._form.m_formWidgets[27].CustomSetActive(false);
					this._form.m_formWidgets[37].CustomSetActive(false);
					this._form.m_formWidgets[43].CustomSetActive(false);
					this._form.m_formWidgets[60].CustomSetActive(false);
					this._form.m_formWidgets[81].CustomSetActive(false);
					this._form.m_formWidgets[83].CustomSetActive(false);
					this.InitBasic();
				}
				else if (settingType == CSettingsSys.SettingType.Operation)
				{
					this._form.m_formWidgets[1].CustomSetActive(false);
					this._form.m_formWidgets[2].CustomSetActive(true);
					this._form.m_formWidgets[27].CustomSetActive(false);
					this._form.m_formWidgets[37].CustomSetActive(false);
					this._form.m_formWidgets[43].CustomSetActive(false);
					this._form.m_formWidgets[60].CustomSetActive(false);
					this._form.m_formWidgets[81].CustomSetActive(false);
					this._form.m_formWidgets[83].CustomSetActive(false);
					this.InitOperation();
					Singleton<CUINewFlagSystem>.GetInstance().DelNewFlag(this._tabList.GetElemenet(selectedIndex).gameObject, enNewFlagKey.New_OperationTab_V1, true);
				}
				else if (settingType == CSettingsSys.SettingType.SelectHero)
				{
					this._form.m_formWidgets[1].CustomSetActive(false);
					this._form.m_formWidgets[2].CustomSetActive(false);
					this._form.m_formWidgets[27].CustomSetActive(false);
					this._form.m_formWidgets[37].CustomSetActive(false);
					this._form.m_formWidgets[43].CustomSetActive(false);
					this._form.m_formWidgets[60].CustomSetActive(false);
					this._form.m_formWidgets[81].CustomSetActive(true);
					this._form.m_formWidgets[83].CustomSetActive(false);
					this.InitSelectHero();
				}
				else if (settingType == CSettingsSys.SettingType.VoiceSetting)
				{
					this._form.m_formWidgets[1].CustomSetActive(false);
					this._form.m_formWidgets[2].CustomSetActive(false);
					this._form.m_formWidgets[27].CustomSetActive(true);
					this._form.m_formWidgets[37].CustomSetActive(false);
					this._form.m_formWidgets[43].CustomSetActive(false);
					this._form.m_formWidgets[60].CustomSetActive(false);
					this._form.m_formWidgets[81].CustomSetActive(false);
					this._form.m_formWidgets[83].CustomSetActive(false);
				}
				else if (settingType == CSettingsSys.SettingType.NetAcc)
				{
					this._form.m_formWidgets[1].CustomSetActive(false);
					this._form.m_formWidgets[2].CustomSetActive(false);
					this._form.m_formWidgets[27].CustomSetActive(false);
					this._form.m_formWidgets[37].CustomSetActive(true);
					this._form.m_formWidgets[43].CustomSetActive(false);
					this._form.m_formWidgets[60].CustomSetActive(false);
					this._form.m_formWidgets[81].CustomSetActive(false);
					this._form.m_formWidgets[83].CustomSetActive(false);
				}
				else if (settingType == CSettingsSys.SettingType.KingTime)
				{
					this._form.m_formWidgets[1].CustomSetActive(false);
					this._form.m_formWidgets[2].CustomSetActive(false);
					this._form.m_formWidgets[27].CustomSetActive(false);
					this._form.m_formWidgets[37].CustomSetActive(false);
					this._form.m_formWidgets[43].CustomSetActive(true);
					this._form.m_formWidgets[60].CustomSetActive(false);
					this._form.m_formWidgets[81].CustomSetActive(false);
					this._form.m_formWidgets[83].CustomSetActive(false);
					this.InitRecorderWidget();
					Singleton<CUINewFlagSystem>.GetInstance().DelNewFlag(this._tabList.GetElemenet(selectedIndex).gameObject, enNewFlagKey.New_KingTimeTab_V2, true);
					if (CSysDynamicBlock.bLobbyEntryBlocked)
					{
						Transform transform = this._form.m_formWidgets[43].transform.FindChild("Button_Course");
						if (transform)
						{
							transform.gameObject.CustomSetActive(false);
						}
					}
				}
				else if (settingType == CSettingsSys.SettingType.SecurePwd)
				{
					this._form.m_formWidgets[1].CustomSetActive(false);
					this._form.m_formWidgets[2].CustomSetActive(false);
					this._form.m_formWidgets[27].CustomSetActive(false);
					this._form.m_formWidgets[37].CustomSetActive(false);
					this._form.m_formWidgets[43].CustomSetActive(false);
					this._form.m_formWidgets[60].CustomSetActive(true);
					this._form.m_formWidgets[81].CustomSetActive(false);
					this._form.m_formWidgets[83].CustomSetActive(false);
					this.InitSecurePwdSetting();
				}
				else if (settingType == CSettingsSys.SettingType.ReplayKit)
				{
					this._form.m_formWidgets[1].CustomSetActive(false);
					this._form.m_formWidgets[2].CustomSetActive(false);
					this._form.m_formWidgets[27].CustomSetActive(false);
					this._form.m_formWidgets[37].CustomSetActive(false);
					this._form.m_formWidgets[43].CustomSetActive(false);
					this._form.m_formWidgets[60].CustomSetActive(false);
					this._form.m_formWidgets[81].CustomSetActive(false);
					this._form.m_formWidgets[83].CustomSetActive(true);
					this.InitReplayKitSetting();
				}
			}
		}

		private void InitBasic()
		{
			if (Singleton<BattleLogic>.GetInstance().isRuning)
			{
				Singleton<CUINewFlagSystem>.GetInstance().DelNewFlag(this._form.m_formWidgets[66].transform.FindChild("Text").gameObject, enNewFlagKey.New_SettingShowTargetInfo_V1, false);
			}
		}

		private void InitOperation()
		{
			Singleton<CUINewFlagSystem>.GetInstance().DelNewFlag(this._form.m_formWidgets[79].transform.FindChild("Text").gameObject, enNewFlagKey.New_Setting_LockEnemyHero_V1, false);
			Singleton<CUINewFlagSystem>.GetInstance().DelNewFlag(this._form.m_formWidgets[77].transform.FindChild("Text").gameObject, enNewFlagKey.New_Setting_ShowEnemyHeroBtn_V1, false);
			Singleton<CUINewFlagSystem>.GetInstance().DelNewFlag(this._form.m_formWidgets[78].transform.FindChild("Text").gameObject, enNewFlagKey.New_Setting_LastHitMode_V1, false);
		}

		private void InitSelectHero()
		{
			Singleton<CUINewFlagSystem>.GetInstance().DelNewFlag(this._form.m_formWidgets[80].transform.FindChild("Text").gameObject, enNewFlagKey.New_Setting_HeroSelectHeroSortType_V1, false);
		}

		private void UpdateCameraSensitivitySlider()
		{
			float curCameraSensitivity = GameSettings.GetCurCameraSensitivity();
			if (curCameraSensitivity < 0f)
			{
				this.SliderStatusChange(false, null, SettingFormWidget.CameraSensitivity);
				this.GetSliderBarScript(this._form.m_formWidgets[48]).value = curCameraSensitivity;
			}
			else
			{
				this.SliderStatusChange(true, null, SettingFormWidget.CameraSensitivity);
				this.GetSliderBarScript(this._form.m_formWidgets[48]).value = curCameraSensitivity;
			}
		}

		private static void sliderMoveCameraAdjustment()
		{
			CSettingsSys instance = Singleton<CSettingsSys>.GetInstance();
			CUISliderEventScript cUISliderEventScript = (!(instance._form != null)) ? null : instance._form.m_formWidgets[42].transform.FindChild("Slider").GetComponent<CUISliderEventScript>();
			if (Singleton<BattleLogic>.get_instance().isRuning)
			{
				if (cUISliderEventScript != null && cUISliderEventScript.value == 1f)
				{
					CUIEvent uIEvent = Singleton<CUIEventManager>.GetInstance().GetUIEvent();
					uIEvent.m_eventID = enUIEventID.Settings_Slider_onMoveCameraChange;
					uIEvent.m_eventParams.sliderValue = 2f;
					Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(uIEvent);
				}
			}
			else if (GameSettings.TheCameraMoveType == CameraMoveType.JoyStick)
			{
				GameSettings.TheCameraMoveType = CameraMoveType.Slide;
			}
		}

		private void SetHDBarShow()
		{
			if (this._form == null)
			{
				return;
			}
			GameObject gameObject = this._form.m_formWidgets[49];
			bool flag = GameSettings.SupportHDMode();
			if (Singleton<BattleLogic>.HasInstance())
			{
				flag &= !Singleton<BattleLogic>.GetInstance().isRuning;
			}
			gameObject.CustomSetActive(flag);
			gameObject.transform.parent.gameObject.CustomSetActive(flag);
		}

		private void Set3DTouchBarShow()
		{
			if (this._form == null)
			{
				return;
			}
			GameObject obj = this._form.m_formWidgets[82];
			obj.CustomSetActive(GameSettings.Supported3DTouch);
		}

		private void InitVoiceSetting()
		{
			this.ShowSoundSettingLevel(GameSettings.EnableMusic, null, SettingFormWidget.MusicLevel);
			this.ShowSoundSettingLevel(GameSettings.EnableSound, null, SettingFormWidget.SoundEffectLevel);
			this.ShowSoundSettingLevel(GameSettings.EnableVoice, null, SettingFormWidget.VoiceEffectLevel);
		}

		private void InitReplayKitSetting()
		{
			GameObject gameObject = this._form.m_formWidgets[86];
			GameObject gameObject2 = this._form.m_formWidgets[87];
			Text text = null;
			if (gameObject != null)
			{
				text = gameObject.GetComponent<Text>();
			}
			Text text2 = null;
			if (gameObject2 != null)
			{
				text2 = gameObject2.GetComponent<Text>();
			}
			if (text != null)
			{
				text.text = Singleton<CTextManager>.GetInstance().GetText("ReplayKit_Enable_Desc");
			}
			if (text2 != null)
			{
				text2.text = Singleton<CTextManager>.GetInstance().GetText("ReplayKit_Mode_Desc");
			}
			this.GetSliderBarScript(this._form.m_formWidgets[84]).value = (float)((!GameSettings.EnableReplayKit) ? 0 : 1);
			this.GetSliderBarScript(this._form.m_formWidgets[85]).value = (float)((!GameSettings.EnableReplayKitAutoMode) ? 0 : 1);
		}

		private void InitSecurePwdSetting()
		{
			CSecurePwdSystem instance = Singleton<CSecurePwdSystem>.GetInstance();
			bool flag = instance.EnableStatus == PwdStatus.Enable;
			bool flag2 = instance.CloseStatus == PwdCloseStatus.Open;
			this.GetSliderBarScript(this._form.m_formWidgets[61]).value = (float)((!flag) ? 0 : 1);
			if (flag2)
			{
				this._form.m_formWidgets[62].CustomSetActive(false);
				this._form.m_formWidgets[63].CustomSetActive(true);
				CUITimerScript component = this._form.m_formWidgets[64].GetComponent<CUITimerScript>();
				if (component == null)
				{
					this._form.m_formWidgets[63].CustomSetActive(false);
					return;
				}
				int leftTime = Singleton<CTimerManager>.GetInstance().GetLeftTime(Singleton<CSecurePwdSystem>.GetInstance().CloseTimerSeq);
				if (leftTime > 0)
				{
					this._form.m_formWidgets[65].CustomSetActive(false);
					this._form.m_formWidgets[72].CustomSetActive(true);
					component.SetTotalTime((float)leftTime);
					component.ReStartTimer();
				}
				else
				{
					this._form.m_formWidgets[63].CustomSetActive(false);
					this._form.m_formWidgets[65].CustomSetActive(true);
					this._form.m_formWidgets[72].CustomSetActive(false);
				}
			}
			else
			{
				if (flag)
				{
					this._form.m_formWidgets[62].CustomSetActive(true);
					this._form.m_formWidgets[65].CustomSetActive(false);
					this._form.m_formWidgets[72].CustomSetActive(true);
				}
				else
				{
					this._form.m_formWidgets[62].CustomSetActive(false);
					this._form.m_formWidgets[65].CustomSetActive(true);
					this._form.m_formWidgets[72].CustomSetActive(false);
				}
				this._form.m_formWidgets[63].CustomSetActive(false);
			}
		}

		private void ChangeText(CUIListElementScript element, string InText)
		{
			DebugHelper.Assert(element != null);
			if (element != null)
			{
				Transform transform = element.gameObject.transform;
				DebugHelper.Assert(transform != null);
				Transform transform2 = transform.FindChild("Text");
				DebugHelper.Assert(transform2 != null);
				Text text = (!(transform2 != null)) ? null : transform2.GetComponent<Text>();
				DebugHelper.Assert(text != null);
				if (text != null)
				{
					text.text = InText;
				}
			}
		}

		private void LunPanSettingsStatusChange(bool bEnable)
		{
			this.SliderStatusChange(bEnable, null, SettingFormWidget.OpLunPanSensi);
			this.ToggleStatusChange(bEnable, SettingFormWidget.RightJoyStickBtnLoc);
			this.ToggleStatusChange(bEnable, SettingFormWidget.RightJoyStickFingerLoc);
			this.ToggleStatusChange(bEnable, SettingFormWidget.LunPanLock);
			this.ToggleStatusChange(bEnable, SettingFormWidget.EnemyHeroLock);
		}

		private void ShowSoundSettingLevel(bool bEnable, Slider soundObj, SettingFormWidget widgetEnum)
		{
			if (bEnable)
			{
				if (soundObj == null && this._form != null)
				{
					soundObj = this._form.m_formWidgets[(int)widgetEnum].transform.FindChild("Slider").gameObject.GetComponent<Slider>();
				}
				if (soundObj)
				{
					soundObj.interactable = true;
					Transform transform = soundObj.transform.Find("Background");
					if (transform)
					{
						Image component = transform.GetComponent<Image>();
						if (component)
						{
							component.color = CUIUtility.s_Color_White;
						}
					}
					transform = soundObj.transform.Find("Handle Slide Area/Handle");
					if (transform)
					{
						Image component2 = transform.GetComponent<Image>();
						if (component2)
						{
							component2.color = CUIUtility.s_Color_White;
						}
					}
					transform = soundObj.transform.FindChild("Fill Area/Fill");
					if (transform)
					{
						Image component3 = transform.GetComponent<Image>();
						if (component3)
						{
							component3.color = CSettingsSys.s_sliderFillColor;
						}
					}
				}
			}
			else
			{
				if (soundObj == null && this._form != null)
				{
					soundObj = this._form.m_formWidgets[(int)widgetEnum].transform.FindChild("Slider").gameObject.GetComponent<Slider>();
				}
				if (soundObj)
				{
					soundObj.interactable = false;
					Transform transform2 = soundObj.transform.Find("Background");
					if (transform2)
					{
						Image component4 = transform2.GetComponent<Image>();
						if (component4)
						{
							component4.color = CUIUtility.s_Color_GrayShader;
						}
					}
					transform2 = soundObj.transform.Find("Handle Slide Area/Handle");
					if (transform2)
					{
						Image component5 = transform2.GetComponent<Image>();
						if (component5)
						{
							component5.color = CUIUtility.s_Color_GrayShader;
						}
					}
					transform2 = soundObj.transform.FindChild("Fill Area/Fill");
					if (transform2)
					{
						Image component6 = transform2.GetComponent<Image>();
						if (component6)
						{
							component6.color = CUIUtility.s_Color_Grey;
						}
					}
				}
			}
		}

		private void ChangeImageColor(Transform imageTransform, Color color)
		{
			if (imageTransform)
			{
				Image component = imageTransform.GetComponent<Image>();
				if (component)
				{
					component.color = color;
				}
			}
		}

		private void SliderStatusChange(bool bEnable, Slider slliderObj, SettingFormWidget widgetEnum)
		{
			if (slliderObj == null && this._form != null)
			{
				slliderObj = this._form.m_formWidgets[(int)widgetEnum].transform.FindChild("Slider").gameObject.GetComponent<Slider>();
			}
			if (slliderObj)
			{
				slliderObj.interactable = bEnable;
				Color color = (!bEnable) ? CUIUtility.s_Color_GrayShader : CUIUtility.s_Color_White;
				Transform imageTransform = slliderObj.transform.Find("Background");
				this.ChangeImageColor(imageTransform, color);
				imageTransform = slliderObj.transform.Find("Handle Slide Area/Handle");
				this.ChangeImageColor(imageTransform, color);
				imageTransform = slliderObj.transform.Find("Fill Area/Fill");
				this.ChangeImageColor(imageTransform, (!bEnable) ? CUIUtility.s_Color_Grey : CSettingsSys.s_sliderFillColor);
			}
		}

		private void ToggleStatusChange(bool bEnable, SettingFormWidget widgetEnum)
		{
			Toggle component = this._form.m_formWidgets[(int)widgetEnum].GetComponent<Toggle>();
			if (component)
			{
				component.interactable = bEnable;
				Color color = (!bEnable) ? new Color(0f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 1f);
				Transform imageTransform = component.transform.Find("Background/Bg");
				this.ChangeImageColor(imageTransform, color);
				imageTransform = component.transform.Find("Background/Checkmark");
				this.ChangeImageColor(imageTransform, color);
			}
		}

		private CUISliderEventScript GetSliderBarScript(GameObject bar)
		{
			Transform transform = bar.transform.FindChild("Slider");
			if (transform)
			{
				return transform.GetComponent<CUISliderEventScript>();
			}
			return bar.GetComponent<CUISliderEventScript>();
		}

		private void OnSmartCastChange(CUIEvent uiEvent)
		{
			if (!uiEvent.m_eventParams.togleIsOn)
			{
				return;
			}
			this.LunPanSettingsStatusChange(false);
			GameSettings.TheCastType = CastType.SmartCast;
		}

		private void OnLunPanCastChange(CUIEvent uiEvent)
		{
			if (!uiEvent.m_eventParams.togleIsOn)
			{
				return;
			}
			this.LunPanSettingsStatusChange(true);
			GameSettings.TheCastType = CastType.LunPanCast;
		}

		private void OnLockEnemyHeroChange(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.LunPanLockEnemyHeroMode = true;
			}
		}

		private void OnNotLockEnemyHeroChange(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.LunPanLockEnemyHeroMode = false;
			}
		}

		private void OnShowEnemyHeroChange(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.ShowEnemyHeroHeadBtnMode = true;
				if (Singleton<CBattleSystem>.get_instance().FightForm != null && Singleton<CBattleSystem>.get_instance().FightForm.m_enemyHeroAtkBtn != null)
				{
					Singleton<CBattleSystem>.get_instance().FightForm.m_enemyHeroAtkBtn.ShowEnemyHeroHeadBtn();
				}
			}
		}

		private void OnNotShowEnemyHeroChange(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.ShowEnemyHeroHeadBtnMode = false;
				if (Singleton<CBattleSystem>.get_instance().FightForm != null && Singleton<CBattleSystem>.get_instance().FightForm.m_enemyHeroAtkBtn != null)
				{
					Singleton<CBattleSystem>.get_instance().FightForm.m_enemyHeroAtkBtn.HideEnemyHeroHeadBtn();
				}
			}
		}

		private static void OnPickNearestChange(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.TheSelectType = SelectEnemyType.SelectNearest;
			}
		}

		private static void OnPickMinHpChange(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.TheSelectType = SelectEnemyType.SelectLowHp;
			}
		}

		private static void OnNoneLastHitModeChange(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.TheLastHitMode = LastHitMode.None;
			}
		}

		private static void OnLastHitModeChange(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.TheLastHitMode = LastHitMode.LastHit;
			}
		}

		private static void OnCommonAttackType1Change(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.TheCommonAttackType = CommonAttactType.Type1;
			}
		}

		private static void OnCommonAttackType2Change(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				CSettingsSys.sliderMoveCameraAdjustment();
				GameSettings.TheCommonAttackType = CommonAttactType.Type2;
			}
		}

		private static void OnSkillCanleType1Change(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.TheSkillCancleType = SkillCancleType.AreaCancle;
			}
		}

		private static void OnSkillCanleType2Change(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.TheSkillCancleType = SkillCancleType.DisitanceCancle;
			}
		}

		private static void OnJoyStickMoveChange(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.JoyStickMoveType = 0;
			}
		}

		private static void OnJoyStickNoMoveChange(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.JoyStickMoveType = 1;
			}
		}

		private static void OnRightJoyStickBtnLocChange(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.JoyStickShowType = 0;
			}
		}

		private static void OnRightJoyStickFingerLocChange(CUIEvent uiEvent)
		{
			if (uiEvent.m_eventParams.togleIsOn)
			{
				GameSettings.JoyStickShowType = 1;
			}
		}

		private void OnMiusicChange(CUIEvent uiEvent)
		{
			GameSettings.EnableMusic = (uiEvent.m_eventParams.sliderValue == 1f);
			this.ShowSoundSettingLevel(GameSettings.EnableMusic, null, SettingFormWidget.MusicLevel);
		}

		private void OnSoundEffectChange(CUIEvent uiEvent)
		{
			GameSettings.EnableSound = (uiEvent.m_eventParams.sliderValue == 1f);
			this.ShowSoundSettingLevel(GameSettings.EnableSound, null, SettingFormWidget.SoundEffectLevel);
		}

		private void OnModeLODChange(CUIEvent uiEvent)
		{
			int num = Convert.ToInt32(uiEvent.m_eventParams.sliderValue);
			if ((uiEvent.m_srcWidgetScript as CUISliderEventScript).MaxValue - num < GameSettings.ModelLOD && PlayerPrefs.GetInt("degrade", 0) == 1)
			{
				stUIEventParams par = default(stUIEventParams);
				par.tag = 0;
				par.tag2 = num;
				Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(Singleton<CTextManager>.GetInstance().GetText("Setting_Quality_Confirm"), enUIEventID.Settings_ConfirmQuality_Accept, enUIEventID.Settings_ConfirmQuality_Cancel, par, false);
			}
			else
			{
				CUISliderEventScript exists = uiEvent.m_srcWidgetScript as CUISliderEventScript;
				if (exists)
				{
					GameSettings.ModelLOD = (uiEvent.m_srcWidgetScript as CUISliderEventScript).MaxValue - num;
				}
			}
		}

		private void OnParticleLODChange(CUIEvent uiEvent)
		{
			int num = Convert.ToInt32(uiEvent.m_eventParams.sliderValue);
			if ((uiEvent.m_srcWidgetScript as CUISliderEventScript).MaxValue - num < GameSettings.ParticleLOD && PlayerPrefs.GetInt("degrade", 0) == 1)
			{
				stUIEventParams par = default(stUIEventParams);
				par.tag = 1;
				par.tag2 = num;
				Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(Singleton<CTextManager>.GetInstance().GetText("Setting_Quality_Confirm"), enUIEventID.Settings_ConfirmQuality_Accept, enUIEventID.Settings_ConfirmQuality_Cancel, par, false);
			}
			else
			{
				CUISliderEventScript exists = uiEvent.m_srcWidgetScript as CUISliderEventScript;
				if (exists)
				{
					GameSettings.ParticleLOD = (uiEvent.m_srcWidgetScript as CUISliderEventScript).MaxValue - num;
				}
			}
		}

		private void OnSkillTipChange(CUIEvent uiEvent)
		{
			int num = (int)uiEvent.m_eventParams.sliderValue;
			if (num != 0 && !GameSettings.EnableOutline && PlayerPrefs.GetInt("degrade", 0) == 1)
			{
				stUIEventParams par = default(stUIEventParams);
				par.tag = 2;
				par.tag2 = num;
				Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(Singleton<CTextManager>.GetInstance().GetText("Setting_Quality_Confirm"), enUIEventID.Settings_ConfirmQuality_Accept, enUIEventID.Settings_ConfirmQuality_Cancel, par, false);
			}
			else
			{
				GameSettings.EnableOutline = (num != 0);
			}
		}

		private void OnFpsChange(CUIEvent uiEvent)
		{
			GameSettings.FpsShowType = (int)uiEvent.m_eventParams.sliderValue;
		}

		private void OnInputChatChange(CUIEvent uiEvent)
		{
			GameSettings.InBattleInputChatEnable = (int)uiEvent.m_eventParams.sliderValue;
		}

		private void OnVoiceChange(CUIEvent uiEvent)
		{
			GameSettings.EnableVoice = (uiEvent.m_eventParams.sliderValue == 1f);
			if (Singleton<CBattleSystem>.GetInstance().FightForm != null)
			{
				Singleton<CBattleSystem>.GetInstance().FightForm.ChangeSpeakerBtnState();
			}
			this.ShowSoundSettingLevel(GameSettings.EnableVoice, null, SettingFormWidget.VoiceEffectLevel);
		}

		private void OnVibrateChange(CUIEvent uiEvent)
		{
			GameSettings.EnableVibrate = (uiEvent.m_eventParams.sliderValue == 1f);
		}

		private void OnMoveCameraChange(CUIEvent uiEvent)
		{
			int num = (int)uiEvent.m_eventParams.sliderValue;
			if (GameSettings.TheCommonAttackType == CommonAttactType.Type2 && num == 1)
			{
				num = 2;
			}
			GameSettings.TheCameraMoveType = (CameraMoveType)num;
			this.UpdateCameraSensitivitySlider();
		}

		private void On3DTouchChange(CUIEvent uiEvent)
		{
			GameSettings.Unity3DTouchEnable = (uiEvent.m_eventParams.sliderValue == 1f);
		}

		private void OnReplayKitEnableAutoModeChange(CUIEvent uiEvent)
		{
			bool flag = uiEvent.m_eventParams.sliderValue == 1f;
			if (flag)
			{
				if (!Singleton<CReplayKitSys>.GetInstance().Enable || !Singleton<CReplayKitSys>.GetInstance().Cap || !GameSettings.EnableReplayKit)
				{
					this.GetSliderBarScript(this._form.m_formWidgets[85]).value = 0f;
					Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("ReplayKit_Enable_First"), false, 1.5f, null, new object[0]);
					return;
				}
				Singleton<CReplayKitSys>.GetInstance().CheckStorage(true);
			}
			GameSettings.EnableReplayKitAutoMode = (uiEvent.m_eventParams.sliderValue == 1f);
		}

		private void OnReplayKitEnableChange(CUIEvent uiEvent)
		{
			bool flag = uiEvent.m_eventParams.sliderValue == 1f;
			if (flag && (!Singleton<CReplayKitSys>.GetInstance().Enable || !Singleton<CReplayKitSys>.GetInstance().Cap))
			{
				this.GetSliderBarScript(this._form.m_formWidgets[84]).value = 0f;
				Singleton<CUIManager>.GetInstance().OpenTips(Singleton<CTextManager>.GetInstance().GetText("ReplayKit_Not_Support"), false, 1.5f, null, new object[0]);
				return;
			}
			CReplayKitSys.StorageStatus storageStatus = Singleton<CReplayKitSys>.GetInstance().CheckStorage(true);
			CReplayKitSys.StorageStatus storageStatus2 = storageStatus;
			if (storageStatus2 != CReplayKitSys.StorageStatus.Disable)
			{
				GameSettings.EnableReplayKit = flag;
				if (GameSettings.EnableReplayKit)
				{
					GameSettings.EnableKingTimeMode = false;
				}
				if (!GameSettings.EnableReplayKit)
				{
					this.GetSliderBarScript(this._form.m_formWidgets[85]).value = 0f;
				}
				return;
			}
			this.GetSliderBarScript(this._form.m_formWidgets[84]).value = 0f;
		}

		private void On3DTouchBarChange(CUIEvent uiEvent)
		{
			GameSettings.Unity3DTouchEnable = (uiEvent.m_eventParams.sliderValue == 1f);
		}

		private void OnSecurePwdEnableChange(CUIEvent uiEvent)
		{
			bool flag = uiEvent.m_eventParams.sliderValue == 1f;
			CSecurePwdSystem instance = Singleton<CSecurePwdSystem>.GetInstance();
			if (flag)
			{
				if (instance.EnableStatus == PwdStatus.Enable)
				{
					return;
				}
				Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.SecurePwd_OpenSetPwdForm);
			}
			else
			{
				if (instance.EnableStatus == PwdStatus.Disable)
				{
					return;
				}
				Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(enUIEventID.SecurePwd_OpenClosePwdForm);
			}
		}

		private void OnSecurePwdStatusChange()
		{
			if (this._form != null)
			{
				this.InitSecurePwdSetting();
			}
		}

		public void OnRecordKingTimeEnableChange(CUIEvent uiEvent)
		{
			bool flag = uiEvent.m_eventParams.sliderValue == 1f;
			bool enableKingTimeMode = GameSettings.EnableKingTimeMode;
			if (enableKingTimeMode != flag)
			{
				GameSettings.EnableKingTimeMode = flag;
				if (flag && !enableKingTimeMode)
				{
					if (GameSettings.EnableKingTimeMode)
					{
						GameSettings.EnableReplayKit = false;
					}
					Singleton<CRecordUseSDK>.get_instance().OpenRecorderCheck(this._form.m_formWidgets[57]);
				}
			}
		}

		private void OnHDBarChange(CUIEvent uiEvent)
		{
			GameSettings.EnableHDMode = (uiEvent.m_eventParams.sliderValue == 1f);
		}

		private void OnCameraHeightChange(CUIEvent uiEvent)
		{
			int cameraHeight = Convert.ToInt32(uiEvent.m_eventParams.sliderValue);
			GameSettings.CameraHeight = cameraHeight;
		}

		private void OnSensitivityChange(CUIEvent uiEvent)
		{
			GameSettings.LunPanSensitivity = uiEvent.m_eventParams.sliderValue;
		}

		private void OnCameraSensitivityChange(CUIEvent uiEvent)
		{
			GameSettings.SetCurCameraSensitivity(uiEvent.m_eventParams.sliderValue);
			this.UpdateCameraSensitivitySlider();
		}

		private void OnHeroInfoBarChange(CUIEvent uiEvent)
		{
			GameSettings.EnableHeroInfo = (uiEvent.m_eventParams.sliderValue == 1f);
		}

		private void OnPauseMultiGame(CUIEvent uiEvent)
		{
			PauseControl pauseControl = Singleton<CBattleSystem>.GetInstance().pauseControl;
			if (pauseControl != null)
			{
				pauseControl.RequestPause(true);
				this._form.Close();
			}
		}

		private void OnClickHeroSelectSortTypeDefault(CUIEvent uiEvent)
		{
			if (!uiEvent.m_eventParams.togleIsOn)
			{
				return;
			}
			GameSettings.HeroSelectHeroViewSortType = CMallSortHelper.HeroViewSortType.Name;
		}

		private void OnClickHeroSelectSortTypeProficiency(CUIEvent uiEvent)
		{
			if (!uiEvent.m_eventParams.togleIsOn)
			{
				return;
			}
			GameSettings.HeroSelectHeroViewSortType = CMallSortHelper.HeroViewSortType.Proficiency;
		}

		private void ValidatePauseState()
		{
			GameObject gameObject = this._form.m_formWidgets[69];
			if (gameObject)
			{
				PauseControl pauseControl = Singleton<CBattleSystem>.GetInstance().pauseControl;
				if (pauseControl != null)
				{
					bool enablePause = pauseControl.EnablePause;
					gameObject.CustomSetActive(enablePause);
					if (enablePause)
					{
						Text componetInChild = Utility.GetComponetInChild<Text>(gameObject, "Text");
						string text = Singleton<CTextManager>.GetInstance().GetText("PauseGame");
						if (pauseControl.MaxAllowTimes == 255)
						{
							componetInChild.text = text;
						}
						else
						{
							componetInChild.text = string.Concat(new object[]
							{
								text,
								"(",
								pauseControl.CurPauseTimes,
								"/",
								pauseControl.MaxAllowTimes,
								")"
							});
						}
						bool canPause = pauseControl.CanPause;
						CUICommonSystem.SetButtonEnable(gameObject.GetComponent<Button>(), canPause, canPause, true);
					}
				}
				else
				{
					gameObject.CustomSetActive(false);
				}
			}
		}

		private void OnSensitivityChangeMusic(CUIEvent uiEvent)
		{
			GameSettings.MusicEffectLevel = uiEvent.m_eventParams.sliderValue * 100f;
		}

		private void OnSensitivityChangeSound(CUIEvent uiEvent)
		{
			GameSettings.SoundEffectLevel = uiEvent.m_eventParams.sliderValue * 100f;
		}

		private void OnSensitivityChangeVoice(CUIEvent uiEvent)
		{
			GameSettings.VoiceEffectLevel = uiEvent.m_eventParams.sliderValue * 100f;
		}

		private void OnNetAccChange(CUIEvent uiEvent)
		{
			int num = Convert.ToInt32(uiEvent.m_eventParams.sliderValue);
			NetworkAccelerator.SetNetAccConfig(num > 0);
			this.SetUseACC();
		}

		private void SetUseACC()
		{
			if (NetworkAccelerator.IsNetAccConfigOpen() || NetworkAccelerator.IsAutoNetAccConfigOpen())
			{
				NetworkAccelerator.SetUseACC(true);
			}
			if (!NetworkAccelerator.IsNetAccConfigOpen() && !NetworkAccelerator.IsAutoNetAccConfigOpen())
			{
				NetworkAccelerator.SetUseACC(false);
			}
		}

		private void OnAutoNetAccChange(CUIEvent uiEvent)
		{
			int num = (int)uiEvent.m_eventParams.sliderValue;
			int num2 = (!NetworkAccelerator.IsAutoNetAccConfigOpen()) ? 0 : 1;
			NetworkAccelerator.SetAutoNetAccConfig(num > 0);
			if (num2 != num && NetworkAccelerator.IsAutoNetAccConfigOpen())
			{
				this.GetSliderBarScript(this._form.m_formWidgets[35]).value = 1f;
			}
			this.SetUseACC();
		}

		private void onQualitySettingAccept(CUIEvent uiEvent)
		{
			stUIEventParams eventParams = uiEvent.m_eventParams;
			switch (eventParams.tag)
			{
			case 0:
				GameSettings.ModelLOD = this.GetSliderBarScript(this._form.m_formWidgets[6]).MaxValue - eventParams.tag2;
				break;
			case 1:
				GameSettings.ParticleLOD = this.GetSliderBarScript(this._form.m_formWidgets[7]).MaxValue - eventParams.tag2;
				break;
			case 2:
				GameSettings.EnableOutline = (eventParams.tag2 != 0);
				break;
			}
			PlayerPrefs.SetInt("degrade", 0);
			PlayerPrefs.Save();
			Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("Force_Modify_Quality", null, true);
		}

		private void onQualitySettingCancel(CUIEvent uiEvent)
		{
			Singleton<ApolloHelper>.GetInstance().ApolloRepoertEvent("Give_Up_Modify_Quality", null, true);
			CUISliderEventScript sliderBarScript = this.GetSliderBarScript(this._form.m_formWidgets[6]);
			CUISliderEventScript sliderBarScript2 = this.GetSliderBarScript(this._form.m_formWidgets[7]);
			sliderBarScript.value = (float)(sliderBarScript.MaxValue - GameSettings.ModelLOD);
			sliderBarScript2.value = (float)(sliderBarScript2.MaxValue - GameSettings.ParticleLOD);
			this.GetSliderBarScript(this._form.m_formWidgets[8]).value = (float)((!GameSettings.EnableOutline) ? 0 : 1);
		}

		private void onOpenSetting(CUIEvent uiEvent)
		{
			this.InitWidget(CSettingsSys.SETTING_FORM);
			Singleton<CUINewFlagSystem>.get_instance().SetNewFlagForSettingEntry(false);
		}

		private void onReqLogout(CUIEvent uiEvent)
		{
			Singleton<CUIManager>.GetInstance().OpenMessageBoxWithCancel(Singleton<CTextManager>.GetInstance().GetText("Common_Exit_Tip"), enUIEventID.Settings_ConfirmLogout, enUIEventID.None, false);
		}

		private void onConfirmLogout(CUIEvent uiEvent)
		{
			CSPkg cSPkg = NetworkModule.CreateDefaultCSPKG(1016u);
			cSPkg.stPkgData.get_stGameLogoutReq().iLogoutType = 0;
			Singleton<NetworkModule>.GetInstance().SendLobbyMsg(ref cSPkg, false);
		}

		private void onClickMoveCameraGuide(CUIEvent uiEvent)
		{
			Singleton<CBattleGuideManager>.GetInstance().OpenBannerDlgByBannerGuideId(1u, null, false);
		}

		private void onClickSkillCancleTypeHelp(CUIEvent uiEvent)
		{
			Singleton<CBattleGuideManager>.GetInstance().OpenBannerDlgByBannerGuideId(6u, null, false);
		}

		private void onClickReplayKitHelp(CUIEvent uiEvent)
		{
			Singleton<CBattleGuideManager>.GetInstance().OpenBannerDlgByBannerGuideId(8u, null, false);
		}

		private void onClickHeroInfoBarHelp(CUIEvent uiEvent)
		{
			Singleton<CBattleGuideManager>.GetInstance().OpenBannerDlgByBannerGuideId(10u, null, false);
		}

		private void onClickKingTimeHelp(CUIEvent uiEvent)
		{
			Singleton<CBattleGuideManager>.GetInstance().OpenBannerDlgByBannerGuideId(8u, null, false);
		}

		private void onClickLastHitModeHelp(CUIEvent uiEvent)
		{
			Singleton<CBattleGuideManager>.GetInstance().OpenBannerDlgByBannerGuideId(12u, null, false);
		}

		private void onClickLockEnemyHeroHelp(CUIEvent uiEvent)
		{
			Singleton<CBattleGuideManager>.GetInstance().OpenBannerDlgByBannerGuideId(13u, null, false);
		}

		private void onClickShowEnemyHeroHelp(CUIEvent uiEvent)
		{
			Singleton<CBattleGuideManager>.GetInstance().OpenBannerDlgByBannerGuideId(14u, null, false);
		}

		private void OnClickBPHeroSelectHelpBtn(CUIEvent uiEvent)
		{
			Singleton<CBattleGuideManager>.GetInstance().OpenBannerDlgByBannerGuideId(17u, null, false);
		}

		protected void OnCloseSetting(CUIEvent uiEvent)
		{
			this._availableSettingTypesCnt = 0;
			this.UnInitWidget();
			GameSettings.Save();
		}

		private void OnClickPrivacyPolicy(CUIEvent uiEvent)
		{
			CUICommonSystem.OpenUrl("http://www.tencent.com/en-us/zc/privacypolicy.shtml", false, 0);
		}

		private void OnClickTermOfService(CUIEvent uiEvent)
		{
			CUICommonSystem.OpenUrl("http://www.tencent.com/en-us/zc/termsofservice.shtml", false, 0);
		}

		private void OnClickContract(CUIEvent uiEvent)
		{
			CUICommonSystem.OpenUrl("http://game.qq.com/contract.shtml", false, 0);
		}

		private void OnClickOBFormOpen(CUIEvent uiEvent)
		{
			if (this._form)
			{
				Singleton<CUIManager>.get_instance().CloseForm(this._form);
			}
			Singleton<COBSystem>.get_instance().OnOBFormOpen(uiEvent);
		}

		private void onClickReplayKitCourse(CUIEvent uiEvent)
		{
			string text = Singleton<CTextManager>.GetInstance().GetText("ReplayKit_Course_Link");
			if (!string.IsNullOrEmpty(text))
			{
				CUICommonSystem.OpenUrl(text, false, 0);
			}
		}

		private void OnSurrenderCDReady(CUIEvent uiEvent)
		{
			if (Singleton<BattleLogic>.get_instance().isRuning && this._form != null)
			{
				GameObject widget = this._form.GetWidget(26);
				if (widget == null)
				{
					return;
				}
				GameObject gameObject = widget;
				if (gameObject == null)
				{
					return;
				}
				Button component = gameObject.GetComponent<Button>();
				if (component == null)
				{
					return;
				}
				GameObject gameObject2 = Utility.FindChild(gameObject, "CountDown");
				if (gameObject2 == null)
				{
					return;
				}
				gameObject2.CustomSetActive(false);
				CUICommonSystem.SetButtonEnable(component, true, true, true);
			}
		}

		private void OnUpdateTimer(CUIEvent uiEvent)
		{
			if (this._form != null && NetworkAccelerator.SettingUIEnbaled)
			{
				int netType = NetworkAccelerator.GetNetType();
				string text = this.PLAYCEHOLDER;
				if (netType == 1)
				{
					text = "WIFI";
				}
				else if (netType == 2)
				{
					text = "2G";
				}
				else if (netType == 3)
				{
					text = "3G";
				}
				else if (netType == 4)
				{
					text = "4G";
				}
				this._form.m_formWidgets[33].GetComponent<Text>().text = text;
				SLevelContext curLvelContext = Singleton<BattleLogic>.GetInstance().GetCurLvelContext();
				if (curLvelContext == null || !curLvelContext.IsMobaMode())
				{
					if (NetworkAccelerator.started)
					{
						this._form.m_formWidgets[34].GetComponent<Text>().text = Singleton<CTextManager>.GetInstance().GetText("NETWORK_ACCELERATOR_LEBEL_NETWORK_DETAIL_ACC_STATE_UNKNOWN");
					}
					else
					{
						this._form.m_formWidgets[34].GetComponent<Text>().text = Singleton<CTextManager>.GetInstance().GetText("NETWORK_ACCELERATOR_LEBEL_NETWORK_DETAIL_ACC_STATE_CLOSE");
					}
					this._form.m_formWidgets[31].GetComponent<Text>().text = this.PLAYCEHOLDER;
				}
				else
				{
					if (Singleton<CBattleSystem>.GetInstance().FightForm != null)
					{
						this._form.m_formWidgets[31].GetComponent<Text>().text = Singleton<CBattleSystem>.GetInstance().FightForm.GetDisplayPing() + "ms";
					}
					if (!NetworkAccelerator.started)
					{
						this._form.m_formWidgets[34].GetComponent<Text>().text = Singleton<CTextManager>.GetInstance().GetText("NETWORK_ACCELERATOR_LEBEL_NETWORK_DETAIL_ACC_STATE_CLOSE");
					}
					else if (Singleton<FrameSynchr>.GetInstance().bActive && NetworkAccelerator.isAccerating())
					{
						this._form.m_formWidgets[34].GetComponent<Text>().text = Singleton<CTextManager>.GetInstance().GetText("NETWORK_ACCELERATOR_LEBEL_NETWORK_DETAIL_ACC_STATE_ACC");
					}
					else
					{
						this._form.m_formWidgets[34].GetComponent<Text>().text = Singleton<CTextManager>.GetInstance().GetText("NETWORK_ACCELERATOR_LEBEL_NETWORK_DETAIL_ACC_STATE_DIRECT");
					}
				}
			}
		}
	}
}
