
public class Singleton<T> where T : new()   //where T : new()为泛型约束，通俗来说就是确保T类型是可以被new的
{
    private static T instance;     //私有的T类型的静态变量
    public static T GetInstance()       //获取实例的函数
    {
        if (instance == null)      //判断实例是否已存在
        {
            instance = new T();    //不存在则创建新的实例
        }
        return instance;   //返回实例
    }
}
