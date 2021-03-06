using Plugins.ECSEntityBuilder.Variables;
using Unity.Mathematics;
using Unity.Transforms;

namespace Plugins.ECSEntityBuilder.Steps
{
    public class SetPosition : IEntityBuilderStep
    {
        private float3 m_position;

        public SetPosition(float3 position)
        {
            m_position = position;
        }

        public void Process(EntityManagerWrapper wrapper, EntityVariableMap variables, ref EntityBuilderData data)
        {
            wrapper.SetComponentData(data.entity, new Translation {Value = m_position});
        }
    }
}