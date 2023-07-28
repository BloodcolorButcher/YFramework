using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 持久化数据管理类
/// </summary>
public class PersistentMgr 
{
   
    #region 单例
    private static PersistentMgr _instance = null;

    public static PersistentMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PersistentMgr();
            }
            return _instance;

        }
    }
    #endregion

    /// <summary>
    /// App的设置
    /// </summary>
    public AppSettingData AppSettingData = null;
    private string _appSettingDataPath = null;

    
    /// <summary>
    /// 构造函数
    /// </summary>
    private PersistentMgr()
    {
        // InitAppSetting();
      
    }

    /// <summary>
    /// 初始化AppConfig
    /// </summary>
    public void InitAppSetting()
    {
        _appSettingDataPath = Application.persistentDataPath + "/AppSetting.json";
        if (FileHelper.isExists(_appSettingDataPath))
        {
            string josn = FileHelper.GetFileData(_appSettingDataPath);
            AppSettingData = JsonHelper.FormJosn<AppSettingData>(josn);
            if (AppSettingData == null )
            {
                AppSettingData = new AppSettingData();
                AppSettingData.Init();
                SaveAppSetting();
            }
        }
        else
        {
            AppSettingData = new AppSettingData();
            AppSettingData.Init();
            SaveAppSetting();
        }
    
    }

    /// <summary>
    /// 保存app的设置信息
    /// </summary>
    public void SaveAppSetting()
    {
        string json = JsonHelper.ToJson(AppSettingData);
        FileHelper.SaveFile(_appSettingDataPath,json);
    }


 
  


}
