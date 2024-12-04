using System.Collections.Generic;
using Optimization.Baseline;
using UnityEngine;

namespace Optimization
{
    public class ProjectilePool : MonoBehaviour
    {
        public static ProjectilePool Instance { get; private set; }


        private readonly List<Projectile> openList = new();
       private readonly Dictionary<int, Queue<Projectile>> closedList = new();
       
       private void Awake()
       {
           if (Instance && Instance != this)
           {
               Destroy(gameObject);
               return;
           }

           Instance = this;
       }

       public Projectile AddToPool(Projectile projectile, Vector3 position, Quaternion rotation)
       {
           int id = projectile.Stats.GetID;
           Projectile myProjectile;

           if (closedList.TryGetValue(id, out Queue<Projectile> queue))
           {
               if (!queue.TryDequeue(out myProjectile)) myProjectile = Instantiate(projectile, position, rotation, transform);
               else myProjectile.transform.SetPositionAndRotation(position, rotation);
           }
           else
           {
               closedList.Add(id, new Queue<Projectile>());
               myProjectile = Instantiate(projectile, position, rotation, transform);
           }
           myProjectile.gameObject.SetActive(true);
           openList.Add(myProjectile);
           return myProjectile;
       }

       private void Update()
       {
           float dt = Time.deltaTime;

           for (int i = openList.Count - 1; i >= 0; i--)
           {
               Projectile myProjectile = openList[i];

               if (!myProjectile.Tick(dt))
               {
                   openList.RemoveAt(i);
                   closedList[myProjectile.Stats.GetID].Enqueue(myProjectile);
               }
               
           }
       }
    }
}
