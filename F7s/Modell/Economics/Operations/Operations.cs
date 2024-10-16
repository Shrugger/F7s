using F7s.Modell.Abstract;
using F7s.Modell.Economics.Industry;

namespace F7s.Modell.Economics.Operations
{
    public abstract class Operations : GameEntity
    {
        // Operations are (economic) activities in which work is done over time to accomplish some desired result.
        // These can but needn't be associated with a given facility.

        /// product / (work-force * time)
        public float Efficiency { get; private set; } = 0;

        public Work Workforce { get; private set; } = new Work();

        protected const float DefaultEfficiency = 0;
        public Operations(float efficiency = DefaultEfficiency)
        {
            Efficiency = efficiency;
        }

        public void ProvideWorkforce(Work force)
        {
            Workforce.AddForce(force.Workforce);
        }

        public void Operate(double deltaTime)
        {
            if (!Operational())
            {
                return;
            }
            float effort = Workforce.ExtractLabor((float)deltaTime);
            float results = effort * Efficiency;
            OperateOperationally(results);
        }

        protected abstract void OperateOperationally(float effectiveWorkAccomplished);

        public virtual bool Operational()
        {
            return true;
        }
    }
}
