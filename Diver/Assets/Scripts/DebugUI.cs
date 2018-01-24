using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
	string _debugText = "";
	Text _uiText = null;

	static DebugUI _instance = null;

	void Awake()
	{
		if (_instance != null)
		{
			Debug.LogWarning("You can only have one instance of DebugUI! Ignoring additional...");
		}
		else
		{
			_instance = this;
		}

		_uiText = gameObject.GetComponent<Text>();
		if (_uiText == null)
		{
			Debug.LogWarning("You need to have a Text component!");
		}
	}

	void Update()
	{
		if (_uiText != null)
		{
			_uiText.text = _debugText;
			_debugText = "";
		}
	}

	// Call this to add debug text to display next frame.
	public void AddText(string text)
	{
		_debugText += text + "\n";
	}
}
