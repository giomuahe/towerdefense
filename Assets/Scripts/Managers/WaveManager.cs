using UnityEngine;
using MapConfigs;
using System.Collections.Generic;

namespace Managers
{
    public class WaveManager : MonoBehaviour
    {
        public MapConfig mapConfig;

        public List<WaveConfig> _waves;
        public int _currentWaveIndex;

        public static WaveManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Init()
        {
            InitializeWaves();
        }

        private void InitializeWaves()
        {
            if (mapConfig == null || mapConfig.waves == null || mapConfig.waves.Count == 0)
            {
                Debug.LogError("MapConfig or waves in MapConfig is not properly configured");
                _waves = new List<WaveConfig>();
                return;
            }
            
            _waves = new List<WaveConfig>(mapConfig.waves);
            _currentWaveIndex = -1;
            
            Debug.Log($"Initialized {_waves.Count} wave");
        }

        public List<WaveConfig> GetWaves()
        {
            return _waves;
        }

        public WaveConfig GetCurrentWave()
        {
            if (_currentWaveIndex >= 0 && _currentWaveIndex < _waves.Count)
            {
                return _waves[_currentWaveIndex];
            }
            
            Debug.LogWarning($"Current wave index {_currentWaveIndex} not found");
            return null;
        }

        public bool AdvanceToNextWave()
        {
            if (_currentWaveIndex + 1 < _waves.Count)
            {
                _currentWaveIndex++;
                Debug.LogWarning($"Advanced to wave {_currentWaveIndex + 1}");
            }
            Debug.LogWarning($"No more waves to advance to next wave");
            return false;
        }

        public bool AreAllWavesCompleted()
        {
            return _currentWaveIndex >= _waves.Count - 1 ;
        }

        public int GetTotalEnemyCount()
        {
            int totalCount = 0;
            foreach (WaveConfig wave in _waves)
            {
                foreach (var enemySpawn in wave.enemies)
                {
                    totalCount += enemySpawn.amount;
                }
            }
            return totalCount;
        }
    }
}
