using System;
using System.Collections;
using Rewired;
using UnityEngine;
using UniverseLib;

namespace SOD.Common.Custom
{
    internal sealed class InputSuppressionEntry
    {
        public InputSuppressionEntry(string id, KeyCode keyCode, TimeSpan? time = null)
        {
            Id = id;
            KeyCode = keyCode;
            InteractionKey = InteractablePreset.InteractionKey.none;
            ElementIdentifierName = string.Empty;
            ElementIdentifierId = -1;
            TimeRemainingSec = (float)(time?.TotalSeconds ?? 0d);
        }

        public InputSuppressionEntry(string id, InteractablePreset.InteractionKey interactionKey, TimeSpan? time = null)
        {
            Id = id;
            InteractionKey = interactionKey;
            var binding = Lib.InputDetection.GetBinding(interactionKey);
            ElementIdentifierName = binding.elementIdentifierName;
            ElementIdentifierId = binding.elementIdentifierId;
            KeyCode = Lib.InputDetection.GetApproximateKeyCode(binding);
            TimeRemainingSec = (float)(time?.TotalSeconds ?? 0d);
        }

        public KeyCode KeyCode { get; }
        public string ElementIdentifierName { get; }
        public int ElementIdentifierId { get; }
        public string Id { get; }
        public InteractablePreset.InteractionKey InteractionKey { get; }
        public float TimeRemainingSec { get; set; }

        private Coroutine _coroutine;

        public void Start()
        {
            if (_coroutine != null || TimeRemainingSec <= 0) return;
            _coroutine = RuntimeHelper.StartCoroutine(Tick());
        }

        public void Stop()
        {
            if (_coroutine == null) return;
            RuntimeHelper.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        private IEnumerator Tick()
        {
            while (TimeRemainingSec > 0f)
            {
                yield return new WaitForEndOfFrame();

                if (!Lib.Time.IsInitialized || Lib.Time.IsGamePaused || Lib.SaveGame.IsSaving)
                    continue;

                float deltaTime = Time.deltaTime;
                TimeRemainingSec -= deltaTime;
            }

            TimeRemainingSec = 0f;
            if (Lib.InputDetection.InputSuppressionDictionary != null)
                Lib.InputDetection.InputSuppressionDictionary.Remove(Id);
            _coroutine = null;
        }

        /// <summary>
        /// Used to store the data into json object
        /// </summary>
        public class JsonData
        {
            public string Id { get; set; }
            public string ElementIdentifierName { get; set; }
            public int ElementIdentifierId { get; set; }
            public KeyCode KeyCode { get; set; }
            public InteractablePreset.InteractionKey InteractionKey { get; set; }
            public float Time { get; set; }
        }
    }
}