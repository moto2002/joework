using Assets.Scripts.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.GameSystem
{
	public class UI3DEventMgr
	{
		public enum EventComType
		{
			Hero,
			Tower,
			Eye
		}

		private const float SkillEventSizeScale = 3f;

		private ListView<UI3DEventCom> m_evtComsHeros = new ListView<UI3DEventCom>();

		private ListView<UI3DEventCom> m_evtComsTowers = new ListView<UI3DEventCom>();

		private ListView<UI3DEventCom> m_evtComsEyes = new ListView<UI3DEventCom>();

		public int HeroComCount
		{
			get
			{
				return this.m_evtComsHeros.get_Count();
			}
		}

		public int TowerComCount
		{
			get
			{
				return this.m_evtComsTowers.get_Count();
			}
		}

		public int EyeComCount
		{
			get
			{
				return this.m_evtComsEyes.get_Count();
			}
		}

		public void Init()
		{
		}

		public void Clear()
		{
			this.m_evtComsHeros.Clear();
			this.m_evtComsTowers.Clear();
			this.m_evtComsEyes.Clear();
		}

		public void Register(UI3DEventCom com, bool bHero)
		{
			com.isDead = false;
			if (bHero)
			{
				if (!this.m_evtComsHeros.Contains(com))
				{
					this.m_evtComsHeros.Add(com);
				}
			}
			else if (!this.m_evtComsTowers.Contains(com))
			{
				this.m_evtComsTowers.Add(com);
			}
		}

		public void Register(UI3DEventCom com, UI3DEventMgr.EventComType comType)
		{
			com.isDead = false;
			switch (comType)
			{
			case UI3DEventMgr.EventComType.Hero:
				if (!this.m_evtComsHeros.Contains(com))
				{
					this.m_evtComsHeros.Add(com);
				}
				break;
			case UI3DEventMgr.EventComType.Tower:
				if (!this.m_evtComsTowers.Contains(com))
				{
					this.m_evtComsTowers.Add(com);
				}
				break;
			case UI3DEventMgr.EventComType.Eye:
				if (!this.m_evtComsEyes.Contains(com))
				{
					this.m_evtComsEyes.Add(com);
				}
				break;
			}
		}

		public void UnRegister(UI3DEventCom com)
		{
			this.m_evtComsHeros.Remove(com);
			this.m_evtComsTowers.Remove(com);
			this.m_evtComsEyes.Remove(com);
		}

		public bool HandleClickEvent(PointerEventData pointerEventData)
		{
			for (int i = 0; i < this.m_evtComsHeros.get_Count(); i++)
			{
				UI3DEventCom uI3DEventCom = this.m_evtComsHeros.get_Item(i);
				if (uI3DEventCom != null && !uI3DEventCom.isDead && uI3DEventCom.m_screenSize.Contains(pointerEventData.pressPosition))
				{
					this.DispatchEvent(uI3DEventCom, pointerEventData);
					return true;
				}
			}
			for (int j = 0; j < this.m_evtComsTowers.get_Count(); j++)
			{
				UI3DEventCom uI3DEventCom2 = this.m_evtComsTowers.get_Item(j);
				if (uI3DEventCom2 != null && !uI3DEventCom2.isDead && uI3DEventCom2.m_screenSize.Contains(pointerEventData.pressPosition))
				{
					this.DispatchEvent(uI3DEventCom2, pointerEventData);
					return true;
				}
			}
			return false;
		}

		public bool HandleSkillClickEvent(PointerEventData pointerEventData)
		{
			float num = 3.40282347E+38f;
			UI3DEventCom uI3DEventCom = null;
			for (int i = 0; i < this.m_evtComsTowers.get_Count(); i++)
			{
				UI3DEventCom uI3DEventCom2 = this.m_evtComsTowers.get_Item(i);
				Rect screenSize = uI3DEventCom2.m_screenSize;
				screenSize.size *= 3f;
				float num2 = (screenSize.size.x <= screenSize.size.y) ? screenSize.size.y : screenSize.size.x;
				num2 *= num2;
				if (uI3DEventCom2 != null)
				{
					float sqrMagnitude = (uI3DEventCom2.m_screenSize.center - pointerEventData.pressPosition).sqrMagnitude;
					if (num2 >= sqrMagnitude && sqrMagnitude < num)
					{
						num = sqrMagnitude;
						uI3DEventCom = uI3DEventCom2;
					}
				}
			}
			for (int j = 0; j < this.m_evtComsEyes.get_Count(); j++)
			{
				UI3DEventCom uI3DEventCom3 = this.m_evtComsEyes.get_Item(j);
				Rect screenSize2 = uI3DEventCom3.m_screenSize;
				screenSize2.size *= 3f;
				float num3 = (screenSize2.size.x <= screenSize2.size.y) ? screenSize2.size.y : screenSize2.size.x;
				num3 *= num3;
				if (uI3DEventCom3 != null)
				{
					float sqrMagnitude2 = (uI3DEventCom3.m_screenSize.center - pointerEventData.pressPosition).sqrMagnitude;
					if (num3 >= sqrMagnitude2 && sqrMagnitude2 < num)
					{
						num = sqrMagnitude2;
						uI3DEventCom = uI3DEventCom3;
					}
				}
			}
			if (uI3DEventCom != null)
			{
				this.DispatchEvent(uI3DEventCom, pointerEventData);
				return true;
			}
			return false;
		}

		private void DispatchEvent(UI3DEventCom eventScript, PointerEventData pointerEventData)
		{
			if (eventScript == null || pointerEventData == null)
			{
				return;
			}
			CUIEvent uIEvent = Singleton<CUIEventManager>.GetInstance().GetUIEvent();
			uIEvent.m_eventID = eventScript.m_eventID;
			uIEvent.m_eventParams = eventScript.m_eventParams;
			uIEvent.m_pointerEventData = pointerEventData;
			if (Singleton<CUIEventManager>.GetInstance() != null)
			{
				Singleton<CUIEventManager>.GetInstance().DispatchUIEvent(uIEvent);
			}
		}
	}
}
