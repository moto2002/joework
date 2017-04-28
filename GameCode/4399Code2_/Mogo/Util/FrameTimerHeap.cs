namespace Mogo.Util
{
    using MS;
    using System;
    using UnityEngine;

    public class FrameTimerHeap
    {
        private static uint m_nNextTimerId;
        private static KeyedPriorityQueue<uint, AbsTimerData, ulong> m_queue = new KeyedPriorityQueue<uint, AbsTimerData, ulong>();
        private static readonly object m_queueLock = new object();
        private static uint m_unTick;

        private FrameTimerHeap()
        {
        }

        private static uint AddTimer(AbsTimerData p)
        {
            lock (m_queueLock)
            {
                m_queue.Enqueue(p.NTimerId, p, p.UnNextTick);
            }
            return p.NTimerId;
        }

        public static uint AddTimer(uint start, int interval, Action handler)
        {
            TimerData p = GetTimerData<TimerData>(new TimerData(), start, interval);
            p.Action = handler;
            return AddTimer(p);
        }

        public static uint AddTimer<T>(uint start, int interval, Action<T> handler, T arg1)
        {
            TimerData<T> p = GetTimerData<TimerData<T>>(new TimerData<T>(), start, interval);
            p.Action = handler;
            p.Arg1 = arg1;
            return AddTimer(p);
        }

        public static uint AddTimer<T, U>(uint start, int interval, Action<T, U> handler, T arg1, U arg2)
        {
            TimerData<T, U> p = GetTimerData<TimerData<T, U>>(new TimerData<T, U>(), start, interval);
            p.Action = handler;
            p.Arg1 = arg1;
            p.Arg2 = arg2;
            return AddTimer(p);
        }

        public static uint AddTimer<T, U, V>(uint start, int interval, Action<T, U, V> handler, T arg1, U arg2, V arg3)
        {
            TimerData<T, U, V> p = GetTimerData<TimerData<T, U, V>>(new TimerData<T, U, V>(), start, interval);
            p.Action = handler;
            p.Arg1 = arg1;
            p.Arg2 = arg2;
            p.Arg3 = arg3;
            return AddTimer(p);
        }

        public static void DelTimer(uint timerId)
        {
            lock (m_queueLock)
            {
                m_queue.Remove(timerId);
            }
        }

        private static T GetTimerData<T>(T p, uint start, int interval) where T: AbsTimerData
        {
            p.NInterval = interval;
            p.NTimerId = ++m_nNextTimerId;
            p.UnNextTick = (m_unTick + 1) + start;
            return p;
        }

        public static void Reset()
        {
            m_unTick = 0;
            m_nNextTimerId = 0;
            lock (m_queueLock)
            {
                while (m_queue.Count != 0)
                {
                    m_queue.Dequeue();
                }
            }
        }

        public static void Tick()
        {
            m_unTick = (uint) (1000f * Time.time);
            while (m_queue.Count != 0)
            {
                AbsTimerData data;
                object obj2;
                lock ((obj2 = m_queueLock))
                {
                    data = m_queue.Peek();
                }
                if (m_unTick < data.UnNextTick)
                {
                    break;
                }
                lock ((obj2 = m_queueLock))
                {
                    m_queue.Dequeue();
                }
                if (data.NInterval > 0)
                {
                    data.UnNextTick += data.NInterval;
                    lock ((obj2 = m_queueLock))
                    {
                        m_queue.Enqueue(data.NTimerId, data, data.UnNextTick);
                    }
                    data.DoAction();
                }
                else
                {
                    data.DoAction();
                }
            }
        }
    }
}

