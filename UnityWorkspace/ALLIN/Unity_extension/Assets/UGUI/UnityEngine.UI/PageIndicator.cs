using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// author:zhouzhanglin
    /// 页码.
    /// </summary>
    [AddComponentMenu("UI/Page Indicator", 1352)]
    public class PageIndicator : UIBehaviour
    {
        public Sprite defaultState;
        public Sprite selectedState;
        public bool interactive = true;//是否可交互.
        public float space=0f; //空隙.

        private float m_PageItemWidth;
        private float m_PageItemHeight;

		public Material material;

        private int m_prevPage=0;
        private RectTransform m_layer;

        private IPage m_IPage=null;
        public IPage iPage {
            set { m_IPage = value; }
        }

        /// <summary>
        /// 创建.
        /// </summary>
        /// <param name="count">数量.</param>
        public void Build(int count)
        {
            if (defaultState && selectedState)
            {
                m_PageItemWidth = defaultState.bounds.size.x * 100f;
                m_PageItemHeight = defaultState.bounds.size.y * 100f;

                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }

                float total_width = (m_PageItemWidth + space) * (count -1);

                for (int i = 0; i < count; i++)
                {
                    Image img = (new GameObject()).AddComponent<Image>();
                    if (interactive)
                    {
                        EventTriggerListener.Get(img.gameObject).OnClick += OnPageItemClick;
                    }
                    if (i != m_prevPage)
                    {
                        img.sprite = defaultState;
                    }
                    else
                    {
                        img.sprite = selectedState;
                    }
					if(material!=null){
						img.material=material;
					}
                    RectTransform rectTrans = (RectTransform)img.transform;
                    rectTrans.sizeDelta = new Vector2(m_PageItemWidth, m_PageItemHeight);
                    float size = m_PageItemWidth;
                    rectTrans.SetParent(transform);
                    rectTrans.name = "PageItem";
                    rectTrans.localScale = Vector3.one;
                    rectTrans.anchoredPosition = new Vector2((size + space) * i - total_width/2f, 0);
                }
            }
        }

        /// <summary>
        /// 显示到某一页.
        /// </summary>
        /// <param name="pageIndex"></param>
        public void ShowPage(int pageIndex)
        {
            if (m_prevPage != pageIndex)
            {
                transform.GetChild(m_prevPage).GetComponent<Image>().sprite = defaultState;
                transform.GetChild(pageIndex).GetComponent<Image>().sprite = selectedState;
                m_prevPage = pageIndex;
            }
        }

        private void OnPageItemClick(GameObject go , PointerEventData eventData)
        {
            if (m_IPage!=null)
            {
                if(Input.touchCount<=1)
                    m_IPage.ShowPage(go.transform.GetSiblingIndex());
            }
            
        }
    }
}
