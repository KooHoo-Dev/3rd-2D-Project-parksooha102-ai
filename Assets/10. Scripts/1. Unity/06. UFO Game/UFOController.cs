using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UFOController : MonoBehaviour
{
    // 위,아래 움직임 기능 넣어보기
    // 기본적으로 하강을 하지만 스페이스를 눌렀을때는 상승

    public float Speed = 1.0f;
    public int CoinCount = 0;

    public Transform UpPivot;
    public Transform DownPivot;
    public GameObject WinText;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        WinText.SetActive(false);
        // 게임오브젝트에 붙어있는 SpriteRenderer 컴포넌트를 가져온다.
    }

    // Update is called once per frame
    void Update()
    {
        //기본적으로 하강
        Vector3 moveVector = new Vector3(0, -1, 0);

        // Space를 누르면 상승한다
        if (Keyboard.current.spaceKey.isPressed)
        {
            moveVector = moveVector * (-1f); // -1 * -1 = +1;
        }

        //속도 * 프레임당 이동량을 구해준다
        moveVector = moveVector * Speed * Time.deltaTime;

        transform.position += moveVector;

        // 화면 밖으로 나가지 못하게 고정
         if (transform.position.y <= DownPivot.transform.position.y)
        {
            float xPos = spriteRenderer.transform.position.x;
            float yLimit = DownPivot.transform.position.y;
            spriteRenderer.transform.position = new Vector3(xPos, yLimit, 0);
        }
         if (transform.position.y >= UpPivot.transform.position.y)
        {
            float xPos = spriteRenderer.transform.position.x;
            float yLimit = UpPivot.transform.position.y;
            spriteRenderer.transform.position = new Vector3(xPos, yLimit, 0);
        }

        if(CoinCount >= 30)
        {
            WinText.SetActive(true);
            while (true) { }
        }
    }

    private bool isColliding = false;
    private SpriteRenderer spriteRenderer;

    // RigidBody를 가진 GameObject가 Collider(isTriggerd가 트루)인 개체와 접촉하면 1회 호출된다.
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Coin"))
        {
            Destroy(collider.gameObject);
            CoinCount += 1;
        }
        else if (isColliding == false && collider.CompareTag("Obstacle"))
        {
            isColliding = true;
            StartCoroutine(CollisionEffect());
        }
    }

    private IEnumerator CollisionEffect()
    {
        Debug.LogWarning("충돌 시작");
        Color origin = spriteRenderer.color;
        Color effect = spriteRenderer.color;
        effect.a = 0.2f;
        int times = 2;
        float term = 1.0f / (float)times;
        for(int i = 0; i < times; ++i)
        {
            spriteRenderer.color = effect;
            yield return new WaitForSeconds(term);
            spriteRenderer.color = origin;
            yield return new WaitForSeconds(term);
        }

        Debug.LogWarning("충돌 종료");
        isColliding = false;
    }

}