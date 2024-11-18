using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    public int score;
    public int round;
    public string gameMessage;
    public bool is_start;
    private IUserAction action;
    public GUIStyle style1, style2;
    private int _width = Screen.width / 5;
    private int _height = Screen.width / 10;

    // Start is called before the first frame update
    void Start()
    {
        is_start = false;
        gameMessage = "";
        action = SSDirector.getInstance().currentSceneController as IUserAction;
        style1 = new GUIStyle();
        style1.normal.textColor = Color.white;
        style1.fontSize = 50;
        style1.alignment = TextAnchor.MiddleCenter;
        style2 = new GUIStyle();
        style2.normal.textColor = Color.white;
        style2.fontSize = 25;
        style2.alignment = TextAnchor.MiddleCenter;
    }

    void OnGUI()
    {
        GUI.skin.button.fontSize = 35;
        if (is_start)
        {
            Game_Start();
        }
        else
        {
            Home_Page();
        }
    }

    void Home_Page()
    {
        float centerX = Screen.width / 2;
        float centerY = Screen.height / 2;
        GUI.Label(new Rect(centerX - _width * 0.5f, Screen.height * 0.1f, _width, _height), "Hit UFO", style1);
        bool start_button = GUI.Button(new Rect(centerX - _width * 0.5f, centerY - _height * 0.5f, _width, _height), "Start");
        if (start_button)
        {
            is_start = true;
        }
    }

    void Game_Start()
    {
        GUI.Label(new Rect(350, 60, 50, 200), gameMessage, style1);
        if (gameMessage == "游戏结束")
        {
            GUI.Label(new Rect(250, 120, 50, 200), "最终得分：" + score, style1);
        }
        if (score == 0)
        {
            GUI.Label(new Rect(0, 0, 100, 50), "得分：", style2);
        }
        else if (score != 0)
        {
            GUI.Label(new Rect(0, 0, 100, 50), "得分：" + score, style2);
        }
        if (round == 0)
        {
            GUI.Label(new Rect(560, 0, 100, 50), "当前回合：", style2);
        }
        else if (round != 0)
        {
            GUI.Label(new Rect(560, 0, 100, 50), "当前回合：" + round, style2);
        }
    }

    void Update()
    {
        
    }
}
