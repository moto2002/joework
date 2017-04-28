using System;
using UnityEngine;
using System.Collections.Generic;

namespace Core.Extension.UIFrameEffect
{
    /// <summary>
    /// 帧特效某帧回调事件列表
    /// </summary>
    public class UIFrameEffectArgs
    {
        /// <summary>
        /// 特效 ID
        /// </summary>
        public string EffectID { get; private set; }
        /// <summary>
        /// 特效所在的父物体
        /// </summary>
        public Transform Parent { get; private set; }

        /// <summary>
        /// 帧特效某帧回调事件列表
        /// </summary>
        public Dictionary<int, Action<IUIFrameEffect>> BackDic = new Dictionary<int, Action<IUIFrameEffect>>();

        /// <summary>
        /// 帧特效参数
        /// </summary>
        /// <param name="effectID">特效 ID</param>
        /// <param name="parent">特效所在的父物体</param>
        public UIFrameEffectArgs(string effectID, Transform parent)
        {
            EffectID = effectID;
            Parent = parent;
        }

        /// <summary>
        /// 设置回调
        /// </summary>
        /// <param name="index">回调所在帧序号</param>
        /// <param name="back">回调事件</param>
        public void SetAction(int index, Action<IUIFrameEffect> back)
        {
            if (!BackDic.ContainsKey(index))
                BackDic.Add(index, back);
            else
                BackDic[index] += back;
        }
    }
}