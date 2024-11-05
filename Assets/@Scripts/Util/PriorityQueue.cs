using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

[Serializable]
public class PriorityQueue<T> where T : IComparable<T>
{
    List<T> _list = new List<T>();

    public void Enqueue(T item)
    {
        _list.Add(item);

        int now = _list.Count - 1;

        while(now > 0)
        {
            int parent = (now - 1) / 2;

            if (_list[now].CompareTo(_list[parent]) > 0)
                break;

            T temp = _list[now];
            _list[now] = _list[parent];
            _list[parent] = temp;
            now = parent;
        }
    }

    public T Dequeue()
    {
        T ret = _list[0];

        int now = 0;
        int end = _list.Count - 1;

        _list[0] = _list[end];
        _list.RemoveAt(end--);

        while(now < end)
        {
            //���� �ڽ� ��� �ε����� ������ �ε������� ũ�� break
            int idx = now * 2 + 1;
            if (idx > end) break;

            //������ �ڽ� ���� ���� �ڽ� ��� �� �� ���� �� ����
            if (idx + 1 <= end && _list[idx].CompareTo(_list[idx + 1]) > 0)
                idx++;

            //���� ��尡 �� ������ break
            if (_list[now].CompareTo(_list[idx]) < 0) break;

            //���� ��尡 �ڽ� ��庸�� ũ�� swap
            T temp = _list[now];
            _list[now] = _list[idx];
            _list[idx] = temp;
            now = idx;
        }

        return ret;
    }

    public bool Contains(T item)
    {
        return _list.Contains(item);
    }

    public void Reposition(T item)
    {
        int idx = _list.IndexOf(item);
        while (idx > 0)
        {
            int parent = (idx - 1) / 2;
            
            if (_list[idx].CompareTo(_list[parent]) > 0) break;

            T temp = _list[idx];
            _list[idx] = _list[parent];
            _list[parent] = temp;
            idx = parent;
        }
    }

    public int Count => _list.Count;
    public List<T> List => _list;
}
