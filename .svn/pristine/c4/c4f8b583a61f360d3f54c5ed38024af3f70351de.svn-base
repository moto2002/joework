using Assets.Scripts.Common;
using Assets.Scripts.Framework;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.UI;
using System;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public class HeroHeadHud : MonoBehaviour
	{
		public PlayerHead[] heroHeads;

		public Vector3 pickedScale = new Vector3(1.15f, 1.15f, 1.15f);

		public void Init()
		{
			ReadonlyContext<PoolObjHandle<ActorRoot>>.Enumerator enumerator = Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().GetAllHeroes().GetEnumerator();
			int num = -1;
			while (enumerator.MoveNext() && ++num < this.heroHeads.Length)
			{
				this.heroHeads[num].gameObject.CustomSetActive(true);
				this.heroHeads[num].Init(this, enumerator.get_Current());
				this.heroHeads[num].SetPicked(0 == num);
			}
			while (++num < this.heroHeads.Length)
			{
				this.heroHeads[num].gameObject.CustomSetActive(false);
			}
			Singleton<CUIEventManager>.GetInstance().AddUIEventListener(enUIEventID.Battle_PickHeroHead, new CUIEventManager.OnUIEventHandler(this.onClickHead));
			Singleton<GameEventSys>.get_instance().AddEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
			Singleton<GameEventSys>.get_instance().AddEventHandler<DefaultGameEventParam>(GameEventDef.Event_ActorRevive, new RefAction<DefaultGameEventParam>(this.OnActorRevive));
			Singleton<EventRouter>.GetInstance().AddEventHandler<PoolObjHandle<ActorRoot>, int, int>("HeroHpChange", new Action<PoolObjHandle<ActorRoot>, int, int>(this.OnHeroHpChange));
			Singleton<EventRouter>.GetInstance().AddEventHandler<PoolObjHandle<ActorRoot>, int>("HeroSoulLevelChange", new Action<PoolObjHandle<ActorRoot>, int>(this.OnHeroSoulLvlChange));
		}

		public void Clear()
		{
			Singleton<CUIEventManager>.GetInstance().RemoveUIEventListener(enUIEventID.Battle_PickHeroHead, new CUIEventManager.OnUIEventHandler(this.onClickHead));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<GameDeadEventParam>(GameEventDef.Event_ActorDead, new RefAction<GameDeadEventParam>(this.onActorDead));
			Singleton<GameEventSys>.get_instance().RmvEventHandler<DefaultGameEventParam>(GameEventDef.Event_ActorRevive, new RefAction<DefaultGameEventParam>(this.OnActorRevive));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<PoolObjHandle<ActorRoot>, int, int>("HeroHpChange", new Action<PoolObjHandle<ActorRoot>, int, int>(this.OnHeroHpChange));
			Singleton<EventRouter>.GetInstance().RemoveEventHandler<PoolObjHandle<ActorRoot>, int>("HeroSoulLevelChange", new Action<PoolObjHandle<ActorRoot>, int>(this.OnHeroSoulLvlChange));
		}

		private void onClickHead(CUIEvent evt)
		{
			PlayerHead component = evt.m_srcWidget.GetComponent<PlayerHead>();
			if (component.state == PlayerHead.HeadState.ReviveReady)
			{
				if (component.MyHero.get_handle().ActorControl.CanRevive)
				{
					component.MyHero.get_handle().ActorControl.Revive(false);
				}
			}
			else
			{
				this.pickHeroHead(component);
			}
		}

		public void pickHeroHead(PlayerHead ph)
		{
			if (ph.MyHero.get_handle() == null || Singleton<GamePlayerCenter>.get_instance().GetHostPlayer() == null || Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain.get_handle() == null)
			{
				return;
			}
			if (ph.MyHero == Singleton<GamePlayerCenter>.get_instance().GetHostPlayer().Captain)
			{
				return;
			}
			for (int i = 0; i < this.heroHeads.Length; i++)
			{
				PlayerHead playerHead = this.heroHeads[i];
				if (null == playerHead || !playerHead.MyHero)
				{
					break;
				}
				playerHead.SetPicked(ph == playerHead);
			}
			FrameCommand<SwitchCaptainCommand> frameCommand = FrameCommandFactory.CreateFrameCommand<SwitchCaptainCommand>();
			frameCommand.cmdData.ObjectID = ph.MyHero.get_handle().ObjID;
			frameCommand.Send();
		}

		public void onActorDead(ref GameDeadEventParam prm)
		{
			if (prm.src.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer().PlayerCamp == prm.src.get_handle().TheActorMeta.ActorCamp)
			{
				int num = -1;
				while (++num < this.heroHeads.Length)
				{
					PlayerHead playerHead = this.heroHeads[num];
					if (playerHead.MyHero && playerHead.MyHero == prm.src)
					{
						playerHead.OnMyHeroDead();
						break;
					}
				}
				if (num < this.heroHeads.Length && !Singleton<BattleLogic>.get_instance().GetCurLvelContext().IsMobaMode() && ActorHelper.IsCaptainActor(ref prm.src))
				{
					int num2 = -1;
					while (++num2 < this.heroHeads.Length)
					{
						if (num2 != num)
						{
							if (this.heroHeads[num2].MyHero && !this.heroHeads[num2].MyHero.get_handle().ActorControl.IsDeadState)
							{
								this.pickHeroHead(this.heroHeads[num2]);
								break;
							}
						}
					}
				}
			}
		}

		public void OnActorRevive(ref DefaultGameEventParam prm)
		{
			if (prm.src.get_handle().TheActorMeta.ActorType == ActorTypeDef.Actor_Type_Hero && Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer().PlayerCamp == prm.src.get_handle().TheActorMeta.ActorCamp)
			{
				for (int i = 0; i < this.heroHeads.Length; i++)
				{
					PlayerHead playerHead = this.heroHeads[i];
					if (playerHead.MyHero && playerHead.MyHero == prm.src)
					{
						playerHead.OnMyHeroRevive();
						break;
					}
				}
			}
		}

		public void OnHeroSoulLvlChange(PoolObjHandle<ActorRoot> hero, int level)
		{
			if (!hero)
			{
				return;
			}
			for (int i = 0; i < this.heroHeads.Length; i++)
			{
				PlayerHead playerHead = this.heroHeads[i];
				if (playerHead.MyHero && playerHead.MyHero == hero)
				{
					playerHead.OnHeroSoulLvlChange(level);
					break;
				}
			}
		}

		public void OnHeroHpChange(PoolObjHandle<ActorRoot> hero, int curVal, int maxVal)
		{
			if (!hero)
			{
				return;
			}
			for (int i = 0; i < this.heroHeads.Length; i++)
			{
				PlayerHead playerHead = this.heroHeads[i];
				if (playerHead.MyHero && playerHead.MyHero == hero)
				{
					playerHead.OnHeroHpChange(curVal, maxVal);
					break;
				}
			}
		}
	}
}
