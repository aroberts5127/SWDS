using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ForcePushDirection { NONE, RIGHT, LEFT, UP}

public class EnemyColliderDetection : MonoBehaviour {

    public ForcePushDirection ApplicableForce;
    public float EnemyForceMagnitude;
}
