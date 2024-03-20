using System;
using UnityEngine;

[DisallowMultipleComponent]
public class NutrientBall : Interactable
{
    [SerializeField] private float _timeToDecompose = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] [Range(0,1)] private float slownesMultiplier = 1;

    private SpriteRenderer spriteRenderer = null;
    private Collider2D nutrientCollider2D = null;
    private Rigidbody2D rigidBody = null;

    private int _nutrientValue = 10;
    public int nutrientValue => _nutrientValue;
    public float timeToDecompose => _timeToDecompose;
    private Vector3 baseDestination = Vector3.zero;



    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        nutrientCollider2D = GetComponent<Collider2D>();
        //rigidBody = GetComponent<Rigidbody2D>();
        baseDestination = new Vector3(0, 11, 0);
    }

    public void BeginDecomposing()
    {
        nutrientCollider2D.enabled = false;
        //rigidBody.simulated = false;
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
        transform.Translate((baseDestination - transform.position).normalized * minion.GetCarryPower() * slownesMultiplier);
    }

    public override void InteractEnemy(Enemy enemy)
    {
        FinishTask();
    }

    private void ReleaseMinions()
    {
        foreach (MushroomMinion minion in GetComponentsInChildren<MushroomMinion>())
        {
            minion.Release();
            minion.transform.parent = null;
        };
    }

    public void ReduceNutrient(int nutrients)
    {
        animator.SetBool("isBit", true);
        _nutrientValue -= nutrients;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Base other = collision.GetComponent<Base>();
        if (other)
        {
            other.AbsorbNutrients(nutrientValue);
            ReleaseMinions();
            FinishTask();
        }

        base.OnTriggerEnter2D(collision);
    }
}
