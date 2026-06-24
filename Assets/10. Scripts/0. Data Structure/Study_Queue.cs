using UnityEngine;
using System.Collections.Generic;

namespace Study_DataStructure
{
    public class Study_Queue : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Queue();
        }

        // Update is called once per frame
        private void Queue()
        {
            Queue<string> waitingLine;
            waitingLine = new Queue<string>();

            Debug.Log("1. Queue 생성 직후");
            Debug.Log($"대기열의 사람 수: {waitingLine.Count}");
            Debug.Log($"");

            waitingLine.Enqueue("앨리스");
            waitingLine.Enqueue("밥");
            waitingLine.Enqueue("찰리");

            Debug.Log("2. Queue 데이터 추가 후");
            Debug.Log($"대기열의 사람 수: {waitingLine.Count}");
            Debug.Log($"");


            Debug.Log("3. Queue 첫 데이터 확인하기");
            Debug.Log($"대기열의 사람 수: {waitingLine.Count}");
            Debug.Log($"대기열의 첫 사람: {waitingLine.Peek()}");
            Debug.Log($"");

            Debug.Log("4. Queue에서 데이터 꺼내기");
            string servedPreson = waitingLine.Dequeue();
            Debug.Log($"대기열의 사람 수: {waitingLine.Count}");
            Debug.Log($"대기열의 첫 사람: {waitingLine.Peek()}");
            Debug.Log($"서비스 받는 사람: {servedPreson}");
            Debug.Log($"대기열의 사람 수: {waitingLine.Count}");
            Debug.Log($"");

            Debug.Log("5. 대기열의 모든 사람 확인하기");
            foreach(string person in waitingLine)
            {
                Debug.Log($"{person}");
            }
            Debug.Log($"");

        }
    }
}
