using UnityEngine;

namespace DataBase
{
    [CreateAssetMenu(fileName = "Data", menuName = "Game Data")]
    public class GameData : ScriptableObject
    {
        public int chosenUpgrade;
        
        private int _chosenPlanet;
        
        public int ChosenPlanet
        {
            get => _chosenPlanet;
            set
            {
                if (value == -1) _chosenPlanet = Random.Range(0, planetsDescription.Length);
                else _chosenPlanet = value;
            }
        }
        
        [Header("Game Data")]
        [TextArea(3,10)] public string[] planetsDescription;
        public readonly int[] DaysRequirementForUpgrade = {5, 10, 20, 28, 25, 30};
    }
}