using Unity.Android.Types;
using UnityEngine;

public class Actor
{
    private Matrix4x4 matrix;
    private uint nameIndex = 0;
    private string name = string.Empty;
    private long hp = 100;
    private long maxHP = 100;
    private float hpProgress = 0;
    private Vector3 pos = Vector3.zero;

    public void Init(uint index)
    {
        name = string.Format("name{0}", index);
        nameIndex = index;

        this.RandomPos();
        this.RandomHP();
        this.RandomMatrix();
    }

    public ref Matrix4x4 GetMatrix()
    {
        return ref matrix;
        //Matrix = Matrix4x4.TRS(new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity, Vector3.one);
    }

    public void RandomPos()
    {
        pos.x = Random.Range(-10f, 10f);
        pos.y = Random.Range(-10f, 10f);
        pos.z = 0;
    }

    public void RandomMatrix()
    {
        matrix = Matrix4x4.TRS(this.pos, Quaternion.identity, Vector3.one);
    }

    public void RandomHP()
    {
        hp = Random.Range(50, 100);
        this.hpProgress = hp * 1.0f / this.maxHP;
    }

    public float GetHPProgress()
    {
        return this.hpProgress;
    }

    public uint GetNameIndex()
    {
        return this.nameIndex;
    }

    public string GetName()
    {
        return this.name;
    }
}
