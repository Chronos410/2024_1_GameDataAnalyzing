using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{

    //밸런스 조절용 변수 선언

    /// <summary> 강화 확률 계수 (곱연산) </summary>
    double upgradePercentChange = 0.93;
    /// <summary> 강화 비용 계수 (곱연산) </summary>
    double upgradePriceChange = 1.5;
    /// <summary> 판매 비용 계수 (곱연산) </summary>
    double sellChange = 1.8;

    //변수 선언

    /// <summary> 소지금 </summary>
    int playerMoney = 0;
    /// <summary> 현재 검의 레벨 </summary>
    int level = 0;

    /// <summary> 이번 강화에 소모될 비용 </summary>
    double upgradePrice = 0;
    /// <summary> 강화 소모 비용을 int형으로 변환한 값 </summary>
    int iupgradePrice = 0;

    /// <summary> 강화에 성공할 확률 </summary>
    double upgradePercent = 0;
    /// <summary> 강화 성공 확률을 int형으로 변환한 값 </summary>
    int iupgradePercent = 0;

    /// <summary> 판매시 받는 금액 </summary>
    double sellPrice = 0;
    /// <summary> 판매 금액을 int형으로 변환한 값 </summary>
    int isellPrice = 0;


    public Text PlayerMoneyText, LevelText, UpgradePercentText, UpgradeBtnText, SellBtnText;    //화면에 보이는 UI를 수정할 수 있게 연결함


    /// <summary>
    /// 무기 강화를 1레벨로 초기화 하는 함수
    /// </summary>
    void ResetWeapon()
    {
        level = 1;
        upgradePrice = 10;
        upgradePercent = 100;
        sellPrice = 100;
    }

    void Start()
    {
        //!!!csv파일 생성하는 문구 넣어야됨(맨 윗 라인)
        //!!!게임 시작 로그
        playerMoney = 1000;
        ResetWeapon();
    }

    /// <summary>
    /// 강화 버튼을 클릭 시 실행되는 함수
    /// </summary>
    public void OnClickUpgrade()
    {
        if (playerMoney >= iupgradePrice)   //소지금이 충분한지 체크
        {
            playerMoney = playerMoney - iupgradePrice;  //강화 비용 지불
            if (UnityEngine.Random.Range(0, 100) <= iupgradePercent)        //강화 성공 확률보다 Random (0~100) 값이 낮거나 같은 경우 강화 성공
            {
                //!!!강화 성공 로그 작성
                upgradePercent = upgradePercent * upgradePercentChange;
                upgradePrice = upgradePrice * upgradePriceChange;
                sellPrice = sellPrice * sellChange;
                level++;
                
            }
            else    //강화 실패
            {
                //!!!강화 실패 로그 작성
                ResetWeapon();
            }
        }
        else    //소지금이 강화비용보다 적은 경우
        {
            //!!!소지금 부족 로그 작성
        }
    }

    /// <summary>
    /// 판매 버튼을 클릭 시 실행되는 함수
    /// </summary>
    public void OnClickSell()       //검 판매 버튼 클릭
    {
        //!!!검 판매 로그 작성
        WriteSaleData();
        playerMoney = playerMoney + isellPrice;
        ResetWeapon();
    }


    // 추가된 함수: 무기 판매 데이터를 CSV 파일에 저장
    void WriteSaleData()
    {
        string saleData = string.Format("{0},{1}", level, isellPrice);
        WriteToCSV("SaleData.csv", saleData);
    }

    // 추가된 함수: CSV 파일에 데이터를 쓰는 함수
    void WriteToCSV(string filePath, string data)
    {
        using (StreamWriter sw = File.AppendText(filePath))
        {
            sw.WriteLine(data);
        }
    }



    /// <summary>
    /// 게임 종료 버튼을 클릭 시 실행되는 함수
    /// </summary>
    public void OnClickExit()       //게임 종료 버튼 클릭
    {
        //!!!게임 종료 로그 작성
        //!!!csv 파일 저장?

        Debug.Log("Exit");
        Application.Quit();         //게임 종료됨
    }

    

    void Update()
    {
        //double 변수들을 int로 변환해 저장
        iupgradePercent = Convert.ToInt32(upgradePercent);
        iupgradePrice = Convert.ToInt32(upgradePrice);
        isellPrice = Convert.ToInt32(sellPrice);

        //UI에 나타낼 텍스트
        PlayerMoneyText.text = "소지금: " + playerMoney.ToString() + "G";
        LevelText.text = "Lv." + level.ToString();
        UpgradePercentText.text = iupgradePercent.ToString() + "%";
        UpgradeBtnText.text = "☆강화☆\n"+iupgradePrice.ToString()+"G";
        SellBtnText.text = "판매\n"+isellPrice.ToString()+"G";
    }
}
