using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    GameObject PlayerPrefab;
    void Start()
    {
        PlayerPrefab = Resources.Load("Player") as GameObject;
        
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        { 
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           RaycastHit hit;
           LayerMask mask = 1 << LayerMask.NameToLayer("StartPlace");
           if (Physics.Raycast(ray,out hit, 100,mask.value))
           {
               print("Hit :"+hit.point+"layer:"+hit.transform.gameObject.layer);
				Vector3 finalPos = new Vector3(hit.point.x,2f,hit.point.z);
				Instantiate(PlayerPrefab,finalPos, Quaternion.identity);
               
           }
        }         
    }
}
