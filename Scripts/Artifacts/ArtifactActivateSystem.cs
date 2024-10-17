using UnityEngine;

namespace Artifacts
{
    public class ArtifactActivateSystem : MonoBehaviour
    {
        [Header("Artifacts")]
        [SerializeField] private GameObject thunderbolt;
        [SerializeField] private GameObject plasmaMissileLauncher, launchBtn;
        [SerializeField] private GameObject focusedMissileLauncher, converter;
        [SerializeField] private GameObject resourceElevator;

        public void Activate(Artifact artifact)
        {
            switch (artifact.type)
            {
                case "Unstable Tesla Coil":
                    thunderbolt.SetActive(true);
                    break;
                case "Aegis":
                    ArtifactsManager.Instance.ArtifactsState.IsAegisActive = true;
                    break;
                case "Solus Cores":
                    ArtifactsManager.Instance.ArtifactsState.IsSolusCoresActive = true;
                    SolusCores.Instance.ActivateCoreArtifact();
                    break;
                case "Preon Merger":
                    ArtifactsManager.Instance.ArtifactsState.IsPreonMergerActive = true;
                    break;
                case "Plasma Missile Launcher":
                    plasmaMissileLauncher.SetActive(true);
                    launchBtn.SetActive(true);
                    break;
                case "Focused Missile":
                    focusedMissileLauncher.SetActive(true);
                    ArtifactsManager.Instance.ArtifactsState.IsFocusedMissileActive = true;
                    break;
                case "Protection Piercer":
                    ArtifactsManager.Instance.ArtifactsState.IsProtectionPiercerActive = true;
                    break;
                case "Concentrated Damage":
                    ArtifactsManager.Instance.ArtifactsState.IsConcentratedDamageActive = true;
                    break;
                case "Concentrated Shots":
                    ArtifactsManager.Instance.ArtifactsState.IsConcentratedShotsActive = true;
                    break;
                case "Weak Point":
                    ArtifactsManager.Instance.ArtifactsState.IsWeakPointActive = true;
                    break;
                case "Resource Elevator":
                    resourceElevator.SetActive(true);
                    break;
                case "Converter":
                    converter.SetActive(true);
                    ArtifactsManager.Instance.ArtifactsState.IsConverterActive = true;
                    break;
            }
            
            artifact.Activate();
        }
    }
}