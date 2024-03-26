using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using System;

[RequireComponent(typeof(NutrientHandler))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float callBackRadius = 5f;
    [SerializeField] private LayerMask minionLayer;
    [SerializeField] private Transform[] minionFollowPoints;
    [SerializeField] private float castingDuration = 1f;
    [SerializeField] private float commandDuration = 1f;
    [SerializeField] private float winSpellDuration = 999f;

    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private NutrientHandler mana;
    private Interactable closestInteractable;

    private List<MushroomMinion> minionTroops = new List<MushroomMinion>();
    [SerializeField] private MushroomTypeSO selectedMushroomType;   //Serialize for now testing
    [SerializeField] private SpellTypeSO selectedSpellType;         //Serialize for now testing
    private bool isBusy;
    private Coroutine waitingTimerCoroutine;

    public bool isCommanding { private set; get; }          //For Anims
    public bool isCasting { private set; get; }             //For Anims
    public bool isFailingCasting { private set; get; }      //For Anims
    public bool isDed { private set; get; }                 //For Anims
    public event Action onRepeatCommand;                    //For Anim Repeat
    public event Action onTroopUpdate;                      //For UI Update

    [SerializeField] private AudioSource playerSource = null;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mana = GetComponent<NutrientHandler>();
        MushroomMinion.ResetMinionCount();
    }

    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.PlayerOverworld.Enable();

        playerInput.PlayerOverworld.Interact.performed += OnInteract;
        playerInput.PlayerOverworld.CastSpell.performed += OnSpellCast;
        playerInput.PlayerOverworld.CallBack.performed += OnCallBack;

        playerInput.PlayerOverworld.Pause.performed += OnPauseButtonHit;
        playerInput.UI.Unpause.performed += OnPauseButtonHit;

        EventManager.instance.Subscribe(EventTypes.PauseMenuClosedExternally, OnExternalUnpause);
    }

    private void OnDisable()
    {
        playerInput.PlayerOverworld.Interact.performed -= OnInteract;
        playerInput.PlayerOverworld.CastSpell.performed -= OnSpellCast;
        playerInput.PlayerOverworld.CallBack.performed -= OnCallBack;

        playerInput.PlayerOverworld.Pause.performed -= OnPauseButtonHit;
        playerInput.UI.Unpause.performed -= OnPauseButtonHit;

        playerInput.Disable();

        EventManager.instance.Unsubscribe(EventTypes.PauseMenuClosedExternally, OnExternalUnpause);
    }

    private void FixedUpdate()
    {
        if (!isBusy)
        {
            MovePlayer();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        UpdateDestinationFollow();
    }

    private void MovePlayer()
    {
        Vector2 movement = playerInput.PlayerOverworld.Movement.ReadValue<Vector2>();
        rb.velocity = movement * moveSpeed;
        TurnMeTowards(movement);

        if (movement == Vector2.zero)
        {
            playerSource.Stop();
        }

        else if (!playerSource.isPlaying && rb.velocity != Vector2.zero)
        {
            AudioManager.instance.PlaySFX(playerSource, AudioManager.instance.playerFootsteps.SelectRandom());
        }
    }
    private void UpdateDestinationFollow()
    {
        for (int i = 0; i < minionTroops.Count; i++)
        {
            minionTroops[i].SetDestination(minionFollowPoints[i].transform.position);
        }
    }
    private void OnInteract(InputAction.CallbackContext ctx)
    {
        closestInteractable?.NearInteraction();
    }
    public void SetClosestNearInteractable(Interactable interactable)
    {
        closestInteractable = interactable;
    }
    private void OnSpellCast(InputAction.CallbackContext ctx)
    {
        if (!isBusy)
        {
            isCasting = true;
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = cursorPosition - transform.position;
            TurnMeTowards(direction);

            if (selectedSpellType.type == SpellTypes.WinGame && mana.SpendNutrients(selectedSpellType.manaCost))
            {
                BusyForSeconds(winSpellDuration);
                AudioManager.instance.PlayMusic(AudioManager.instance.sourceMusic, AudioManager.instance.victoryMusic);
                EventManager.instance.Notify(EventTypes.Victory);
            }

            else if (CheckMinionCount() && mana.SpendNutrients(selectedSpellType.manaCost))
            {
                BusyForSeconds(castingDuration);
                Instantiate(selectedSpellType.spellPrefab, new Vector3(cursorPosition.x, cursorPosition.y, 0), Quaternion.identity);
                AudioManager.instance.PlaySFX(playerSource, AudioManager.instance.playerSummons.SelectRandom());
            }
            else
            {
                BusyForSeconds(castingDuration);
                isFailingCasting = true;
                AudioManager.instance.PlaySFX(playerSource, AudioManager.instance.playerSummonFails.SelectRandom());
            }
        }
    }

    private bool CheckMinionCount()
    {
        if (selectedSpellType.type == SpellTypes.SummonTroopy || selectedSpellType.type == SpellTypes.SummonBulky || selectedSpellType.type == SpellTypes.SummonAngy || selectedSpellType.type == SpellTypes.SummonGhosty)
        {
            if (MushroomMinion.minionCount < 20)
            {
                return true;
            }
            return false;
        }
        else
        {
            return true;
        }
    }
    private void OnCallBack(InputAction.CallbackContext obj)
    {
        if (isCommanding)
        {
            onRepeatCommand?.Invoke();
        }
        isCommanding = true;
        foreach(Collider2D col in Physics2D.OverlapCircleAll(transform.position, callBackRadius, minionLayer))    //7 is minion
        {
            MushroomMinion minion = col.gameObject.GetComponent<MushroomMinion>();
            if (minion != null && !minionTroops.Contains(minion))
            {
                minion.CallBack();
            }
        }
        for (int i = minionTroops.Count - 1; i >= 0; i--)
        {
            if (minionTroops[i] == null)
            {
                minionTroops.RemoveAt(i);
            }
        }
        MushroomMinion.RefreshMinionCount();
        onTroopUpdate?.Invoke();
        BusyForSeconds(commandDuration);

        AudioManager.instance.PlaySFX(playerSource, AudioManager.instance.playerCommand);
    }
    public MushroomMinion TryToCommandMinionTo(Interactable interactable)
    {
        if (!PauseMenu.instance.isPaused)
        {
            if (isCommanding)
            {
                onRepeatCommand?.Invoke();
            }
            isCommanding = true;
            Vector3 direction = interactable.transform.position - transform.position;
            TurnMeTowards(direction);
            BusyForSeconds(commandDuration);

            AudioManager.instance.PlaySFX(playerSource, AudioManager.instance.playerCommand);

            return PickMinion();
        }
        
        else
        {
            return null;
        }
    }

    private MushroomMinion PickMinion()
    {
        foreach (MushroomMinion minion in minionTroops)
        {
            if(minion.GetMushroomTypeSO() == selectedMushroomType) return minion;
        }
        return null;
    }


    private void BusyForSeconds(float seconds)
    {
        isBusy = true;
        if (waitingTimerCoroutine != null)
        {
            StopCoroutine(waitingTimerCoroutine);
        }
        waitingTimerCoroutine = StartCoroutine(WaitingTimer(seconds));
    }
    IEnumerator WaitingTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isBusy = false;
        isCasting = false;
        isFailingCasting = false;
        isCommanding = false;
        waitingTimerCoroutine = null;
    }

    public void MinionTroopJoin(MushroomMinion mushroomMinion)
    {
        minionTroops.Add(mushroomMinion);
        onTroopUpdate?.Invoke();
    }
    public void MinionTroopRemove(MushroomMinion mushroomMinion)
    {
        minionTroops.Remove(mushroomMinion);
        onTroopUpdate?.Invoke();
    }
    public void SetSelectedMushroomType(MushroomTypeSO type)
    {
        selectedMushroomType = type;
    }
    public void SetSelectedSpellType(SpellTypeSO type)
    {
        selectedSpellType = type;
    }
    public int GetMinionTypeCount(int index)
    {
        int count = 0;
        foreach (MushroomMinion minion in minionTroops)
        {
            if ((int)minion.GetMushroomTypeSO().type == index)
            {
                count += 1;
            }
        }
        return count;
    }
    private void TurnMeTowards(Vector2 direction)
    {
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x > 0)
        {
            transform.localScale = Vector3.one;
        }
    }
    public void GetHit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            isDed = true;
            isBusy = true;

            AudioManager.instance.PlaySFX(playerSource, AudioManager.instance.playerDeath);

            if (waitingTimerCoroutine != null)
            {
                StopCoroutine(waitingTimerCoroutine);
                waitingTimerCoroutine = null;
            }
            rb.velocity = Vector3.zero;

            EventManager.instance.Notify(EventTypes.PlayerDeath);
        }
    }

    private void OnPauseButtonHit(InputAction.CallbackContext obj)
    {
        OnExternalUnpause();
    }

    private void OnExternalUnpause()
    {
        // Pause
        if (!PauseMenu.instance.isPaused)
        {
            playerInput.PlayerOverworld.Disable();
            playerInput.UI.Enable();
            PauseMenu.instance.PauseGame();
        }

        // Unpause
        else
        {
            PauseMenu.instance.UnpauseGame();
            playerInput.UI.Disable();
            playerInput.PlayerOverworld.Enable();
        }
    }
}