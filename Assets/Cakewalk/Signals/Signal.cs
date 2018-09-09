using System.Linq;
using UnityEngine;

namespace Cakewalk.Signal {

    /// <summary>
    /// Basic signal with no parameters.
    /// </summary>
    public class Signal {

        public delegate void SignalDelegate();
        private event SignalDelegate SendSignal;

        /// <summary>
        /// Add a unique listener to the signal.
        /// </summary>
        public void AddListener(SignalDelegate listener) {
            if(SendSignal != null && SendSignal.GetInvocationList().Contains(listener)) {
                Debug.LogWarning(string.Format("Signal already has registered the listener {0}", listener.Method.Name));
            }
            SendSignal += listener;
        }

        public void RemoveListener(SignalDelegate listener) {
            SendSignal -= listener;
        }

        public void Dispatch() {
            if(SendSignal != null) {
                SendSignal();
            }
        }
    }

    /// <summary>
    /// Basic signal with one parameter.
    /// </summary>
    public class Signal<T> {

        public delegate void SignalDelegate(T item);
        private event SignalDelegate SendSignal;

        /// <summary>
        /// Add a unique listener to the signal.
        /// </summary>
        public void AddListener(SignalDelegate listener) {
            if (SendSignal != null && SendSignal.GetInvocationList().Contains(listener)) {
                Debug.LogWarning(string.Format("Signal already has registered the listener {0}", listener.Method.Name));
            }
            SendSignal += listener;
        }

        public void RemoveListener(SignalDelegate listener) {
            SendSignal -= listener;
        }

        public void Dispatch(T item) {
            if(SendSignal != null) {
                SendSignal(item);
            }
        }
    }

    /// <summary>
    /// Signal with 2 parameters.
    /// </summary>
    public class Signal<T, U> {

        public delegate void SignalDelegate(T item1, U item2);
        private event SignalDelegate SendSignal;

        /// <summary>
        /// Add a unique listener to the signal.
        /// </summary>
        public void AddListener(SignalDelegate listener) {
            if (SendSignal != null && SendSignal.GetInvocationList().Contains(listener)) {
                Debug.LogWarning(string.Format("Signal already has registered the listener {0}", listener.Method.Name));
            }
            SendSignal += listener;
        }

        public void RemoveListener(SignalDelegate listener) {
            SendSignal -= listener;
        }

        public void Dispatch(T item1, U item2) {
            if(SendSignal != null) {
                SendSignal(item1, item2);
            }
        }
    }

    /// <summary>
    /// Signal with 3 parameters.
    /// </summary>
    public class Signal<T, U, V> {

        public delegate void SignalDelegate(T item1, U item2, V item3);
        private event SignalDelegate SendSignal;

        /// <summary>
        /// Add a unique listener to the signal.
        /// </summary>
        public void AddListener(SignalDelegate listener) {
            if (SendSignal != null && SendSignal.GetInvocationList().Contains(listener)) {
                Debug.LogWarning(string.Format("Signal already has registered the listener {0}", listener.Method.Name));
            }
            SendSignal += listener;
        }

        public void RemoveListener(SignalDelegate listener) {
            SendSignal -= listener;
        }

        public void Dispatch(T item1, U item2, V item3) {
            if(SendSignal != null) {
                SendSignal(item1, item2, item3);
            }
        }
    }

    /// <summary>
    /// Signal with 4 parameters.
    /// </summary>
    public class Signal<T, U, V, W> {

        public delegate void SignalDelegate(T item1, U item2, V item3, W item4);
        private event SignalDelegate SendSignal;

        /// <summary>
        /// Add a unique listener to the signal.
        /// </summary>
        public void AddListener(SignalDelegate listener) {
            if (SendSignal != null && SendSignal.GetInvocationList().Contains(listener)) {
                Debug.LogWarning(string.Format("Signal already has registered the listener {0}", listener.Method.Name));
            }
            SendSignal += listener;
        }

        public void RemoveListener(SignalDelegate listener) {
            SendSignal -= listener;
        }

        public void Dispatch(T item1, U item2, V item3, W item4) {
            if(SendSignal != null) {
                SendSignal(item1, item2, item3, item4);
            }
        }
    } 
}