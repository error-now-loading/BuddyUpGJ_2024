using System;
using UnityEngine;

[DisallowMultipleComponent]
public class NutrientBall : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Collider2D nutrientCollider2D = null;
    [SerializeField] private Rigidbody2D rigidBody = null;
    
    private int _nutrientValue = 10;
    public int nutrientValue => _nutrientValue;

    [SerializeField] private float _timeToDecompose = 5f;
    public float timeToDecompose => _timeToDecompose;



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
}