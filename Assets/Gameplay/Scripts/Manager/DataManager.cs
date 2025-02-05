using UnityEngine;

public static class DataManager
{
    private const string MONEY = "Money";
    
    
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
}
