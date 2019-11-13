
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class StickController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform holder;
    [SerializeField] RectTransform handler;
    [SerializeField] float radius;

    public event Action<Vector3> onDrag;

    private Vector3 _originPos;
    private bool _dragging;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originPos = eventData.position;
        handler.position = eventData.position;
        _dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        handler.position = eventData.position;
        FixedPos();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        handler.position = holder.position;
        _dragging = false;
    }

    private void FixedPos()
    {
        var dir = handler.anchoredPosition - holder.anchoredPosition;
        var dis = dir.magnitude;
        if (radius < dis)
            handler.anchoredPosition = holder.anchoredPosition + dir.normalized * radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (_dragging)
            onDrag?.Invoke(handler.anchoredPosition - holder.anchoredPosition);
    }
}
