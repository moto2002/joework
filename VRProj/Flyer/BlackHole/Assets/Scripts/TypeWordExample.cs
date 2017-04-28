using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TypeWordExample : MonoBehaviour
{
    ITextTyper t;
    void Start()
    {
       
    }

    void OnEnable()
    {
        //PlayTypeWordEffect();
    }

    public void PlayTypeWordEffect()
    {
        if(t == null)
            t = GetComponent<Text>().TypeEffect();
        t.Start(10);
        //t.Done = () => { print("fffffffffd"); };
        this.gameObject.SetActive(true);
    }

    public void StopTypeWordEffect()
    {
        t.Stop();
    }
}