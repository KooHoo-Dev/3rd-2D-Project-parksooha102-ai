using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

namespace _2DProject
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 1.0f;
        [field: SerializeField] public string OpponentTag {  get; private set; }
        [field: SerializeField] public string AllyTag { get; private set; }
        [field: SerializeField] public LayerMask WallLayer { get; private set; }
        [field: SerializeField] public BulletPool BulletPool { get; private set; }

        private bool isFired;
        public int damage {  get; private set; }
        public int genaration { get; private set; }

        private List<IBulletModule> modules = new();

        public void Set(Vector3 spawnPosition, Vector3 direction, int damage, int genaration)
        {
            isFired = false;
            gameObject.SetActive(true);
            transform.position = spawnPosition;
            transform.up = direction;
            this.damage = damage;
            this.genaration = genaration;

            foreach (IBulletModule m in modules)
            {
                m.OnInit(this);
            }
        }

        public void Fire()
        {
            gameObject.SetActive(true);
            isFired = true;
        }
        private void Start()
        {
            AddModule(new ModuleA());
            //AddModule(new ModuleB());
            //AddModule(new ModuleC());
            AddModule(new ModuleD());
            AddModule(new ModuleE());
            //AddModule(new ModuleF());
        }

        private void Update()
        {
            if (isFired == false)
                return;

            foreach (IBulletModule m in modules)
                m.OnUpdate();

            MoveBullet();
        }

        private void MoveBullet()
        {
            float moveDistance = speed * Time.deltaTime;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, moveDistance);

            if (hit.collider != null && !hit.collider.CompareTag(AllyTag))
            {
                ProcessHit(hit);
            }

            if (gameObject.activeSelf)
            {
                transform.position += transform.up * moveDistance;
            }
        }

        private void ProcessHit(RaycastHit2D hit)
        {
            GameObject target = hit.collider.gameObject;

            if (target.CompareTag(OpponentTag))
            {
                ProcessEnemyHit(hit);
                return;
            }

            if (WallLayer.Contains(target))
            {
                ProcessWallHit(hit);
                return;
            }
        }

        private void ProcessEnemyHit(RaycastHit2D hit)
        {
            bool canSurvive = false;

            foreach (IBulletModule m in modules)
            {
                if (m.OnHitEnemy(hit))
                    canSurvive = true;
            }

            if (canSurvive == false)
                Kill();
        }

        private void ProcessWallHit(RaycastHit2D hit)
        {
            bool canSurvive = false;

            foreach (IBulletModule m in modules)
            {
                if (m.OnHitWall(hit))
                    canSurvive = true;
            }

            if (canSurvive == false)
                Kill();
        }

        public void Kill()
        {
            gameObject.SetActive(false);
        }

        public void AddModule(IBulletModule module)
        {
            if (modules.Contains(module) == false)
            {
                modules.Add(module);
                module.OnInit(this);
            }
        }

    }
}