using System;

public class LoopControl : SingletonSingleScene<LoopControl>
{
    public Action somethingUpdate;
    
    private void Update()
    {
        somethingUpdate?.Invoke();
    }
}
