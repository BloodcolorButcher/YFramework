using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcelTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMgr.SetLanguageType(TextType.CN);
        Debug.Log(TextMgr.GetTextVal(TextKey.TEXT_GUIDE_Action));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
