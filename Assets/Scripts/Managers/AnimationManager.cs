using UnityEngine;
using System.Collections.Generic;

namespace Managers
{
    public class AnimationManager
    {
        private readonly Dictionary<Animator, Dictionary<string, int>> _animatorHashes = new Dictionary <Animator, Dictionary<string, int>>();
        private static AnimationManager _instance;
        public static AnimationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AnimationManager();
                }
                return _instance;
            }
        }
        
        public void InitializeAnimators(Animator animator, params string[] parameters)
        {
            if (!_animatorHashes.ContainsKey(animator))
            {
                var hashes = new Dictionary<string, int>();
                foreach (var parameter in parameters)
                {
                    hashes[parameter] = Animator.StringToHash(parameter);
                }
                _animatorHashes[animator] = hashes;
            }
        }

        public void SetBool(Animator animator,string parameter, bool value)
        {
            if (_animatorHashes.TryGetValue(animator, out var hashes) && hashes.TryGetValue(parameter, out var hash))
            {
                animator.SetBool(hash, value);
            }
        }

        public void SetTrigger(Animator animator, string parameter)
        {
            if (_animatorHashes.TryGetValue(animator, out var hashes) && hashes.TryGetValue(parameter, out var hash))
            {
                animator.SetTrigger(hash);
            }
        }
    }
}
