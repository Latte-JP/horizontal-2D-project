using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraChecker : MonoBehaviour
{
    private enum Mode
    {
        None,
        Render,
        RenderOut,
    }

    private Mode _mode;
    private Camera currentCamera;

    // Start is called before the first frame update
    void Start()
    {
        _mode = Mode.None;
    }

    // Update is called once per frame
    void Update()
    {
        _Dead();
    }

    void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }

    void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }

    void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        currentCamera = camera;
    }

    private void OnWillRenderObject()
    {
        if ((currentCamera == null) || (currentCamera.cullingMask & (1 << gameObject.layer)) == 0)
        {
            return;
        }

        if (currentCamera.name == "Main Camera")
        {
            _mode = Mode.Render;
        }
    }

    private void _Dead()
    {
        if (_mode == Mode.RenderOut)
        {
            Destroy(gameObject);
        }

        if (_mode == Mode.Render)
        {
            _mode = Mode.RenderOut;
        }
    }
}