using UnityEngine;

/// <summary>
/// Ability to load a specific level
/// The level must be added to the Build Settings
/// </summary>
public class LoadSpecificLevel : MonoBehaviour
{
	/// <summary>
	/// The name of the level to load
	/// </summary>
	[SerializeField]
	string _levelName;

	public void Execute()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene( _levelName );
	}
}
