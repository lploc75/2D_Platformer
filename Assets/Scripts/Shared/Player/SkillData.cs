using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Shared.Player
{
    [System.Serializable]
    public class SkillData
    {
        public int skillID;
        public string skillName;
        public float manaCost;
        public int animationIndex;
        public int magicDamage;
        public Vector2 knockback;     // nếu skill có knockback
    }

}
