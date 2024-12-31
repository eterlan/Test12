using System;

public class Stack<T>
{
    private T[] items;
    private int count;

    public void Init()
    {
        items = Array.Empty<T>();
    }
    
    /// <summary>
    /// 出栈
    /// </summary>
    /// <returns></returns>
    public T Pop()
    {
        if (count == 0)
        {
            throw new InvalidOperationException("Stack is empty");
        }
        var value = items[count - 1];
        items[count - 1] = default;
        count--;
        return value;
    }

    /// <summary>
    /// 入栈
    /// </summary>
    /// <param name="item"></param>
    public void Push(T item)
    {
        count++;
        if (items.Length < count)
        {
            Array.Resize(ref items,  count);
        }
        items[count - 1] = item;
    }

    public T Peek()
    {
        if (count == 0)
        {
            throw new InvalidOperationException("Stack is empty");
        }
        return items[count - 1];
    }
}
