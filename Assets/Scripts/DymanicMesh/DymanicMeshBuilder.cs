using System.Collections.Generic;
using UnityEngine;

public class DymanicMeshBuilder
{
    public Vector3 worldPos = Vector3.zero;

    /*
    private BetterList<Vector3> vertList = new BetterList<Vector3>();
    private BetterList<Vector2> offsetList = new BetterList<Vector2>();
    private BetterList<Vector2> uvList = new BetterList<Vector2>();
    private BetterList<Color32> colorList = new BetterList<Color32>();
    private BetterList<int> indiceList = new BetterList<int>();
    */

    private List<Vector3> vertList = new List<Vector3>();
    private List<Vector2> offsetList = new List<Vector2>();
    private List<Vector2> uvList = new List<Vector2>();
    private List<Color> colorList = new List<Color>();
    private List<int> indiceList = new List<int>();

    private Mesh mesh = new Mesh();
    private bool isDirty = false;

    public void AddSprite(Vector3 lt, Vector3 rt, Vector3 rb, Vector3 lb)
    {
        vertList.Add(lt);
        vertList.Add(rt);
        vertList.Add(rb);
        vertList.Add(lb);

        this.isDirty = true;
    }

    public void AddText(float width, float height, float yOffset, Color textColor)
    {
        int indexOffset = vertList.Count;
        Vector3 pos = Vector3.up * yOffset;

        vertList.Add(pos + new Vector3(0, -height * 0.5f, 0));
        vertList.Add(pos + new Vector3(width * 0.5f, -height * 0.5f, 0));
        vertList.Add(pos + new Vector3(0, height * 0.5f, 0));
        vertList.Add(pos + new Vector3(width * 0.5f, height * 0.5f, 0));

        uvList.Add(new Vector2(0f, 0f));
        uvList.Add(new Vector2(1f, 0f));
        uvList.Add(new Vector2(0f, 1f));
        uvList.Add(new Vector2(1f, 1f));

        colorList.Add(textColor);
        colorList.Add(textColor);
        colorList.Add(textColor);
        colorList.Add(textColor);

        {
            indiceList.Add(indexOffset);
            indiceList.Add(indexOffset + 2);
            indiceList.Add(indexOffset + 1);

            indiceList.Add(indexOffset + 1);
            indiceList.Add(indexOffset + 2);
            indiceList.Add(indexOffset + 3);
        }

        this.isDirty = true;
    }

    public void AddProgress(float width, float height, float yOffset, Color32 color)
    {
        int indexOffset = vertList.Count;
        Vector3 pos = Vector3.up * yOffset;

        vertList.Add(pos + new Vector3(0, -height * 0.5f, 0));
        vertList.Add(pos + new Vector3(width * 0.5f, -height * 0.5f, 0));
        vertList.Add(pos + new Vector3(0, height * 0.5f, 0));
        vertList.Add(pos + new Vector3(width * 0.5f, height * 0.5f, 0));

        uvList.Add(new Vector2(0f, 0f));
        uvList.Add(new Vector2(1f, 0f));
        uvList.Add(new Vector2(0f, 1f));
        uvList.Add(new Vector2(1f, 1f));

        // 这个得优化TODO
        //通过顶点颜色的alpha识别是进度条还是文字
        colorList.Add(color + new Color(0, 0, 0, 0.1f));
        colorList.Add(color + new Color(0, 0, 0, 0.1f));
        colorList.Add(color + new Color(0, 0, 0, 0.1f));
        colorList.Add(color + new Color(0, 0, 0, 0.1f));

        {
            indiceList.Add(indexOffset);
            indiceList.Add(indexOffset + 2);
            indiceList.Add(indexOffset + 1);

            indiceList.Add(indexOffset + 1);
            indiceList.Add(indexOffset + 2);
            indiceList.Add(indexOffset + 3);
        }

        this.isDirty = true;
    }

    public Mesh BuildMesh()
    {
        mesh.vertices = vertList.ToArray();
        mesh.colors = colorList.ToArray();
        mesh.uv = uvList.ToArray();
        mesh.SetTriangles(indiceList.ToArray(), 0);
        mesh.MarkDynamic();

        return mesh;
    }

    public void CheckMesh()
    {
        if (!this.isDirty) {
            return;
        }

        this.isDirty = false;
        this.BuildMesh();
    }

    public ref Mesh GetMesh()
    {
        return ref mesh;
    }
}
