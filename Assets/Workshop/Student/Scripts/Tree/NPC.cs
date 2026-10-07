using Solution;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class NPC : Identity
{
    public DialogueUI dialogueUI;
    public DialogueSequen sequen;
    public bool canTalk = true;

    public override bool Hit()
    {
        // 돤푭姦뵉脘솝禹톰뮐勵昆루족戾들系⌒촉쵠琨쥡
        if (canTalk)
        {
            dialogueUI.Setup(sequen);
            return false;
        }
        else
        {
            Debug.Log("I not neet to talk to you");
            return false;
        }
    }
}