using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] private float spellDuration;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Sprite altDummySprite = null;
    private static Dummy activeInstance;



    private void Start()
    {
        if (activeInstance != null)
        {
            Destroy(activeInstance.gameObject);
        }
        activeInstance = this;

        if (SaveDataUtility.LoadBool(SaveDataUtility.SHROOLOO_MODE_KEY))
        {
            spriteRenderer.sprite = altDummySprite;
        }
    }

    private void Update()
    {
        spellDuration -= Time.deltaTime;
        if (spellDuration < 0)
        {
            activeInstance = null;
            Destroy(gameObject);
        }
    }
}