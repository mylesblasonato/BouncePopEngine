using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType
{
    RED,
    GREEN,
    BLUE,
    WHITE,
    BLACK,
}

[ExecuteAlways]
public class BallController : MonoBehaviour
{
    [SerializeField] BallType _ballType;
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] AudioClip _ballKnockSfx;
    public BallType TypeOfBall => _ballType;

    void Update()
    {
        _meshRenderer.material = Instantiate(Resources.Load("Ball") as Material);

        switch (_ballType)
        {
            case BallType.RED:
                _meshRenderer.sharedMaterial.color = Color.red;
                break;
            case BallType.GREEN:
                _meshRenderer.sharedMaterial.color = Color.green;
                break;
            case BallType.BLUE:
                _meshRenderer.sharedMaterial.color = Color.blue;
                break;
            case BallType.WHITE:
                _meshRenderer.sharedMaterial.color = Color.white;
                break;
            case BallType.BLACK:
                _meshRenderer.sharedMaterial.color = Color.black;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        MundoSound.Play(_ballKnockSfx);
    }
}