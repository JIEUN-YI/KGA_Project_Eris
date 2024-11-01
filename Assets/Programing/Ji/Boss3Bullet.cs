using UnityEngine;

public class Boss3Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed; // źȯ�� �̵��ӵ�
    Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        gameObject.transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime, Space.Self);
    }

    public void SetSpeed(float speed)
    {
        this.bulletSpeed = speed;
    }
}
