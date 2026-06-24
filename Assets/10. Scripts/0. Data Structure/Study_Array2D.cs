using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Study_DataStructure
{
    public class Study_Array2D : MonoBehaviour
    {
        public void Start()
        {
            Array2D();
        }

        public void Array2D()
        {
            int[,] gameBoard = new int[3, 3];
            gameBoard[0, 1] = 3;

            Debug.Log($"1. 크기만 지정하고 나중에 값 할당 \n" +
                $"gameBoard의 행 개수: {gameBoard.GetLength(0)}\n" +
                $"gameBoard의 열 개수: {gameBoard.GetLength(1)}" +
                $"gameBoard의 [0,1]의 값: {gameBoard[0, 1]}");
            string[,] characters =
            {
                {"전사", "마법사", "궁수" },
                {"힐러", "탱커", "도적" }
            };
            Debug.Log($"characters[0, 0]의 값: {characters[0, 0]}");
            Debug.Log($"characters[0, 1]의 값: {characters[0, 1]}");
            Debug.Log($"characters[0, 2]의 값: {characters[0, 2]}");
            Debug.Log($"characters[1, 0]의 값: {characters[1, 0]}");
            Debug.Log($"characters[1, 1]의 값: {characters[1, 1]}");
            Debug.Log($"characters[1, 2]의 값: {characters[1, 2]}");

            //재할당
            characters[1, 0] = "바드";

            // 2. 순회
            string log = "로그창 클릭\n---\n";
            for (int row = 0; row < characters.GetLength(0); row++)
            {
                for (int col = 0; col < characters.GetLength(1); col++)
                {
                    log += $"{characters[row, col]}";
                }
                log += "\n";
            }
            Debug.Log(log);

            foreach(string character in characters)
            {
                Debug.Log(character);
            }
        }
    }
}