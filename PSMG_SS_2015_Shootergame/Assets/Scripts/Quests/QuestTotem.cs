﻿using UnityEngine;
using System.Collections;

public class QuestTotem : Quest {

    public Totem[] totems;
    public Totem[] bonusTotems;

    public int maxBonus = 1;

    private int activeTotems = 0;
    private int totalTotems = 0;

    private ArrayList inactiveBonusTotems;
    private ArrayList activeBonusTotems;


    protected override void OnStart()
    {
        totalTotems = totems.Length;
        activeBonusTotems = new ArrayList();
        inactiveBonusTotems = new ArrayList();

        foreach (Totem t in bonusTotems)
        {
            inactiveBonusTotems.Add(t);
        }
    }
	

    protected override void OnUpdate()
    {
        GetActiveTotems();
    }

    void GetActiveTotems()
    {
        activeTotems = 0;

        foreach (Totem t in totems)
        {
            if (t.IsActive())
            {
                activeTotems++;
            }
        }

        if (activeTotems == totalTotems)
        {
            TotemsActivate();
            GetActiveBonusTotems();
        }
        else
        {
            TotemsInactive();
        }
    }

    void TotemsActivate()
    {
        foreach (Totem t in bonusTotems)
        {
            if (!t.IsActive())
            {
                t.SetActivatable(true);
            }
        }
    }

    void TotemsInactive()
    {
        foreach (Totem t in bonusTotems)
        {
            t.SetActivatable(false);
            if (t.IsActive())
            {
                t.Deactivate();
            }
        }
    }

    void GetActiveBonusTotems()
    {
        for (int i = 0; i < inactiveBonusTotems.Count; i++)
        {
            if (((Totem)inactiveBonusTotems[i]).IsActive())
            {
                if (activeBonusTotems.Count + 1 > maxBonus)
                {
                    ((Totem)activeBonusTotems[0]).Deactivate();
                    inactiveBonusTotems.Add(activeBonusTotems[0]);
                    activeBonusTotems.RemoveAt(0);
                }

                activeBonusTotems.Add(inactiveBonusTotems[i]);
                inactiveBonusTotems.Remove(inactiveBonusTotems[i]);
            }
        }
    }
}
