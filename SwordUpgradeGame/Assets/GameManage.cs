using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{
    public int playerMoney = 1000;
    public int level = 1;
    public double upgradePrice = 100;
    public double upgradePercent = 100;
    public double sellPrice = 10;
    public Text PlayerMoneyText, LevelText, UpgradePercentText, UpgradeBtnText, SellBtnText;

    //������
    double upgradePercentChange = 0.98;     //��ȭ Ȯ�� ���
    double upgradePriceChange = 1.5;        //��ȭ ��� ���
    double sellChange = 2.5;                //�Ǹ� ��� ���
    
    // Start is called before the first frame update
    void Start()
    {
        //csv���� ����
    }
    public void resetWeapon()
    {
        level = 1;
        upgradePrice = 1;
        upgradePercent = 100;
        sellPrice = 1;
    }
    public void OnClickUpgrade()    //���׷��̵� �õ� �Լ�
    {
        if (playerMoney >= Convert.ToInt32(upgradePrice))
        {
            playerMoney = playerMoney - Convert.ToInt32(upgradePrice);
            if (UnityEngine.Random.Range(1, 100) <= Convert.ToInt32(upgradePercent))
            {
                upgradePercent = upgradePercent * upgradePercentChange;
                upgradePrice = upgradePrice * upgradePriceChange;
                sellPrice = sellPrice * sellChange;
                level++;
                //��ȭ ����
            }
            else
            {
                resetWeapon();
                //��ȭ ����
            }
            //�α� �� �߰�
        }
        else
        {
            //�α� �� �߰�
        }
    }

    public void OnClickSell()       //�� �Ǹ� �Լ�
    {
        playerMoney = playerMoney + Convert.ToInt32(sellPrice);
        resetWeapon();
        //�α� �� �߰�
    }

    public void OnClickExit()       //���� ���� ��ư �Լ�
    {
        Debug.Log("Exit");
        //csv ���� ����
        //Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoneyText.text = "������: " + playerMoney.ToString() + "G";
        LevelText.text = "Lv." + level.ToString();
        UpgradePercentText.text = upgradePercent.ToString() + "%";
        UpgradeBtnText.text = "!!!��ȭ!!!\n"+upgradePrice.ToString()+"G";
        SellBtnText.text = "�Ǹ�\n"+sellPrice.ToString()+"G";
    }
}
