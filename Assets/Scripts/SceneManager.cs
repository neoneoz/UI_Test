using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    //quick singleton implementation for switching between "game" and menu.
   private static bool m_ShuttingDown = false;
   private static object m_Lock = new object();
   private static SceneManager _instance;
   public Transform Game, Menu;
	private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        Screen.SetResolution(480, 800, false);
    }
    
    void Start () {
        Game = GameObject.Find("Canvas_game").transform;
        Menu = GameObject.Find("Canvas_base").transform;
        Game.gameObject.SetActive(false);
	}

    public void StartGame(bool start)
    {
        if(start)
        {
            Game.gameObject.SetActive(true);
            Menu.gameObject.SetActive(false);
        }
        else
        {
            Game.gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }

    }

    public static SceneManager Getinstance()
    {
        return _instance;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
