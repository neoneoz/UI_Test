using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Card : MonoBehaviour {

    CanvasGroup _CanvasGroup;
    void Awake()
    {
        //canvasgroup controls alpha for entire group
        _CanvasGroup = GetComponent<CanvasGroup>();
    }
    public void Init()
    {
    
        _CanvasGroup.alpha = 0;//set the opacity of the card to 0
        GetComponent<Button>().onClick.AddListener(CardClick);//listner for starting the game
    }


    public void FadeIn()
    {
        StartCoroutine(FadeGroup(_CanvasGroup, _CanvasGroup.alpha, 1));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeGroup(_CanvasGroup, _CanvasGroup.alpha, 0));
    }

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator FadeGroup(CanvasGroup canvasgroup,float start,float end,float duration = 0.5f)
    {
        float TimeStart = Time.time;
        float TimeElapsed = Time.time - TimeStart;
        float progress = TimeElapsed / duration;


        while(true)//loopsuntil progress increments to duration
        {
            TimeElapsed = Time.time - TimeStart;
            progress = TimeElapsed / duration;


            float current = Mathf.Lerp(start, end, progress);

            canvasgroup.alpha = current;
            if (progress >= 1)
            {
                if (end == 0)
                    gameObject.SetActive(false);
               break;  
            }
            yield return new WaitForEndOfFrame();
        }

    }

    void CardClick()
    {
        //start the game through singleton scenemanager
        SceneManager.Getinstance().StartGame(true);


    }
}
