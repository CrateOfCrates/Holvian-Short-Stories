using UnityEngine;

public abstract class StatusEffectObject : ScriptableObject
{
    [SerializeField] protected string effectName;

    [SerializeField] protected StatusEffectDecayRate decayRate;
    [SerializeField] protected StatusEffectPartType decayType;
    [Tooltip("DecayCount will be out of 100 if DecayRate is multiplicative, 50 decayCount = halve")]
    [SerializeField] protected int decayCount;

    [SerializeField] protected Sprite displaySprite;

    public string EffectName => effectName;
    public StatusEffectDecayRate DecayRate => decayRate;
    public StatusEffectPartType DecayType => decayType;
    public int DecayCount => decayCount;
    public Sprite DisplaySprite => displaySprite;

}



