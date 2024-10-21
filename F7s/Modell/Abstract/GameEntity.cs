using F7s.Engine;
using F7s.Modell.Handling;
using F7s.Modell.Handling.PhysicalData;
using F7s.Modell.Physical.Localities;
using F7s.Utility;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;

namespace F7s.Modell.Abstract {


    public class GameEntity : IGameValue {

        private static double currentSimulationDate;
        private static readonly List<GameEntity> gameEntities = new List<GameEntity>();
        private static readonly PriorityQueue<GameEntity, double> updateQueue = new PriorityQueue<GameEntity, double>();


        public static string ReportEntities () {
            return
                "Sim Date: " + Math.Round(currentSimulationDate) + "\n" +
                "GEs: " + gameEntities.Count + " / " + updateQueue.Count;
        }

        public bool Deleted { get; private set; }
        private double lastUpdateDate = 0;

        public GameEntity (string name = null) {
            if (name != null) {
                SetName(name);
            }
            gameEntities.Add(this);
            lastUpdateDate = currentSimulationDate;
            EnqueueForUpdate();
        }

        public virtual void Delete () {
            Console.WriteLine("Deleting GE " + this + ".");
            Deleted = true;
            gameEntities.Remove(this);
            SetName("DELETED " + GetName());
        }

        public override string ToString () {
            return Name ?? GetType().Name;
        }

        /// <summary>
        /// Folds this entity into a less detailed super-entity.
        /// </summary>
        /// <returns>True if successfully zipped, false if zipping was not possible.</returns>
        public virtual bool Zip () {
            return false;
        }
        /// <summary>
        /// Replaces this entity with a number of more detailed sub-entities.
        /// </summary>
        /// <returns>True if successfully popped, false if popping was not possible.</returns>
        public virtual bool Pop () {
            return false;
        }
        public Infoblock GenerateInfoblock (bool preview, Vector2? position = null) {
            throw new NotImplementedException();
        }

        public virtual Farbe RepresentativeColor () {
            return Farbe.white;
        }

        public static implicit operator bool (GameEntity value) {
            return value != null && !value.Deleted;
        }

        public string Name {
            get { return GetName(); }
            set {
                SetName(value);
            }
        }
        private string name;
        public void SetName (string name) {
            this.name = name;
        }
        public string GetName () {
            return name ?? GetType().Name;
        }

        public virtual int UiHierarchyIndentationLevel () {
            return 0;
        }
        public virtual string UiHierarchyIndentation () {
            string indentation = "";
            for (int i = 0; i < UiHierarchyIndentationLevel(); i++) {
                indentation += " ";
            }
            return indentation;
        }

        public void ScheduledUpdate () {
            if (Deleted) {
                return;
            }

            double deltaTime = currentSimulationDate - lastUpdateDate;
            lastUpdateDate = currentSimulationDate;
            Update(deltaTime);

            EnqueueForUpdate();
        }

        public virtual void RenderUpdate (double deltaTime) {

        }

        public virtual void PhysicsUpdate (double deltaTime) {
            GetLocality()?.Update(deltaTime);

        }

        protected virtual void Update (double deltaTime) {
            // Up to subtype.
        }

        private static double CurrentEngineTime () {
            return Zeit.GetEngineTimeSeconds();
        }

        private double CalculateDateOfNextUpdate () {
            double timeUntilNextUpdate = CurrentEngineTime();
            return currentSimulationDate + timeUntilNextUpdate;
        }

        private void EnqueueForUpdate () {
            updateQueue.Enqueue(this, CalculateDateOfNextUpdate());
        }

        public static void OnEngineUpdate (double speedFactor, bool skipToNextDate) {

            if (Zeit.Paused) {
                return;
            }

            if (skipToNextDate && updateQueue.Count > 0) {
                double nextDate = PeekUpdatePriority();
                currentSimulationDate = nextDate;
            } else {
                double maximumDeltaTime = updateQueue.Count > 0 ? PeekUpdatePriority() - currentSimulationDate : double.MaxValue;
                double simulationDeltaTime = Math.Clamp(Zeit.DeltaTimeSeconds() * speedFactor, 0, maximumDeltaTime);
                currentSimulationDate += simulationDeltaTime;
            }

            UpdateScheduledEntities();
        }

        private static double PeekUpdatePriority () {
            return updateQueue.Peek().GetUpdatePriority();
        }

        private double GetUpdatePriority () {
            throw new NotImplementedException();
        }

        public static void OnRenderUpdate (double deltaTime) {
            foreach (GameEntity ge in gameEntities) {
                ge.RenderUpdate(deltaTime);
            }
        }

        public static void OnPhysicsUpdate (double deltaTime) {
            foreach (GameEntity ge in gameEntities) {
                ge.PhysicsUpdate(deltaTime);
            }
        }

        private static void UpdateScheduledEntities () {
            bool abort = false;

            ulong startTime = Zeit.GetTicksMsec();

            while (!abort) {
                GameEntity toUpdate = null;

                double nextPriority = PeekUpdatePriority();
                if (nextPriority <= currentSimulationDate) {
                    toUpdate = updateQueue.Dequeue();
                } else {
                    abort = true;
                }

                if (toUpdate != null) {
                    toUpdate.ScheduledUpdate();
                }


                ulong timeElapsed = Zeit.GetTicksMsec() - startTime;
                if (timeElapsed > 100) {
                    abort = true;
                }
            }
        }

        public virtual PhysicalRepresentationData GetPhysicalData () {
            PresumedPhysicalData data = new PresumedPhysicalData(this);
            data.boundingRadius = 1;
            data.scale = new Double3(1, 1, 1);
            return data;
        }

        public virtual Farbe? UiColor () {
            return null;
        }

        public virtual void ConfigureInfoblock (Infoblock infoblock) {
            infoblock.Text = Name;
            infoblock.TooltipText = Name;
            return;
        }
        public virtual void ConfigureContextMenu (ContextMenu contextMenu) {
            return;
        }

        public virtual Locality GetLocality () {
            return null;
        }

        public virtual GameEntity UiHierarchyParent () {
            return null;
        }

        public virtual int SubentityCount () {
            return 0;
        }

        public virtual List<GameEntity> SubEntities () {
            return null;
        }
    }

}
