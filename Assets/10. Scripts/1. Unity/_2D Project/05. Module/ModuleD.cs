using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    public class ModuleD : IBulletModule
    {
        private Bullet bullet;

        private int maxPenetrateCount = 3;
        private int currentPenetrateCount;

        private HashSet<GameObject> checkList = new HashSet<GameObject>();

        public void OnInit(Bullet bullet)
        {
            this.bullet = bullet;
            currentPenetrateCount = maxPenetrateCount;
            checkList.Clear();
        }

        public void OnUpdate()
        {
        }

        public bool OnHitEnemy(RaycastHit2D hit)
        {
            return ProcessPenetrate(hit);
        }

        public bool OnHitWall(RaycastHit2D hit)
        {
            return false;
        }

        private bool ProcessPenetrate(RaycastHit2D hit)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject == null)
                return true;

            if (checkList.Contains(hitObject))
                return true;

            checkList.Add(hitObject);
            currentPenetrateCount--;

            if (currentPenetrateCount <= 0)
            {
                return false;
            }

            return true;
        }

    }
}