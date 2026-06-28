using System.Collections.Generic;
using UnityEngine;

namespace _2DProject
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private string opponentTag;
        [SerializeField] private string allyTag;
        [SerializeField] private LayerMask wallLayer;

        private bool isFired;
        private int damage;

        public bool isModuleA = false;
        public bool isModuleB = false;
        public bool isModuleC = false;
        public bool isModuleD = false;
        public bool isModuleE = false;
        public bool isModuleF = false;

        [Header("ModuleA")]

        [SerializeField] private int maxRicochetCount = 3;
        private int currentRicochetCount;

        [Header("ModuleB")]

        [SerializeField] private float trackingRange = 10f;
        [SerializeField] private float trackingAngle = 45f;
        [SerializeField] private float trackingSpeed = 4f;

        [Header("ModuleD")]

        [SerializeField] private int maxPenetrateCount = 3;

        private HashSet<GameObject> checkList = new HashSet<GameObject>();
        private int currentPenetrateCount;

        public void Set(Vector3 spawnPosition, Vector3 direction, int damage)
        {
            isFired = false;
            gameObject.SetActive(true);
            transform.position = spawnPosition;
            transform.up = direction;
            this.damage = damage;

            ModuleAReset();
            ModuleDReset();
        }

        public void Fire()
        {
            gameObject.SetActive(true);
            isFired = true;
        }

        void Start()
        {
        }

        private void Update()
        {
            if (isFired == false)
                return;

            if (isModuleB)
                UpdateTracking();

            MoveBullet();
        }

        private void MoveBullet()
        {
            float moveDistance = speed * Time.deltaTime;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, moveDistance);

            if (hit.collider != null && hit.collider.gameObject.CompareTag(allyTag) == false)
            {
                ProcessHit(hit);
                return;
            }

            transform.position += transform.up * moveDistance;
        }

        private void ProcessHit(RaycastHit2D hit)
        {
            GameObject target = hit.collider.gameObject;

            // 적 충돌
            if (target.CompareTag(opponentTag))
            {
                ProcessEnemyHit(hit);
                return;
            }

            // 벽 충돌
            if (wallLayer.Contains(target))
            {
                ProcessWallHit(hit);
                return;
            }
        }

        private void ProcessEnemyHit(RaycastHit2D hit)
        {
            if (isModuleC)
            {
                // 대충 폭발
            }

            if (isModuleD)
            {
                ProcessPenetrate(hit);
                return;
            }

            if (isModuleE)
            {
                // 대충 분열
            }

            gameObject.SetActive(false);
        }

        private void ProcessWallHit(RaycastHit2D hit)
        {
            if (isModuleA)
            {
                ProcessRicochet(hit);
                return;
            }

            if (isModuleC)
            {
                // 대충 폭발
            }

            gameObject.SetActive(false);
        }

        #region ModuleA

        private void ModuleAReset()
        {
            currentRicochetCount = maxRicochetCount;
        }

        private void ProcessRicochet(RaycastHit2D hit)
        {
            currentRicochetCount--;

            if (currentRicochetCount < 0)
            {
                gameObject.SetActive(false);
                return;
            }

            transform.position = hit.point + hit.normal * 0.01f;

            Vector3 reflected = Vector3.Reflect(transform.up, hit.normal);

            transform.up = reflected;
        }

        #endregion

        #region ModuleB

        private void UpdateTracking()
        {
            GameObject target = null;

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, trackingRange);

            float nearestSqr = trackingRange * trackingRange;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].CompareTag(opponentTag) == false)
                    continue;

                Vector3 preTargetDir = hits[i].transform.position - transform.position;
                float distanceSqr = preTargetDir.sqrMagnitude;

                if (distanceSqr >= nearestSqr)
                    continue;

                if (Vector3.Angle(transform.up, preTargetDir) > trackingAngle)
                    continue;

                float distance = Mathf.Sqrt(distanceSqr);

                bool isBlocked;

                isBlocked = Physics2D.Linecast(transform.position, hits[i].transform.position, wallLayer);

                if (isBlocked)
                    continue;

                nearestSqr = distanceSqr;
                target = hits[i].gameObject;
            }

            if (target != null)
            {
                Vector3 targetDir = (target.transform.position - transform.position);
                targetDir = targetDir.normalized;

                transform.up = Vector3.RotateTowards(transform.up, targetDir, trackingSpeed * Time.deltaTime, 0.0f);
            }
        }

        #endregion

        #region ModuleD
        private void ModuleDReset()
        {
            currentPenetrateCount = maxPenetrateCount;
            checkList.Clear();
        }

        private void ProcessPenetrate(RaycastHit2D hit)
        {
            if (currentPenetrateCount < 0)
            {
                gameObject.SetActive(false);
                return;
            }

            GameObject hitObject = hit.collider.gameObject;

            if (hitObject == null) 
                return;

            if (checkList.Contains(hitObject)) 
                return;

            checkList.Add(hitObject);
            currentPenetrateCount--;
        }

        #endregion

    }
}