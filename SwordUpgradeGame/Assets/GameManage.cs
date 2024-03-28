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

    //고정값
    double upgradePercentChange = 0.98;     //강화 확률 계수
    double upgradePriceChange = 1.5;        //강화 비용 계수
    double sellChange = 2.5;                //판매 비용 계수
    
    // Start is called before the first frame update
    void Start()
    {
        //csv파일 생성
    }
    public void resetWeapon()
    {
        level = 1;
        upgradePrice = 1;
        upgradePercent = 100;
        sellPrice = 1;
    }
    public void OnClickUpgrade()    //업그레이드 시도 함수
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
                //강화 성공
            }
            else
            {
                resetWeapon();
                //강화 실패
            }
            //로그 줄 추가
        }
        else
        {
            //로그 줄 추가
        }
    }

    public void OnClickSell()       //검 판매 함수
    {
        playerMoney = playerMoney + Convert.ToInt32(sellPrice);
        resetWeapon();
        //로그 줄 추가
    }

    public void OnClickExit()       //게임 종료 버튼 함수
    {
        Debug.Log("Exit");
        //csv 파일 저장
        //Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoneyText.text = "소지금: " + playerMoney.ToString() + "G";
        LevelText.text = "Lv." + level.ToString();
        UpgradePercentText.text = upgradePercent.ToString() + "%";
        UpgradeBtnText.text = "!!!강화!!!\n"+upgradePrice.ToString()+"G";
        SellBtnText.text = "판매\n"+sellPrice.ToString()+"G";
    }
}
