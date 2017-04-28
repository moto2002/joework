using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// 用于CenterView.
    /// </summary>
	/// 
	[AddComponentMenu("UI/Center View",1351)]
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(CenterViewController))]
    public class CenterViewItem : MonoBehaviour,IPointerClickHandler,System.IComparable
    {
        /// <summary>
        /// 用于排序.
        /// </summary>
        public int index;

        /// <summary>
        /// 点击后显示在中间.
        /// </summary>
        [HideInInspector]
        public bool clickToCenter = false ;

        /// <summary>
        /// 用于点击后居中显示; 如果已经是中间的对象，则点击后触选中事件.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (Input.touchCount > 1) return;
            CenterView centerView = GetComponentInParent<CenterView>();
            if (clickToCenter)
            {
                if (centerView.CenterItem.GetComponent<CenterViewItem>().index != this.index)
                {
                    centerView.GotoPage(index);
                }
                else
                {
                    centerView.controller.onSelect.Invoke();
                }
            }
            else if (centerView.CenterItem.GetComponent<CenterViewItem>().index == this.index)
            {
                centerView.controller.onSelect.Invoke();
            }
        }

        /// <summary>
        /// 用于排序.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(object other)
        {
            CenterViewItem item = other as CenterViewItem;
            if (this.index > item.index) return 1;
            else if (this.index < item.index) return -1;
            return 0;
        }
    }
}