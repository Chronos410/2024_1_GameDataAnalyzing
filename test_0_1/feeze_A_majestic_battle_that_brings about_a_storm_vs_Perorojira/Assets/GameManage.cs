using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{

    //�뷱�� ������ ���� ����

    /// <summary> ���ݼ��� ��� (������) </summary>
    double attackPercentChange = 0.93;
    /// <summary> ��ȭ ��� ��� (������) </summary>
 
    //���ݷ�
    int attackDamage = 0;
    int attackDamageUpgradeCost = 0;
    int attackDamageUpgradeValue = 0;
    int attackDamageLevel = 0;

    /// <summary> ��ȭ ��� (������) </summary>
    int attackPercentUpgradeCost = 0;
    /// <summary> ��ȭ�� �����ϴ� ���ݼ��� Ȯ�� (�ջ�) </summary>
    int attackPercentUpgradeValue = 0;
    int attackPercentLevel = 0;

    //���� ����
    int bossLife = 0;
    int life = 3;
    /// <summary> ����ġ </summary>
    int exp = 0;
    /// <summary> ���ݼ��� Ƚ�� </summary>
    int attackCount = 0;



    /// <summary> ���ݼ���Ȯ�� </summary>
    double attackPercent = 0;
    /// <summary> ��ȭ ���� Ȯ���� int������ ��ȯ�� �� </summary>
    int iattackPercent = 0;



    public RawImage lifeLifeFirst, lifeSecond, lifeThird;
    public Text PlayerMoneyText, LevelText, bossLifeText, AttackButtonText,  UpgradePercentButtonText, UpgradeDamageButtonText;    //ȭ�鿡 ���̴� UI�� ������ �� �ְ� ������

    static DateTime nowTime = DateTime.Now;
    string filepath = "";

    /// <summary>
    /// ���� ��ȭ�� 1������ �ʱ�ȭ �ϴ� �Լ�
    /// </summary>
    void GameReset()
    {
        attackDamageLevel = 1;
        attackPercentLevel = 1;


        attackDamage = 1;
        attackDamageUpgradeValue = 6;

        attackPercentUpgradeCost = 45;
        attackDamageUpgradeCost = 53;

        exp = 0;
        life = 3;
        bossLife = 300;
        attackCount = 0;
        attackPercent = 100;

 
    }
    void GameEnd()
    {
        Application.Quit();
    }



    // �߰��� �Լ�: �α׸� ����� �Լ�
    void Logger(string btn, string act)
    {
        nowTime = DateTime.Now;
        using (StreamWriter sw = new StreamWriter(filepath, true, System.Text.Encoding.GetEncoding("utf-8")))
        {
            sw.WriteLine("{0},{1},{2},{3},{4},{5}", nowTime.ToString("yyyy/MM/dd HH:mm:ss:ff"), btn, act, attackCount, exp
              , iattackPercent);
        }
    }
    
    /// <summary>
    /// ��ȭ ��ư�� Ŭ�� �� ����Ǵ� �Լ�
    /// </summary>
    public void OnClickAttack()
    {
        if (life <= 0)
        {
            GameReset();
        }
        

        else if (bossLife > 0 && UnityEngine.Random.Range(0, 100) <= iattackPercent)
        {
            bossLife -= attackDamage;
            attackPercent *= attackPercentChange;
            exp += 30;
            attackCount += 1;
            
        }
        else
        {
            life -= 1;
            
        }

    }


    /// <summary>
    /// �Ǹ� ��ư�� Ŭ�� �� ����Ǵ� �Լ�
    /// </summary>
    public void OnClickUpgradePercent()       //�� �Ǹ� ��ư Ŭ��
    {
      if(exp >= attackPercentUpgradeCost)
        {
            attackPercent += 8;
            exp -= attackPercentUpgradeCost;
            attackPercentLevel += 1;
        }
    }

    public void OnClickUpgradeDamage()
    {
        if (exp >= attackDamageUpgradeCost)
        {
            attackDamage += 6;
            exp -= attackDamageUpgradeCost;
            attackDamageLevel += 1;
        }
    }

    /// <summary>
    /// ���� ���� ��ư�� Ŭ�� �� ����Ǵ� �Լ�
    /// </summary>
    public void OnClickExit()       //���� ���� ��ư Ŭ��
    {
        //!!!���� ���� �α� �ۼ�
        Logger("����", "���� ����");

        Debug.Log("Exit");
        Application.Quit();         //���� �����
    }


    public void Start()
    {
       
        GameReset();

       
    }


    void Update()
    {
        switch (life)
        {
            case 0:
                lifeThird.enabled = false; // �� ��° ��Ʈ �̹��� ��Ȱ��ȭ
                break;
            case 1:
                lifeThird.enabled = true; // �� ��° ��Ʈ �̹��� Ȱ��ȭ
                lifeSecond.enabled = false; // �� ��° ��Ʈ �̹��� ��Ȱ��ȭ
                break;
            case 2:
                lifeThird.enabled = true; // �� ��° ��Ʈ �̹��� Ȱ��ȭ
                lifeSecond.enabled = true; // �� ��° ��Ʈ �̹��� Ȱ��ȭ
                lifeLifeFirst.enabled = false; // ù ��° ��Ʈ �̹��� ��Ȱ��ȭ
                break;
            case 3:
                lifeThird.enabled = true; // �� ��° ��Ʈ �̹��� Ȱ��ȭ
                lifeSecond.enabled = true; // �� ��° ��Ʈ �̹��� Ȱ��ȭ
                lifeLifeFirst.enabled = true; // ù ��° ��Ʈ �̹��� Ȱ��ȭ
                break;
        }


        //double �������� int�� ��ȯ�� ����
        iattackPercent = Convert.ToInt32(attackPercent);




        //UI�� ��Ÿ�� �ؽ�Ʈ
        PlayerMoneyText.text = "����ġ: " + exp.ToString() + "G";
        LevelText.text = "���ݼ���Ƚ��:" + attackCount.ToString();
        
        AttackButtonText.text = "�ٰ��ݡ�\n" +"����Ȯ��"+ iattackPercent.ToString() + "%"+ "\n���ݷ�"+ attackDamage.ToString();

        bossLifeText.text = "����ü��\n" + bossLife.ToString();


        UpgradePercentButtonText.text = "��Ȯ����ȭ!\n" + attackPercentUpgradeCost.ToString() + "exp\nLV."+ attackPercentLevel.ToString();
        UpgradeDamageButtonText.text = "���ݷ°�ȭ!\n" + attackDamageUpgradeCost.ToString()+ "exp\nLV." + attackDamageLevel.ToString();


    }
}
