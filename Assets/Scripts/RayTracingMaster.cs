﻿using UnityEngine;
using System.Collections.Generic;
public class RayTracingMaster : MonoBehaviour
{
    struct Sphere
    {
        public Vector3 position;
        public float radius;
        public Vector3 albedo;
        public Vector3 specular;
    }
    struct AABB
    {
        public Vector3 min;
        public Vector3 max;
        public Vector3 albedo;
        public Vector3 specular;
    }
    public Texture SkyboxTexture;
    public ComputeShader RayTracingShader;
    private RenderTexture _target;
    private Camera _camera;

    private uint _currentSample = 0;
    private Material _addMaterial;

    public Light DirectionalLight;

    public SpherePrimitive[] Spheres;
    private ComputeBuffer _sphereBuffer;
    private int _sphereCount;

    public AABBPrimitive[] AABBs; 
    private ComputeBuffer _aabbBuffer;
    private int _aabbCount;

    private void OnEnable()
    {
        _currentSample = 0;
        SetUpScene();
    }
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    private void Update()
    {
        if (transform.hasChanged || DirectionalLight.transform.hasChanged)
        {
            _currentSample = 0;
            transform.hasChanged = false;
            DirectionalLight.transform.hasChanged = false;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetShaderParameters();
        Render(destination);
    }
    private void OnDisable()
    {
        if (_sphereBuffer != null)
            _sphereBuffer.Release();
        if (_aabbBuffer != null)
            _aabbBuffer.Release();
    }

    private void SetUpScene()
    {
        List<Sphere> spheres = new List<Sphere>();
        // Add a number of random spheres
        for (int i = 0; i < Spheres.Length; i++)
        {
            Sphere sphere = new Sphere();
            sphere.radius = Spheres[i].transform.localScale.x * 0.5f;
            sphere.position = Spheres[i].transform.position;
            sphere.albedo = Spheres[i].albedo;
            sphere.specular = Spheres[i].specular;
            spheres.Add(sphere);
        }
        // Assign to compute buffer
        _sphereCount = spheres.Count;
        _sphereBuffer = new ComputeBuffer(_sphereCount != 0? _sphereCount:1, 40);
        _sphereBuffer.SetData(spheres);


        List<AABB> aabbs = new List<AABB>();
        // Add a number of random spheres
        for (int i = 0; i < AABBs.Length; i++)
        {
            AABB aabb = new AABB();
            aabb.min = AABBs[i].transform.position - AABBs[i].transform.localScale * 0.5f;
            aabb.max = AABBs[i].transform.position + AABBs[i].transform.localScale * 0.5f;
            Debug.Log("min: " + aabb.min);
            Debug.Log("max: " + aabb.max);
            aabb.albedo = AABBs[i].albedo;
            aabb.specular = AABBs[i].specular;
            aabbs.Add(aabb);
        }
        // Assign to compute buffer
        _aabbCount = aabbs.Count;
        _aabbBuffer = new ComputeBuffer(_aabbCount != 0 ? _aabbCount : 1, 48);
        _aabbBuffer.SetData(aabbs);
    }
    private void SetShaderParameters()
    {
        RayTracingShader.SetMatrix("_CameraToWorld", _camera.cameraToWorldMatrix);
        RayTracingShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);
        RayTracingShader.SetTexture(0, "_SkyboxTexture", SkyboxTexture);
        RayTracingShader.SetVector("_PixelOffset", new Vector2(Random.value, Random.value));
        Vector3 l = DirectionalLight.transform.forward;
        RayTracingShader.SetVector("_DirectionalLight", new Vector4(l.x, l.y, l.z, DirectionalLight.intensity));
        RayTracingShader.SetBuffer(0, "_Spheres", _sphereBuffer);
        RayTracingShader.SetInt("_SphereCount", _sphereCount);
        RayTracingShader.SetBuffer(0, "_AABBs", _aabbBuffer);
        RayTracingShader.SetInt("_AABBCount", _aabbCount);
    }
    private void Render(RenderTexture destination)
    {
        // Make sure we have a current render target
        InitRenderTexture();
        // Set the target and dispatch the compute shader
        RayTracingShader.SetTexture(0, "Result", _target);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        RayTracingShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
        // Blit the result texture to the screen
        if (_addMaterial == null)
            _addMaterial = new Material(Shader.Find("Hidden/AddShader"));
        _addMaterial.SetFloat("_Sample", _currentSample);
        Graphics.Blit(_target, destination, _addMaterial);
        _currentSample++;
    }
    private void InitRenderTexture()
    {
        if (_target == null || _target.width != Screen.width || _target.height != Screen.height)
        {
            // Release render texture if we already have one
            if (_target != null)
                _target.Release();
            // Get a render target for Ray Tracing
            _target = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _target.enableRandomWrite = true;
            _target.Create();
        }
    }
}