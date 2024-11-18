using UnityEngine;

public class RoundController : MonoBehaviour, ISceneController, IUserAction
{

	public CCActionManager actionManager;
	GameObject disk;
	public ScoreRecorder score_recorder;
	public UserGUI userGUI;
	int round;
	int max_round;
	float time;
	DiskFactory factory;

	// the first scripts
	void Awake()
	{
		SSDirector director = SSDirector.getInstance();
		director.currentSceneController = this;
		director.currentSceneController.LoadResources();
		gameObject.AddComponent<UserGUI>();
		gameObject.AddComponent<CCActionManager>();
		gameObject.AddComponent<ScoreRecorder>();
		gameObject.AddComponent<DiskFactory>();
		factory = Singleton<DiskFactory>.Instance;//创建工厂单实例
		userGUI = gameObject.GetComponent<UserGUI>();
	}

	// loading resources for first scence
	public void LoadResources()
	{
		//在工厂中实现
	}


	public void GameOver()
	{
		if (round > max_round && actionManager.remain_action_count() == 0)
		{
			userGUI.gameMessage = "游戏结束";
		}
	}


	// Use this for initialization
	void Start()
	{
		//初始化
		round = -1;
		max_round = 9;
		time = 1.0f;
	}

	// Update is called once per frame
	void Update()
	{
		if (!userGUI.is_start)
		{
			return;
		}
		UFO_Clicked();
		GameOver();
		if (round > max_round)
		{
			return;
		}
		time -= Time.deltaTime;
		if (time <= 0 && actionManager.remain_action_count() == 0)
		{
			round++;
			if (round <= max_round + 1)
			{
				userGUI.round = round;
			}
			for (int i = 0; i < 10; i++)
			{
				disk = factory.GetDisk(round);
				actionManager.Move_Disk(disk);
			}
			if (round > 0)
			{
				time = 4.0f;
			}
			userGUI.gameMessage = "";
		}
	}

	public void UFO_Clicked()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Camera ca = Camera.main;
			Ray ray = ca.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				score_recorder.Record(hit.transform.gameObject);
				hit.transform.gameObject.SetActive(false);
			}
		}
	}
}
