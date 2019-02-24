using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ImageSwitcher : MonoBehaviour
{
    [SerializeField] List<UnityEngine.Material> materials;
    private int index = 0;
    private MeshRenderer renderer;

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    public void NextImage()
    {
        if (!renderer) return;
        if (materials.Count == 0) return;
        if (index + 1 > materials.Count) index = 0;
        renderer.material = materials[index++];
    }
}
