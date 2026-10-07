using System.Collections.Generic;
using UnityEngine;
using System.Collections; // ÍéÒ§ÍÔ§¶Ö§ Namespace ¢Í§ SkillBook

public class SkillTreeUI : MonoBehaviour
{
    public static SkillTreeUI Instance;

    [Header("Required References")]
    public SkillBook skillBook;
    public SkillNodeUI skillNodePrefab;
    public Transform skillNodeContainer; // µÓáË¹è§·Õè¨ÐÇÒ§ Node UI

    // **ÊèÇ¹·Õèà¾ÔèÁà¢éÒÁÒÊÓËÃÑº¡ÒÃ¨Ñ´¡ÒÃ Scroll View Content**
    public RectTransform contentSkill; // ÅÒ¡ Content RectTransform ¢Í§ Scroll View ÁÒãÊè

    // µÑÇá»ÃÊÓËÃÑºµÔ´µÒÁ¢Íºà¢µ¢Í§ Skill Node ·Õè¶Ù¡ÊÃéÒ§
    private float minX = 0f;
    private float maxX = 0f;
    private float minY = 0f;
    private float maxY = 0f; // à¹×èÍ§¨Ò¡ Y ¨Ðà»ç¹¤èÒÅº

    // ¡ÓË¹´¢¹Ò´ Node áÅÐÃÐÂÐËèÒ§à¾×èÍãËé¤Ó¹Ç³§èÒÂ¢Öé¹
    private readonly float NODE_WIDTH = 150f;
    private readonly float NODE_HEIGHT = 150f;
    private readonly float X_SPACING = 300f; // ÃÐÂÐËèÒ§ÃÇÁÃÐËÇèÒ§ Node
    private readonly float Y_SPACING = 200f; // ÃÐÂÐËèÒ§ÃÇÁÃÐËÇèÒ§ªÑé¹

    private Dictionary<Skill, SkillNodeUI> skillUIMap = new Dictionary<Skill, SkillNodeUI>();
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        if (skillBook == null || contentSkill == null)
        {
            Debug.LogError("SkillBook ËÃ×Í ContentSkill reference is missing!");
            return;
        }

