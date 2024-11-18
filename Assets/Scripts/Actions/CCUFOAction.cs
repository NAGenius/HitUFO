using UnityEngine;

public class CCUFOAction : SSAction
{
	public Vector3 origin_position;
	public float speedX;
	public float speedY;
	public float movedTime;

	public static CCUFOAction GetSSAction(float x, float y){
		CCUFOAction action = ScriptableObject.CreateInstance<CCUFOAction> ();
		action.speedX = x;
		action.speedY = y;
		return action;
	}

	Vector3 next_position(float time)
    {
		Vector3 position;
		position.x = origin_position.x + speedX * time;
		position.y = origin_position.y + speedY * time - 0.5f * 5f * time * time;
		position.z = origin_position.z;
		return position;
    }

	public override void Update ()
	{
		Vector3 vec3 = Camera.main.WorldToScreenPoint(transform.position);
		if (!transform.gameObject.activeSelf || vec3.x < -100 || vec3.x > Camera.main.pixelWidth + 100 || vec3.y < -100 || vec3.y > Camera.main.pixelHeight + 100) {
			//如果超出了屏幕范围则销毁对象
			destory = true;  
			callback.SSActionEvent (this);
			return;
		}
		transform.position = next_position(movedTime);
		movedTime = movedTime + Time.deltaTime;
	}

	public override void Start () {
		origin_position = transform.position;
		movedTime = 0;
	}
}

