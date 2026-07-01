using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    public class ModuleE : IBulletModule
    {
        private Bullet bullet;
        private int maxGenaration = 3;
        private int splitAmount = 3;
        private float splitAngle = 45.0f;
        private float multiplier = 0.5f;

        private int currentGeneration = 0;

        private HashSet<GameObject> checkList = new HashSet<GameObject>();

        private static GameObject nextIgnoreObject;
        private static int nextGeneration;
        private static bool isTransferring;
        public void OnInit(Bullet bullet)
        {
            this.bullet = bullet;
            checkList.Clear();

            if (isTransferring)
            {
                currentGeneration = nextGeneration;
                if (nextIgnoreObject != null) checkList.Add(nextIgnoreObject);
            }
            else
            {
                currentGeneration = 0;
            }
        }

        public void OnUpdate()
        {
        }

        public bool OnHitEnemy(RaycastHit2D hit)
        {
            return ProcessSplit(hit);
        }

        public bool OnHitWall(RaycastHit2D hit)
        {
            return ProcessSplit(hit);
        }

        private bool ProcessSplit(RaycastHit2D hit)
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject == null || checkList.Contains(hitObject)) return false;

            if (currentGeneration < maxGenaration)
            {
                checkList.Add(hitObject);

                isTransferring = true;
                nextIgnoreObject = hitObject;
                nextGeneration = currentGeneration + 1;

                float startAngle = -((splitAmount - 1) * splitAngle) * 0.5f;
                for (int i = 0; i < splitAmount; i++)
                {
                    float angleOffset = startAngle + (i * splitAngle);
                    Quaternion rotation = Quaternion.Euler(0, 0, angleOffset);
                    Vector3 newDirection = rotation * bullet.transform.up;

                    Bullet subBullet = bullet.BulletPool.Get();

                    subBullet.Set(bullet.transform.position, newDirection, Mathf.RoundToInt(bullet.damage * multiplier));
                }

                isTransferring = false;
                nextIgnoreObject = null;

                bullet.Kill();
            }
            return false;
        }

    }
}