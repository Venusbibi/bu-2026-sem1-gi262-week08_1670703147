using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI npcText;
    public Transform choiceContainer;
    public Button choiceButtonPrefab; // ÅÒ¡ Prefab »ØèÁµÑÇàÅ×Í¡ÁÒãÊè
    public GameObject closeButtonDialogue;
    private DialogueSequen InteractNpcSequen;

    // à¡çº»ØèÁ·Õè¶Ù¡ÊÃéÒ§¢Öé¹ à¾×èÍ¹Óä»·ÓÅÒÂ/«èÍ¹ã¹ÀÒÂËÅÑ§
    private List<Button> activeButtons = new List<Button>();

    public void Setup(DialogueSequen sequen)
    {
        //1. Set Dialogue Sequen
        InteractNpcSequen = sequen;
        DialogueNode currrentNode = InteractNpcSequen.tree.root;
        ShowDialogue(currrentNode);

        //Show UI
        gameObject.SetActive(true);
        closeButtonDialogue.SetActive(false);
    }

    public void ShowDialogue(DialogueNode node)
    {
        // 2. set ãËéà»ç¹ âË¹´»Ñ¨¨ØºÑ¹
        InteractNpcSequen.currentNode = node;

        // 3. áÊ´§¢éÍ¤ÇÒÁ¢Í§ NPC
        npcText.text = node.text;

        // 4. ÅéÒ§»ØèÁµÑÇàÅ×Í¡à¡èÒ
        ClearChoices();

        // 5. ÊÃéÒ§»ØèÁµÑÇàÅ×Í¡ãËÁèµÒÁ nexts
        var choices = new List<string>(node.nexts.Keys);
        for (int i = 0; i < choices.Count; i++)
        {
            string choiceText = choices[i];
            CreateChoiceButton(choiceText, i);
        }

    }

    private void CreateChoiceButton(string text, int index)
    {
        Button newButton = Instantiate(choiceButtonPrefab, choiceContainer);

        // µÑé§¤èÒ¢éÍ¤ÇÒÁº¹»ØèÁ
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = text;

        // à¾ÔèÁ Listener àÁ×èÍ¡´»ØèÁ
        // ãªé Lambda Expression à¾×èÍÊè§ index ¡ÅÑºä»ãËé DialogueManager
        newButton.onClick.AddListener(() => OnChoiceSelected(index));

        activeButtons.Add(newButton);
    }

    private void ClearChoices()
    {
        foreach (Button button in activeButtons)
        {
            Destroy(button.gameObject);
        }
        activeButtons.Clear();
    }

    private void OnChoiceSelected(int index)
    {
        // Êè§ index ¢Í§µÑÇàÅ×Í¡·Õè¼ÙéàÅè¹àÅ×Í¡¡ÅÑºä»ãËé DialogueManager ¨Ñ´¡ÒÃ
        InteractNpcSequen.SelectChoice(index);
    }
    public void ShowCloseButtonDialog()
    {
        closeButtonDialogue.gameObject.SetActive(true);
    }
    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        ClearChoices();
    }
}