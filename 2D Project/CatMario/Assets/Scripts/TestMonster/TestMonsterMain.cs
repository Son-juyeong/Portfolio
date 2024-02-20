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
                Debug.Log("죽어랏!");
                this.player.Die();
            };
            this.fishMonster.onAttackPlayer = () =>
            {
                this.player.Die();
            };
            this.defaultMonster.onGetHit = () =>
            {
                Debug.LogFormat("{0}가 공격받았습니다.", defaultMonster.name);
                if (!player.IsDie)
                    this.defaultMonster.Die();
            };

        }
    }
}