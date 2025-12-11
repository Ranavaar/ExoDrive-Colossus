using Deforestation.Interaction;
using Deforestation.Recolectables;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Deforestation.UI
{
	public class UIGameController : MonoBehaviour
	{
		#region Fields
		private Inventory _inventory => GameController.Instance.Inventory;
		private InteractionSystem _interactionSystem => GameController.Instance.InteractionSystem;

		[Header("MenuPause")]
		[SerializeField] private GameObject _menuPanel;
		[SerializeField] private Button _resumeButton;
		[SerializeField] private Button _resetButton;
		[SerializeField] private Button _settingsButton;
		[SerializeField] private Button _exitButton;
		[Header("Settings")]
		[SerializeField] private Button _closeButton;
		[SerializeField] private AudioMixer _mixer;
		[SerializeField] private GameObject _settingsPanel;
		[SerializeField] private Slider _masterVolumeSlider;
		[SerializeField] private Slider _musicSlider;
		[SerializeField] private Slider _fxSlider;
		[SerializeField] private Slider _brightness;
		[SerializeField] private Slider _shadows;
		[SerializeField] private Volume _postProcessVolume;
		[SerializeField] private Light _mainLight;
		private ColorAdjustments _colorAdjustments;
		[Header("Message")]
		[SerializeField] private GameObject _warningPanel;
		[Header("Inventory")]
		[SerializeField] private TextMeshProUGUI _crystal1Text;
		[SerializeField] private TextMeshProUGUI _crystal2Text;
		[SerializeField] private TextMeshProUGUI _crystal3Text;
		[Header("Interaction")]
		[SerializeField] private InteractionPanel _interactionPanel;
		[Header("Live")]
		[SerializeField] private Slider _machineSlider;
		[SerializeField] private Slider _playerSlider;
		[Header("FinalGame")]
		[SerializeField] private GameObject _finalPanel;
		[SerializeField] private TextMeshProUGUI _timeText;
		[SerializeField] private TextMeshProUGUI _crystalGreenText;
		[SerializeField] private TextMeshProUGUI _crystalBlueText;
		[SerializeField] private TextMeshProUGUI _crystalRedText;

		private bool _settingsOn = false;
		private bool _menuOn = false;
		private
		#endregion

		#region Unity Callbacks
		void Start()
		{
			_menuPanel.SetActive(false);
			_settingsPanel.SetActive(false);
			_warningPanel.SetActive(false);
			_finalPanel.SetActive(false);

			GameController.Instance.OnFinalGame += EndGame;
			GameController.Instance.OnWarningPanelOn += WarningPanelOn;
			//My Events
			_inventory.OnInventoryUpdated += UpdateUIInventory;
			_interactionSystem.OnShowInteraction += ShowInteraction;
			_interactionSystem.OnHideInteraction += HideInteraction;
			//Menu Events
			_resumeButton.onClick.AddListener(ResumeGame);
			_resetButton.onClick.AddListener(ResetGame);
			_settingsButton.onClick.AddListener(SettingsGame);
			_exitButton.onClick.AddListener(ExitGame);
			//Settings events
			_closeButton.onClick.AddListener(SettingsGame);
			_masterVolumeSlider.onValueChanged.AddListener(MasterVolumeChange);
			_musicSlider.onValueChanged.AddListener(MusicVolumeChange);
			_fxSlider.onValueChanged.AddListener(FXVolumeChange);
			_brightness.onValueChanged.AddListener(BrightnessChange);
			_shadows.onValueChanged.AddListener(ShadowsChange);

			_postProcessVolume.profile.TryGet(out _colorAdjustments);
		}
		public void MenuOn()
		{
			_menuOn = !_menuOn;
			_menuPanel.SetActive(_menuOn);
			if (_menuOn)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
		}

		internal void UpdateMachineHealth(float value)
		{
			_machineSlider.value = value;
		}

		internal void UpdatePlayerHealth(float value)
		{
			_playerSlider.value = value;
		}
		#endregion

		#region Public Methods
		public void ShowInteraction(string message)
		{
			_interactionPanel.Show(message);
		}
		public void HideInteraction()
		{
			_interactionPanel.Hide();
		}
		public void WarningPanelOn(bool PanelOn)
		{
			_warningPanel.SetActive(PanelOn);
		}
		#endregion

		#region Private Methods
		private void UpdateUIInventory()
		{
			if (_inventory.InventoryStack.ContainsKey(RecolectableType.ShootingCrystal))
				_crystal1Text.text = _inventory.InventoryStack[RecolectableType.ShootingCrystal].ToString();
			else
				_crystal1Text.text = "0";
			if (_inventory.InventoryStack.ContainsKey(RecolectableType.DrivingCrystal))
				_crystal2Text.text = _inventory.InventoryStack[RecolectableType.DrivingCrystal].ToString();
			else
				_crystal2Text.text = "0";
			if (_inventory.InventoryStack.ContainsKey(RecolectableType.JumpingCrystal))
				_crystal3Text.text = _inventory.InventoryStack[RecolectableType.JumpingCrystal].ToString();
			else
				_crystal3Text.text = "0";
		}
		private void ResumeGame()
		{
			MenuOn();
		}
		private void ResetGame()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		private void SettingsGame()
		{
			_settingsOn = !_settingsOn;
			_settingsPanel.SetActive(_settingsOn);
		}
		private void ExitGame()
		{
			Application.Quit();
		}

		private void FXVolumeChange(float value)
		{
			_mixer.SetFloat("FXVolume", Mathf.Lerp(-60f, 0f, value));
		}

		private void MusicVolumeChange(float value)
		{
			_mixer.SetFloat("MusicVolume", Mathf.Lerp(-60f, 0f, value));

		}
		private void MasterVolumeChange(float value)
		{
			_mixer.SetFloat("MasterVolume", Mathf.Lerp(-60f, 0f, value));

		}
		private void BrightnessChange(float value)
		{
			if (_colorAdjustments == null)
				return;

			float minExposure = -2f;
			float maxExposure = 2f;

			_colorAdjustments.postExposure.value = Mathf.Lerp(minExposure, maxExposure, value);
		}

		private void ShadowsChange(float value)
		{
			if (_mainLight == null)
			{
				return;
			}

			int level = Mathf.RoundToInt(value);

			switch (level)
			{
				case 0:
					_mainLight.shadows = LightShadows.None;
					break;

				case 1:
					_mainLight.shadows = LightShadows.Soft;
					_mainLight.shadowStrength = 0.5f;
					break;

				case 2:
				default:
					_mainLight.shadows = LightShadows.Soft;
					_mainLight.shadowStrength = 1f;
					break;
			}
		}
		private void EndGame()
		{
			Time.timeScale = 0;
			_finalPanel.SetActive(true);
			_crystalGreenText.text = _inventory.InventoryStack[RecolectableType.ShootingCrystal].ToString();
			_crystalBlueText.text = _inventory.InventoryStack[RecolectableType.DrivingCrystal].ToString();
			_crystalRedText.text = _inventory.InventoryStack[RecolectableType.JumpingCrystal].ToString();

			float totalSeconds = Time.time;

			int hours = Mathf.FloorToInt(totalSeconds / 3600f);
			int minutes = Mathf.FloorToInt((totalSeconds % 3600f) / 60f);
			int seconds = Mathf.FloorToInt(totalSeconds % 60f);

			_timeText.text = "Time Score (H/M/S): " + hours + ":" + minutes + ":" + seconds;

		}
		#endregion
	}

}