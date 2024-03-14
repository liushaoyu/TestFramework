using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Tex2DArrayTest : MonoBehaviour
{
    public MeshRenderer render;
    public Texture2D[] textures;
    public ECopyTexMethpd copyTexMethod;   // ��Texrure2D��Ϣ������Texture2DArray������ʹ�õķ�ʽ //

    public enum ECopyTexMethpd
    {
        CopyTexture = 0,     // ʹ�� Graphics.CopyTexture ���� //
        SetPexels = 1,       // ʹ�� Texture2DArray.SetPixels ���� //
    }

    private Material m_mat;

    void Start()
    {
        if (textures == null || textures.Length == 0)
        {
            enabled = false;
            return;
        }

        if (SystemInfo.copyTextureSupport == CopyTextureSupport.None ||
            !SystemInfo.supports2DArrayTextures)
        {
            enabled = false;
            return;
        }

        Texture2DArray texArr = new Texture2DArray(textures[0].width, textures[0].width, textures.Length, textures[0].format, false, false);

        // ���� //
        // Graphics.CopyTexture��ʱ(��λ:Tick): 5914, 8092, 6807, 5706, 5993, 5865, 6104, 5780 //
        // Texture2DArray.SetPixels��ʱ(��λ:Tick): 253608, 255041, 225135, 256947, 260036, 295523, 250641, 266044 //
        // Graphics.CopyTexture ���Կ��� Texture2DArray.SetPixels ���� //
        // Texture2DArray.SetPixels �����ĺ�ʱ��Լ�� Graphics.CopyTexture ��50������ //
        // Texture2DArray.SetPixels ��ʱ��ԭ������Ҫ���������ݴ�cpu����gpu, ԭ��: Call Apply to actually upload the changed pixels to the graphics card //
        // ��Graphics.CopyTextureֻ��gpu�˽��в���, ԭ��: operates on GPU-side data exclusively //
        // ����ʹ��Graphics.CopyTexture������Texture����һ���ô��ǿɲ���ѡԴ����Ϊ�ɶ�д��Ҳ�С�

        //using (Timer timer = new Timer(Timer.ETimerLogType.Tick))
        //{
        if (copyTexMethod == ECopyTexMethpd.CopyTexture)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                // �������ж����� //
                //Graphics.CopyTexture(textures[i], 0, texArr, i);
                Graphics.CopyTexture(textures[i], 0, 0, texArr, i, 0);
            }
        }
        else if (copyTexMethod == ECopyTexMethpd.SetPexels)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                // �������ж����� //
                //texArr.SetPixels(textures[i].GetPixels(), i);
                texArr.SetPixels(textures[i].GetPixels(), i, 0);
            }

            texArr.Apply();
        }
        //}

        texArr.wrapMode = TextureWrapMode.Clamp;
        texArr.filterMode = FilterMode.Bilinear;

        m_mat = render.material;

        m_mat.SetTexture("_TexArr", texArr);
        m_mat.SetFloat("_Index", Random.Range(0, textures.Length));
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 200, 100), "Change Texture"))
        {
            m_mat.SetFloat("_Index", Random.Range(0, textures.Length));
        }
    }
}
