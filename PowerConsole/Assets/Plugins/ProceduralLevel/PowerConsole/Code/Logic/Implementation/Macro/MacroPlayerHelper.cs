using System;
using System.Collections;
using UnityEngine;

namespace ProceduralLevel.PowerConsole.Logic
{
    public class MacroPlayerHelper : MonoBehaviour
    {
        public static MacroPlayerHelper Instance;

        private void Awake() => Instance = this;

        public static void PlayMacro(ConsoleInstance console, Macro macro)
        {
            foreach (var command in macro.Queries) console.Execute(command, false);
        }

        public void PlayMacroWithDelay(ConsoleInstance console, Macro macro, float delay) =>
            StartCoroutine(PlayMacroCoroutine(console, macro, delay));

        private static IEnumerator PlayMacroCoroutine(ConsoleInstance console, Macro macro, float delay)
        {
            foreach (var command in macro.Queries)
            {
                console.Execute(command, false);
                yield return new WaitForSecondsRealtime(delay);
            }

            yield return null;
        }
    }
}