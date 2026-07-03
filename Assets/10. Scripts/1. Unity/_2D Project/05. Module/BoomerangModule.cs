using _2DProject;
using Study.Utilities;
using UnityEngine;

public class BoomerangModule : IBulletModule
{
    private Bullet bullet;

    private bool isReturning;

    private float returnDistance = 10.0f;
    private float killDistance = 0.5f;

    private Vector3 startPosition;

    public BoomerangModule(BoomerangModuleData data)
    {
        returnDistance = data.ReturnDistance;
        killDistance = data.KillDistance;
    }

    public void OnInit(Bullet bullet)
    {
        this.bullet = bullet;

        startPosition = bullet.transform.position;
        isReturning = false;
    }

    public void OnUpdate()
    {
        if (isReturning == false)
        {
            Vector3 currentPos = bullet.transform.position;
            float distance = Vector3.Distance(currentPos, startPosition);

            if (distance >= returnDistance)
            {
                isReturning = true;
            }
        }

        if (isReturning)
        {
            Vector3 currentPos = bullet.transform.position;
            Vector3 goalPoint = bullet.Owner.transform.position;

            Vector3 direction = (goalPoint - currentPos);
            direction = direction.normalized;

            bullet.transform.up = direction;

            if (bullet.transform.IsInRange(goalPoint, killDistance))
            {
                bullet.Kill();
            }
        }
    }

    public void OnHitEnemy(RaycastHit2D hit)
    {
    }

    public void OnHitWall(RaycastHit2D hit)
    {
    }

    public bool CanSurviveEnemy(RaycastHit2D hit)
    {
        return false;
    }

    public bool CanSurviveWall(RaycastHit2D hit)
    {
        return false;
    }
}