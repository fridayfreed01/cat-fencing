using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public abstract class State
    {
        protected BattleSystem BattleSystem;
        public State(BattleSystem battleSystem)
        {
            BattleSystem = battleSystem;
        }
        public virtual IEnumerator Start()
        {
            yield break;
        }

        public virtual IEnumerator PlayCard()
        {
            yield break;
        }

    }
}
