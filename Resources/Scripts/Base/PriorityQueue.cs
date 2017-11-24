using System.Collections;
using System.Collections.Generic;

public abstract class HeapNode
{

    public abstract bool CompareTo(HeapNode other);

}

public class PriorityQueue
{
    List<HeapNode> heap;

    int size;

    public PriorityQueue()
    {
        heap = new List<HeapNode>();
        heap.Add(null);
        size = 0;
    }


    public void Enqueue(HeapNode node)
    {
        heap.Add(node);
        Swim(Count);
    }

    public HeapNode Dequeue()
    {
        Exchange(1, Count);
        HeapNode node = heap[Count];
        Sink(1);
        heap.Remove(node);
        return node;
    }

    private bool Less(int i, int j)
    {
        return heap[i].CompareTo(heap[j]);
    }

    private void Exchange(int i, int j)
    {
        HeapNode tmp = heap[i];
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


    public int Count
    {
        get
        {
            return heap.Count - 1;
        }
    }


}
