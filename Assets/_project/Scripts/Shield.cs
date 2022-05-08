using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Shield : MonoBehaviour
{
    [SerializeField] List<RingDefinition> _ringDefinitions = null;
    [SerializeField] GameObject _segmentPrefab = null;
    [SerializeField] float _overlap = 0.1f;
    [SerializeField] float _thickness = 0.05f;
    [SerializeField] float _height = 0.5f;
    [SerializeField] float _rateOfRotationBase = 90f;
    [SerializeField] float _extraRotation = 30f;

    List<GameObject>[] _rings;
    List<Transform> _ringTransforms;
    GameObject _castle;
    float _age;
    bool _spinning = false;

    IEnumerator Start()
    {
        CreateRings();
        while (true)
        {
            SpinRings();
            yield return null;
        }
    }

    private void CreateRings()
    {
        if (_rings != null) return;
        _spinning = false;
        List<RingDefinition> rings = new List<RingDefinition>();
        foreach(RingDefinition ringDefinition in _ringDefinitions)
        {
            rings.Add(ringDefinition);
        }

        SegmentDefinition segmentDefinition = new SegmentDefinition
        {
            Overlap = _overlap,
            Thickness = _thickness,
            Height = _height
        };

        _castle = CreateRingSegments(transform.position, rings.ToArray(), segmentDefinition);
        _spinning = true;
    }

    private GameObject CreateRingSegments(Vector3 position, RingDefinition[] rings, SegmentDefinition segmentDefinition)
    {
        if (_ringTransforms?.Count > 0)
        {
            foreach(Transform ringTransform in _ringTransforms)
            {
                ringTransform.SetParent(null);
            }
            _ringTransforms.Clear();
        }

        _ringTransforms = new List<Transform>();
        _rings = new List<GameObject>[rings.Length];
        for (int i = 0; i < rings.Length; ++i)
        {
            _rings[i] = new List<GameObject>();
            RingDefinition ring = rings[i];
            float circumference = ring.Radius * 2 * Mathf.PI;
            float segmentLength = circumference / ring.Segments;
            segmentLength += segmentLength * segmentDefinition.Overlap;
            Transform ringParent = new GameObject(ring.Name).transform;
            _ringTransforms.Add(ringParent);
            ringParent.transform.SetParent(transform);
            for (int segment = 0; segment < ring.Segments; segment++)
            {
                float angle = (360.0f * segment) / ring.Segments;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                GameObject segmentGo = Instantiate(_segmentPrefab);
                segmentGo.GetComponent<Renderer>().material.color = ring.RingColor;
                //segmentGo.GetComponent<RingSegment>()?.Init(ring.color);
                segmentGo.transform.SetParent(ringParent);
                segmentGo.transform.localScale = new Vector3(segmentLength, segmentDefinition.Thickness, segmentDefinition.Height);
                Vector3 segmentPosition = Vector3.up * ring.Radius;
                segmentPosition = rotation * segmentPosition;
                segmentGo.transform.position = segmentPosition;
                segmentGo.transform.localRotation = rotation;
                _rings[i].Add(segmentGo);
            }
        }

        transform.position = position;

        return gameObject;
    }

    private void SpinRings()
    {
        if (!_spinning) return;
        _age += Time.deltaTime;

        for (int i = 0; i < _castle.transform.childCount; ++i)
        {
            Transform pivot = _castle.transform.GetChild(i);
            float angle = _rateOfRotationBase + i * _extraRotation;
            angle *= _age;

            // ever other ring should rotate in the opposite direction
            angle *= ((i & 1) == 0) ? -1 : 1;

            pivot.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }


}
