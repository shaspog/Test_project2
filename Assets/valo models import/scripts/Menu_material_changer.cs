using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Menu_material_changer : MonoBehaviour
{
    private XRGrabInteractable _grabInteractable;
    private Renderer _renderer;
    private Color _originalColor;
    private Color _hoverColor = new Color(1f, 0.647f, 0f); // Orange
    private Color _grabColor = new Color(0f, 1f, 0f); // Green

    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;


        _grabInteractable.hoverEntered.AddListener(OnHoverEnter);
        _grabInteractable.hoverExited.AddListener(OnHoverExit);
        _grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        _renderer.material.color = _hoverColor;
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        _renderer.material.color = _originalColor;
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        _renderer.material.color = _grabColor;
    }
}