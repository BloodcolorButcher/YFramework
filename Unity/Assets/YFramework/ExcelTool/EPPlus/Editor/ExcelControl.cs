using OfficeOpenXml;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


public class ExcelControl : EditorWindow
{
	//窗口实例对象，必须是一个static
	private static ExcelControl m_window;
	// Start is called before the first frame update
	[MenuItem("YFramework/ExcelControl")]
	public static void OpenWindow()
	{
		m_window = GetWindow<ExcelControl>(false, "LanguageTool", false);
		m_window.Show();
	}


	private string _excelPath;
	
	// private LanguageTypeEnum mLanguageEnum = LanguageTypeEnum.All;
		
	
	/// <summary>
	/// 窗口内显示的GUI面板
	/// </summary>
	private void OnGUI()
	{
		//选中excel按钮
		if(GUILayout.Button("填写选中excel的地址"))
		{
			WritePath();
		}
		//标签按钮
		EditorGUILayout.LabelField("ExcelPath", _excelPath);
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("打开Excel位置",GUILayout.Width(100),GUILayout.Height(40)))
		{
			//加载想要选中的文件/文件夹
			Object obj = AssetDatabase.LoadAssetAtPath<Object>("Assets/YFramework/ExcelTool/TextExcel/Language.xlsx");
			EditorGUIUtility.PingObject(obj);
		}
		// mLanguageEnum = (LanguageTypeEnum)EditorGUILayout.EnumPopup("枚举选择", mLanguageEnum);
	
		
		if(GUILayout.Button("打开目录",GUILayout.Width(100),GUILayout.Height(40)))
		{
			//加载想要选中的文件/文件夹
			Object obj = AssetDatabase.LoadAssetAtPath<Object>("Assets/YFramework/Scripts/Languages/TextMgr.cs");
			EditorGUIUtility.PingObject(obj);
		}
		
		if(GUILayout.Button("把excel生成json",GUILayout.Width(100),GUILayout.Height(40)))
		{
			if(_excelPath != null)
			{
				CreateFile();
			}
		}
		GUILayout.EndHorizontal();



	}
	/// <summary>
	/// 写入excel的地址方法
	/// </summary>
	private void WritePath()
	{
		if(Selection.assetGUIDs.Length == 0)
		{
			EditorUtility.DisplayDialog("操作提示！", "选中excel语言文件然后点击按钮，自动填写完成地址！", "确认");
			return;
		}
		string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
		FileInfo existingFile = new FileInfo(path);
		if(existingFile.Exists && Path.GetExtension(path) == ".xlsx")
		{

			_excelPath = path;
			EditorUtility.DisplayDialog("操作提示！", "已正确填写excel文件路径", "确认");
		}
		else
		{
			EditorUtility.DisplayDialog("操作提示！", "请正确选择excel文件", "确认");
		}
	}

	/// <summary>
	/// 创建多语言文件
	/// </summary>
	private void CreateFile()
	{
		ReadExcel();
		AssetDatabase.Refresh();
		EditorUtility.DisplayDialog("操作提示！", "语言文件生成成功，请在Assets/Scripts/LanguagesDataClass文件夹下查看", "确认");
	}
	
	
	
	/// <summary>
	/// 读取excel文件 ReadExcel(string name,int col)
	/// </summary>
	private void ReadExcel()
	{
		
		FileInfo excelFile = new FileInfo(_excelPath);
		//excel操作授权码 ，不知道什么玩意
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

		using (var package = new ExcelPackage(excelFile))
		{
			var sheet = package.Workbook.Worksheets[0];
			var colCount = sheet.Dimension.End.Column;
			var rowCount = sheet.Dimension.End.Row;
			StringBuilder tempKeyStr = new StringBuilder();
		
			
			tempKeyStr.Append("public enum TextKey" +
			               "\n{");
			//string Simplified_Chinese = "[\"mLanguageDataList\":";
			//sheet.Dimension.Start.Row
			//取excel第一行key组成文本索引枚举
			int col = 0;
			for( int r = 2; r <= rowCount; r++ )
			{
				string key = sheet.GetValue<string>(r, 1);
				//把空的和注释去掉
				if(!string.IsNullOrEmpty(key) && key[0] != '#')
				{
					tempKeyStr.Append( "\n\t"+key+"="+col+",");
					col += 1;
				}
			}
			tempKeyStr.Append("\n}");
			CreateFile("TextKey", tempKeyStr);
			
			
			StringBuilder tempValsStr = new StringBuilder();
			StringBuilder tempTypeStr = new StringBuilder();
			tempValsStr.Append("public static class TextVals\n{");
			tempTypeStr.Append("public enum TextType\n{");
			string[] ArrayNames = new string[colCount - 1];
			//取出数组名称
			for( int r = 2; r <= colCount; r++ )
			{
				string key = sheet.GetValue<string>(1, r);
				//把空的和注释去掉
				if(!string.IsNullOrEmpty(key) && key[0] != '#')
				{
					ArrayNames[r - 2] = key;
					tempTypeStr.Append("\n\t" + key+",");
				}
			}

			tempTypeStr.Append("\n}");
		
			CreateFile("TextType", tempTypeStr);
			
			
			for( int cols = 2; cols <= colCount; cols++ )
			{
				
				if (!string.IsNullOrEmpty(ArrayNames[cols - 2]))
				{
					tempValsStr.Append("\n\tpublic static string[] "+ArrayNames[cols - 2]+" = new string[]\n\t{");
				}
				else
				{
					continue;
				}
				for (int r = 2; r <= rowCount; r++)
				{
					string key = sheet.GetValue<string>(r, cols);
					//把空的和注释去掉
					if(!string.IsNullOrEmpty(key) && key[0] != '#')
					{
						tempValsStr.Append("\n\t\t\""+key+"\",");
					}
				}
				tempValsStr.Append("\n\t};");
				
			}
			tempValsStr.Append("\n}");
			
			CreateFile("TextVals", tempValsStr);
			//tempStr.Remove(tempStr.Length - 1, 1);



		}

	}
	
	/// <summary>
	/// 在对应的文件夹下面创建文件
	/// </summary>
	/// <param name="fileName">文件名称</param>
	/// <param name="data">文件内容</param>
	private void CreateFile(string fileName, StringBuilder data)
	{
		try
		{
			string path = $"Assets/YFramework/Scripts/Languages/{fileName}.cs";
			if(!File.Exists(path))
			{
				///创建文件
				//FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
				using (StreamWriter sw = new StreamWriter(path))
				{
					sw.Write(data);
					sw.Close();
				}

			}
			else
			{
				//追加内容
				// FileStream fs = new FileStream(path,FileMode.Append);
				// FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Write);
				//重写内容
				using (StreamWriter sw = new StreamWriter(path))
				{
					sw.Write(data);
					sw.Close();
				}
				
			}
			
		}
		catch (Exception ex)
		{
			EditorUtility.DisplayDialog("错误提示！", ex.Message, "确认");
		}
	}
}