using UnityEngine;
using UnityEngine.UI;
using TMPro; // ãªéÊÓËÃÑº TextMeshPro ¶éÒ¤Ø³ãªéÍ§¤ì»ÃÐ¡Íº¹Õé
public class SkillNodeUI : MonoBehaviour
{
    // ÍéÒ§ÍÔ§¶Ö§ Component UI
    [Header("UI References")]
    public Button button;
    public Image background;
    public TextMeshProUGUI skillNameText; // ËÃ×Í public Text skillNameText; ¶éÒäÁèãªé TMP

    // ÍéÒ§ÍÔ§¶Ö§¢éÍÁÙÅ Skill
    [HideInInspector] public Skill skillData;

    // Ê¶Ò¹ÐÊÕ (¡ÓË¹´ÊÕàËÅèÒ¹Õéã¹ Inspector)
    [Header("Colors")]
    public Color colorLearned = Color.yellow;
    public Color colorAvailable = Color.green;
    public Color colorLocked = Color.gray;

    public void Initialize(Skill skill)
    {
        this.skillData = skill;
        skillNameText.text = skill.name; // ËÃ×Í skill.Name; ¢Öé¹ÍÂÙè¡Ñº Skill class

        button.onClick.AddListener(OnNodeClicked);
        UpdateUI();
    }

    public void UpdateUI()
    {
        // µéÍ§ÁÕ property isLearned ã¹¤ÅÒÊ Skill à¾×èÍÃÐºØÊ¶Ò¹Ð»Å´ÅçÍ¤
        // ÊÁÁµÔ: skillData.IsLearned à»ç¹ true àÁ×èÍ¶Ù¡ Unlock

        if (skillData.isUnlocked) // ¶éÒ Skill ¶Ù¡àÃÕÂ¹ÃÙéáÅéÇ (Learned)
        {
            background.color = colorLearned;
            button.interactable = false; // ¤ÅÔ¡ÍÕ¡äÁèä´é
        }
        else if (skillData.isAvailable) // ¶éÒ Skill »Å´ÅçÍ¤ãËéàÃÕÂ¹ÃÙéä´é (Available)
        {
            background.color = colorAvailable;
            button.interactable = true; // ¤ÅÔ¡à¾×èÍàÃÕÂ¹ÃÙé
        }
        else // ¶éÒ Skill ÂÑ§¶Ù¡ÅçÍ¤ (Locked)
        {
            background.color = colorLocked;
            button.interactable = false; // ¤ÅÔ¡äÁèä´é
        }
    }

    private void OnNodeClicked()
    {
        if (skillData.isAvailable && !skillData.isUnlocked)
        {
            // àÃÕÂ¡àÁ¸Í´ Unlock() ã¹¤ÅÒÊ Skill
            skillData.Unlock();

            // á¨é§ãËé UI ·Ø¡µÑÇÍÑ»à´µÊ¶Ò¹Ð (¶éÒÁÕ)
            SkillTreeUI.Instance.RefreshAllUI();
        }
    }
}