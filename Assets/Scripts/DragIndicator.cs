using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragIndicator : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private Vector3 cameraOffset = new Vector3(0, 0, 10f);
	private Vector3 canvasOffset = new Vector3(0, 0, -90f);
	private Vector3 startPosition;

    private void Start()
	{
		lineRenderer.enabled = false;
	}

    public void StartedDragging(Vector3 territoryPosition)
	{
		lineRenderer.enabled = true;

		startPosition = territoryPosition + canvasOffset;
    }

    public void Dragging()
	{
        Vector3 dragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + cameraOffset;

        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, dragPosition);
    }

    public void StoppedDragging()
	{
		lineRenderer.enabled = false;
    }
}
