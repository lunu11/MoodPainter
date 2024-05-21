using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDemo : MonoBehaviour
{
    public Camera m_camera;
    public GameObject brush;

    LineRenderer currentLineRenderer;

    Vector2 lastPos;
    private Rect drawArea;
    private Vector3 virtualKeyPosition = Vector2.zero;
    private RuntimePlatform platform;

    private void Start()
    {
        drawArea = new Rect(0,200, Screen.width, Screen.height-200);
    }
    private void Update()
    {
        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0)
            {
                virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            }
        }
        Draw();
    }
    private void Draw()
    {
        if (drawArea.Contains(virtualKeyPosition))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CreateBrush();
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                if (mousePos != lastPos)
                {
                    AddAPoint(mousePos);
                    lastPos = mousePos;
                }
            }
            else
            {
                currentLineRenderer = null;
            }
        }
    }

    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer =brushInstance.GetComponent<LineRenderer>();
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    void AddAPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }
    public void ChangeColor(Color colorPicked)
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(colorPicked, 0.0f), new GradientColorKey(colorPicked, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });
        currentLineRenderer.colorGradient = gradient;
    }
}
    