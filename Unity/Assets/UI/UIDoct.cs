using UnityEngine;
using UnityEngine.UIElements;

public class UIDoct : MonoBehaviour
{

    private UIDocument _document;
    private VisualElement main;
    private Button m_sendBtn,m_cleanBtn;

    private TextField m_sendMsgInput;

    private TextField m_recordMsg;

    private ScrollView _scrollView;
    // Start is called before the first frame update
    void Start()
    {
        _document = GetComponent<UIDocument>();
        main = _document.rootVisualElement;
        m_sendBtn = main.Q<Button>("SendBtn");
        m_cleanBtn = main.Q<Button>("CleanBtn");
        m_sendBtn.RegisterCallback<MouseUpEvent>(BtnClick);
        m_cleanBtn.RegisterCallback<MouseUpEvent>(CleanBtnClick);
        m_sendMsgInput = main.Q<TextField>("SendMsgInput");
        m_recordMsg = main.Q<TextField>("RecMsg");

        _scrollView = main.Q<ScrollView>("ScrollView");


    }
    private void CleanBtnClick(MouseUpEvent evt)
    {
        TextField temp = new TextField() { value = m_sendMsgInput.value,isReadOnly = true,multiline = true};
        temp.AddToClassList("rec-msg-input");
        _scrollView.Add(temp);
        
            // Add(m_recordMsg);
       // m_recordMsg.value += "<align=\"right\">"+m_sendMsgInput.value+"</align>";
        m_sendMsgInput.value = string.Empty;
    }


    private void BtnClick<MouseUpEvent>(MouseUpEvent evt)
    {
     
        // Func<VisualElement> makeItem = () => new Label(text:m_sendMsgInput.value);

        TextField temp = new TextField() { value = m_sendMsgInput.value,isReadOnly = true,multiline = true};
        temp.AddToClassList("send-msg-input");
        _scrollView.Add(temp);
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
