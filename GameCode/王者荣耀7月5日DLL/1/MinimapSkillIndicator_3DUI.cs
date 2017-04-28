using Assets.Scripts.GameLogic;
using Assets.Scripts.GameLogic.GameKernal;
using Assets.Scripts.GameSystem;
using Assets.Scripts.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MinimapSkillIndicator_3DUI
{
	private GameObject mini_normalImgNode;

	private GameObject mini_redImgNode;

	private GameObject big_normalImgNode;

	private GameObject big_redImgNode;

	private Mask mini_maskCom;

	private Mask big_maskCom;

	private bool m_bEnable;

	private Vector2 m_dir = Vector2.zero;

	private Vector3 m_pos = Vector3.zero;

	private bool m_bDirDirty;

	private bool m_bPosDirty;

	public bool BInited
	{
		get;
		private set;
	}

	public static void InitIndicator(string normalImg, string redImg, float imgHeight, float bigImgHeight)
	{
		MinimapSys theMinimapSys = Singleton<CBattleSystem>.GetInstance().TheMinimapSys;
		if (theMinimapSys == null)
		{
			return;
		}
		if (theMinimapSys.MMinimapSkillIndicator_3Dui != null && !theMinimapSys.MMinimapSkillIndicator_3Dui.BInited)
		{
			theMinimapSys.MMinimapSkillIndicator_3Dui.SetInitData(normalImg, redImg, imgHeight, bigImgHeight);
		}
	}

	public static bool IsIndicatorInited()
	{
		MinimapSys theMinimapSys = Singleton<CBattleSystem>.GetInstance().TheMinimapSys;
		return theMinimapSys == null || theMinimapSys.MMinimapSkillIndicator_3Dui == null || theMinimapSys.MMinimapSkillIndicator_3Dui.BInited;
	}

	public static void UpdateIndicator(ref Vector2 dir, bool bSetEnable = false)
	{
		MinimapSys theMinimapSys = Singleton<CBattleSystem>.GetInstance().TheMinimapSys;
		if (theMinimapSys == null)
		{
			return;
		}
		if (theMinimapSys.MMinimapSkillIndicator_3Dui == null || !theMinimapSys.MMinimapSkillIndicator_3Dui.BInited)
		{
			return;
		}
		if (bSetEnable)
		{
			theMinimapSys.MMinimapSkillIndicator_3Dui.SetEnable(true, false);
		}
		theMinimapSys.MMinimapSkillIndicator_3Dui.Update(ref dir);
	}

	public static void SetIndicator(ref Vector3 forward, bool bSetEnable = false)
	{
		MinimapSys theMinimapSys = Singleton<CBattleSystem>.GetInstance().TheMinimapSys;
		if (theMinimapSys == null)
		{
			return;
		}
		if (theMinimapSys.MMinimapSkillIndicator_3Dui == null || !theMinimapSys.MMinimapSkillIndicator_3Dui.BInited)
		{
			return;
		}
		if (bSetEnable)
		{
			theMinimapSys.MMinimapSkillIndicator_3Dui.SetEnable(true, false);
		}
		theMinimapSys.MMinimapSkillIndicator_3Dui.SetIndicatorForward(ref forward, false);
	}

	public static void SetIndicatorColor(bool bNormal)
	{
		MinimapSys theMinimapSys = Singleton<CBattleSystem>.GetInstance().TheMinimapSys;
		if (theMinimapSys == null)
		{
			return;
		}
		if (theMinimapSys.MMinimapSkillIndicator_3Dui == null || !theMinimapSys.MMinimapSkillIndicator_3Dui.BInited)
		{
			return;
		}
		theMinimapSys.MMinimapSkillIndicator_3Dui.SetColor(bNormal);
	}

	public static void CancelIndicator()
	{
		MinimapSys theMinimapSys = Singleton<CBattleSystem>.GetInstance().TheMinimapSys;
		if (theMinimapSys == null)
		{
			return;
		}
		if (theMinimapSys.MMinimapSkillIndicator_3Dui == null || !theMinimapSys.MMinimapSkillIndicator_3Dui.BInited)
		{
			return;
		}
		theMinimapSys.MMinimapSkillIndicator_3Dui.SetEnable(false, true);
	}

	public void Clear()
	{
		this.mini_normalImgNode = (this.mini_redImgNode = null);
		this.big_normalImgNode = (this.big_redImgNode = null);
		this.mini_maskCom = (this.big_maskCom = null);
		this.BInited = false;
	}

	public void Init(GameObject miniTrackNode, GameObject bigTrackNode)
	{
		if (miniTrackNode == null)
		{
			return;
		}
		if (bigTrackNode == null)
		{
			return;
		}
		this.mini_normalImgNode = miniTrackNode.transform.Find("normal").gameObject;
		this.mini_redImgNode = miniTrackNode.transform.Find("red").gameObject;
		this.big_normalImgNode = bigTrackNode.transform.Find("normal").gameObject;
		this.big_redImgNode = bigTrackNode.transform.Find("red").gameObject;
		this.mini_maskCom = miniTrackNode.GetComponent<Mask>();
		this.big_maskCom = bigTrackNode.GetComponent<Mask>();
		this.SetEnable(false, true);
	}

	public void SetInitData(string normalImg, string redImg, float smallImgHeight, float bigImgHeight)
	{
		if (Singleton<CBattleSystem>.GetInstance().TheMinimapSys == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(normalImg) || string.IsNullOrEmpty(redImg) || smallImgHeight == 0f || bigImgHeight == 0f)
		{
			return;
		}
		this.mini_normalImgNode.GetComponent<Image>().SetSprite(normalImg, Singleton<CBattleSystem>.get_instance().FightFormScript, true, false, false, false);
		this.mini_redImgNode.GetComponent<Image>().SetSprite(redImg, Singleton<CBattleSystem>.get_instance().FightFormScript, true, false, false, false);
		this.big_normalImgNode.GetComponent<Image>().SetSprite(normalImg, Singleton<CBattleSystem>.get_instance().FightFormScript, true, false, false, false);
		this.big_redImgNode.GetComponent<Image>().SetSprite(redImg, Singleton<CBattleSystem>.get_instance().FightFormScript, true, false, false, false);
		this.SetWidthHeight(this.mini_normalImgNode, 400f, smallImgHeight);
		this.SetWidthHeight(this.mini_redImgNode, 400f, smallImgHeight);
		this.SetWidthHeight(this.big_normalImgNode, 800f, bigImgHeight);
		this.SetWidthHeight(this.big_redImgNode, 800f, bigImgHeight);
		this.BInited = true;
	}

	private void SetWidthHeight(GameObject obj, float width, float height)
	{
		if (obj == null)
		{
			return;
		}
		RectTransform rectTransform = obj.transform as RectTransform;
		if (rectTransform == null)
		{
			return;
		}
		rectTransform.sizeDelta = new Vector2(width, height);
	}

	public void SetEnable(bool bEnable, bool bForce = false)
	{
		if (bForce || (!bForce && this.m_bEnable != bEnable))
		{
			if (this.mini_maskCom != null)
			{
				this.mini_maskCom.enabled = bEnable;
			}
			if (this.big_maskCom != null)
			{
				this.big_maskCom.enabled = bEnable;
			}
			this.m_bEnable = bEnable;
			if (this.mini_normalImgNode != null)
			{
				this.mini_normalImgNode.transform.parent.gameObject.CustomSetActive(bEnable);
			}
			if (this.big_normalImgNode != null)
			{
				this.big_normalImgNode.transform.parent.gameObject.CustomSetActive(bEnable);
			}
		}
	}

	public void ForceUpdate()
	{
		this.m_dir = Vector2.zero;
		this.m_pos = Vector3.zero;
	}

	public void Update()
	{
		MinimapSys theMinimapSys = Singleton<CBattleSystem>.GetInstance().TheMinimapSys;
		if (theMinimapSys == null)
		{
			return;
		}
		Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
		if (hostPlayer == null || !hostPlayer.Captain)
		{
			return;
		}
		Vector3 vector = (Vector3)hostPlayer.Captain.get_handle().location;
		this.m_bPosDirty = (this.m_pos != vector);
		if (theMinimapSys.CurMapType() == MinimapSys.EMapType.Mini)
		{
			if (this.m_bPosDirty)
			{
				this.UpdatePosition(this.mini_normalImgNode, ref vector, true);
				this.UpdatePosition(this.mini_redImgNode, ref vector, true);
				this.m_pos = vector;
			}
		}
		else if (theMinimapSys.CurMapType() == MinimapSys.EMapType.Big && this.m_bPosDirty)
		{
			this.UpdatePosition(this.big_normalImgNode, ref vector, false);
			this.UpdatePosition(this.big_redImgNode, ref vector, false);
			this.m_pos = vector;
		}
	}

	public void SetIndicatorForward(ref Vector3 forward, bool bSetEnable = false)
	{
		float num = Mathf.Atan2(forward.z, forward.x) * 57.29578f;
		if (Singleton<BattleLogic>.get_instance().GetCurLvelContext().m_isCameraFlip)
		{
			num -= 180f;
		}
		Quaternion rotation = Quaternion.AngleAxis(num, Vector3.forward);
		if (this.mini_normalImgNode != null)
		{
			this.mini_normalImgNode.transform.rotation = rotation;
		}
		if (this.mini_redImgNode != null)
		{
			this.mini_redImgNode.transform.rotation = rotation;
		}
		if (this.big_normalImgNode != null)
		{
			this.big_normalImgNode.transform.rotation = rotation;
		}
		if (this.big_redImgNode != null)
		{
			this.big_redImgNode.transform.rotation = rotation;
		}
	}

	public void Update(ref Vector2 dir)
	{
		if (dir == Vector2.zero)
		{
			return;
		}
		MinimapSys theMinimapSys = Singleton<CBattleSystem>.GetInstance().TheMinimapSys;
		if (theMinimapSys == null)
		{
			return;
		}
		Player hostPlayer = Singleton<GamePlayerCenter>.GetInstance().GetHostPlayer();
		if (hostPlayer == null || !hostPlayer.Captain)
		{
			return;
		}
		this.m_bDirDirty = (this.m_dir != dir);
		Vector3 vector = (Vector3)hostPlayer.Captain.get_handle().location;
		this.m_bPosDirty = (this.m_pos != vector);
		bool bSmallMap = theMinimapSys.CurMapType() == MinimapSys.EMapType.Mini;
		if (this.m_bDirDirty)
		{
			this.UpdateRotation(this.mini_normalImgNode, ref dir);
			this.UpdateRotation(this.mini_redImgNode, ref dir);
			this.UpdateRotation(this.big_normalImgNode, ref dir);
			this.UpdateRotation(this.big_redImgNode, ref dir);
			this.m_dir = dir;
		}
		if (this.m_bPosDirty)
		{
			this.UpdatePosition(this.mini_normalImgNode, ref vector, bSmallMap);
			this.UpdatePosition(this.mini_redImgNode, ref vector, bSmallMap);
			this.UpdatePosition(this.big_normalImgNode, ref vector, bSmallMap);
			this.UpdatePosition(this.big_redImgNode, ref vector, bSmallMap);
			this.m_pos = vector;
		}
	}

	private void UpdateRotation(GameObject node, ref Vector2 dir)
	{
		float angle = Mathf.Atan2(dir.y, dir.x) * 57.29578f;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		node.transform.rotation = rotation;
	}

	private void UpdatePosition(GameObject node, ref Vector3 pos, bool bSmallMap)
	{
		if (node != null)
		{
			RectTransform rectTransform = node.transform as RectTransform;
			if (rectTransform != null)
			{
				if (bSmallMap)
				{
					rectTransform.anchoredPosition = new Vector2(pos.x * Singleton<CBattleSystem>.get_instance().world_UI_Factor_Small.x, pos.z * Singleton<CBattleSystem>.get_instance().world_UI_Factor_Small.y);
				}
				else
				{
					rectTransform.anchoredPosition = new Vector2(pos.x * Singleton<CBattleSystem>.get_instance().world_UI_Factor_Big.x, pos.z * Singleton<CBattleSystem>.get_instance().world_UI_Factor_Big.y);
				}
			}
		}
	}

	private void SetColor(bool bNormal)
	{
		if (Singleton<CBattleSystem>.GetInstance().TheMinimapSys == null)
		{
			return;
		}
		this.mini_normalImgNode.CustomSetActive(bNormal);
		this.mini_redImgNode.CustomSetActive(!bNormal);
		this.big_normalImgNode.CustomSetActive(bNormal);
		this.big_redImgNode.CustomSetActive(!bNormal);
	}
}
