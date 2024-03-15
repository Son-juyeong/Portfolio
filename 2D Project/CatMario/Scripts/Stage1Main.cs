using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Main : MonoBehaviour
{
    [SerializeField]
    private MainCamera mainCamera;

    [SerializeField]
    private Player player;

    [SerializeField]
    private ObstacleController obstacleController;

    [SerializeField]
    private ItemGenerator itemGenerator;

    [SerializeField]
    private Monster[] arrMonsters;

    [SerializeField]
    private HiddenBrockTrigger[] arrHiddenBrockTriggers;

    [SerializeField]
    private EventsTrigger[] arrFallDownBrockTriggers;

    [SerializeField]
    private EventsTrigger[] arrEventsTriggers;

    public float stageLength;
    private float elapsedTime = 0f;
    private float jumpKeyPressTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        this.stageLength = 3849;
        this.mainCamera.Init(this.stageLength-240);
        this.player.Init(new Vector2(-170,-110), this.stageLength-12);
        this.player.onNoteJumpImpossible = () =>
        {
            Debug.Log("현재 플레이어는 점프를 할 수 없는 상태입니다.");
        };
        this.player.onQueryMonsterLife = (go) =>
        {
            if (go == null || go.activeSelf == true)
                return true;
            else
                return false;
        };
        this.obstacleController.onNotePlayerReach = () =>
        {
            Debug.Log("player does not jump&fall");
            this.player.ReachGround();
        };
        this.obstacleController.onNoteBrokenBrock = () =>
        {
            Debug.Log("brock is broken");
        };
        this.obstacleController.onNoteGenerateItem = (itemType, parent) => {
            Debug.LogFormat("{0} is generated", itemType);
            itemGenerator.GenerateItem(itemType, parent);
        };
        this.obstacleController.onAttackPlayer = () =>
        {
            if (this.player.IsDie) return;
            Debug.Log("obstacle attacks player");
            StartCoroutine(this.player.Die());
        };
        this.obstacleController.Init();
        for(int i=0;i<arrMonsters.Length;i++)
        {
            int idx = i;
            DefaultMonster defaultMonster = arrMonsters[idx] as DefaultMonster;
            FishMonster fishMonster = arrMonsters[idx] as FishMonster;
            if (defaultMonster != null)
            {
                defaultMonster.Init();
                defaultMonster.onGetHit = () =>
                {
                    if (this.player.IsDie) return;
                    Debug.Log("Monster is die");
                    this.player.AfterAttack();
                    defaultMonster.Die();
                };
                defaultMonster.onAttackPlayer = () =>
                {
                    if (this.player.IsDie||defaultMonster.IsDie) return;
                    Debug.Log("Monster attacks player");
                    StartCoroutine(this.player.Die(defaultMonster.gameObject));
                };
            }
            else if (fishMonster != null)
            {
                fishMonster.Init();
                fishMonster.onAttackPlayer = () =>
                {
                    if (this.player.IsDie) return;
                    Debug.Log("Monster attacks player");
                    StartCoroutine(this.player.Die(fishMonster.gameObject));
                };
            }
        }
        for(int i=0;i<this.arrHiddenBrockTriggers.Length;i++)
        {
            int idx = i;
            arrHiddenBrockTriggers[idx].onAppearBrock = () =>
            {
                if (!this.player.IsFall)
                {
                    this.obstacleController.SetActiveHiddenBrock(idx);
                    this.itemGenerator.GenerateItem(0, this.obstacleController.GetHiddenBrockTransform(idx));
                    this.arrHiddenBrockTriggers[idx].gameObject.SetActive(false);
                }
            };
        }
        for(int i = 0;i<this.arrFallDownBrockTriggers.Length; i++)
        {
            int idx = i;
            arrFallDownBrockTriggers[idx].onStartEvent = () =>
            {
                this.obstacleController.StartFallDownEvent(idx);
            };
        }

        for(int i = 0; i < arrEventsTriggers.Length; i++)
        {
            int idx = i;
            this.arrEventsTriggers[idx].onStartEvent = () =>
            {
                DefaultMonster defaultMonster = this.arrMonsters[idx] as DefaultMonster;
                FishMonster fishMonster = this.arrMonsters[idx] as FishMonster;
                if (defaultMonster!=null)
                {
                    defaultMonster.Move();
                }
                else if (fishMonster != null)
                {
                    fishMonster.Move();
                }
                this.arrEventsTriggers[idx].gameObject.SetActive(false);
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)|| Input.GetKey(KeyCode.Z))
        {
            this.elapsedTime += Time.deltaTime;
            if (this.elapsedTime >= this.jumpKeyPressTime)
            {
                this.player.Jump(true);
                this.elapsedTime = 0f;
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Z))
        {
            if (this.elapsedTime >= this.jumpKeyPressTime)
                this.player.Jump(true);
            else
                this.player.Jump(false);
            this.elapsedTime = 0f;
        }
        float playerDirX = Input.GetAxisRaw("Horizontal");
        this.player.Move(playerDirX, this.mainCamera.transform.position.x - 220);
        float playerPosX = this.player.transform.position.x;
        this.mainCamera.Move(playerPosX);
    }
}
