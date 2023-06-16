using TMPro;
using UnityEngine;

namespace Ui
{
    public class ScoreUiController: MonoBehaviour
    {
        [SerializeField]
        private TMP_Text rankTextField;
        
        [SerializeField]
        private TMP_Text nameTextField;
        
        [SerializeField]
        private TMP_Text scoreTextField;

        public void ShowScore(HighScoreData highScoreData)
        {
            rankTextField.text = $"{highScoreData.rank}.";

            nameTextField.text = highScoreData.playerName;

            scoreTextField.text = highScoreData.playerScore.ToString();
        }
    }
}