        StartCoroutine(DelayShowTree());


    }
    IEnumerator DelayShowTree()
    {
        yield return new WaitForSeconds(0.1f);
        // ÃÕà«çµ¢Íºà¢µàÃÔèÁµé¹
        minX = 0f;
        maxX = 0f;
        minY = 0f;
        maxY = 0f;
        // àÃÔèÁÊÃéÒ§ UI Nodes ·Ñé§ËÁ´¨Ò¡ Skill Tree (·ÕèµÓáË¹è§àÃÔèÁµé¹ 0, 0)
        CreateAllSkillNodes(skillBook.attackSkillTree.rootSkill, Vector2.zero);

        // **¤Ó¹Ç³áÅÐ¡ÓË¹´¢¹Ò´ Content ¢Í§ Scroll View**
        CalculateAndSetContentSize();

        // ÍÑ»à´µ UI ¤ÃÑé§áÃ¡
        RefreshAllUI();
    }

    private void CalculateAndSetContentSize()
    {
        if (skillUIMap.Count == 0) return;

        // ¤Ó¹Ç³¤ÇÒÁ¡ÇéÒ§: ¨Ò¡«éÒÂÊØ´¶Ö§¢ÇÒÊØ´ (ºÇ¡¢ÍºàÅç¡¹éÍÂ)
        float contentWidth = (maxX - minX) + NODE_WIDTH + 50f; // +50f ¤×Í Margin

        // ¤Ó¹Ç³¤ÇÒÁÊÙ§: ¨Ò¡¨Ø´ÊÙ§ÊØ´ (0) ¶Ö§¨Ø´µèÓÊØ´ (minY) (ºÇ¡¢ÍºàÅç¡¹éÍÂ)
        // à¹×èÍ§¨Ò¡¤èÒ minY ¨Ðà»ç¹¤èÒÅº àÃÒãªé¤èÒÊÑÁºÙÃ³ì
        float contentHeight = Mathf.Abs(minY) + NODE_HEIGHT + 50f; // +50f ¤×Í Margin

        // ¡ÓË¹´¢¹Ò´ãËé¡Ñº RectTransform ¢Í§ Content
        contentSkill.sizeDelta = new Vector2(contentWidth, contentHeight);

        // ËÁÒÂàËµØ: ÊÓËÃÑº Scroll View á¹ÇµÑé§·Õè Node ¶Ù¡ÇÒ§¨Ò¡º¹Å§ÅèÒ§ (Y à»ç¹Åº) 
        // ¤ÇÃµÑé§¤èÒ Anchor/Pivot ¢Í§ contentSkill à»ç¹ Top-Left (0, 1) 
        // à¾×èÍãËé¡ÒÃ¤Ó¹Ç³¤ÇÒÁÊÙ§·Ó§Ò¹ä´éÍÂèÒ§¶Ù¡µéÍ§
    }

    /// <summary>
    /// Ç¹«éÓà¾×èÍÊÃéÒ§ Skill Node UI µÒÁÅÓ´ÑºªÑé¹
    /// </summary>
    private void CreateAllSkillNodes(Skill currentSkill, Vector2 position)
    {
        if (skillUIMap.ContainsKey(currentSkill)) return;

        // 1. ÊÃéÒ§ Node UI
        SkillNodeUI newNode = Instantiate(skillNodePrefab, skillNodeContainer);
        newNode.Initialize(currentSkill);
        skillUIMap.Add(currentSkill, newNode);

        // ¡ÓË¹´µÓáË¹è§
        RectTransform rt = newNode.GetComponent<RectTransform>();
        rt.localPosition = position;

        // 2. µÔ´µÒÁ¢Íºà¢µ¢Í§ Node ·Õè¶Ù¡ÊÃéÒ§¢Öé¹
        float nodeHalfWidth = NODE_WIDTH / 2f;
        float nodeHalfHeight = NODE_HEIGHT / 2f;

        minX = Mathf.Min(minX, position.x - nodeHalfWidth);
        maxX = Mathf.Max(maxX, position.x + nodeHalfWidth);
        // à¹×èÍ§¨Ò¡ Y àÃÔèÁ¨Ò¡ 0 áÅÐÅ´Å§ (à»ç¹Åº)
        minY = Mathf.Min(minY, position.y - nodeHalfHeight);
        maxY = Mathf.Max(maxY, position.y + nodeHalfHeight);


        // 3. ÊÃéÒ§ Node ÊÓËÃÑº Skill ¶Ñ´ä»ã¹ÅÓ´ÑºªÑé¹ (ÅÙ¡)

        int numChildren = currentSkill.nextSkills.Count;

        // ¤Ó¹Ç³µÓáË¹è§àÃÔèÁµé¹¢Í§ÅÙ¡¤¹áÃ¡ à¾×èÍãËé Node ·Ñé§ËÁ´ÍÂÙè¡Öè§¡ÅÒ§
        float totalWidth = (numChildren - 1) * X_SPACING;
        float startX = position.x - (totalWidth / 2f);

        for (int i = 0; i < numChildren; i++)
        {
            Skill nextSkill = currentSkill.nextSkills[i];

            // µÓáË¹è§ÅÙ¡¶Ñ´ä»¨Ðà¾ÔèÁ¨Ò¡ startX ä»àÃ×èÍÂæ
            Vector2 nextPos = new Vector2(
                startX + (i * X_SPACING),
                position.y - Y_SPACING // Å§ä»Ë¹Öè§ªÑé¹
            );

            CreateAllSkillNodes(nextSkill, nextPos);
        }
    }

    public void RefreshAllUI()
    {
        foreach (var uiNode in skillUIMap.Values)
        {
            uiNode.UpdateUI();
        }
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    // ÍÒ¨à¾ÔèÁ Logic ÊÓËÃÑº¡ÒÃÇÒ´àÊé¹àª×èÍÁµèÍ (Lines) ÃÐËÇèÒ§ Node ä´é·Õè¹Õè
    // «Öè§µéÍ§ãªé Component àªè¹ UILineRenderer ËÃ×Í UI.Graphic ·Õè¡ÓË¹´àÍ§
}