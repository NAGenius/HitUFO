using UnityEngine;

public class CCActionManager : SSActionManager, ISSActionCallback {

	public RoundController round_ctrl;
	public CCUFOAction action;
	public DiskFactory factory;

	protected new void Start() {
		round_ctrl = (RoundController)SSDirector.getInstance().currentSceneController;
		round_ctrl.actionManager = this;
		factory = Singleton<DiskFactory>.Instance;
	}

	// Update is called once per frame
	protected new void Update ()
	{
		base.Update ();
	}
		
	//回调，当动作完成时，将飞碟回收
	public void SSActionEvent (SSAction source, SSActionEventType events = SSActionEventType.Competeted, int intParam = 0, string strParam = null, Object objectParam = null)
	{
		factory.FreeDisk(source.transform.gameObject);
	}

	public void Move_Disk(GameObject disk)
    {
		action = CCUFOAction.GetSSAction(disk.GetComponent<Disk_Attributes>().speedX, disk.GetComponent<Disk_Attributes>().speedY);
		RunAction(disk, action, this);
    }
	
}

