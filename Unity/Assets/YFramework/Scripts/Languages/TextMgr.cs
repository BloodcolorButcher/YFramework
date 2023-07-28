public static class TextMgr
{

  
    private static string[] Language = null;
    
   
    
    /// <summary>
    /// 设置当前语言类型
    /// </summary>
    /// <param name="languageType"></param>
    public static void SetLanguageType(TextType languageType)
    {
        switch (languageType)
        {
            case TextType.CN:
                Language = TextVals.CN;
                break;
            case TextType.EN:
                Language = TextVals.EN;
                break;
            case TextType.JP:
                Language = TextVals.JP;
                break;
            case TextType.KOREA:
                Language = TextVals.KOREA;
                break;
            default:
                Language = TextVals.CN;
                break;
        }
    }
    
    /// <summary>
    /// 获取当前文本的值
    /// </summary>
    /// <param name="textKey"></param>
    /// <returns></returns>
    public static string GetTextVal(TextKey textKey)
    {
        return Language[(int)textKey];
    }
    
    
    
}
