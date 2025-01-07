using UnityEngine;
using System;
using MapConfigs;

namespace Managers
{
    public class WaveManager : MonoBehaviour
    {
        public MapConfig mapConfig;
        private int _currentWaveIndex = 0;
        private readonly bool _isWaveInProgress = false;
        
        //Event for wave states: Call to UI 
        public event Action<int> OnWaveStarted; //Call when current wave is completed
        public event Action<int> OnWaveComplete; //Call when next wave is started
        
        void Start()
        {
            
        }

        public void StartNextWave()
        {
            if (_currentWaveIndex < mapConfig.waves.Count && !_isWaveInProgress)
            {
                Debug.Log("Starting wave" + (_currentWaveIndex + 1));
                NotifyWaveStarted(_currentWaveIndex + 1);
                //StartCoroutine(SpawnWave(mapConfig.waves[_currentWaveIndex]));
                _currentWaveIndex++;
            }
            else if (_currentWaveIndex >= mapConfig.waves.Count)
            {
                Debug.Log("All waves finished");
            }
        }

        // private IEnumerator SpawnWave(WaveConfig waveConfig)
        // {
        //     _isWaveInProgress = true;
        //
        //     foreach (var enemyConfig in waveConfig.enemies)
        //     {
        //         for (int i = 0; i < enemyConfig.amount; i++)
        //         {
        //             yield return new WaitForSeconds(enemyConfig.spawnDelay);
        //         }
        //     }
        //     
        //     _isWaveInProgress = false;
        //
        //     NotifyWaveCompleted();
        // }

        private void NotifyWaveStarted(int waveNumber)
        {
            OnWaveStarted?.Invoke(waveNumber);
        }

        private void NotifyWaveCompleted()
        {
            OnWaveComplete?.Invoke(_currentWaveIndex);
        }
        
    }
}
