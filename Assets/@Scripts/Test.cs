using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    PriorityQueue<int> queue;

    void Start()
    {
        queue.Enqueue(10);
        queue.Enqueue(1);
        queue.Enqueue(5);
        queue.Enqueue(3);
        queue.Enqueue(8);

        while (queue.Count > 0)
        {
            print(queue.Dequeue());
        }
    }


    void Update()
    {
        
    }
}
