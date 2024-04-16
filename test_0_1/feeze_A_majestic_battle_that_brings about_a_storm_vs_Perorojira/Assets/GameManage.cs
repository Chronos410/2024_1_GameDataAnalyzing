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

    /// <summary> 공격성공 계수 (곱연산) </summary>
    double attackPercentChange = 0.93;
    /// <summary> 강화 비용 계수 (곱연산) </summary>
 
    //공격력
    int attackDamage = 0;
    int attackDamageUpgradeCost = 0;
    int attackDamageUpgradeValue = 0;
    int attackDamageLevel = 0;

    /// <summary> 강화 비용 (곱연산) </summary>
    int attackPercentUpgradeCost = 0;
    /// <summary> 강화시 증가하는 공격성공 확률 (합산) </summary>
    int attackPercentUpgradeValue = 0;
    int attackPercentLevel = 0;

    //변수 선언
    int bossLife = 0;
    int life = 3;
    /// <summary> 경험치 </summary>
    int exp = 0;
    /// <summary> 공격성공 횟수 </summary>
    int attackCount = 0;



    /// <summary> 공격성공확률 </summary>
    double attackPercent = 0;
    /// <summary> 강화 성공 확률을 int형으로 변환한 값 </summary>
    int iattackPercent = 0;



    public RawImage lifeLifeFirst, lifeSecond, lifeThird;
    public Text PlayerMoneyText, LevelText, bossLifeText, AttackButtonText,  UpgradePercentButtonText, UpgradeDamageButtonText;    //화면에 보이는 UI를 수정할 수 있게 연결함

    static DateTime nowTime = DateTime.Now;
    string filepath = "";

    /// <summary>
    /// 무기 강화를 1레벨로 초기화 하는 함수
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



    // 추가된 함수: 로그를 남기는 함수
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
    /// 강화 버튼을 클릭 시 실행되는 함수
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
    /// 판매 버튼을 클릭 시 실행되는 함수
    /// </summary>
    public void OnClickUpgradePercent()       //검 판매 버튼 클릭
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
    /// 게임 종료 버튼을 클릭 시 실행되는 함수
    /// </summary>
    public void OnClickExit()       //게임 종료 버튼 클릭
    {
        //!!!게임 종료 로그 작성
        Logger("종료", "게임 종료");

        Debug.Log("Exit");
        Application.Quit();         //게임 종료됨
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
                lifeThird.enabled = false; // 세 번째 하트 이미지 비활성화
                break;
            case 1:
                lifeThird.enabled = true; // 세 번째 하트 이미지 활성화
                lifeSecond.enabled = false; // 두 번째 하트 이미지 비활성화
                break;
            case 2:
                lifeThird.enabled = true; // 세 번째 하트 이미지 활성화
                lifeSecond.enabled = true; // 두 번째 하트 이미지 활성화
                lifeLifeFirst.enabled = false; // 첫 번째 하트 이미지 비활성화
                break;
            case 3:
                lifeThird.enabled = true; // 세 번째 하트 이미지 활성화
                lifeSecond.enabled = true; // 두 번째 하트 이미지 활성화
                lifeLifeFirst.enabled = true; // 첫 번째 하트 이미지 활성화
                break;
        }


        //double 변수들을 int로 변환해 저장
        iattackPercent = Convert.ToInt32(attackPercent);




        //UI에 나타낼 텍스트
        PlayerMoneyText.text = "경험치: " + exp.ToString() + "G";
        LevelText.text = "공격성공횟수:" + attackCount.ToString();
        
        AttackButtonText.text = "☆공격☆\n" +"성공확률"+ iattackPercent.ToString() + "%"+ "\n공격력"+ attackDamage.ToString();

        bossLifeText.text = "남은체력\n" + bossLife.ToString();


        UpgradePercentButtonText.text = "정확도강화!\n" + attackPercentUpgradeCost.ToString() + "exp\nLV."+ attackPercentLevel.ToString();
        UpgradeDamageButtonText.text = "공격력강화!\n" + attackDamageUpgradeCost.ToString()+ "exp\nLV." + attackDamageLevel.ToString();


    }
}
