using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private bool cursorRange = true;
    [SerializeField] private Outline outliner;
    [SerializeField] private MinionSpot[] minionSpots;
    [SerializeField] private MushroomJobs interactableType = MushroomJobs.Error;

private bool interactable = false;
    protected PlayerController playerReference;

    private void Start()
    {
        playerReference = FindObjectOfType<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cursorRange && collision.GetComponent<CursorRangeTrigger>())
        {
            outliner.enabled = true;
            interactable = true;
        }
        else if (!cursorRange && collision.GetComponent<NearRangeTrigger>())
        {
            outliner.enabled = true;
            interactable = true;
            playerReference.SetClosestNearInteractable(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (cursorRange && collision.GetComponent<CursorRangeTrigger>())
        {
            outliner.enabled = false;
            interactable = false;
        }
        else if (!cursorRange && collision.GetComponent<NearRangeTrigger>())
        {
            outliner.enabled = false;
            interactable = false;
            playerReference.SetClosestNearInteractable(null); //Asumming only one near will be at all times. OnTriggerStay2D can be added if we have more than one
        }
    }
    private void OnMouseEnter()
    {
        if (cursorRange)
        {
            outliner.SetHover(true);
        }
    }
    private void OnMouseExit()
    {
        if (cursorRange)
        {
            outliner.SetHover(false);
        }
    }
    private void OnMouseDown()
    {
        if (cursorRange && interactable)
        {
            Interact();
        }
    }
    public void NearInteraction()
    {
        if (!cursorRange && interactable)
        {
            Interact();
        }
    }
    protected virtual void Interact()
    {
        Debug.Log("Interacted. Behaviour must be overriden in the inherited class");
    }
    public virtual void InteractMinion(MushroomMinion minion)
    {
        Debug.Log("Interacted by a minion. Behaviour must be overriden in the inherited class");
    }
    public MushroomJobs GetInteractableType()
    {
        return interactableType;
    }
    public void TryAssignSpotTo(MushroomMinion minion)
    {
        MinionSpot closestSpot = null;
        Vector3 position = minion.transform.position;
        float closestDistance = float.MaxValue;
        foreach (MinionSpot spot in minionSpots)
        {
            float distance = Vector3.Distance(position, spot.transform.position);
            if (!spot.occupied && distance < closestDistance)
            {
                closestSpot = spot;
                closestDistance = distance;
            }
        }
        if (closestSpot != null)
        {
            closestSpot.occupied = true;
            minion.SetTargetAndSpot(this,closestSpot);
        }
    }
}
[Serializable]
public class MinionSpot
{
    public Transform transform;
    public bool occupied;
}