using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawInstance
{
    private FontRender2Texture font2Texture = null;
    private DymanicMeshBuilder meshBuilder = null;

    private int instanceCount = 0;
    private Matrix4x4[] matrices = new Matrix4x4[500];
    private MaterialPropertyBlock mpBlock = new MaterialPropertyBlock();
    private Vector4[] shaderParam = new Vector4[500];
    private Material instanceMaterial = null;

    public void Init(Material instanceMat)
    {
        this.instanceMaterial = instanceMat;

        this.BuildMesh();
        this.BuildTextRenderTexture();

        instanceMat.SetTexture("_FontTex", this.font2Texture.TextureArray);
    }

    public void BuildTextRenderTexture()
    {
        GameObject uiCameraObject = GameObject.Find("UICamera");
        Camera uiCamera = uiCameraObject.GetComponent<Camera>();
        GameObject textObject = GameObject.Find("Text");
        TextMeshProUGUI textObj = textObject.GetComponent<TextMeshProUGUI>();
        textObject.SetActive(true);

        var actorMgr = ActorManager.GetInstance();
        var actorCount = actorMgr.GetActorCount();

        // text渲到rt
        int rtWidth = 120;
        int rtHeight = 30;
        int textureDepth = 1023;
        font2Texture = new FontRender2Texture();
        font2Texture.Init(rtWidth, rtHeight, textureDepth);
        uiCamera.enabled = false;
        uiCamera.targetTexture = font2Texture.GetRenderTexture();
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        {
            for (int i = 0; i < actorCount; ++i)
            {
                var actor = actorMgr.GetActor(i);
                textObj.text = actor.GetName();
                font2Texture.Draw(uiCamera);
            }
        }
        stopwatch.Stop();
        Debug.LogFormat("初始化字体耗时:{0}ms", stopwatch.ElapsedMilliseconds);

        textObject.SetActive(false);
    }

    public void BuildMesh()
    {
        // 构建网格
        meshBuilder = new DymanicMeshBuilder();

        // 构建text网格
        float textWidth = 3.5f;
        float textHeight = 0.4f;
        float yOffset = 0f;
        meshBuilder.AddText(textWidth, textHeight, yOffset, Color.blue);

        // 构建进度条网格
        float progressWidth = 3.5f;
        float progressHeight = 0.4f;
        float progressYOffset = 0.4f;
        meshBuilder.AddProgress(progressWidth, progressHeight, progressYOffset, Color.red);
    }

    public void DrawInstanced()
    {
        instanceCount = 0;

        var actorMgr = ActorManager.GetInstance();
        var actorCount = actorMgr.GetActorCount();
        
        for (int index = 0; index < actorCount; index++)
        {
            var actor = actorMgr.GetActor(index);
            matrices[index] = actor.GetMatrix();
            shaderParam[index].x = actor.GetHPProgress();
            shaderParam[index].y = actor.GetNameIndex();
            instanceCount++;
        }
        this.mpBlock.SetVectorArray("_Parms", this.shaderParam);

        meshBuilder.CheckMesh();

        Graphics.DrawMeshInstanced(meshBuilder.GetMesh(), 0, this.instanceMaterial, this.matrices, this.instanceCount, this.mpBlock, UnityEngine.Rendering.ShadowCastingMode.Off, false);
    }
}
