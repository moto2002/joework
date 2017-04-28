using UnityEngine;
using System.Collections;

public class particleIgnoreTimeScale : MonoBehaviour
{
    private double lastTime;
    private ParticleSystem particle;
    

    void Awake()
    { 
        particle = GetComponent<ParticleSystem>();
    }

    // Use this for initialization
    void Start()
    {
        lastTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.realtimeSinceStartup - (float)lastTime;
        particle.Simulate(deltaTime,true,false);//last must be false;
        lastTime = Time.realtimeSinceStartup;
    }
}
