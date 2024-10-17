using DataBase;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class Difficult : MonoBehaviour
    {
        [SerializeField] private TMP_Text diff;
        [SerializeField] private GameObject outline;

        public float[] XPositions;
        private readonly string[] _gameDifficultName = {"Easy", "Normal", "Hard", "Monsoon"};

        private void Start() => UpdateValues();

        public void OnChangeDifficultBtnClick(int difficult)
        {
            DB.Access.gameDifficult = difficult;
            UpdateValues();
        }
        
        private void UpdateValues()
        {
            diff.text = _gameDifficultName[DB.Access.gameDifficult];
            outline.transform.localPosition = new Vector3(XPositions[DB.Access.gameDifficult], 0, 0);
        }
    }
}