using System.Collections;
using System.Collections.Generic;
using PrototypingTools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonKeeperRogue.Gameplay.Run
{
    public class RunController : MonoBehaviour
    {
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private List<SceneReference> _scenarios = new();
        [SerializeField] private bool _autoPlayOnStart = true;
    
        private int _currentScenarioIndex;
        private SceneReference _currentScenario;

        private void Awake()
        {
            Scenario.OnScenarioEnd += OnScenarioEnd;
        }

        private void OnDestroy()
        {
            Scenario.OnScenarioEnd -= OnScenarioEnd;
        }

        private void Start()
        {
	        if(_autoPlayOnStart) StartRun();
        }

        private void OnScenarioEnd(Team winner)
        {
            ProcessScenarioOutcome(winner);
        }

        [ContextMenu("Start Run")]
        public void StartRun()
        {
            _currentScenario = null;
            _currentScenarioIndex = 0;
            _playerManager.CreateNewPlayerState();
        
            LoadScenarioAtIndex(_currentScenarioIndex);
        }
    
        private void ProcessScenarioOutcome(Team prevWinner)
        {
            ++_currentScenarioIndex;
            UnloadCurrentScenario();
            
            if (prevWinner == Team.Player && _currentScenarioIndex < _scenarios.Count)
            {
                LoadScenarioAtIndex(_currentScenarioIndex);
                return;
            }
            
            //renable run menu stuff here
        }

        private void LoadScenarioAtIndex(int scenarioIndex)
        {
            StartCoroutine(LoadNextScenarioRoutine(scenarioIndex));
        }

        private IEnumerator LoadNextScenarioRoutine(int scenarioIndex)
        {
            yield return UnloadCurrentScenarioRoutine();
            _currentScenario = _scenarios[scenarioIndex];
        
            yield return SceneManager.LoadSceneAsync(_currentScenario.SceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentScenario.SceneName));
        }

        private void UnloadCurrentScenario()
        {
            StartCoroutine(UnloadCurrentScenarioRoutine());
        }
    
        private IEnumerator UnloadCurrentScenarioRoutine()
        {
            if (_currentScenario != null)
            {
                yield return SceneManager.UnloadSceneAsync(_currentScenario.SceneName);
            }
        }
    }
}
