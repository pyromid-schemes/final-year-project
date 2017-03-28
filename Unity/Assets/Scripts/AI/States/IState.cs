using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

/*
 * @author Daniel Burnley
 */
namespace AI.States
{
    public interface IState
    {
        void OnEnter();

        void OnUpdate();

        void OnFixedUpdate();
    }
}
