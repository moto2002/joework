using UnityEngine;
using System.Collections;

public class Add10Items : MonoBehaviour
{
    static int index;

    GameObject item;
    
    void Start()
    {
        item = Resources.Load("Item") as GameObject;
        var itemline = Resources.Load("Item2") as GameObject;
        UIGridDyn grid = GameObject.Find("UI Root/Camera/New/Panel/Grid").gameObject.GetComponent<UIGridDyn>();
        grid.srcObj = item;
        grid.srcCutLine = itemline;
        //grid.onItemShow += this.OnItemShow;
        //grid.onItemHide += this.OnItemHide;
        index = 0;
    }
    void OnClick()
    {
        UIGridDyn grid = GameObject.Find("UI Root/Camera/New/Panel/Grid").gameObject.GetComponent<UIGridDyn>();
        for (int i = 0; i < 9; i++)
        {
            grid.AddItem(index.ToString(), new ItemClass(index.ToString()));
            index++;

        }
        grid.AddCutLine("cut0", null);
        //grid.Reposition();
    }

    class ItemClass : UIGridDyn.IItemObj
    {
        string myp;
        public ItemClass(string myp)
        {
            this.myp = myp;
        }
        public void OnItemShow(string key, GameObject obj)
        {
            UILabel label = obj.transform.FindChild("Label1").GetComponent<UILabel>();
            label.text = "Item " + myp;
            label = obj.transform.FindChild("Label2").GetComponent<UILabel>();
            label.text = "Test " + myp;
        }

        public void OnItemHide(string key, GameObject obj)
        {
           
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 300, 50), "Count=" + index);
    }
}
