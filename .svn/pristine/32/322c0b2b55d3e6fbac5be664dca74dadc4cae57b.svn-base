using UnityEngine;
using System.Collections;

public class TaskManager : MonoBehaviour
{
    public class TaskState
    {
        private bool running;
        public bool Running
        {
            get { return running; }
        }

        private bool paused;
        public bool Paused
        {
            get { return paused; }
        }

        bool stopped;
        public delegate void FinishHandler(bool manual);
        public event FinishHandler Finished;
        IEnumerator coroutine;

        public TaskState(IEnumerator c)
        {
            coroutine = c;
        }

        public void Pause()
        {
            paused = true;
        }
        public void UnPause()
        {
            paused = false;
        }
        public void Stop()
        {
            stopped = true;
            running = false;
        }
        public void Start()
        {
            running = true;
            inst.StartCoroutine(CallWrapper());
        }

        IEnumerator CallWrapper()
        {
            yield return null;
            IEnumerator e = coroutine;
            while (running)
            {
                if (paused)
                {
                    yield return null;
                }
                else
                {
                    if (e != null && e.MoveNext())
                    {
                        yield return e.Current;

                    }
                    else
                    {
                        running = false;

                    }
                }
            }

            FinishHandler handler = Finished;
            if (handler != null)
            {
                handler(stopped);
            }
        }
    }

    private static TaskManager inst;
    public static TaskState CreateTask(IEnumerator coroutine)
    {
        if (inst == null)
        {
            GameObject go = new GameObject("TaskManager");
            inst = go.AddComponent<TaskManager>();
        }
        return new TaskState(coroutine);
    }
}
