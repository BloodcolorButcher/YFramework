using System;
using System.IO;

/// <summary>
/// 文件IO操作类的辅助类
/// </summary>
public static class FileHelper
{

    /// <summary>
    /// 检查文件夹是否存在，如果不存在创建文件夹
    /// </summary>
    /// <param name="dirPath">文件夹名称</param>
    public static void CheckDir(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }

    /// <summary>
    /// 仅仅检查文件是否存在
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static bool isExists(string filePath)
    {
        return File.Exists(filePath);
    }

    /// <summary>
    /// 获取一个以时间戳命名的文件名
    /// </summary>
    /// <returns></returns>
    public static string GetFileName()
    {

        return DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
    }

    /// <summary>
    /// 创建文件并写入数据。
    /// </summary>
    /// <param name="filePath">文件地址</param>
    /// <param name="data">文件需要写入的数据</param>
    public static void CreateFile(string filePath, string data)
    {
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.Write(data);
            sw.Close();
        }
    }

    /// <summary>
    /// 保存文件并写入数据
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="data"></param>
    public static void SaveFile(string filePath, string data)
    {
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.Write(data);
            sw.Close();
        }
    }

    /// <summary>
    /// 获取文件内容
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string GetFileData(string filePath)
    {

        return File.ReadAllText(filePath);

    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="filePath"></param>
    public static void DelFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

    }




    /// <summary>
    /// 为文件重命名
    /// </summary>
    /// <param name="oldFilePath">原文件名</param>
    /// <param name="directoryPath">文件所在目录的位置</param>
    /// <param name="fileName">文件名</param>
    /// <param name="fileSuffix">文件后缀名</param>
    /// <returns></returns>
    public static string ReplaceFileName(string oldFilePath, string directoryPath, string fileName, string fileSuffix)
    {
        string filePath = directoryPath + fileName + fileSuffix;
        int i = 0;
        if (File.Exists(filePath))
        {
            while (File.Exists(filePath))
            {
                i++;
                filePath = directoryPath + fileName + i + fileSuffix;

            }
        }

        // 要移动的文件名称  文件的新路径和名称
        File.Move(filePath, oldFilePath);
        if (i > 0)
        {
            return fileName + i;
        }

        return fileName;

    }

    /// <summary>
    /// 获取目录下所有的后缀名为 “fileSuffix”的文件名集合
    /// 如果目录不存在，直接创建目录
    /// </summary>
    /// <param name="directoryPath">查询的地址</param>
    /// <param name="fileSuffix"></param>
    /// <returns></returns>
    public static string[] GetDirectoryFiles(string directoryPath, string fileSuffix)
    {
        //判断目录是否存在 如果不存在创建 新的目录
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        return Directory.GetFiles(directoryPath, fileSuffix);
    }

    /// <summary>
    /// 保存数据处理名字重复
    /// </summary>
    /// <param name="directoryPath">文件目录位置</param>
    /// <param name="fileName">文件名</param>
    /// <param name="fileSuffix">文件后缀名</param>
    /// <param name="data">文件中的数据</param>
    public static string SaveFile(string directoryPath, string fileName, string fileSuffix, string data)
    {
        string filePath = directoryPath + fileName + fileSuffix;
        int i = 0;
        if (File.Exists(filePath))
        {

            while (File.Exists(filePath))
            {
                i++;
                filePath = directoryPath + fileName + i + fileSuffix;

            }
        }

        CreateFile(filePath, data);

        if (i > 0)
        {
            return fileName + i;
        }

        return fileName;
    }

   
}

