using System;
using SOD.Common.Custom;

namespace SOD.Common.Helpers
{
    public sealed class Interaction
    {
        internal SimpleActionArgs currentPlayerInteraction;

        private float lastAmount = float.MaxValue;
        internal bool longActionInProgress = false;

        /// <summary>
        /// Raised just prior to when the player starts an action, whether the action is long or immediate.
        /// </summary>
        public event EventHandler<SimpleActionArgs> OnBeforeActionStarted;

        /// <summary>
        /// Raised just after when the player starts an action, whether the action is long or immediate.
        /// </summary>
        public event EventHandler<SimpleActionArgs> OnAfterActionStarted;

        /// <summary>
        /// Raised just after the player cancels a long action like lockpicking or searching.
        /// </summary>
        public event EventHandler<SimpleActionArgs> OnAfterLongActionCancelled;

        /// <summary>
        /// Raised just after the player completes a long action like lockpicking or searching.
        /// </summary>
        public event EventHandler<SimpleActionArgs> OnAfterLongActionCompleted;

        /// <summary>
        /// Raised each frame when the player has made progress on a long action (while the player is looking at a lock they are picking, for example).
        /// </summary>
        public event EventHandler<ProgressChangedActionArgs> OnAfterLongActionProgressed;

        internal void OnLongActionCancelled()
        {
            longActionInProgress = false;
            OnAfterLongActionCancelled?.Invoke(this, currentPlayerInteraction);
        }

        internal void OnLongActionCompleted()
        {
            longActionInProgress = false;
            OnAfterLongActionCompleted?.Invoke(this, currentPlayerInteraction);
        }

        internal void OnLongActionProgressed(float amountThisFrame, float amountTotal)
        {
            if (amountTotal < lastAmount)
            {
                // Just started making progress
                longActionInProgress = true;
                lastAmount = amountTotal;
                OnAfterActionStarted?.Invoke(this, currentPlayerInteraction);
            }
            OnAfterLongActionProgressed?.Invoke(
                this,
                new ProgressChangedActionArgs(amountThisFrame, amountTotal)
            );
        }

        internal void OnActionStarted(SimpleActionArgs args, bool after)
        {
            if (after)
                OnAfterActionStarted?.Invoke(this, args);
            else
                OnBeforeActionStarted?.Invoke(this, args);
        }

        public sealed class SimpleActionArgs : EventArgs
        {
            internal InteractablePreset.InteractionAction Action { get; set; }
            internal Interactable.InteractableCurrentAction CurrentAction { get; set; }
            internal InteractablePreset.InteractionKey Key { get; set; }
            public InteractableInstanceData InteractableInstanceData { get; internal set; }
            public bool IsFpsItemTarget { get; internal set; }

            public bool TryGetAction(out InteractablePreset.InteractionAction action)
            {
                action = null;
                if (this.Action == null)
                {
                    return false;
                }
                action = this.Action;
                return true;
            }

            public bool TryGetCurrentAction(
                out Interactable.InteractableCurrentAction currentAction
            )
            {
                currentAction = null;
                if (this.CurrentAction == null)
                {
                    return false;
                }
                currentAction = this.CurrentAction;
                return true;
            }

            public bool TryGetKey(out InteractablePreset.InteractionKey key)
            {
                key = default;
                if (this.Key == default)
                {
                    return false;
                }
                key = this.Key;
                return true;
            }
        }

        public sealed class ProgressChangedActionArgs : EventArgs
        {
            public ProgressChangedActionArgs(float amountThisFrame, float amountTotal)
            {
                AmountThisFrame = amountThisFrame;
                AmountTotal = amountTotal;
            }

            public SimpleActionArgs ActionArgs => Lib.Interaction.currentPlayerInteraction;
            public float AmountThisFrame { get; }
            public float AmountTotal { get; }
        }
    }
}
