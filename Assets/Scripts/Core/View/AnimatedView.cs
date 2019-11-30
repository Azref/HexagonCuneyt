using System.Collections;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Assets.Scripts.Core.View
{
    public class AnimatedView : EventView
    {
        /// <summary>
        /// Delay time
        /// </summary>
        public float DispatchDelay = 0;

        /// <summary>
        /// Animations array
        /// </summary>
        public Animation[] CloseAnimations;

        /// <summary>
        /// Inner coroutine functions to wait animations
        /// </summary>
        protected IEnumerator DispatchDelayedInner(object eventType, float delay)
        {
            yield return new WaitForSeconds(delay);

            dispatcher.Dispatch(eventType);
        }

        /// <summary>
        /// Delayed dispatch function with given delay time
        /// </summary>
        protected void DispatchDelayed(object eventType, float delay)
        {
            PlayAnimations();
            StartCoroutine(DispatchDelayedInner(eventType, delay));
        }

        /// <summary>
        /// Delayed dispatch function. Works according to the animations attached
        /// </summary>
        protected void DispatchDelayed(object eventType)
        {
            PlayAnimations();
            StartCoroutine(DispatchDelayedInner(eventType, DispatchDelay));
        }

        /// <summary>
        /// Play attached animations
        /// </summary>
        protected void PlayAnimations()
        {
            if (CloseAnimations == null)
                return;

            foreach (var closeAnimation in CloseAnimations)
            {
                PlayByIndex(closeAnimation, 0);
            }
        }

        /// <summary>
        /// Play animation with given index
        /// </summary>
        private void PlayByIndex(Animation anim, int index)
        {
            if (anim == null)
                return;

            var i = 0;
            foreach (AnimationState animationState in anim)
            {
                if (i == index)
                {
                    anim.Play(animationState.clip.name);
                    return;
                }

                i++;
            }
        }
    }
}