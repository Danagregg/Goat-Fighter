using UnityEngine;

/// <summary>
/// Ability to load the next level in the game.
/// The level order is configured through the Build Settings menu.
/// </summary>
public class LoadNextLevel : MonoBehaviour
{
	/// <summary>
	/// I've chosen to call this Execute so it can be hooked-up through the UI Event System.
	/// There is nothing that calls this unless it is specifically hooked-up!
	/// </summary>
	public void Execute()
	{
		int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
		UnityEngine.SceneManagement.SceneManager.LoadScene( currentScene + 1 );
	}
}
