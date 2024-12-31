
using System;

public class TestEvent
{
    public event Action<object, int> OnFire;    
    public void Test()
    {
        OnFire?.Invoke(this, 5);
    }
}

public class TestEvent2
{
    public void Test()
    {
        var testEvent = new TestEvent();
        testEvent.OnFire += Handler;
    }

    private void Handler(object sender, int value)
    {
        
    }
}
