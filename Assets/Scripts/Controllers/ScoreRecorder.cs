using UnityEngine;

public class ScoreRecorder : MonoBehaviour
{
    int score;
    public RoundController round_ctrl;
    public UserGUI userGUI;
    // Start is called before the first frame update
    void Start()
    {
        round_ctrl = (RoundController)SSDirector.getInstance().currentSceneController;
        round_ctrl.score_recorder = this;
        userGUI = gameObject.GetComponent<UserGUI>();
    }

    public void Record(GameObject disk)
    {
        int score_onetime = disk.GetComponent<Disk_Attributes>().score;
        userGUI.gameMessage = "+" + score_onetime;
        score = score + score_onetime;
        userGUI.score = score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
