using UnityEngine;
using System.Collections;

public class QuestManager : MonoBehaviour {

    private static Quest currentQuest;

    public void SetQuest(Quest quest)
    {
        currentQuest = quest;
    }

    public void ResetCurrentQuest()
    {
        currentQuest.ForceReset();
    }

	public void OnPlayerSpotted() {
		currentQuest.PlayerSpotted ();
	}
}
