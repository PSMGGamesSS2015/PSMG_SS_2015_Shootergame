using UnityEngine;
using System.Collections;

public class Tutorial : Quest
{
    protected override void OnQuestStarted()
    {
        QuestFinished();
    }
}
