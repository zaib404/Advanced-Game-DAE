using Engine.Collision;
using Engine.Entity;
using System;

namespace DemoCode.Entities
{
    class Artefact : GameEntity, iCollidable
    {
        public Artefact() { IsTrigger = true; }

        public override void OnTriggerEnter(iEntity gameObject)
        {
            Console.WriteLine("Enter");
        }

        public override void OnTriggerStay(iEntity gameObject)
        {
            Console.WriteLine("Stay");
        }

        public override void OnTriggerExit(iEntity gameObject)
        {
            Console.WriteLine("Exit");
        }
    }
}
