using Newtonsoft.Json;

/// <summary>
/// json的辅助类
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// 转化成json字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string ToJson<T>(T t)
    {
        string json = JsonConvert.SerializeObject(t, Formatting.Indented);
        return json;
    }

    /// <summary>
    /// 从字符串转化成类
    /// </summary>
    /// <typeparam name="T">需要反序列化的类</typeparam>
    /// <param name="json">josn字符串</param>
    /// <returns></returns>
    public static T FormJosn<T>(string json)
    {
        T t  = JsonConvert.DeserializeObject<T>(json);
        return t;
    }

}
