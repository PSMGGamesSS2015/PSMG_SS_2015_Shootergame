using UnityEngine;
using System.Collections;

public class Tutorial : Quest
{
    void Start()
    {
        base.Start();
    }

    protected override void OnQuestStarted()
    {
        QuestFinished();
    }
}
