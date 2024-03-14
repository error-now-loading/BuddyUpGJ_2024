using System;
using UnityEngine;


[DisallowMultipleComponent]
public class NutrientBall : Interactable
{
    [SerializeField] private float _timeToDecompose = 5f;

    private SpriteRenderer spriteRenderer = null;
    private Collider2D nutrientCollider2D = null;
    private Rigidbody2D rigidBody = null;

    private int _nutrientValue = 10;
    public int nutrientValue => _nutrientValue;
    public float timeToDecompose => _timeToDecompose;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        nutrientCollider2D = GetComponent<Collider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void BeginDecomposing()
    {
        nutrientCollider2D.enabled = false;
        rigidBody.simulated = false;
        FadeOut();
    }

    private Coroutine FadeOut()
    {
        Action<float> tweenAction = lerp =>
        {
            Color newColor = spriteRenderer.color;
            newColor.a = Mathf.Lerp(1f, 0f, lerp);
            spriteRenderer.color = newColor;
        };

        return this.DoTween(tweenAction, null, _timeToDecompose, 0, EaseType.linear, true);
    }
    protected override void Interact()
    {
        MushroomMinion minion = playerReference.TryToCommandMinionTo(this);
        if (minion != null && minion.GetMushroomTypeSO().type != MushroomTypes.Ghosty)
        {
            TryAssignSpotTo(minion);
        }
    }
    public override void InteractMinion(MushroomMinion minion)
    {
        transform.Translate(minion.GetMushroomTypeSO().carryPerSecond *Time.deltaTime,0,0);
    }
}
