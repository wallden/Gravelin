using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Events
{
    public class PlayerDiedEvent : GameEvent
    {
        public GameObject SourceOfDeath { get; set; }
        public GameObject Weapon { get; set; }
        public GameObject PlayerKilled { get; set; }
    }
}
