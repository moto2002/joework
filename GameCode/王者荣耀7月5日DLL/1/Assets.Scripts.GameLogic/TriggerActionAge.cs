using AGE;
using Assets.Scripts.Common;
using System;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
	public class TriggerActionAge : TriggerActionBase
	{
		private ListView<Action> m_duraActsPrivate = new ListView<Action>();

		public TriggerActionAge(TriggerActionWrapper inWrapper, int inTriggerId) : base(inWrapper, inTriggerId)
		{
		}

		private ListView<Action> PlayAgeActionPrivate(AreaEventTrigger.EActTiming inTiming, GameObject inSrc, GameObject inAtker)
		{
			return this.PlayAgeActionShared(inTiming, this.TimingActionsInter, new ActionStopDelegate(this.OnActionStopedPrivate), this.m_duraActsPrivate, inSrc, inAtker);
		}

		protected static Action PlayAgeActionShared(string inActionName, string inHelperName, GameObject inSrc, GameObject inAtker, int inHelperIndex = -1, ActionStopDelegate inCallback = null)
		{
			return DialogueProcessor.PlayAgeAction(inActionName, inHelperName, inSrc, inAtker, inCallback, inHelperIndex);
		}

		protected virtual ListView<Action> PlayAgeActionShared(AreaEventTrigger.EActTiming inTiming, AreaEventTrigger.STimingAction[] inTimingActs, ActionStopDelegate inCallback, ListView<Action> outDuraActs, GameObject inSrc, GameObject inAtker)
		{
			ListView<Action> listView = new ListView<Action>();
			for (int i = 0; i < inTimingActs.Length; i++)
			{
				AreaEventTrigger.STimingAction sTimingAction = inTimingActs[i];
				if (sTimingAction.Timing == inTiming)
				{
					ActionStopDelegate actionStopDelegate = null;
					if (inTiming == AreaEventTrigger.EActTiming.EnterDura)
					{
						actionStopDelegate = inCallback;
					}
					Action action = TriggerActionAge.PlayAgeActionShared(sTimingAction.ActionName, sTimingAction.HelperName, inSrc, inAtker, sTimingAction.HelperIndex, inCallback);
					if (action != null)
					{
						listView.Add(action);
						if (actionStopDelegate != null)
						{
							outDuraActs.Add(action);
						}
					}
				}
			}
			return listView;
		}

		public override RefParamOperator TriggerEnter(PoolObjHandle<ActorRoot> src, PoolObjHandle<ActorRoot> atker, ITrigger inTrigger)
		{
			GameObject inSrc = (!src) ? null : src.get_handle().gameObject;
			GameObject gameObject = (inTrigger == null) ? null : inTrigger.GetTriggerObj();
			if (!gameObject)
			{
				gameObject = ((!atker) ? null : atker.get_handle().gameObject);
			}
			this.PlayAgeActionPrivate(AreaEventTrigger.EActTiming.Enter, inSrc, gameObject);
			ListView<Action> value = this.PlayAgeActionPrivate(AreaEventTrigger.EActTiming.EnterDura, inSrc, gameObject);
			RefParamOperator refParamOperator = new RefParamOperator();
			refParamOperator.AddRefParam("TriggerActionAgeEnterDura", value);
			return refParamOperator;
		}

		public override void TriggerLeave(PoolObjHandle<ActorRoot> src, ITrigger inTrigger)
		{
			GameObject inSrc = (!src) ? null : src.get_handle().gameObject;
			GameObject inAtker = (inTrigger == null) ? null : inTrigger.GetTriggerObj();
			this.PlayAgeActionPrivate(AreaEventTrigger.EActTiming.Leave, inSrc, inAtker);
			AreaEventTrigger areaEventTrigger = inTrigger as AreaEventTrigger;
			if (areaEventTrigger != null)
			{
				RefParamOperator refParamOperator = areaEventTrigger._inActors.get_Item(src.get_handle().ObjID).refParams.get_Item(this);
				if (refParamOperator != null)
				{
					ListView<Action> refParamObject = refParamOperator.GetRefParamObject<ListView<Action>>("TriggerActionAgeEnterDura");
					if (refParamObject != null)
					{
						ListView<Action>.Enumerator enumerator = refParamObject.GetEnumerator();
						while (enumerator.MoveNext())
						{
							enumerator.get_Current().Stop(false);
						}
					}
				}
			}
		}

		public override void TriggerUpdate(PoolObjHandle<ActorRoot> src, PoolObjHandle<ActorRoot> atker, ITrigger inTrigger)
		{
			GameObject inSrc = (!src) ? null : src.get_handle().gameObject;
			GameObject inAtker = (inTrigger == null) ? null : inTrigger.GetTriggerObj();
			this.PlayAgeActionPrivate(AreaEventTrigger.EActTiming.Update, inSrc, inAtker);
		}

		private void OnActionStopedPrivate(ref PoolObjHandle<Action> action)
		{
			if (!action)
			{
				return;
			}
			action.get_handle().onActionStop -= new ActionStopDelegate(this.OnActionStopedPrivate);
			this.m_duraActsPrivate.Remove(action.get_handle());
		}
	}
}
