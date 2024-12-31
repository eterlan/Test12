
using System;

public class Queue<T>
{
    private T[] items;
    private int capacity;
    private int head = 0;
    private int tail = 0;
    private int count = 0;

    public Queue(int capacity)
    {
        this.capacity = capacity;
        items = new T[capacity];
    }

    public void Enqueue(T item)
    {
        if (count == capacity)
        {
            throw new InvalidOperationException("Queue is full");
        }
        items[tail] = item;
        tail = (tail + 1) % capacity;
		;
        count++;
    }

    public T Dequeue()
    {
        if (count == 0)
        {
            throw new InvalidOperationException("Queue is empty");
        }
        var value = items[head];
        head = (head + 1) % capacity;
        count--;
        return value;
    }
}
