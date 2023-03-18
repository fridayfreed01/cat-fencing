using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    internal class Begin : State
    {
        public Begin(BattleSystem battleSystem) : base(battleSystem) { }

        public override IEnumerator Start()
        {
            
            BattleSystem.playerHUD.SetHUD(BattleSystem.playerUnit);
            BattleSystem.enemyHUD.SetHUD(BattleSystem.enemyUnit);

            yield return new WaitForSeconds(2f);

            //SetState(new PlayerTurn(BattleSystem));

        }
    }
}
