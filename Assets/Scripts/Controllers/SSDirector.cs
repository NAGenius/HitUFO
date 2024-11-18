
public class SSDirector : object
{
	// singlton instance
	private static SSDirector _instance;

	public ISceneController currentSceneController { get; set;}
	//public bool running{ get; set;} 

	// get instance anytime anywhare!
	public static SSDirector getInstance() {
		if (_instance == null) {
			_instance = new SSDirector ();
		}
		return _instance;
	}
}
