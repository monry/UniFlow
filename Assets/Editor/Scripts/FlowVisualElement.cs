using UnityEngine.UIElements;

namespace UniFlow.Editor
{
    public class FlowVisualElement : VisualElement
    {
        public FlowVisualElement()
        {
            styleSheets.Add(AssetReferences.Instance.FlowVisualElement);

            var content = new VisualElement
            {
                name = "Content",
            };

            {
                var graphView = new FlowGraphView
                {
                    name = typeof(FlowGraphView).Name,
                };
                graphView.Initialize();

                content.Add(graphView);
            }

            Add(content);
        }
    }
}