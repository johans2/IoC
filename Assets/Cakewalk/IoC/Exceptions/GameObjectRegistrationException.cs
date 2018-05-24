using UnityEngine;
using System.Collections;

namespace CakewalkIoC.Exceptions {

    public class GameObjectRegistrationException : RegistrationException  {
        public GameObjectRegistrationException(string message) : base(message) { }
    }

}
