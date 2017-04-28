using UnityEngine;
using System.Collections;

/// <summary>
/// 为什么要自定义随机数？
/// 因为你不知道系统内部还有哪些地方调用了系统的随机数。避免不是自己主动调用的。
/// 因为调用的次数不一致的话，产生的结果就不相同了。
/// 所以，要自定义随机数，用相同的随机数种子，然后保证调用的次数一样，那么产生的随机数一定是一样的。
/// </summary>
public class TestXRandom : MonoBehaviour 
{
    CopySystem.Random random;

	void Start () 
    {
        int m_seed = 98;
        random = new CopySystem.Random(m_seed);
        
	}

	void Update () 
    {
        Debug.Log(random.Next(0, 100));
	}
}
