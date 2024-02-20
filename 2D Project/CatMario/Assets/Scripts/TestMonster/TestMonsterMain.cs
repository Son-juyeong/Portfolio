using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TestMonster
{
    public class TestMonsterMain : MonoBehaviour
    {
        [SerializeField]
        private DefaultMonster defaultMonster;
        [SerializeField]
        private FishMonster fishMonster;
        [SerializeField]
        private Player player;

        void Start()
        {
            this.player.Init(new Vector2(-110, -80), 2000);
            this.defaultMonster.Init();
            this.defaultMonster.Move();
            this.fishMonster.Init();
            this.fishMonster.Move();
            this.defaultMonster.onAttackPlayer = () =>
            {
                Debug.Log("�׾��!");
                this.player.Die();
            };
            this.fishMonster.onAttackPlayer = () =>
            {
                this.player.Die();
            };
            this.defaultMonster.onGetHit = () =>
            {
                Debug.LogFormat("{0}�� ���ݹ޾ҽ��ϴ�.", defaultMonster.name);
                if (!player.IsDie)
                    this.defaultMonster.Die();
            };

        }
    }
}