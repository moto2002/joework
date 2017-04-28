using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour
{
    void OnGUI()
    {
        if (GUILayout.Button("Start"))
        {
            SomeCodeThatCouldBeAnywhereInTheUniverse();
        }
        if (GUILayout.Button("Pause"))
        {
            T_Pause();
        }
        if (GUILayout.Button("UnPause"))
        {
            T_UnPause();
        }
        if (GUILayout.Button("Stop"))
        {
            T_Stop();
        }
    }

    IEnumerator MySimpleTask()
    {
        while (true)
        {
            Debug.Log("simple task");
            yield return null;
        }
    }

    IEnumerator TaskKiller(float delay, Task t)
    {
        yield return new WaitForSeconds(delay);
        t.Stop();
    }

    Task _task;
    void SomeCodeThatCouldBeAnywhereInTheUniverse()
    {
         _task = new Task(MySimpleTask());

        //任务在什么情况下完成了呢，完成后做什么事
        //例子：5秒后将其完成
         Task _killTask = new Task(TaskKiller(5, _task));
         _killTask.Finished += TestSimpleTaskFinish;
    }
    void T_Pause()
    {
        _task.Pause();
    }
    void T_UnPause()
    {
        _task.UnPause();
    }
    void T_Stop()
    {
        _task.Stop();
    }

    void TestSimpleTaskFinish(bool b)
    {
        Debug.Log("TestSimpleTaskFinish");
    }
}
