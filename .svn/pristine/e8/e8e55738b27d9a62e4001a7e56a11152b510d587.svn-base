using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// author:zhouzhanglin
    /// 居中显示.
    /// 说明：如果要调用此类中的属性和方法.
    /// </summary>
    [AddComponentMenu("UI/Center View", 1351)]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CenterViewController))]
    public class CenterView : ScrollRect, IPointerUpHandler
    {
        //控制器.
        private CenterViewController m_controller;
        public CenterViewController controller { get { return m_controller; } }

        private bool m_drag = false;
        private Vector2 m_end = Vector2.zero;

		private float m_dragTime = 0;
        private int m_totalPage = 0;
        //从0开始.
        public int totalPage { get { return m_totalPage; } }

        private int m_currentPage = 0;
        //从0开始.
        public int currentPage { get { return m_currentPage; } }

        private bool _enableUpdate = false;

        private Vector2 m_contentPos = Vector2.zero;
        private ArrayList m_all = new ArrayList(); //用于排序.

        //当前居中显示的是哪个item
        private RectTransform m_centerItem=null;
        public RectTransform CenterItem
        {
            get { return m_centerItem; }
        }

        /// <summary>
        /// 初始化此控件.
        /// </summary>
        public CenterView Init()
        {
            if (horizontal)
            {
                content.pivot = new Vector2(0, 0.5f); //内容的原点在最左边.
            }
            else if (vertical)
            {
                content.pivot = new Vector2(0.5f, 1f); //内容的原点在最上边.
            }
            m_totalPage = content.childCount - 1;

            m_end = -((RectTransform)content.GetChild(0).transform).anchoredPosition;
            SetContentAnchoredPosition(m_end);
            m_controller = GetComponent<CenterViewController>();

            foreach (RectTransform child in content)
            {
                CenterViewItem item = child.GetComponent<CenterViewItem>();
                if (item == null)
                {
                    item = child.gameObject.AddComponent<CenterViewItem>();
                    item.index = child.GetSiblingIndex();
                }
                item.clickToCenter = controller.clickItemToCenter;
                m_all.Add(item);
            }
            m_all.Sort();
            for (int i = 0; i < m_totalPage + 1; i++)
            {
                CenterViewItem item = m_all[i] as CenterViewItem;
                ((RectTransform)item.transform).SetSiblingIndex(i);
            }
            m_all.Clear();

            if (horizontal)
            {
                _scaleXRenders();
            }
            else if (vertical)
            {
                _scaleYRenders();
            }

			LayoutGroup group = transform.GetComponentInChildren<LayoutGroup>();
			if(group){
				group.enabled = false;
			}
            return this;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (Input.touchCount > 1) return;
            if (!controller.dragEnable) return;
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            m_drag = true;
            _enableUpdate = true;
            //记录开始拖动时内容的位置和开始拖动时的时间戳.
            m_contentPos = content.anchoredPosition;
			m_dragTime = Time.realtimeSinceStartup;
            base.OnBeginDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (Input.touchCount > 1) return;
            if (controller.dragEnable)
                base.OnDrag(eventData);
        }

        override public void OnEndDrag(PointerEventData eventData)
        {
            if (Input.touchCount > 1) return;
            if (!controller.dragEnable) return;
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            m_drag = false;
            base.OnEndDrag(eventData);

            //遍历所有的，判断离哪个最近.
            if (horizontal)
            {
				if (Time.realtimeSinceStartup - m_dragTime < 0.4f)
                {
                    //时间很短.
                    m_end.x += (eventData.delta.x + content.anchoredPosition.x - m_contentPos.x)*2f;
                }
                else if (Mathf.Abs(eventData.delta.x) > 5f)
                {
                    m_end.x += eventData.delta.x + content.anchoredPosition.x - m_contentPos.x;
                }
                else
                {
                    m_end = content.anchoredPosition;
                }

                float minDistance = float.MaxValue;
                foreach (RectTransform child in content)
                {
                    float pos = child.anchoredPosition.x + m_end.x;
                    float dis = Mathf.Abs(pos);
                    if (dis < minDistance)
                    {
                        minDistance = dis;
                        m_centerItem = child;
                    }
                }
                if (m_centerItem)
                {
                    m_end.x = -m_centerItem.anchoredPosition.x;
                    if (Mathf.Abs(m_end.x- content.anchoredPosition.x) > 1f)
                    {
                        _enableUpdate = true;
                        m_currentPage = m_centerItem.GetComponent<CenterViewItem>().index;
                        m_controller.onPageChange.Invoke();
                        if (m_controller.pageIndicator)
                            m_controller.pageIndicator.ShowPage(m_currentPage);
                    }
                }
                
            }
            else if (vertical)
            {
				if (Time.realtimeSinceStartup - m_dragTime < 0.4f)
                {
                    //时间很短.
                    m_end.y += (eventData.delta.y + content.anchoredPosition.y - m_contentPos.y)*2f;
                }
                else if (Mathf.Abs(eventData.delta.y) > 5f)
                {
                    m_end.y += eventData.delta.y + content.anchoredPosition.y - m_contentPos.y;
                }
                else
                {
                    m_end = content.anchoredPosition;
                }

                float minDistance = float.MaxValue;
                foreach (RectTransform child in content)
                {
                    float pos = child.anchoredPosition.y + m_end.y;
                    float dis = Mathf.Abs(pos);
                    if (dis < minDistance)
                    {
                        minDistance = dis;
                        m_centerItem = child;
                    }
                }
                if (m_centerItem)
                {
                    m_end.y = -m_centerItem.anchoredPosition.y;
                    if (Mathf.Abs(m_end.y - content.anchoredPosition.y) > 1f)
                    {
                        _enableUpdate = true;
                        m_currentPage = m_centerItem.GetComponent<CenterViewItem>().index;
                        m_controller.onPageChange.Invoke();
                        if (m_controller.pageIndicator)
                            m_controller.pageIndicator.ShowPage(m_currentPage);
                    }
                }
            }
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            OnEndDrag(eventData);
        }

        /// <summary>
        /// 用来排序.
        /// </summary>
        class Comp : IComparable
        {
            public RectTransform rect;
            public int CompareTo(object other)
            {
                float result = rect.localScale.x*100f - (other as Comp).rect.localScale.x*100f;
                if (result > 0) return 1;
                else if (result < 0) return -1;
                return 0;
            }
        }

        private void _scaleXRenders()
        {
            //遍历所有的，判断离哪个最近.
            foreach (RectTransform child in content)
            {
                var posX = child.anchoredPosition.x + content.anchoredPosition.x;
                float sc =Mathf.Abs( Mathf.Sin((child.sizeDelta.x * 4f - Mathf.Abs(posX)) / child.sizeDelta.x / 4f) * controller.maxScale);
                if (posX > viewRect.sizeDelta.x / 2f || posX < -viewRect.sizeDelta.x / 2f)
                {
                    sc = controller.minScale;
                }
                child.localScale = new Vector2(sc, sc);
                Comp comp = new Comp();
                comp.rect = child;
                m_all.Add(comp);
            }
            m_all.Sort();
            for (int i = 0; i < m_totalPage + 1; i++)
            {
                Comp comp = m_all[i] as Comp;
                comp.rect.SetSiblingIndex(i);
            }
            m_all.Clear();
        }


        private void _scaleYRenders()
        {
            //遍历所有的，判断离哪个最近.
            foreach (RectTransform child in content)
            {
                var posY = child.anchoredPosition.y + content.anchoredPosition.y;
                float sc = Mathf.Abs(Mathf.Sin((child.sizeDelta.y * 4f - Mathf.Abs(posY)) / child.sizeDelta.y / 4f) * controller.maxScale);
                if (posY > viewRect.sizeDelta.y / 2f || posY < -viewRect.sizeDelta.y / 2f)
                {
                    sc = controller.minScale;
                }
                child.localScale = new Vector2(sc, sc);
                Comp comp = new Comp();
                comp.rect = child;
                m_all.Add(comp);
            }
            m_all.Sort();
            for (int i = 0; i < m_totalPage + 1; i++)
            {
                Comp comp = m_all[i] as Comp;
                comp.rect.SetSiblingIndex(i);
            }
            m_all.Clear();
        }


        override protected void LateUpdate()
        {
            if (m_drag)
            {
                base.LateUpdate();
                if (horizontal)
                {
                    _scaleXRenders();
                }
                else if (vertical)
                {
                    _scaleYRenders();
                }
            }
            else if (_enableUpdate)
            {
                if (horizontal)
                {
                    if (Mathf.Abs(content.anchoredPosition.x - m_end.x) > 1f)
                    {
                        Vector2 pos = Vector2.Lerp(content.anchoredPosition, m_end, m_controller.pageDamp);
                        SetContentAnchoredPosition(pos);
                    }
                    else
                    {
                        SetContentAnchoredPosition(m_end);
                        _enableUpdate = false;
                        m_controller.onScrollOver.Invoke();
                    }
                    _scaleXRenders();
                }
                else if (vertical)
                {
                    if (Mathf.Abs(content.anchoredPosition.y - m_end.y) > 1f)
                    {
                        Vector2 pos = Vector2.Lerp(content.anchoredPosition, m_end, m_controller.pageDamp);
                        SetContentAnchoredPosition(pos);
                    }
                    else
                    {
                        SetContentAnchoredPosition(m_end);
                        _enableUpdate = false;
                        m_controller.onScrollOver.Invoke();
                    }
                    _scaleYRenders();
                }
            }
        }

        /// <summary>
        /// 是否还有下一页.
        /// </summary>
        /// <returns></returns>
        public bool HasNextPage()
        {
            return m_currentPage < m_totalPage;
        }
        /// <summary>
        /// 是否有上一页.
        /// </summary>
        /// <returns></returns>
        public bool HasPrevPage()
        {
            return m_currentPage > 0;
        }

        /// <summary>
        /// 下一页.
        /// </summary>
        public void NextPage()
        {
            if (HasNextPage())
            {
                GotoPage(m_currentPage+1);
            }
        }
        /// <summary>
        /// 上一页.
        /// </summary>
        public void PrevPage()
        {
            if (HasPrevPage())
            {
                GotoPage(m_currentPage-1);
            }
        }

        /// <summary>
        /// 切换到哪一页.
        /// </summary>
        /// <param name="pageIndex">从0开始.</param>
        /// <param name="anim">是否有缓动画.默认为true</param>
        public void GotoPage(int pageIndex,bool anim=true)
        {
            if (m_currentPage != pageIndex)
            {
                m_currentPage = pageIndex;
                RectTransform item = FindItemByIndex(m_currentPage);
                if (item)
                {
                    m_centerItem = item;
                    Vector2 endPos = -item.anchoredPosition;
                    m_end = endPos;
                    _enableUpdate = true;
                    m_controller.onPageChange.Invoke();
                    if (m_controller.pageIndicator)
                        m_controller.pageIndicator.ShowPage(m_currentPage);

                    if (!anim)
                    {
                        _enableUpdate = false;
                        SetContentAnchoredPosition(m_end);
                        if (horizontal)
                        {
                            _scaleXRenders();
                        }
                        else if (vertical)
                        {
                            _scaleYRenders();
                        }
                    }
                }
            }
        }

        public RectTransform FindItemByIndex(int index)
        {
            foreach (RectTransform child in content)
            {
                if (child.GetComponent<CenterViewItem>().index == index)
                {
                    return child;
                }
            }
            return null;
        }
    }
}


