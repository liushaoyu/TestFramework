using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class FontRender2Texture
{
    public Texture2DArray TextureArray { get; private set; }
    private RenderTexture renderTexture;
    private int width = 0;
    private int height = 0;

    private int slice = 0;

    public void Init(int width, int height, int textureDepth)
    {
        this.width = width;
        this.height = height;

        TextureArray = new Texture2DArray(this.width, this.height, textureDepth, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None);
        TextureArray.name = "FontTextureArray";

        renderTexture = new RenderTexture(width, height, 0, GraphicsFormat.R8G8B8A8_UNorm);
        renderTexture.name = "FontRenderTexture";
        renderTexture.useMipMap = false;
    }

    public int Draw(Camera camera)
    {
        camera.enabled = true;
        {
            RenderTexture.active = renderTexture;
            camera.Render();
            RenderTexture.active = null;
        }
        camera.enabled = false;

        Graphics.CopyTexture(renderTexture, 0, 0, 0, 0, this.width, this.height, TextureArray, slice, 0, 0, 0);
        slice++;
        return slice - 1;
    }

    public RenderTexture GetRenderTexture()
    {
        return renderTexture;
    }
}
