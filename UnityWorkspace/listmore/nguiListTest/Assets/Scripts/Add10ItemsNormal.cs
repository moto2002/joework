using UnityEngine;
using System.Collections;

public class Add10ItemsNormal : MonoBehaviour
{
    static int index;

    GameObject item;

    void Start()
    {
        item = Resources.Load("Item") as GameObject;

        index = 0;
    }

    void OnClick()
    {
        UIGridDyn grid = GameObject.Find("UI Root/Camera/New/Panel/Grid").gameObject.GetComponent<UIGridDyn>();

        int i = Random.Range(1, 20);
        grid.RemoveItem(i.ToString());
        //UIGrid grid = GameObject.Find("UI Root/Camera/Normal/Panel/Grid").GetComponent<UIGrid>();
        //for (int i = 0; i < 10; i++)
        //{
        //    GameObject myItem = GameObject.Instantiate(item) as GameObject;
        //    myItem.transform.parent = grid.transform;
        //    myItem.transform.localScale = new Vector3(1, 1, 1);

        //    UILabel label = myItem.transform.FindChild("Label1").GetComponent<UILabel>();
        //    label.text = "Item " + index;
        //    label = myItem.transform.FindChild("Label2").GetComponent<UILabel>();
        //    label.text = "Test " + index;

        //    index++;
        //}
        //grid.Reposition();

        grid.Resort();
    }
}
