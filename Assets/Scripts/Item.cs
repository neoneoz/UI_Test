using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	// Use this for initialization
    public Item_Card _Item_Card;

	void Start () {
        _Item_Card.Init();//initialize card object linked to this item

	}

    public void SelectCard(bool select)
    {
        if (select)//set active and fade card in
        {
            //Debug.Log("selecting card");
            _Item_Card.gameObject.SetActive(true);
            _Item_Card.FadeIn();
            return;
        }
        _Item_Card.FadeOut();//fade card out
        


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
