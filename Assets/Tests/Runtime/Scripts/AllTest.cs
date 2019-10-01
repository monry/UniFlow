using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniFlow.Connector.Controller;
using UniFlow.Connector.Event;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.TestTools;
using UnityEngine.UI;
using AnimationEvent = UniFlow.Connector.Event.AnimationEvent;

namespace UniFlow.Tests.Runtime
{
    public class AllTest : UniFlowTestBase
    {
        [UnityTest]
        public IEnumerator All()
        {
            yield return
                RunAssert(
                    "All",
                    AssertAll,
                    () =>
                        typeof(IPointerDownHandler)
                            .GetMethod("OnPointerDown")?
                            .Invoke(Object.FindObjectOfType<ObservablePointerDownTrigger>(), new object[] {new PointerEventData(EventSystem.current)}),
                    3.1
                );
        }

        [UnityTest]
        public IEnumerator Multiple()
        {
            yield return
                RunAssert(
                    "All",
                    AssertAll,
                    () =>
                        typeof(IPointerDownHandler)
                            .GetMethod("OnPointerDown")?
                            .Invoke(Object.FindObjectOfType<ObservablePointerDownTrigger>(), new object[] {new PointerEventData(EventSystem.current)}),
                    3.1,
                    2
                );
        }

        private void AssertAll(IEnumerable<IConnector> sentConnectors)
        {
            var connectors = sentConnectors.ToList();
            Assert.AreEqual(5, connectors.Count);

            {
                var connector = connectors[0] as UIBehaviourEventTrigger;
                Assert.NotNull(connector);
                Assert.IsInstanceOf<Image>(connector.UIBehaviour);
            }

            {
                var connector = connectors[1] as AnimatorTrigger;
                Assert.NotNull(connector);
                Assert.IsInstanceOf<Animator>(connector.Animator);
            }

            {
                var connector = connectors[2] as AnimationEvent;
                Assert.NotNull(connector);
                Assert.IsInstanceOf<Animator>(connector.Animator);
            }

            {
                var connector = connectors[3] as PlayableController;
                Assert.NotNull(connector);
                Assert.IsInstanceOf<PlayableDirector>(connector.PlayableDirector);
            }

            {
                var connector = connectors[4] as TimelineSignal;
                Assert.NotNull(connector);
            }

            HasAssert = true;
        }
    }
}
