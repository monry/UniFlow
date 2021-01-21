using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniFlow.Connector.Controller;
using UnityEngine.TestTools;

namespace UniFlow.Tests.Runtime
{
    public class AnimatorTriggerTest : UniFlowTestBase
    {
        [UnityTest]
        public IEnumerator AnimatorTrigger()
        {
            yield return
                RunAssert(
                    "AnimatorTrigger",
                    AssertAnimatorTrigger,
                    null,
                    1.1
                );
        }

        private void AssertAnimatorTrigger(IEnumerable<IConnector> sentConnectors)
        {
            var connectors = sentConnectors.ToList();
            Assert.AreEqual(3, connectors.Count);

            var connector = connectors[0] as AnimatorTrigger;
            Assert.NotNull(connector);

            HasAssert = true;
        }
    }
}
