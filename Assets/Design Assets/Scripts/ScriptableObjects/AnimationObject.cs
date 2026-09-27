using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "AnyAnimation")]
public class AnimationObject : ScriptableObject
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private float fps;
    [SerializeField] private bool looping;

    public List<Sprite> Sprites => sprites;
    public float Fps => fps;
    public bool Looping => looping;
    public int Frames => sprites.Count;
    
}

