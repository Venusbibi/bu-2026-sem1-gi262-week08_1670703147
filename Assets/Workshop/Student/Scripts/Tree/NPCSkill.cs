using Solution;
using UnityEngine;

public class NPCSkill : Identity
{
    public GameObject skillUi;
    public bool canTalk = true;

    public override bool Hit()
    {
        // 돤푭姦뵉脘솝禹톰뮐勵昆루족戾들系⌒촉쵠琨쥡
        if (canTalk)
        {
            Debug.Log("NPCSkill");
            skillUi.SetActive(true);
            return false;
        }
        else
        {
            Debug.Log("I not neet to talk to you");
            return false;
        }
    }
}