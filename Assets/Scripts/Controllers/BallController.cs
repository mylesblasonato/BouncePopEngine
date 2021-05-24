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

public class BallController : MonoBehaviour
{
    [SerializeField] BallType _ballType;
    public BallType TypeOfBall => _ballType;
}
