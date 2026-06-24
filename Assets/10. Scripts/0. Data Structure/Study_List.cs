using System;
using System.Collections.Generic; // C#에서 지원하는 자료구조 도구들을 사용할 수 있는 네임스페이스
using UnityEngine;

namespace Study_DataStructure
{
    public class Study_List : MonoBehaviour
    {
        private void Start()
        {
            List();

        }

        private void List()
        {
            List<string> fruitBasket = new List<string>();
            Debug.Log($"1. 리스트 생성 직후 \n" +
                $"쇼핑백 안의 과일 갯수: {fruitBasket.Count}");
            fruitBasket.Add("사과");
            fruitBasket.Add("배");
            fruitBasket.Add("수박");

            Debug.Log($"fruitBasket[0]: {fruitBasket[0]}");

            for (int i = 0; i < fruitBasket.Count; i++)
            {
                Debug.Log($"fruitBasket[{i}]: {fruitBasket[i]}");
            }

            fruitBasket.Remove("배");
            fruitBasket.RemoveAt(0);

            fruitBasket.Add("포도");
            fruitBasket.Add("바나나");

            for (int i = 0; i < fruitBasket.Count; i++)
            {
                Debug.Log($"fruitBasket[{i}]: {fruitBasket[i]}");
            }

            fruitBasket.Insert(1, "블루베리");

            string name = "딸기";

            if (fruitBasket.Contains(name))
            {
                Debug.Log($"List 안에 {name}이 있습니다.");
            }
            else
            {
                Debug.Log($"List 안에 {name}이 없습니다.");
            }
        }
    }
}