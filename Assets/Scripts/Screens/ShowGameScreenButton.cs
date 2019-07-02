using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SodaSailor
{
    [RequireComponent(typeof(Button))]
    public class ShowGameScreenButton : MonoBehaviour
    {
        [SerializeField]
        protected GameScreen _gameScreen;

        private GameScreensManager _gameScreensManager;
        private Button _button;

        private void Awake()
        {
            _gameScreensManager = GameObject.Find("GameScreensManager").GetComponent<GameScreensManager>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => { _gameScreensManager.ShowScreen(_gameScreen); });
        }
    }
}