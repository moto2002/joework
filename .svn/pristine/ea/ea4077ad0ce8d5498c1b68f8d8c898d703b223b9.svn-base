using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    /// <summary>
    /// author:zhouzhanglin
    /// PageView 的一些控制.
    /// </summary>
    public class PageViewController : MonoBehaviour, IPage
    {
        public PageIndicator pageIndicator;
        //缓动.
        public float pageDamp = 0.2f;
        //是否支持鼠标拖动.
        public bool dragEnable = true;
		//在区域外时是否还可以拖动
		public bool dragEnableOutSide = true;
        //是否自动初始化.
        public bool autoInit = true;

        [Serializable]
        public class PageViewEvent : UnityEvent { }

        [SerializeField]
        private PageViewEvent m_OnScrollOver = new PageViewEvent();
        public PageViewEvent onScrollOver { get { return m_OnScrollOver; } set { m_OnScrollOver = value; } }

        [SerializeField]
        private PageViewEvent m_OnPageChange = new PageViewEvent();
        public PageViewEvent onPageChange { get { return m_OnPageChange; } set { m_OnPageChange = value; } }

        private PageView m_pageView;
        public PageView pageView
        {
            get { return m_pageView; }
        }

		void Awake(){
			m_pageView = GetComponent<PageView>();
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
        /// 初始化,需要在yield return new WaitForEndOfFrame();后调用.
        /// 主要用于在PageView动态设置后手动调用.
        /// </summary>
        public void Init()
        {
            m_pageView.Init();
            if (pageIndicator)
            {
                pageIndicator.iPage = this;
                pageIndicator.Build(m_pageView.totalPage + 1);
            }
        }

        /// <summary>
        /// 用于点击PageIndicator时改变页码.
        /// </summary>
        /// <param name="index"></param>
        public void ShowPage(int index)
        {
            m_pageView.GotoPage(index);
        }
    }

}