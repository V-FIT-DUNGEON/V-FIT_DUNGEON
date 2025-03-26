using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercise_Pose : MonoBehaviour
{
    [SerializeField] string pose_name;
    [SerializeField] string pose_description;
    [SerializeField] List<Body_part> body_parts;
    [SerializeField] Animation pose_animation;
}