using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 offset;

    void OnMouseDown()
    {
        // 计算鼠标位置和对象位置之间的偏移量
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        // 在鼠标拖拽时更新对象的位置
        transform.position = GetMouseWorldPos() + offset;
    }

    // 获取以世界坐标表示的鼠标位置
    private Vector3 GetMouseWorldPos()
    {
        // 将鼠标位置转换为世界坐标
        Vector3 mousePoint = Input.mousePosition;
        // 假设拖拽发生在与摄像机前方同一平面上
        mousePoint.z = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
