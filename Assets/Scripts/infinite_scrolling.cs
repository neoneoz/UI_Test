using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infinite_scrolling : MonoBehaviour {

    ScrollRect _scrollRect;
    ContentSizeFitter _contentSizeFitter;
    HorizontalLayoutGroup _horizontalLayoutGroup;
    GridLayoutGroup _gridLayoutGroup;

    float threshold = 100f;
    int itemcount = 0;
    float recordOffset = 0;
  

    float DisableMargin = 0;
    bool LayoutDisabled= false;

    protected List<RectTransform> items = new List<RectTransform>();
    Vector2 NewAnchoredPos = Vector2.zero;
    Color NewColor = new Color();
    Coroutine lerpsnap = null;
    public bool Snapped = true;

    //item variables
    Transform CurrentItem = null;
    float SelectedSize = 2f;
    float DefaultAlpha = 0.65f;


    void awake()
    {
       
        //Init();
    }

	void Start () {

        Init();
	}

    void Init()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _scrollRect.onValueChanged.AddListener(OnScroll);
        _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        _horizontalLayoutGroup = _scrollRect.content.GetComponent<HorizontalLayoutGroup>();
        SetItems();

        SelectItem(_scrollRect.content.GetChild(2));//select the default starting item
        // _contentSizeFitter = _scrollRect.content.GetComponent<ContentSizeFitter>();
    }


    void SetItems()
    {
        Color temp = new Color();
        for (int i = 0; i < _scrollRect.content.childCount; i++)//add all the items into items list
        {
            items.Add(_scrollRect.content.GetChild(i).GetComponent<RectTransform>());
            temp =  _scrollRect.content.GetChild(i).GetComponent<Image>().color;//set the opacity for the unselected items
            _scrollRect.content.GetChild(i).GetComponent<Image>().color = new Color(temp.r, temp.g, temp.b, DefaultAlpha);
            
            //all cards default to inactive state
            _scrollRect.content.GetChild(i).GetComponent<Item>()._Item_Card.gameObject.SetActive(false);
           // Debug.Log("itemcount = " + i);
        }

        itemcount = _scrollRect.content.childCount;//update item count
    }
	

	void Update () {

        if (Snapped)//currently Snapped to highlighted item
            return;

        if (Input.GetMouseButtonUp(0))//on input release
        {
           //select the centered item
            _scrollRect.velocity = Vector2.zero;
            SelectItem(_scrollRect.content.GetChild(2));
        }
	}


    void DisaableLayout()
    {
        //local pos difference between 1st and 2nd item
        recordOffset = items[1].GetComponent<RectTransform>().anchoredPosition.x - items[0].GetComponent<RectTransform>().anchoredPosition.x;
        
        if (recordOffset < 0)//is flipped
        {
            recordOffset *= -1;
        }
        DisableMargin = recordOffset * itemcount / 2;//get the scrolling exit threshold
        //Debug.Log("margin = " + DisableMargin);
        _horizontalLayoutGroup.enabled = false;
        //_contentSizeFitter.enabled = false;
        // _gridLayoutGroup.enabled = false;
        LayoutDisabled= true;
    }

    public void OnScroll(Vector2 pos)
    {
        Snapped = false;//unselect current item
        //Debug.Log("scrolling = " + _scrollRect.horizontalNormalizedPosition);

        if (!LayoutDisabled)//disable all of unity UI's alignment
            DisaableLayout();
        for(int i =0; i< items.Count; i++)//check all items
        {
            //*inversetransformpoint uses localpos 

            //check if a item in scrollrect is outside the specefied threshhold (left)
            if (_scrollRect.transform.InverseTransformPoint(items[i].gameObject.transform.position).x > DisableMargin + threshold)
            {
                //place the item 
                NewAnchoredPos = items[i].anchoredPosition;
                NewAnchoredPos.x -= itemcount * recordOffset;//new position is on the far right
               // NewAnchoredPos.y = 0;
                items[i].anchoredPosition = NewAnchoredPos;

                _scrollRect.content.GetChild(itemcount - 1).transform.SetAsFirstSibling();//correct the hierachy
            }
            //(right)
            else if (_scrollRect.transform.InverseTransformPoint(items[i].gameObject.transform.position).x < -DisableMargin)
            {
                NewAnchoredPos = items[i].anchoredPosition;
                NewAnchoredPos.x += itemcount * recordOffset;//new position is on the far left
                //NewAnchoredPos.y = 0;
                items[i].anchoredPosition = NewAnchoredPos;

                _scrollRect.content.GetChild(0).transform.SetAsLastSibling();//correct the hierachy
            }



        }
    }

    void SelectItem(Transform target)
    {
        //check if item is already selected
        if (target == CurrentItem)
        {
            SnapToItem(target);//re-snap but do not reset the itemcard
           return;

        }

        SnapToItem(target);//snap to index2 (center) item

        //deslect current item
        if (CurrentItem != null)
        {
            CurrentItem.localScale = new Vector3(1, 1, 1);
            NewColor = CurrentItem.GetComponent<Image>().color;
            CurrentItem.GetComponent<Image>().color = new Color(NewColor.r, NewColor.g, NewColor.b, DefaultAlpha);
            CurrentItem.GetComponent<Item>().SelectCard(false);
        }

        CurrentItem = _scrollRect.content.GetChild(2);//update current item refrence

        //make current item big & 100% opacity
        target.localScale = target.localScale * SelectedSize;
        NewColor = target.GetComponent<Image>().color;
        target.GetComponent<Image>().color = new Color(NewColor.r, NewColor.g, NewColor.b,1f );
        target.GetComponent<Item>().SelectCard(true);
    }

    void SnapToItem(Transform target)
    {
        //Debug.Log("release");
        Vector2 finalPostion = _scrollRect.transform.InverseTransformPoint(_scrollRect.content.position)
        - _scrollRect.transform.InverseTransformPoint(target.position);
        if(lerpsnap!= null)
        {
            StopCoroutine(lerpsnap);
            lerpsnap = null;
        }

        lerpsnap = StartCoroutine(LerpToItem(finalPostion));

    }

    IEnumerator LerpToItem(Vector2 target)
    {

        float time = 0.5f;
        float elaspedTime = 0;
        while (elaspedTime < time)
        {
            _scrollRect.content.anchoredPosition = Vector2.Lerp(_scrollRect.content.anchoredPosition, target, elaspedTime / time);
            elaspedTime += Time.deltaTime;
            yield return 0;
        }
        Snapped = true;
        _scrollRect.velocity = Vector2.zero;
    }
}
