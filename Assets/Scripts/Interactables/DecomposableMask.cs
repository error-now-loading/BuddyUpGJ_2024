using UnityEngine;

public class DecomposableMask : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private int divisions = 5;
    private int currentDivision = 0;
    private float divisionWidth;
    private Vector3 localPosOffset;

    void Start()
    {
        localPosOffset = transform.localPosition;
        divisionWidth = transform.localScale.x / divisions;
        UpdatePos();
        sprite.transform.parent = transform;
    }

    public void IncreaseDivision()
    {
        currentDivision++;
        sprite.transform.parent = null;
        UpdatePos();
        sprite.transform.parent = transform;
    }

    private void UpdatePos()
    {
        float xOffset = divisionWidth * currentDivision;
        transform.localPosition = new Vector3(xOffset, 0, 0) + localPosOffset;
    }
}
