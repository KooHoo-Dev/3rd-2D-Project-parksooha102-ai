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

        private HashSet<GameObject> checkList = new();

        public void OnInit(Bullet bullet)
        {
            this.bullet = bullet;
            checkList.Clear();
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

            if (hitObject == null)
                return true;

            if (checkList.Contains(hitObject))
                return true;

            if (bullet.genaration < maxGenaration)
            {
                checkList.Add(hitObject);

                float startAngle = -((splitAmount - 1) * splitAngle) * 0.5f;

                for (int i = 0; i < splitAmount; i++)
                {
                    float angleOffset = startAngle + (i * splitAngle);

                    // 2D 회전은 Z축을 기준으로 계산 [3, 4]
                    Quaternion rotation = Quaternion.Euler(0, 0, angleOffset);
                    Vector3 newDirection = rotation * bullet.transform.up;

                    // 오브젝트 풀에서 새 탄환을 가져옴 [5, 6]
                    Bullet subBullet = bullet.BulletPool.Get();

                    // 자식 탄환 설정: 현재 위치에서 새로운 방향으로, 데미지는 배율 적용, 세대 1 증가 [7, 8]
                    subBullet.Set(
                        bullet.transform.position,
                        newDirection,
                        Mathf.RoundToInt(bullet.damage * multiplier),
                        bullet.genaration + 1
                    );
                }

                bullet.Kill();
            }

            return false;
        }
    }
}