using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GoogleChatManager : MonoBehaviour
{
    const string URL = "https://docs.google.com/spreadsheets/d/1vEIYwJxcvIPef04BImkU2SSzwgj-pPdf3L0S96J1VE8/export?format=tsv&range=B:B";
    const string WebURL = "https://script.google.com/macros/s/AKfycbyeahPfzzwq6n6nEz56vbfOomtKmPO4ArSf3eLn29EBfRWqxSz1CjuZ8zGojTgXrfNg/exec";


    public Text ChatText;   //출력
    public InputField NicknameInput, ChatInput; //입력




    void Start()
    {

#if !UNITY_ANDROID
        Screen.SetResolution(960, 540, false);
#endif
        StartCoroutine(Get());

    }


    IEnumerator Get()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        ChatText.text = data;

        StartCoroutine(Get());
    }



    public void ChatPost()
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", NicknameInput.text);
        form.AddField("chat", ChatInput.text);

        StartCoroutine(Post(form));
    }


    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebURL, form)) // 반드시 using을 써야한다
        {
            yield return www.SendWebRequest();
        }
    }
}