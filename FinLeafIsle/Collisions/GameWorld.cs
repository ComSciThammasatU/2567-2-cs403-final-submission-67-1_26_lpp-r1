using System;
using System.Collections.Generic;
using FinLeafIsle.Collisions;
using FinLeafIsle.Components;
using Microsoft.Xna.Framework;


namespace FinLeafIsle.Collisions
{
    public class GameWorld
    {
        public GameWorld(Vector2 gravity)
        {
            Gravity = gravity;
        }

        public Vector2 Gravity { get; set; }

        public List<Body> _dynamicBodies = new List<Body>();
        public List<Body> _staticBodies = new List<Body>();
        public List<WaterArea> _waterAreas = new List<WaterArea>();
        public List<GateArea> _gateAreas = new List<GateArea>();
        public List<CameraBlock> _cameraBlocks = new List<CameraBlock>();
        public List<Bed> _beds = new List<Bed>();
        public void AddBody(Body body)
        {
            if (body.BodyType == BodyType.Dynamic)
                _dynamicBodies.Add(body);
            else
                _staticBodies.Add(body);
        }

        public void RemoveBody(Body body)
        {
            if (body.BodyType == BodyType.Dynamic)
                _dynamicBodies.Remove(body);
            else
                _staticBodies.Remove(body);
        }

        public void AddWaterArea(WaterArea waterArea)
        {/*
            if (!_waterAreas.ContainsKey(waterArea.Location))
            {
                _waterAreas[waterArea.Location] = new List<WaterArea>();
            }
            _waterAreas[waterArea.Location].Add(waterArea);*/
            _waterAreas.Add(waterArea);
        }

        public void RemoveWaterArea(WaterArea waterArea)
        { /*
            if (_waterAreas.TryGetValue(waterArea.Location, out var areas))
            {
                areas.Remove(waterArea);

                // If the list is now empty, remove the key from the dictionary to save memory
                if (areas.Count == 0)
                {
                    _waterAreas.Remove(waterArea.Location);
                }
            }*/
            _waterAreas.Clear();
        }

        public void Update(float deltaTime)
        {
            // apply gravity (and other forces?)
            foreach (var body in _dynamicBodies)
                body.Velocity += Gravity;
           

            foreach (var dynamicBody in _dynamicBodies)
            {
                dynamicBody.Position.X += dynamicBody.Velocity.X * deltaTime;
                ResolveCollisions(dynamicBody);

                dynamicBody.Position.Y += dynamicBody.Velocity.Y * deltaTime;
                ResolveCollisions(dynamicBody);
            }
        }

        private void ResolveCollisions(Body dynamicBody)
        {
            foreach (var staticBody in _staticBodies)
            {
                var vector = staticBody.Position - dynamicBody.Position;

                if (CollisionTester.AabbAabb(dynamicBody.BoundingBox, staticBody.BoundingBox, vector, out var manifold))
                {
                    dynamicBody.Position -= manifold.Normal * manifold.Penetration;
                    dynamicBody.Velocity = dynamicBody.Velocity * new Vector2(Math.Abs(manifold.Normal.Y), Math.Abs(manifold.Normal.X));
                }
            }

            foreach (var otherDynamicBody in _dynamicBodies)
            {
                if (dynamicBody != otherDynamicBody) //TODO: Equality should be implemented better than this
                {
                    var vector = otherDynamicBody.Position - dynamicBody.Position;

                    if (CollisionTester.AabbAabb(dynamicBody.BoundingBox, otherDynamicBody.BoundingBox, vector, out var manifold))
                    {
                        //Invert how the manifold is applied from how this is done with static bodyies. 
                        //This will create the effect that the dynamic body in motion is pushing the other dynamic body.
                        otherDynamicBody.Position += manifold.Normal * manifold.Penetration;
                        otherDynamicBody.Velocity = otherDynamicBody.Velocity * new Vector2(System.Math.Abs(manifold.Normal.Y), System.Math.Abs(manifold.Normal.X));
                    }
                }
            }
        }
    }
}
