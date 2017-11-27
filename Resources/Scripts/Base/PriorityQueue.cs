using System.Collections;
using System.Collections.Generic;
using System;
public class HeapNode<T> where T : new()
{
    T v;

    public HeapNode()
    {

    }

    public HeapNode(T obj)
    {
        v = obj;
    }


    public T Value
    {
        get
        {
            return v;
        }
        set
        {
            v = value;
        }
    }


}

public class PriorityQueue<T> where T : new()
{
    List<HeapNode<T>> heap;

    public delegate bool CompareHandler(HeapNode<T> A, HeapNode<T> B);

    CompareHandler compareHandler;

    public bool defaultCompare(HeapNode<T> A, HeapNode<T> B)
    {
        return true;
    }


    public PriorityQueue()
    {
        heap = new List<HeapNode<T>>();
        heap.Add(null);
        compareHandler = defaultCompare;
    }

    public PriorityQueue(CompareHandler compare)
    {
        compareHandler = compare;
        heap = new List<HeapNode<T>>();
        heap.Add(null);

    }


    public void Enqueue(T obj)
    {
        HeapNode<T> node = new HeapNode<T>(obj);
        heap.Add(node);
        Swim(Count);
    }

    public T Dequeue()
    {
        HeapNode<T> node = heap[1];
        Exchange(1, Count);
        heap.Remove(node);
        Sink(1);
        return node.Value;
    }

    private bool Less(int i, int j)
    {
        return compareHandler(heap[i], heap[j]);
    }

    private bool More(int i, int j)
    {
        return !compareHandler(heap[i], heap[j]);
    }

    private void Exchange(int i, int j)
    {
        HeapNode<T> tmp = heap[i];
        heap[i] = heap[j];
        heap[j] = tmp;
    }

    private void Swim(int k)
    {
        while (k > 1 && Less(k / 2, k))
        {
            Exchange(k / 2, k);
            k = k / 2;
        }
    }

    public T[] ToArray()
    {
        T[] array = new T[Count];
        HeapNode<T>[] nodes = heap.ToArray();
        for (int i = 0; i < Count; i++)
        {
            array[i] = nodes[i + 1].Value;
        }
        return array;
    }

    private void Sink(int k)
    {
        while (2 * k <= Count)
        {
            int j = 2 * k;
            if (j < Count && Less(j, j + 1))
            {
                j++;
            }
            if (!Less(k, j))
            {
                break;
            }
            Exchange(k, j);
            k = j;
        }
    }

    private void Sink(int k, int n)
    {
        while (2 * k <= n)
        {
            int j = 2 * k;
            if (j < n && More(j, j + 1))
            {
                j++;
            }
            if (!More(k, j))
            {
                break;
            }
            Exchange(k, j);
            k = j;
        }
    }

    private T Get(int i)
    {
        if (i < 1 && i > Count)
        {
            throw new IndexOutOfRangeException(" index = " + i);
        }
        return heap[i].Value;
    }

    public void Sort()
    {
        int n = Count;
        for (int i = n / 2; i >= 1; i--)
        {
            Sink(i, n);
        }
        while (n > 1)
        {
            Exchange(1, n--);
            Sink(1, n);
        }
    }


    public int Count
    {
        get
        {
            return heap.Count - 1;
        }
    }

    public Enumerator GetEnumerator()
    {
        //获得迭代器前，排次序
        Sort();
        return new Enumerator(this);
    }


    public class Enumerator : IEnumerator
    {
        PriorityQueue<T> m_queue;
        int pos;

        public Enumerator(PriorityQueue<T> queue)
        {
            m_queue = queue;
            pos = 0;
        }

        public object Current
        {
            get
            {
                return m_queue.Get(pos);
            }
        }

        public bool MoveNext()
        {
            pos++;
            if (pos > m_queue.Count || pos < 1 || m_queue.Get(pos) == null)
            {
                return false;
            }
            return true;
        }

        public void Reset()
        {
            pos = 0;
        }
    }
}
