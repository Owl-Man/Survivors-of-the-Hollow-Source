using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class Planet : MonoBehaviour
    {
        private Outline _outline;

        public Sprite icon;
        public int planetID;

        private void Start()
        {
            _outline = GetComponent<Outline>();
            
            if (planetID == -1) ActivatePlanet();
        }

        public void ActivatePlanet() => _outline.enabled = true;

        public void DeactivatePlanet() => _outline.enabled = false;
    }
}