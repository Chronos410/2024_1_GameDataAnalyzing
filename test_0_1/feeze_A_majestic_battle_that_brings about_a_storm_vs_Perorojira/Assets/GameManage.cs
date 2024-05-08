using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManage : MonoBehaviour
{
    
    float timer = 0f;
    float delayInSeconds = 0.1f;

    //���
    /// <summary> ���� ���� Ȯ�� ���� ��� (������) </summary>
    double attackPercentChange = 0.91;

    //�⺻ ����
    /// <summary> �÷��̾� ü�� </summary>
    int life = 3;
    /// <summary> ���� ������ ����ġ </summary>
    int exp = 0;
    /// <summary> ���� ȹ���� ����ġ </summary>
    int totalexp = 0;

    /// <summary> ���� ü�� </summary>
    int bossLife = 0;
  
    /// <summary> ���ݷ� </summary>
    int attackDamage = 0;
    /// <summary> ���� ���� Ȯ�� </summary>
    double attackPercent = 0;
    /// <summary> ���� ���� Ȯ���� int������ ��ȯ�� �� </summary>
    int iattackPercent = 0;


    //����
    /// <summary> ���� ���� Ƚ�� </summary>
    int attackCount = 0;
    /// <summary> ����
    /// [����]
    /// +���ݷ� ��ŭ (�� ���� ��������)
    /// +���� ����ġ ��ŭ (�� ���� ��������)
    /// +3000 (���� Ŭ�����)
    /// </summary>
    int score = 0;


    //��ȭ ����
    /// <summary> ���ݷ� ��ȭ�� �Ҹ��ϴ� ����ġ ��� (�տ���) </summary>
    int attackDamageUpgradeCost = 0;
    /// <summary> ���ݷ� ��ȭ�� �����ϴ� ���ݷ� (�տ���) </summary>
    int attackDamageUpgradeValue = 0;
    /// <summary> ���� ���ݷ� ��ȭ ���� </summary>
    int attackDamageLevel = 0;

    /// <summary> ��Ȯ�� ��ȭ�� �Ҹ��ϴ� ����ġ ��� (�տ���) </summary>
    int attackPercentUpgradeCost = 0;
    /// <summary> ��Ȯ�� ��ȭ�� �����ϴ� ���� ���� Ȯ�� (�տ���) </summary>
    int attackPercentUpgradeValue = 0;
    /// <summary> ���� ��Ȯ�� ��ȭ ���� </summary>
    int attackPercentLevel = 0;
    /// <summary> �÷��̾� ü���� 1 �̻��϶� true </summary>
    bool isPlayerAlive = true;


    
    public GameObject ScoreBoard, BossImage, HitEffect, Null_Image, Nickname_Input, Nickname_Button;
    public RawImage LifeFirst, LifeSecond, LifeThird;
    public Text ExpText, AttackCountText, BossLifeText, AttackButtonText,  UpgradePercentButtonText, UpgradeDamageButtonText, ScoreBoardText, DamageText, PercentText, HitDamegeNumber, Nickname_Input_Text, Nickname_UI_Text;    //ȭ�鿡 ���̴� UI�� ������ �� �ְ� ������
    
    static DateTime nowTime = DateTime.Now;
    string filepath = "";
    string scoreboardsay = "";

    bool isBossHit = false;
    bool isHitEfectOn = false;

    float newX = 0;
    float newY = 0;

    ///�� ���� URL
    const string WebURL = "https://script.google.com/macros/s/AKfycbzMZTQQbQ6pZY0uwDmgohLvMmC1XnO7POrWzFG9vWQEEJJY3AOCKoxHvKRQUICcqro0/exec";
    //�г���
    string nickname;
    int lognum;

    /// <summary>
    /// ������ �ʱ�ȭ �ϴ� �Լ�
    /// </summary>
    void GameReset()
    {
        
        life = 3;
        bossLife = 300;
        exp = 0;
        totalexp = 0;
        attackDamage = 1;
        attackPercent = 100;

        attackDamageLevel = 1;
        attackDamageUpgradeValue = 1;
        attackDamageUpgradeCost = 20;

        attackPercentLevel = 1;
        attackPercentUpgradeValue = 5;
        attackPercentUpgradeCost = 15;

        attackCount = 0;

        ScoreBoard.SetActive(false);
        scoreboardsay = "ERROR";
        score = 0;
    }
    void GameClear()
    {
        score = score + 3000;
        scoreboardsay = "�ڡ�GAME CLEAR�١�";
        ScoreBoard.SetActive(true);
    }
    void GameOver()
    {
        scoreboardsay = "GAME OVER";
        ScoreBoard.SetActive(true);
    }

    //���۽�Ʈ - �α� ���� �Լ�
    public void LogPost(string btn, string act)
    { 
        WWWForm form = new WWWForm();
        form.AddField("nickname", nickname);
        form.AddField("time", nowTime.ToString("yyyy/MM/dd HH:mm:ss:ff"));
        form.AddField("button", btn);
        form.AddField("act", act);
        form.AddField("playerHp", life);
        form.AddField("bossHp", bossLife);
        form.AddField("score", score);
        form.AddField("nowExp", exp);
        form.AddField("totalExp", totalexp);
        form.AddField("attackDamage", attackDamage);
        form.AddField("attackPercent", attackPercent.ToString());
        form.AddField("attackDamageLevel", attackDamageLevel);
        form.AddField("attackPercentLevel", attackPercentLevel);
        StartCoroutine(Post(form));
    }
    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(WebURL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();
        }
    }


    /// <summary>
    /// csv �α׸� ����� �Լ�
    /// </summary>
    /*void LogPost(string btn, string act)
    {

        nowTime = DateTime.Now;
        using (StreamWriter sw = new StreamWriter(filepath, true, System.Text.Encoding.GetEncoding("utf-8")))
        {
            sw.WriteLine($"{nowTime.ToString("yyyy/MM/dd HH:mm:ss:ff")},{btn},{act},{life},{bossLife},{score},{exp},{totalexp},{attackDamage},{attackPercent},{attackDamageLevel},{attackPercentLevel}");
        }
    }*/
    
    /// <summary>
    /// ���� ��ư�� Ŭ�� �� ����Ǵ� �Լ�
    /// </summary>
    public void OnClickAttack()
    {
        //LogPost ("����", "�����׽�Ʈ");
        if (!isPlayerAlive)  //�ٽ��ϱ�
        {
            LogPost("����", "�ٽ��ϱ�");
                
            GameReset();
        }
        else if (UnityEngine.Random.Range(0, 100) <= iattackPercent)    //���� ����
        {
            LogPost("����", "����");
            
            isBossHit = true;

            bossLife -= attackDamage;
            score = attackDamage + exp; 
            attackPercent *= attackPercentChange;
            exp += 30;
            totalexp += 30;
            attackCount++;

        }
        else    //���� ����
        {
            LogPost("����", "����");
            life--;
            attackPercent = 100;
            if(life <= 0)
            {
                GameOver();
            }
        }
        if(bossLife <= 0)
        {
            LogPost("����", "���� Ŭ����");
            GameClear();
        }
    }

    /// <summary>
    /// ���ݷ� ��ȭ ��ư�� Ŭ�� �� ����Ǵ� �Լ�
    /// </summary>
    public void OnClickUpgradeDamage()
    {
        if (exp >= attackDamageUpgradeCost) //����ġ ���
        {

            LogPost("���ݷ� ��ȭ", "����");
            attackDamage += attackDamageUpgradeValue;
            exp -= attackDamageUpgradeCost;
            attackDamageLevel++;

            attackDamageUpgradeCost = attackDamageUpgradeCost + 5 * attackDamageLevel;
            attackDamageUpgradeValue = 4 + attackDamageLevel;
        }
        else
        {
            LogPost("���ݷ� ��ȭ", "����");
        }
    }

    /// <summary>
    /// ��Ȯ�� ��ȭ ��ư�� Ŭ�� �� ����Ǵ� �Լ�
    /// </summary>
    public void OnClickUpgradePercent()
    {
        if(exp >= attackPercentUpgradeCost) //����ġ ���
        {
            LogPost("��Ȯ�� ��ȭ", "����");
            attackPercent += attackPercentUpgradeValue;
            exp -= attackPercentUpgradeCost;
            attackPercentLevel++;

            attackPercentUpgradeCost = attackPercentUpgradeCost + 4 * attackPercentLevel;
            attackPercentUpgradeValue = 4 + attackPercentLevel;
        }
        else
        {
            LogPost("��Ȯ�� ��ȭ", "����");
        }
    }

    /// <summary>
    /// ���� ���� ��ư�� Ŭ�� �� ����Ǵ� �Լ�
    /// </summary>
    public void OnClickExit()
    {
        LogPost("����", "���� ����");
        Debug.Log("Exit");
        Application.Quit();
    }


    public void Start()
    {
        nickname = Nickname_Input_Text.text;
        Null_Image.SetActive(true);
        Nickname_Input.SetActive(true);
        Nickname_Button.SetActive(true);

        HitEffect.SetActive(false);
        filepath = "PlayLog\\Log_" + nowTime.ToString("yyyy_MM_dd_HH_mm_ss_ff") + ".csv";
        nowTime = DateTime.Now;
        using (StreamWriter sw = new StreamWriter(filepath, true, System.Text.Encoding.GetEncoding("utf-8")))
        {
            sw.WriteLine("�ð�,��ư,�ൿ,�÷��̾� ü��,���� ü��,����,���� ����ġ,�� ����ġ,���ݷ�,���� ���� Ȯ��,���ݷ� ��ȭ ����,��Ȯ�� ��ȭ ����");
        }
        LogPost("���� ����", "���� ����");
        GameReset();
    }

    public void OnButtonNickname_Button()
    {
        nickname = Nickname_Input_Text.text;
        Null_Image.SetActive(false);
        Nickname_Input.SetActive(false);
        Nickname_Button.SetActive(false);
        Nickname_UI_Text.text = "ID : "+ nickname;
        LogPost("�г��� ����", "�г��� ����");
    }


    void Update()
    {


        if (isBossHit)
        {
            timer += Time.deltaTime;
            BossImage.SetActive(false);
            if (isHitEfectOn)
            {
                newX = UnityEngine.Random.Range(-300f, 450f);
                newY = UnityEngine.Random.Range(300, -150f);
                isHitEfectOn = false;

            }
            HitEffect.SetActive(true);
            RectTransform rectTransform = HitEffect.GetComponent<RectTransform>(); // HitEffect�� RectTransform ������Ʈ ��������
            rectTransform.anchoredPosition = new Vector2(newX, newY);

            if (timer >= delayInSeconds)
            {
                BossImage.SetActive(true);
                isBossHit = false;
                HitEffect.SetActive(false);
                timer = 0;
                isHitEfectOn = true;

            }
        }



        switch (life)
        {
            case 0:
                LifeThird.enabled = false; // �� ��° ��Ʈ �̹��� ��Ȱ��ȭ
                break;
            case 1:
                LifeThird.enabled = true; // �� ��° ��Ʈ �̹��� Ȱ��ȭ
                LifeSecond.enabled = false; // �� ��° ��Ʈ �̹��� ��Ȱ��ȭ
                break;
            case 2:
                LifeThird.enabled = true; // �� ��° ��Ʈ �̹��� Ȱ��ȭ
                LifeSecond.enabled = true; // �� ��° ��Ʈ �̹��� Ȱ��ȭ
                LifeFirst.enabled = false; // ù ��° ��Ʈ �̹��� ��Ȱ��ȭ
                break;
            case 3:
                LifeThird.enabled = true; // �� ��° ��Ʈ �̹��� Ȱ��ȭ
                LifeSecond.enabled = true; // �� ��° ��Ʈ �̹��� Ȱ��ȭ
                LifeFirst.enabled = true; // ù ��° ��Ʈ �̹��� Ȱ��ȭ
                break;
        }

        


        //double ���� Ȯ���� int�� ��ȯ�� ����
        iattackPercent = Convert.ToInt32(attackPercent);

        //UI�� ��Ÿ�� �ؽ�Ʈ
        ExpText.text = "����ġ: " + exp.ToString();
        BossLifeText.text = bossLife.ToString() + " / 300";
        AttackCountText.text = "���ݼ���Ƚ��:" + attackCount.ToString() + "��";

        if(life <=0)
        {
            isPlayerAlive = false;
            AttackButtonText.text = "�ٽ� ����";
        }
        else
        {
            isPlayerAlive = true;
            AttackButtonText.text = "�ٰ��ݡ�";
        }
        

        UpgradeDamageButtonText.text = "���ݷ� ��ȭ!\n" + attackDamageUpgradeCost.ToString() + "exp\nLV." + attackDamageLevel.ToString()+"\n"+ attackDamageUpgradeValue.ToString()+"����";
        UpgradePercentButtonText.text = "��Ȯ�� ��ȭ!\n" + attackPercentUpgradeCost.ToString() + "exp\nLV."+ attackPercentLevel.ToString() + "\n" + attackPercentUpgradeValue.ToString() + "%����"; ;

        ScoreBoardText.text = $"\n\n��{scoreboardsay}��\n\n\nSCORE : {score}\n\nTOTAL DAMAGE : {300 - bossLife}\n\nATTACK COUNT : {attackCount}";
        DamageText.text = ": "+attackDamage.ToString();
        PercentText.text = ": "+iattackPercent.ToString() + "%";
        HitDamegeNumber.text = "-"+ attackDamage.ToString();

    }
}
