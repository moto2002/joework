using UnityEngine;
using System.Collections;

public class ParticleSysIngoreTimeScale : MonoBehaviour
{
    private double lastTime;
    private ParticleSystem[] particleArray;

    void Awake()
    {
        particleArray = GetComponentsInChildren<ParticleSystem>();
    }

    void Start()
    {
        lastTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        float deltaTime = Time.realtimeSinceStartup - (float)lastTime;

        for (int i = 0; i < particleArray.Length; i++)
        {
            particleArray[i].Simulate(deltaTime, true, false);//last must be false;
        }

        lastTime = Time.realtimeSinceStartup;
    }
}
