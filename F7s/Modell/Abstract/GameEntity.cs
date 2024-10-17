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
                this.SetName(name);
            }
            gameEntities.Add(this);
            this.lastUpdateDate = currentSimulationDate;
            this.EnqueueForUpdate();
        }

        public virtual void Delete () {
            Console.WriteLine("Deleting GE " + this + ".");
            this.Deleted = true;
            gameEntities.Remove(this);
            this.SetName("DELETED " + this.GetName());
        }

        public override string ToString () {
            return this.Name ?? this.GetType().Name;
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
            get { return this.GetName(); }
            set {
                this.SetName(value);
            }
        }
        private string name;
        public void SetName (string name) {
            this.name = name;
        }
        public string GetName () {
            return this.name ?? this.GetType().Name;
        }

        public virtual int UiHierarchyIndentationLevel () {
            return 0;
        }
        public virtual string UiHierarchyIndentation () {
            string indentation = "";
            for (int i = 0; i < this.UiHierarchyIndentationLevel(); i++) {
                indentation += " ";
            }
            return indentation;
        }

        public void ScheduledUpdate () {
            if (this.Deleted) {
                return;
            }

            double deltaTime = currentSimulationDate - this.lastUpdateDate;
            this.lastUpdateDate = currentSimulationDate;
            this.Update(deltaTime);

            this.EnqueueForUpdate();
        }

        public virtual void RenderUpdate (double deltaTime) {

        }

        public virtual void PhysicsUpdate (double deltaTime) {
            this.GetLocality()?.Update(deltaTime);

        }

        protected virtual void Update (double deltaTime) {
            // Up to subtype.
        }

        private static double CurrentEngineTime () {
            throw new NotImplementedException();
        }

        private double CalculateDateOfNextUpdate () {
            double timeUntilNextUpdate = CurrentEngineTime();
            return currentSimulationDate + timeUntilNextUpdate;
        }

        private void EnqueueForUpdate () {
            updateQueue.Enqueue(this, this.CalculateDateOfNextUpdate());
        }

        public static void OnEngineUpdate (double engineDeltaTime, double speedFactor, bool skipToNextDate) {

            if (Zeit.Paused) {
                return;
            }

            if (skipToNextDate && updateQueue.Count > 0) {
                double nextDate = PeekUpdatePriority();
                currentSimulationDate = nextDate;
            } else {
                double maximumDeltaTime = updateQueue.Count > 0 ? PeekUpdatePriority() - currentSimulationDate : double.MaxValue;
                double simulationDeltaTime = Math.Clamp(engineDeltaTime * speedFactor, 0, maximumDeltaTime);
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
            data.boundingRadius = 1f;
            data.scale = new Vector3(1, 1, 1);
            return data;
        }

        public virtual Farbe? UiColor () {
            return null;
        }

        public virtual void ConfigureInfoblock (Infoblock infoblock) {
            infoblock.Text = this.Name;
            infoblock.TooltipText = this.Name;
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
