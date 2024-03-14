
public class Singleton<T> where T : new()   //where T : new()Ϊ����Լ����ͨ����˵����ȷ��T�����ǿ��Ա�new��
{
    private static T instance;     //˽�е�T���͵ľ�̬����
    public static T GetInstance()       //��ȡʵ���ĺ���
    {
        if (instance == null)      //�ж�ʵ���Ƿ��Ѵ���
        {
            instance = new T();    //�������򴴽��µ�ʵ��
        }
        return instance;   //����ʵ��
    }
}
