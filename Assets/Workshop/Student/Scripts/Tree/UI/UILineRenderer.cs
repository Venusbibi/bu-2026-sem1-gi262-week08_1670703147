using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CanvasRenderer))]
public class UILineRenderer : MaskableGraphic
{
    public Transform[] points;

    public float thickness = 10f;
    public bool center = true;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points == null || points.Length < 1)
            return;

        // **¨Ø´àÃÔèÁµé¹ãËÁè:** ãªéµÓáË¹è§¢Í§ GameObject ·ÕèÊ¤ÃÔ»µì¹Õéá¹ºÍÂÙè
        Vector3 startPoint = transform.position;

        // **ÊÃéÒ§ Vertex Template ÊÓËÃÑº Beveled Edges (¶éÒÁÕ)**
        // à¹×èÍ§¨Ò¡â¤Ã§ÊÃéÒ§¡ÒÃÇÒ´à»ÅÕèÂ¹ä» (¨Ò¡¨Ø´à´ÕÂÇä»ÂÑ§ËÅÒÂ¨Ø´)
        // Logic ¡ÒÃÊÃéÒ§ Beveled Edges áººà´ÔÁÍÒ¨µéÍ§»ÃÑº»ÃØ§
        // ã¹â¤é´ãËÁè¹Õé ¨ÐÇÒ´àÊé¹¨Ò¡ startPoint ä»ÂÑ§¨Ø´áÃ¡ã¹ points[0]
        // ¨Ò¡¹Ñé¹ÇÒ´àÊé¹ÃÐËÇèÒ§ points[i] ¡Ñº points[i+1] (¶éÒÁÕ) 

        // *******************************************************************
        // ********* 1. ÊÃéÒ§ Segment áÃ¡: ¨Ò¡ transform.position ä»ÂÑ§ points[0] *********
        // *******************************************************************
        CreateLineSegment(startPoint, points[0].position, vh);

        int index = 0;

        // Add the line segment to the triangles array (Segment 0)
        vh.AddTriangle(index, index + 1, index + 3);
        vh.AddTriangle(index + 3, index + 2, index);


        // *******************************************************************
        // ********* 2. ÊÃéÒ§ Segment ¶Ñ´ä»: ¨Ò¡ points[i] ä»ÂÑ§ points[i+1] *********
        // *******************************************************************
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 p1 = points[i].position;
            Vector3 p2 = points[i + 1].position;

            // ÊÃéÒ§ segment ÃÐËÇèÒ§ points[i] áÅÐ points[i+1]
            CreateLineSegment(p1, p2, vh);

            // ¤Ó¹Ç³ Index ÊÓËÃÑº Segment ãËÁè (àÃÔèÁµé¹·Õè Segment 1)
            // Index ÊÓËÃÑº Segment 1 ¨ÐàÃÔèÁµé¹·Õè vh.currentVertCount ¡èÍ¹àÃÕÂ¡ CreateLineSegment
            // à¹×èÍ§¨Ò¡ vh.currentVertCount ¨Ðà·èÒ¡Ñº 5 ËÅÑ§ Segment áÃ¡¶Ù¡ÊÃéÒ§

            index = (i + 1) * 5; // Index ÊÓËÃÑº Segment ·Õè i+1 (àÃÔèÁµé¹·Õè 5, 10, 15, ...)

            // Add the line segment to the triangles array
            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index);

            // These two triangles create the beveled edges
            // â¤é´à´ÔÁÊÓËÃÑº Beveled Edges ÂÑ§¤§ãªéä´éà¾ÃÒÐÁÑ¹ãªé index ¢Í§ Segment ·ÕèáÅéÇ (index - 5)
            // ÊÓËÃÑº Segment áÃ¡ (i=0) ¨Ðàª×èÍÁµèÍ Segment 0 ¡Ñº Segment 1
            if (i >= 0) // i = 0 ¤×Í Segment ·Õè 1 (àª×èÍÁ Segment 0)
            {
                vh.AddTriangle(index, index - 1, index - 3);
                vh.AddTriangle(index + 1, index - 1, index - 2);
            }
        }
    }

    /// <summary>
    /// Creates a rect from two points that acts as a line segment
    /// </summary>
    /// <param name="point1">The starting point of the segment</param>
    /// <param name="point2">The endint point of the segment</param>
    /// <param name="vh">The vertex helper that the segment is added to</param>
    private void CreateLineSegment(Vector3 point1, Vector3 point2, VertexHelper vh)
    {
        // ¡ÒÃ·Ó§Ò¹ÀÒÂã¹ÂÑ§¤§àËÁ×Í¹à´ÔÁ à¾ÃÒÐ¨Ø´»ÃÐÊ§¤ì¤×Í¡ÒÃÊÃéÒ§ segment ¨Ò¡ 2 ¨Ø´·ÕèãËéÁÒ
        Vector3 offset = center ? (rectTransform.sizeDelta / 2) : Vector2.zero;

        // Create vertex template
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        // Create the start of the segment
        Quaternion point1Rotation = Quaternion.Euler(0, 0, RotatePointTowards(point1, point2) + 90);
        vertex.position = point1Rotation * new Vector3(-thickness / 2, 0);
        vertex.position += point1 - offset;
        vh.AddVert(vertex);
        vertex.position = point1Rotation * new Vector3(thickness / 2, 0);
        vertex.position += point1 - offset;
        vh.AddVert(vertex);

        // Create the end of the segment
        Quaternion point2Rotation = Quaternion.Euler(0, 0, RotatePointTowards(point2, point1) - 90);
        vertex.position = point2Rotation * new Vector3(-thickness / 2, 0);
        vertex.position += point2 - offset;
        vh.AddVert(vertex);
        vertex.position = point2Rotation * new Vector3(thickness / 2, 0);
        vertex.position += point2 - offset;
        vh.AddVert(vertex);

        // Also add the end point
        vertex.position = point2 - offset;
        vh.AddVert(vertex);
    }

    /// <summary>
    /// Gets the angle that a vertex needs to rotate to face target vertex
    /// </summary>
    /// <param name="vertex">The vertex being rotated</param>
    /// <param name="target">The vertex to rotate towards</param>
    /// <returns>The angle required to rotate vertex towards target</returns>
    private float RotatePointTowards(Vector2 vertex, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * (180 / Mathf.PI));
    }
}