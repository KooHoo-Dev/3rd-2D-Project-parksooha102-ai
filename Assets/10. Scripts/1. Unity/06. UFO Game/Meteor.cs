
using Study.CardSelector;
using System.Collections.Generic;
using UnityEngine;

namespace Study.LayerAndScroll
{
    public class Meteor : MonoBehaviour
    {
        private float XMove;
        private float YMove;
        public float Speed = 1;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            XMove = Random.Range(-0.5f, 0.5f);
            YMove = Random.Range(-0.5f, 0.5f);
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 moveVector = new Vector3(XMove, YMove);
            moveVector = moveVector * Speed * Time.deltaTime;
            transform.position += moveVector;
        }
    }
}