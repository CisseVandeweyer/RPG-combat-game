using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float maxNavPathLength = 40f;

        NavMeshAgent navMeshAgent;
        Health health;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void startMoveAction(Vector3 destination, float speedFraction)
        {

            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destenation, float speedFraction)
        {
            navMeshAgent.destination = destenation;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;
            return true;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;

        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return total;
        }

        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {

            // Dictionary mogelijkheid
            // Dictionary<string, object> data = new Dictionary<string, object>();
            // data["position"] = new SerializableVector3(transform.position);
            // data["rotation"] = new SerializableVector3(transform.eulerAngles);
            // return data;

            // struct mogelijkheid
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            // Dictionary mogelijkheid
            // Dictionary<string, object> data = (Dictionary<string, object>)state;
            // GetComponent<NavMeshAgent>().enabled = false;
            // transform.position = ((SerializableVector3)data["position"]).ToVector();
            // transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            // GetComponent<NavMeshAgent>().enabled = true;

            // struct mogelijkheid
            // MoverSaveData data = (MoverSaveData)state;
            // GetComponent<NavMeshAgent>().enabled = false;
            // transform.position = data.position.ToVector();
            // transform.eulerAngles = data.rotation.ToVector();
            // GetComponent<NavMeshAgent>().enabled = true;

            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            navMeshAgent.enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
