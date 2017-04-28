using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework;


public class Boot : MonoBehaviour
{
    void Start()
    {
        GameFramework framework = MonoSingleton<GameFramework>.Instance;
    }
}
