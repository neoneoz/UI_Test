using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infinity_scroll : MonoBehaviour {

    //redid this in infinite scrolling
    //add scrollcontent?
    private Transform scrollcontent;
    private ScrollRect scrollRect;
    private Vector2 lastDragPosition;
    private bool positiveDrag;
    

	// Use this for initialization
	void Start () {
        scrollcontent = gameObject.transform.GetChild(0);
        scrollRect = GetComponent<ScrollRect>();
        //scrollRect.vertical = scrollContent.Vertical;
        //scrollRect.horizontal = scrollContent.Horizontal;
       // scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        scrollRect.onValueChanged.AddListener(scrolling);
	}

   public void scrolling(Vector2 val)
    {
        //Debug.Log(scrollRect.horizontalNormalizedPosition);
        if (scrollRect.horizontalNormalizedPosition <= 0)
        {
            Debug.Log("exceeded left index");
            //pop the last item to the front
            popitem(true);
      
           
        }
        else if (scrollRect.horizontalNormalizedPosition >= 1)
        {
            Debug.Log("exceeded right index");
            //pop the first item to the back
            popitem(false);
        }
    }

    public void popitem(bool tofront)
   {
       if (tofront)
       {
           scrollcontent.GetChild(scrollcontent.childCount - 1).SetAsFirstSibling();
           scrollRect.horizontalNormalizedPosition = 0.001f;
       }
       else
       {
           scrollcontent.GetChild(0).SetAsLastSibling();
           scrollRect.horizontalNormalizedPosition = 0.99999f;
       }

   }
}
