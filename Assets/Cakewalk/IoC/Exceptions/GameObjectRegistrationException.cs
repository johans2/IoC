using UnityEngine;
using System.Collections;

namespace Cakewalk.IoC.Exceptions {

    public class GameObjectRegistrationException : RegistrationException  {
        public GameObjectRegistrationException(string message) : base(message) { }
    }

}
