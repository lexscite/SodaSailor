using System.Collections.Generic;
using UnityEngine;

namespace SodaSailor
{
    public class GameScreensManager : MonoBehaviour
    {
        [SerializeField]
        protected List<GameScreen> _gameScreens;

		private GameScreen _currentScreen;

        private void Awake()
        {
            _gameScreens.ForEach(screen => screen.gameObject.SetActive(false));
			ShowScreen(_gameScreens[0]);
        }

        private void ShowScreen(string id)
        {
            _currentScreen?.gameObject.SetActive(false);
            var screen = _gameScreens.Find(a => a.Id == id);
            screen?.gameObject.SetActive(true);
            _currentScreen = screen;
        }

        public void ShowScreen(GameScreen screen)
		{
			ShowScreen(screen.Id);
		}
    }
}