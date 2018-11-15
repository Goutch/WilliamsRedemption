using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnimatorExtension
{
    static class AnimatorExtension
    {
        public static bool ContainsParam(this Animator _Anim, string _ParamName)
        {
            foreach (AnimatorControllerParameter param in _Anim.parameters)
            {
                if (param.name == _ParamName) return true;
            }
            return false;
        }
    }
}
