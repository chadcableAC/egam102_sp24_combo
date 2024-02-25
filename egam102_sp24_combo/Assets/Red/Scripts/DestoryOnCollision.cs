using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
   

        // 当此碰撞器开始与另一个碰撞器碰撞时调用
        private void OnCollisionEnter2D(Collision2D collision)
    {
        // 销毁碰撞的GameObject
        Destroy(collision.gameObject);
    }

    // 如果你想要在触发器碰撞时销毁GameObject，可以使用以下方法
    // 注意：要使其工作，一个或两个碰撞的对象必须有一个Rigidbody组件
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 销毁进入触发器的GameObject
        Destroy(other.gameObject);
    }
}
