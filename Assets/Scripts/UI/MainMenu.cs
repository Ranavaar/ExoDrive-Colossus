using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	#region Fields
	[SerializeField] private Button _playButton;
	[SerializeField] private Button _exitButton;
	
	#endregion

	#region Unity Callbacks
	// Start is called before the first frame update
	void Start()
    {
		_playButton.onClick.AddListener(PlayGame);
		_exitButton.onClick.AddListener(ExitGame);
	}
	#endregion

	#region Private Methods
	private void PlayGame()
	{
		SceneManager.LoadScene("Main Scene");
	}
	private void ExitGame()
	{
		Application.Quit();
	}
	#endregion

}
