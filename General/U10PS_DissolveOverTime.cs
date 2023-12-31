﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(MeshRenderer))]
public class U10PS_DissolveOverTime : MonoBehaviour
{
    public dissolve dissolve;
    public List<Renderer> meshRenderers;
    public Shader flatShader;
    Shader fixshader;
    public Color emissioncolor;
    public Color defaultemissioncolor;
    public Material blood;

    [Range(0, 1)]
    bool once;
    public bool death;
    public bool loop;

    [Range(0, 1)]
    public int defaultrate = 0;
    public bool autostart;
    public float speed = 0.5f;
    int i;

    void Awake()
    {
        if (gameObject.cclass() != null)
        {
            gameObject.cclass().U10PS_DissolveOverTime = this;
        }

        meshRenderers = gameObject.GetRender();
        dissolve.meshRenderers = meshRenderers;
        dissolve.rate(meshRenderers, defaultrate);
    }

    public void meshflat(float rate)
    {
        foreach (Renderer item in meshRenderers)
        {
            if (item.materials != null)
            {
                Material[] matss = item.materials;
                foreach (var items in matss)
                {
                    if (items.shader != fixshader)
                    {
                        items.shader = flatShader;
                    }

                    items.SetFloat("_Elevation", rate);
                }

                item.materials = matss;
            }
        }
    }

    public void setemission(Color col)
    {
        foreach (Renderer item in meshRenderers)
        {
            if (item.materials != null)
            {
                Material[] matss = item.materials;

                matss[0].SetColor("_emitColor", col);
                item.materials = matss;
            }
        }
    }

    public void dissolvevanish(System.Action action = null)
    {
        dissolve.meshRenderers = meshRenderers;
        dissolve.dissolveOut(() =>
        {
            Destroy(gameObject);
            action();
        });
    }

    public void meshflatstart()
    {
        DOVirtual.Float(
            0f,
            1f,
            1,
            value =>
            {
                meshflat(value);
            }
        );
    }
}
