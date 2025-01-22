using System;
using Managers;
using MapConfigs;
using UnityEngine;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("References")]
        public MapManager mapManager;
        
        [Header("Interaction Settings")]
        public float interactionDistance = 1f;
        public LayerMask whatIsInteractable;
        
        private PlayerControl _playerControl;
        private TurretBase _currentTurretBase;
        private int _cunrentTurretBaseId = -1;
        
        private bool _isInteracting = false;
        private readonly float _interactionCheckInterval = 0.2f;
        private float _interactionCheckTimer;
        
        public event Action<TurretBase> OnTurretBaseNearby;
        public event Action OnTurretBaseLeft;

        private void Awake()
        {
            _playerControl = new PlayerControl();
            
            _playerControl.PlayerInteraction.Interaction.performed += ctx => Interact();
        }

        private void Update()
        {
            _interactionCheckTimer += Time.deltaTime;
            if (_interactionCheckTimer >= _interactionCheckInterval)
            {
                CheckForNearbyTurretBase();
                _interactionCheckTimer = 0f;
            }
        }

        private void CheckForNearbyTurretBase()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, interactionDistance, whatIsInteractable);
            TurretBase nearestTurretBase = null;
            float minDistance = Mathf.Infinity;
        
            foreach (Collider hit in hits)
            {
                TurretBase turretBase = hit.GetComponent<TurretBase>();
                if (turretBase != null)
                {
                    float distance = Vector3.Distance(transform.position, turretBase.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestTurretBase = turretBase;
                    }
                }
            }
        
            if (nearestTurretBase != null)
            {
                if (_currentTurretBase != nearestTurretBase)
                {
                    _currentTurretBase = nearestTurretBase;
                    foreach (var kvp in mapManager.GetTurretBases())
                    {
                        if (kvp.Value == _currentTurretBase)
                        {
                            _cunrentTurretBaseId = kvp.Key;
                            break;
                        }
                    }
                    OnTurretBaseNearby?.Invoke(_currentTurretBase);
                    if (!_isInteracting)
                    {

                        GameManager.Instance.UIManager.InBattleScreenController.ShowUpgradeButton(_cunrentTurretBaseId);
                        _isInteracting = true;
                    }
                }
            }
            else
            {
                if (_currentTurretBase != null)
                {
                    _currentTurretBase = null;
                    OnTurretBaseLeft?.Invoke();
        
                    if (_isInteracting)
                    {
                        GameManager.Instance.UIManager.InBattleScreenController.HideUpgradeButton();
                        _isInteracting = false;
                    }
                }
            }
        }
        
        private void Interact()
        {
            if (_currentTurretBase == null) return;

            int id = _cunrentTurretBaseId;
            if (_currentTurretBase.Turret == null)
            {
                //Notify to UI to display build turret panel
                //GameManager.Instance.UIManager.InBattleScreenController.BuiltTurret(IdBase);
            }
            else
            {
                //Notify UI to display upgrade turret panel
                //GameManager.Instance.UIManager.InBattleScreenController.UpgradeTurret(IdBase, CurrentTurretType);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionDistance);
        }

        private void OnEnable()
        {
            _playerControl.PlayerInteraction.Enable();
        }

        private void OnDisable()
        {
            _playerControl.PlayerInteraction.Disable();
        }
    }
}
