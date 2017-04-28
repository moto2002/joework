using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Core.Extension.UIFrameEffect
{
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(Image))]
    public class UIFrameEffect : MonoBehaviour, IUIFrameEffect
    {
        #region vars

        private string id;
        // 配置表中在特效ID
        public string ID { get { return id; } }

        // 设置是否循环播放
        private bool loop;

        // 每帧播放的时间
        private float speed;

        // 当前定时器ID
        private TimerManager.ITimer curTimer;

        // 旋转定时器
        private TimerManager.ITimer rotateTimer;

        // 旋转速度
        private float rotateSpeed;

        public Sprite this[int index]
        {
            get
            {
                if (index < 0 || index > sprites.Count - 1)
                {
                    return sprites[0];
                }
                return sprites[index];
            }
        }

        private int index;
        // 当前帧序号
        public int CurIndex { get { return index; } }

        // 特效帧列表
        private List<Sprite> sprites = new List<Sprite>();

        // 动画结束的回调事件
        public event Action<IUIFrameEffect> PlayEndEvent;

        // 被销毁回调事件
        public event Action<IUIFrameEffect> DestroyedEvent;

        // 特效承载的 Image
        private Image FrameImage { get { return gameObject.GetComponent<Image>(); } }

        // 某帧回调事件
        public Dictionary<int, Action<IUIFrameEffect>> IndexEvent = new Dictionary<int, Action<IUIFrameEffect>>();

        #endregion

        public void Stop()
        {
            if (rotateTimer != null)
            {
                TimerManager.Remove(rotateTimer);
                rotateTimer = null;
            }
            if (curTimer != null)
            {
                TimerManager.Remove(curTimer);
                curTimer = null;
            }
            if (PlayEndEvent != null)
            {
                PlayEndEvent(this);
                PlayEndEvent = null;
            }
            FrameImage.enabled = false;
            DestroyedEvent = null;
            IndexEvent.Clear();
        }

        public void Reset()
        {
            index = 0;
            rotateSpeed = 0;
            PlayEndEvent = null;
            FrameImage.enabled = false;
        }

        private void OnDestroy()
        {
            if (DestroyedEvent != null)
                DestroyedEvent(this);
            DestroyedEvent = null;
            PlayEndEvent = null;
            IndexEvent.Clear();
            TimerManager.Remove(curTimer);
        }

        public void Play(int index = -1)
        {
            if (sprites.Count < 1)
            {
                Stop();
                return;
            }
            TimerManager.Remove(curTimer);
            FrameImage.enabled = true;
            FrameImage.raycastTarget = false;
            this.index = index < 0 || index > sprites.Count - 1 ? this.index : index;
            FrameImage.sprite = sprites[this.index];
            FrameImage.SetNativeSize();
            if (IndexEvent.ContainsKey(this.index) && this.index == 0)
                IndexEvent[this.index](this);
            curTimer = TimerManager.Add(speed, (arg) =>
            {
                this.index++;
                this.index = this.index > sprites.Count - 1 ? 0 : this.index;
                if (this.index == 0 && !loop) // 第二轮第一帧且非循环播放,结束特效
                    Stop();
                else
                {
                    FrameImage.sprite = sprites[this.index];
                    FrameImage.SetNativeSize();
                    if (IndexEvent.ContainsKey(this.index))
                        IndexEvent[this.index](this);
                }
            }, -1);
        }

        public void Setup(string effectID, List<Sprite> sprites)
        {
            this.sprites = sprites;
            speed = 0.082f;
            id = effectID;
            loop = false;
            Reset();
        }

        public IUIFrameEffect SetIndex(int index)
        {
            Play(index);
            return this;
        }

        public IUIFrameEffect SetLoop(bool loop = true)
        {
            this.loop = loop;
            Play();
            return this;
        }

        public IUIFrameEffect SetSpeed(float speed = 0.083F)
        {
            this.speed = speed;
            Play();
            return this;
        }

        public IUIFrameEffect SetEndEvent(Action<IUIFrameEffect> back)
        {
            PlayEndEvent += back;
            return this;
        }

        public IUIFrameEffect SetDestroyEvent(Action<IUIFrameEffect> back)
        {
            DestroyedEvent += back;
            return this;
        }

        public IUIFrameEffect SetIndexEvent(int index, Action<IUIFrameEffect> back)
        {
            if (!IndexEvent.ContainsKey(index))
                IndexEvent.Add(index, back);
            else
                IndexEvent[index] += back;
            Play();
            return this;
        }

        public IUIFrameEffect SetRotateSpeed(float rotateSpeed)
        {
            this.rotateSpeed = rotateSpeed;
            if (rotateTimer != null)
            {
                TimerManager.Remove(rotateTimer);
                rotateTimer = null;
            }
            if (rotateSpeed != 0)
            {
                rotateTimer = TimerManager.Add(-1, (arg) =>
                {
                    FrameImage.transform.localEulerAngles += new Vector3(0, 0, rotateSpeed * Time.deltaTime);
                }, -1);
            }
            return this;
        }

        public IUIFrameEffect SetRotate(Vector3 rotate)
        {
            FrameImage.transform.localEulerAngles = rotate;
            return this;
        }
    }
}