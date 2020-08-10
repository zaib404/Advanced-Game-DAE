using Engine.Managers;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Input
{
    public class KeyboardInput : IKeyboardInputObserverable, iManager
    {
        //private static List<EntityKey> m_entityKeyList = new List<EntityKey>();
        //private static List<IKeyboardInputObserver> m_subList = new List<IKeyboardInputObserver>();

        private static Dictionary<IKeyboardInputObserver, EntityKey> m_entityKeyList = new Dictionary<IKeyboardInputObserver, EntityKey>();

        private struct EntityKey
        {
            public EntityKey(int id, List<Keys> key)
            {
                uid = id;
                keys = key;
            }

            public int uid;
            public List<Keys> keys;
        }

        public KeyboardInput() { }

        public static void Subscribe(IKeyboardInputObserver sub, List<Keys> keys)
        {
            //m_entityKeyList.Add(new EntityKey(0, keys));
            //m_subList.Add(sub);

            m_entityKeyList.Add(sub, new EntityKey(m_entityKeyList.Count, keys));

        }

        public void Update()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            foreach (var sub in m_entityKeyList.Values.ToList())
            {
                foreach (var key in sub.keys.ToList())
                {
                    if (keyboardState.IsKeyDown(key))
                    {
                        notifyInput(key);
                    }
                }
            }

            //foreach (var sub in m_entityKeyList.ToList())
            //{
            //    foreach (var key in sub.keys.ToList())
            //    {
            //        if (keyboardState.IsKeyDown(key))
            //        {
            //            notifyInput(key);
            //        }
            //    }
            //}
        }

        public void notifyInput(Keys key)
        {
            foreach (var sub in m_entityKeyList.Keys.ToList())
            {
                sub.input(key);
            }
        }

        public static void UnSubscribe(IKeyboardInputObserver unsub)
        {
            if (m_entityKeyList.Keys.Contains(unsub))
            {
                m_entityKeyList.Remove(unsub);
            }
        }

        public static void UnSubscribeAll()
        {
            m_entityKeyList.Clear();
        }
    }
}
