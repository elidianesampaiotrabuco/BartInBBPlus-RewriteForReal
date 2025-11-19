using MTM101BaldAPI.Reflection;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace OMGITSBART
{
    public class Bart : NPC
    {
        public Sprite bartSprite;
        public SoundObject simpsonGamer;

        public override void Initialize()
        {
            base.Initialize();

            spriteRenderer[0].sprite = bartSprite;

            AudioManager audioManager = GetComponent<AudioManager>();
            PropagatedAudioManager simpsonGamerPlayer = base.gameObject.AddComponent<PropagatedAudioManager>();
            simpsonGamerPlayer.audioDevice = base.gameObject.AddComponent<AudioSource>();
            simpsonGamerPlayer.ReflectionSetVariable("soundOnStart", new SoundObject[] { simpsonGamer });
            simpsonGamerPlayer.ReflectionSetVariable("loopOnStart", true);

            behaviorStateMachine.ChangeState(new Bart_Wander(this));
        }
    }

    public class Bart_StateBase : NpcState
    {
        protected Bart SimpsonGamer;

        public Bart_StateBase(Bart simpsongame) : base(simpsongame)
        {
            SimpsonGamer = simpsongame;
        }
    }

    public class Bart_Wander : Bart_StateBase
    {
        public Bart_Wander(Bart simpsongame) : base(simpsongame)
        {

        }

        public override void Enter()
        {
            base.Enter();
            if (!npc.Navigator.HasDestination)
            {
                ChangeNavigationState(new NavigationState_WanderRandom(npc, 0));
            }
        }

        public override void DestinationEmpty()
        {
            base.DestinationEmpty();
            ChangeNavigationState(new NavigationState_WanderRandom(npc, 0));
        }
    }
}
