using UnityEngine;

namespace _2DProject
{
    public class ModuleB : IBulletModule
    {
        private Bullet bullet;

        private float trackingRange = 10f;
        private float trackingAngle = 45f;
        private float trackingSpeed = 4f;

        public void OnInit(Bullet bullet)
        {
            this.bullet = bullet;
        }

        public void OnUpdate()
        {
            UpdateTracking();
        }

        public bool OnHitEnemy(RaycastHit2D hit)
        {
            return false;
        }

        public bool OnHitWall(RaycastHit2D hit)
        {
            return false;
        }

        private void UpdateTracking()
        {
            GameObject target = null;

            Collider2D[] hits = Physics2D.OverlapCircleAll(bullet.transform.position, trackingRange);

            float nearestSqr = trackingRange * trackingRange;

            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag(bullet.OpponentTag) == false)
                    continue;

                Vector3 dir = hit.transform.position - bullet.transform.position;
                float sqr = dir.sqrMagnitude;

                if (sqr >= nearestSqr)
                    continue;

                if (Vector3.Angle(bullet.transform.up, dir) > trackingAngle)
                    continue;

                bool blocked = Physics2D.Linecast(bullet.transform.position, hit.transform.position, bullet.WallLayer);

                if (blocked)
                    continue;

                nearestSqr = sqr;
                target = hit.gameObject;
            }

            if (target != null)
            {
                Vector3 targetDir = (target.transform.position - bullet.transform.position).normalized;

                bullet.transform.up = Vector3.RotateTowards(bullet.transform.up, targetDir, trackingSpeed * Time.deltaTime, 0f);
            }
        }
    }
}