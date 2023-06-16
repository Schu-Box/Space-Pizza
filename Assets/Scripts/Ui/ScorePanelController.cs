using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ui
{
    public class ScorePanelController: MonoBehaviour
    {
        [SerializeField]
        private List<ScoreUiController> scorePanels = new();

        private void Start()
        {
            List<HighScoreData> topFiveData = HighScoreManager.Current.LoadTopFiveScores();

            for (int i = 0; i < 5; i++)
            {
                ScoreUiController scoreUiController = scorePanels[i];

                if (i >= topFiveData.Count)
                {
                    scoreUiController.gameObject.SetActive(false);
                    continue;
                }

                scoreUiController.ShowScore(topFiveData[i]);
            }
        }
    }
}