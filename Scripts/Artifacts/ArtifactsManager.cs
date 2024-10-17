using System.Collections.Generic;
using DataBase;
using Localization;
using TMPro;
using UnityEngine;

namespace Artifacts
{
    public class ArtifactsManager : MonoBehaviour
    {
        [SerializeField] private GameObject panel, artifactByStartPanel;
    
        [SerializeField] private TMP_Text description;
        private string _defaultDescription;
    
        [SerializeField] private AudioSource lockedSound;

        [SerializeField] private Artifact[] artifacts;
        private Artifact _artifactCache;
        
        private List<int> _artifactsIdForPlayerChoose = new List<int>();

        private int _artifactActivateQueue;
        private List<Artifact> _artifactActivateInteractionQueue = new List<Artifact>();
        

        [SerializeField] private NewArtifact newArtifactSystem;

        private ResourcesManager _rm;
        private ArtifactActivateSystem _artifactActivateSystem;

        public ArtifactsState ArtifactsState = new ArtifactsState();

        private bool _isActivatingArtifactQueueStarted;

        public static ArtifactsManager Instance;

        private void Awake() => Instance = this;

        private void Start()
        {
            if (DB.Access.gameData.chosenUpgrade == 5)
            {
                artifactByStartPanel.SetActive(true);
                OnOpenButtonClick();
            }

            _artifactActivateSystem = GetComponent<ArtifactActivateSystem>();
            _rm = ResourcesManager.Instance;
            _defaultDescription = LocalizationManager.Instance.GetLocalizedValue("CHOOSE ARTIFACT");
        }

        public void AddArtifactToActivateQueue()
        {
            _artifactActivateQueue++;
            
            if (!_isActivatingArtifactQueueStarted) //if dont started => start
            {
                _isActivatingArtifactQueueStarted = true;
                CheckForArtifactInQueue(); 
            }
        }

        public void CheckForArtifactInQueue()
        {
            print(_artifactActivateQueue);
            
            if (_artifactActivateQueue > 0)
            {
                _artifactActivateQueue--;
                StartActivatingArtifactInteraction();
            }
            else _isActivatingArtifactQueueStarted = false; //end queue activating
        }

        private void StartActivatingArtifactInteraction()
        {
            int countFreeArtifacts = 0;
            
            for (int i = 0; i < artifacts.Length; i++)
            {
                if (artifacts[i].currentCount < artifacts[i].maxCount)
                {
                    countFreeArtifacts++;
                }
            }

            if (countFreeArtifacts == 0)
            {
                newArtifactSystem.ShowArtifactConversion();
                return;
            }
            
            
            _artifactsIdForPlayerChoose.Clear();

            int countOfArtifactsForChoose = 0;

            if (countFreeArtifacts >= 3) countOfArtifactsForChoose = 3;
            else countOfArtifactsForChoose = countFreeArtifacts;

            while (_artifactsIdForPlayerChoose.Count < countOfArtifactsForChoose)
            {
                int chosenArtifactID = Random.Range(0, artifacts.Length);
                
                if (artifacts[chosenArtifactID].currentCount < artifacts[chosenArtifactID].maxCount
                    && !_artifactsIdForPlayerChoose.Contains(chosenArtifactID))
                    _artifactsIdForPlayerChoose.Add(chosenArtifactID);
            }

            if (countOfArtifactsForChoose == 3)
            {
                newArtifactSystem.ShowArtifactChoosePanel(artifacts[_artifactsIdForPlayerChoose[0]],
                    artifacts[_artifactsIdForPlayerChoose[1]],
                    artifacts[_artifactsIdForPlayerChoose[2]]);
            }
            else if (countOfArtifactsForChoose == 2)
            {
                newArtifactSystem.ShowArtifactChoosePanel(artifacts[_artifactsIdForPlayerChoose[0]],
                    artifacts[_artifactsIdForPlayerChoose[1]]);
            }
            else if (countOfArtifactsForChoose == 1)
            {
                newArtifactSystem.ShowArtifactChoosePanel(artifacts[_artifactsIdForPlayerChoose[0]]);
            }
        }

        public void ActivateArtifactInteraction(int artChosenID)
        {
            newArtifactSystem.ShowNewArtifact(artifacts[_artifactsIdForPlayerChoose[artChosenID]]);

            _artifactActivateInteractionQueue.Add(artifacts[_artifactsIdForPlayerChoose[artChosenID]]);
            ArtifactFound(artifacts[_artifactsIdForPlayerChoose[artChosenID]]);
        }

        public void ArtifactFound(Artifact artifact) => artifact.currentCount++;

        public void OnOpenButtonClick()
        {
            panel.SetActive(true);
            Time.timeScale = 0;
            ActivateArtifactsInQueue();
        }

        private void ActivateArtifactsInQueue()
        {
            if (_artifactActivateInteractionQueue.Count == 0) return;
            
            for (int i = 0; i < _artifactActivateInteractionQueue.Count; i++)
            {
                _artifactActivateInteractionQueue[i].EnableInteraction();
            }
            
            _artifactActivateInteractionQueue.Clear();
        }

        public void OnCloseButtonClick()
        {
            Time.timeScale = 1f;
            _rm.UpdateValues();
            description.text = _defaultDescription;
            _artifactCache = null;
        }

        public void OnArtifactButtonClick(Artifact artifact)
        {
            description.text = artifact.description;
            _artifactCache = artifact;

            if (artifact.activateState) return;

            _rm.UpdateValues();
            if (artifact.goldCost > 0) _rm.PreviewGoldChanging(artifact.goldCost);
            if (artifact.raspberylCost > 0) _rm.PreviewRaspberylChanging(artifact.raspberylCost);
            if (artifact.sapphireCost > 0) _rm.PreviewSapphireChanging(artifact.sapphireCost);
        }

        public void OnAcceptButtonClick()
        {
            if (_artifactCache == null || !EnoughResources() ||
                _artifactCache.activateState)
            {
                lockedSound.Play();
                return;
            }

            _rm.ChangeGoldValue(_artifactCache.goldCost * -1);
            _rm.ChangeRaspberylValue(_artifactCache.raspberylCost * -1);
            _rm.ChangeSapphireValue(_artifactCache.sapphireCost * -1);

            _artifactActivateSystem.Activate(_artifactCache);

            _artifactCache = null;
        }

        private bool EnoughResources() =>
            _rm.GoldCount >= _artifactCache.goldCost &&
            _rm.RaspberylCount >= _artifactCache.raspberylCost &&
            _rm.SapphireCount >= _artifactCache.sapphireCost;
    }

    public class ArtifactsState
    {
        public bool IsAegisActive;
        public bool IsSolusCoresActive;
        public bool IsPreonMergerActive;
        public bool IsFocusedMissileActive;
        public bool IsProtectionPiercerActive;
        public bool IsConcentratedDamageActive;
        public bool IsConcentratedShotsActive;
        public bool IsWeakPointActive;
        public bool IsConverterActive;
    }
}
