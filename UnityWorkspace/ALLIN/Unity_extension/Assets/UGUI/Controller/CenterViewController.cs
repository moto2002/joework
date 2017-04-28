using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    /// <summary>
    /// author:zhouzhanglin
    /// CenterView 的一些控制.
    /// </summary>
    public class CenterViewController : MonoBehaviour, IPage
    {
        public PageIndicator pageIndicator;
        //缓动.
        public float pageDamp = 0.2f;

        public float maxScale = 1f;
        public float minScale = 0.2f;

        //是否支持鼠标拖动.
        public bool dragEnable = true;

        //是否点击后显示在中间.
        public bool clickItemToCenter = false;

        //是否自动初始化 .
        public bool autoInit = true;

        [Serializable]
        public class PageViewEvent : UnityEvent { }

        [SerializeField]
        private PageViewEvent m_OnScrollOver = new PageViewEvent();
        public PageViewEvent onScrollOver { get { return m_OnScrollOver; } set { m_OnScrollOver = value; } }

        [SerializeField]
        private PageViewEvent m_OnPageChange = new PageViewEvent();
        public PageViewEvent onPageChange { get { return m_OnPageChange; } set { m_OnPageChange = value; } }

        [SerializeField]
        private PageViewEvent m_OnSelect = new PageViewEvent();
        public PageViewEvent onSelect { get { return m_OnSelect; } set { m_OnSelect = value; } }

        private CenterView m_centerView;
        public CenterView centerView
        {
            get { return m_centerView; }
        }

        IEnumerator Start()
        {
            if (autoInit)
            {
                yield return new WaitForEndOfFrame();
                this.Init();
            }
        }

        /// <summary>
        /// 初始化.需要在yield return new WaitForEndOfFrame();后调用.
        /// 主要用于在CenterView动态设置后手动调用.
        /// </summary>
        public void Init()
        {
            m_centerView = GetComponent<CenterView>();
            m_centerView.Init();
            if (pageIndicator)
            {
                pageIndicator.iPage = this;
                pageIndicator.Build(m_centerView.totalPage + 1);
            }
        }

        /// <summary>
        /// 用于点击PageIndicator时改变页码.
        /// </summary>
        /// <param name="index"></param>
        public void ShowPage(int index)
        {
            m_centerView.GotoPage(index);
        }
    }

}