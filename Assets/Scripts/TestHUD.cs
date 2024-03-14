using UnityEngine;
using UnityEngine.U2D;

public class TestHUD : MonoBehaviour
{
    public SpriteAtlas hudAtlas;
    [SerializeField]
    private Material instanceMat;
    [SerializeField]
    public uint actorNum = 500;

    private DrawInstance drawInstance = null;

    void Start()
    {
        ActorManager.GetInstance().Init(actorNum);
        drawInstance = new DrawInstance();
        drawInstance.Init(instanceMat);
    }

    private void LateUpdate()
    {
        drawInstance.DrawInstanced();
    }
}
