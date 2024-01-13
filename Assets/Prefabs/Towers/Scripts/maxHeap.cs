using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class maxHeap<T>{
    private T[] heap;
    //if T1 > T2 then 1, if == 0, if T2 > T1 then -1
    private Func<T,T, int> compare;
    private int numbItems;
    public  maxHeap(T[] stuffToHeap, Func<T,T, int> compare){
        heap = stuffToHeap;
        numbItems = stuffToHeap.Length;
        this.compare = compare;
        heapify();
    }
    private void heapify(){
        T temp;
        for (int i = numbItems - 1; i > 0; i--){
            if (i % 2 == 0 && compare(heap[i], heap[(i/2) - 1]) == 1){
                temp = heap[(i/2) - 1];
                heap[(i/2) - 1] = heap[i];
                heap[i] = temp;
            }
            else if (compare(heap[i], heap[((i + 1)/2) - 1]) == 1){
                temp = heap[((i + 1)/2) - 1];
                heap[((i + 1)/2) - 1] = heap[i];
                heap[i] = temp;
            }
        }
    }
    public T pop(){
        T result = heap[0];
        heap[0] = heap[numbItems - 1];
        numbItems--;
        heapify();
        return result;
    }
    public bool isEmpty(){
        return numbItems <= 0;
    }
}