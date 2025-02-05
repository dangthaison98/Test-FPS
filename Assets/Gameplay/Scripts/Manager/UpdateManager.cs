using System;

public class UpdateManager : SingletonSingleScene<UpdateManager>
{
    public Action currencyAction;
    
    public void UpdateCurrency()
    {
        currencyAction?.Invoke();
    }
}
