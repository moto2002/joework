using Assets.Scripts.GameSystem;
using Assets.Scripts.Sound;
using Assets.Scripts.UI;
using System;
using UnityEngine;

namespace Assets.Scripts.Framework
{
	[GameState]
	public class LoginState : BaseState
	{
		public override void OnStateEnter()
		{
			Singleton<CUIManager>.GetInstance().CloseAllForm(null, true, true);
			Singleton<ResourceLoader>.GetInstance().LoadScene("EmptyScene", new ResourceLoader.LoadCompletedDelegate(this.OnLoginSceneCompleted));
			Singleton<CSoundManager>.CreateInstance();
		}

		private void OnLoginSceneCompleted()
		{
			Singleton<CSoundManager>.GetInstance().PostEvent("Login_Play", null);
			Singleton<CLoginSystem>.GetInstance().Draw();
			if (GameDataMgr.GetGlobeValue(244) == 1u && !DeviceCheckSys.CheckAvailMemory() && DeviceCheckSys.GetRecordCurMemNotEnoughPopTimes() < 3)
			{
				Singleton<CUIManager>.GetInstance().OpenMessageBox(Singleton<CTextManager>.GetInstance().GetText("CheckDevice_QuitGame_CurMemNotEnough"), false);
				DeviceCheckSys.RecordCurMemNotEnoughPopTimes();
			}
		}

		public override void OnStateLeave()
		{
			base.OnStateLeave();
			Singleton<CLoginSystem>.GetInstance().CloseLogin();
			Debug.Log("CloseLogin...");
			Singleton<CResourceManager>.GetInstance().RemoveCachedResources(new enResourceType[]
			{
				default(enResourceType),
				5,
				3,
				4,
				6
			});
		}
	}
}
