using UnityEngine;

public static class DataManager
{
    private const string MONEY = "Money";
    private const string SOUND = "IsOnSound";
    private const string MUSIC = "IsOnMusic";
    private const string VIBRATE = "IsOnVibrate";
    
    
    #region  Coin
    public static float GetCoin()
    {
        return PlayerPrefs.GetFloat(MONEY);
    }
    public static void SetCoin(float coin)
    {
        PlayerPrefs.SetFloat(MONEY, coin);
        UpdateManager.Instance.UpdateCurrency();
    }
    public static void EarnCoin(float coin)
    {
        SetCoin(GetCoin() + coin);
        UpdateManager.Instance.UpdateCurrency();
    }
    public static void LostCoin(float coin)
    {
        SetCoin( Mathf.Clamp(GetCoin() - coin, 0, Mathf.Infinity));
        UpdateManager.Instance.UpdateCurrency();
    }
    public static bool SpendCoin(float coin)
    {
        if (GetCoin() >= coin)
        {
            SetCoin(GetCoin() - coin);
            UpdateManager.Instance.UpdateCurrency();
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
    
    #region Sound 
    public static bool GetMusicStatus()
    {
        return PlayerPrefs.GetInt(MUSIC,1)==1;
    }
    public static bool GetSoundStatus()
    {
        return PlayerPrefs.GetInt(SOUND,1) == 1;
    }
    public static bool GetVibrateStatus()
    {
        return PlayerPrefs.GetInt(VIBRATE, 1) == 1;
    }
    public static void SetMusicStatus(bool value)
    {
        PlayerPrefs.SetInt(MUSIC, value?1:0) ;
    }
    public static void SetSoundStatus(bool value)
    {
        PlayerPrefs.SetInt(SOUND,value ? 1 : 0) ;
    }
    public static void SetVibrateStatus(bool value)
    {
        PlayerPrefs.SetInt(VIBRATE,value ? 1 : 0);
    }
    #endregion
}
