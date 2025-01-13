using System;
using Managers;
using MapConfigs;
using UnityEngine;
using System.Collections.Generic;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("References")]
        //public UIManager;
        public MapManager mapManager;
        
        [Header("Interaction Settings")]
        public float interactionDistance = 1f;
        public LayerMask whatIsInteractable;
        
        private PlayerControl _playerControl;
        private TurretBase _currentTurretBase;
        private bool _isInteracting = false;
        
        private float _interactionCheckInterval = 0.5f;
        private float _interactionCheckTimer;
        
        public event Action<TurretBase> OnTurretBaseNearby;
        public event Action OnTurretBaseLeft;

        private void Awake()
        {
            _playerControl = new PlayerControl();

            _playerControl.PlayerInteraction.Interaction.started += ctx => Interact();
            _playerControl.PlayerInteraction.Interaction.canceled += ctx => Interact();
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
                    OnTurretBaseNearby?.Invoke(_currentTurretBase);
                    if (!_isInteracting)
                    {
                        Debug.Log($"Nearby turret base at {_currentTurretBase.name}");
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
                        Debug.Log($"No longer nearby any turret base");
                        _isInteracting = false;
                    }
                }
            }
        }


        private void Interact()
        {
            if (_currentTurretBase == null) return;
            
            if (_currentTurretBase.Turret == null)
            {
                Debug.Log("Turret built!");
                //Notify to UI to display build turret panel
            }
            else
            {
                Debug.Log("Turret upgraded!");
                //Notify UI to display upgrade turret panel
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
