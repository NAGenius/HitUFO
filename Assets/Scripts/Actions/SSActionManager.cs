using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour {

	private Dictionary <int, SSAction> actions = new Dictionary <int, SSAction> ();
	private List <SSAction> waitingAdd = new List<SSAction> (); 
	private List<int> waitingDelete = new List<int> ();

	// Update is called once per frame
	protected void Update () {
		foreach (SSAction ac in waitingAdd) actions [ac.GetInstanceID ()] = ac;
		waitingAdd.Clear ();

		foreach (KeyValuePair <int, SSAction> kv in actions) {
			SSAction ac = kv.Value;
			if (ac.destory) { 
				waitingDelete.Add(ac.GetInstanceID()); // release action
			} else if (ac.enable) { 
				ac.Update (); // update action
			}
		}

		foreach (int key in waitingDelete) { //waiting for delete
			SSAction ac = actions[key]; 
			actions.Remove(key); //从字典中删除这一动作
			Object.Destroy(ac);  //销毁这一动作
		}
		waitingDelete.Clear (); //清空等待删除的动作列表
	}

	public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager) {
		action.gameobject = gameobject; //游戏对象
		action.transform = gameobject.transform; //游戏对象的transform组件
		action.callback = manager; //动作的回调接口
		waitingAdd.Add (action); //添加到等待执行的动作列表
		action.Start (); //开始动作
	}


	// Use this for initialization
	protected void Start () {
	}

	public int remain_action_count()
    {
		return actions.Count;
    }
}
