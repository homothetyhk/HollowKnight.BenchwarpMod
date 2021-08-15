using System;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;

namespace Benchwarp
{
    public class FsmLambda : FsmStateAction
    {
        readonly Action action;

        public FsmLambda(Action a)
        {
            action = a;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                Benchwarp.instance.LogError($"Error in FsmLambda in {Fsm.GameObject.name}-{Fsm.Name}[{State.Name}]:\n{e}");
            }
            base.Finish();
        }
    }
}
