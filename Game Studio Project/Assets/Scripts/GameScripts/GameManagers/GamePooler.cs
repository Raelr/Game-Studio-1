﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlternativeArchitecture {


    // KEY GUIDE:

    /*  0 = null
     *  1 = player
     *  2 = neon ring
     *  3 = regular asteroid
     *  4 = boost asteroid
     *  5 = big asteroids
     *  6 = homing asteroids
     *  7 = big boost asteroids
     *  8 = bad neon rings
     *  9 = slower neon rings
     *  10 = bones
     *  11 = big bones
     *  12 = skull
     *  13 = necro
     *  14 = giant satan
     *  15 = neon lines
     *  16 = boost bubble
     *  17 = giant skulls
     *  18 = man
     *  19 = relic
     *  20 = galaxy
     *  21 = planet
     */




    public class GamePooler : InitialisedEntity {

        private float currentForceMultiplier;

        public float CurrentSpeed { get { return currentForceMultiplier; } }

        public static GamePooler instance;

        public enum PoolingFlags
        {
            POSITION_TO_ZERO
        }

        public enum RetrieveMethod {
            BOTTOM  //retrieves from the bottom to the top of the list
        }

        public enum PoolingAvailability {
            OCCUPIED, // currently being used in-game
            POOLED // is finished and is now in the pool
        }

        public bool debug = false;


        // used for defining the pooled object settings in inspector
        [System.Serializable]
        public struct PooledObjectSetting {
            public int objectType;
            public GameObject objectPrefab;
            public int maxCount;
            public bool preload;
            public List<PoolingFlags> poolingFlags; //flags that action when you retrieve/create the object
        }

        [SerializeField]
        public List<PooledObjectSetting> poolSettings = new List<PooledObjectSetting>();


        // used for storing the current data on the pooled object
        // pool
        // k [object type] | v [pooled object type data]
        //                        list of --> [pooled object data]
        //                                         pooled object,  availability
        //

        public struct PooledObjectData {
            public GameObject pooledObject;
            public PoolingAvailability availability;
        }

        public struct PooledObjectTypeData {
            public List<PooledObjectData> objects;
        }

        public Dictionary<int, PooledObjectTypeData> pool = new Dictionary<int, PooledObjectTypeData>();



        public struct RetrievedObjectData {
            public GameObject retrievedObject;
            public bool allowSpawning;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }


        public override void Initialise() {
            base.Initialise();
            PreloadPool();
        }


        private void PreloadPool() {
            foreach (PooledObjectSetting objectSetting in poolSettings)
                if (objectSetting.preload)
                    PreloadObjectType(objectSetting);
        }

        private void PreloadObjectType(PooledObjectSetting objectSetting) {
            for (int i = 0; i < objectSetting.maxCount; i++) {
                GameObject newObject = RetrieveOrCreate(objectSetting.objectType);
                SetAvailabilityInPool(objectSetting.objectType, newObject, PoolingAvailability.POOLED);
            }
        }

        public void ResetPool()
        {
            foreach (int objType in pool.Keys)
            {
                ResetObjects(pool[objType]);
            }

            pool.Clear();
        }

        public void ResetPoolAndPreload()
        {
            ResetPool();
            PreloadPool();
        }


        private void ResetObjects(PooledObjectTypeData objectsData)
        {
            foreach (PooledObjectData objectData in objectsData.objects)
            {
                Destroy(objectData.pooledObject);
            }
        }

        public GameObject RetrieveOrCreate (int objectType)
        {
            RetrievedObjectData getObject = Retrieve(objectType, RetrieveMethod.BOTTOM);

            //if it found an object in the pooling list, return it and set it to occupied
            if (getObject.retrievedObject.isNotNull())
                return SetupRetrieved(objectType, getObject);

            //if the pooler did not find an object, and does not allow spawning anymore objects in to replace it, return a null object
            if (!getObject.allowSpawning) return null;

            //if it cannot be retrieved because the object pooler requires it to be instantiated
            if (getObject.retrievedObject.isNull())
                getObject = new RetrievedObjectData() { retrievedObject = Create(objectType) };

            return SetupRetrieved(objectType, getObject);
        }

        //sets up the retrieved object in the pool
        private GameObject SetupRetrieved(int objectType, RetrievedObjectData getObject) {
            //sets the availability to occupied now that the specific object is being used
            SetAvailabilityInPool(objectType, getObject.retrievedObject, PoolingAvailability.OCCUPIED);

            //do any pooling flags that the object settings define
            List<PoolingFlags> poolingFlags = GetSettingsFlags(objectType);
            if (getObject.retrievedObject.isNotNull() && poolingFlags.Count > 0) {
                DoFlags(getObject, poolingFlags);
            }

            return getObject.retrievedObject;
        }

        private void DoFlags(RetrievedObjectData getObject, List<PoolingFlags> flags) {
            if (flags.Contains(PoolingFlags.POSITION_TO_ZERO))
                getObject.retrievedObject.transform.position = Vector3.zero;
        }

        // recycle this object into the pool
        // important: ensure objects you pool have been previously created via the RetrieveOrCreate() method
        public void PoolObject(int objectType, GameObject obj) {
            SetAvailabilityInPool(objectType, obj, PoolingAvailability.POOLED);
            obj.Hide();
        }


        // checks whether it can receive the object from the pool
        private RetrievedObjectData Retrieve(int objectType, RetrieveMethod method) {
            // retrieve nothing if the pool doesn't even have an entry for that object type
            if (!pool.ContainsKey(objectType)) {
                if (debug) Debug.Log("[POOLER] No entry exists for " + objectType);
                return new RetrievedObjectData() { allowSpawning = true };
            }

            // retrieve nothing if the pool for that object type hasn't been filled to the brim
            if (GetPoolCount(objectType) < GetSettingsMaxCount(objectType)) {
                if (debug) Debug.Log("[POOLER] " + objectType + " is not at max capacity yet  (" + GetPoolCount(objectType) + " < " + GetSettingsMaxCount(objectType) + ")");
                return new RetrievedObjectData() { allowSpawning = true };
            }

            switch (method) {
                // iterates from bottom to top of the object type's list looking for an object that is 'pooled' to be reused
                case RetrieveMethod.BOTTOM:
                    for (int i = 0; i < pool[objectType].objects.Count; i++) {
                        PooledObjectData pooledObjectData = pool[objectType].objects[i];
                        if (pooledObjectData.availability == PoolingAvailability.POOLED)
                            return new RetrievedObjectData() { retrievedObject = pooledObjectData.pooledObject };
                    }
                    if (debug) Debug.Log("[POOLER] All " + objectType + " objects are currently being used in game");
                    return new RetrievedObjectData() { allowSpawning = false };
            }

            // if the pooling method used is undefined / null
            if (debug) Debug.LogError("[POOLER] Unknown pool retrieving method");
            return new RetrievedObjectData() { allowSpawning = false };
        }

        private GameObject Create(int objectType) {
            if (debug) Debug.Log("[POOLER] Creating new " + objectType);

            //instanties the object based off the prefab in the settings
            GameObject newObject = Instantiate(GetSettingsPrefab(objectType));

            //hides the object
            newObject.Hide();

            //adds the object to the pool
            AddToPool(objectType, newObject);

            if (debug) Debug.Log("[POOLER] " + objectType + " now has " + GetPoolCount(objectType) + " object");

            //returns it
            return newObject;
        }

        
        public List<GameObject> GetObjects(int objectType)
        {
            List<GameObject> objectsList = new List<GameObject>();

            if (!pool.ContainsKey(objectType)) return null;

            PooledObjectTypeData objectTypeData = pool[objectType];

            foreach (PooledObjectData objectData in objectTypeData.objects)
            {
                objectsList.Add(objectData.pooledObject);
            }

            return objectsList;
        }







        ///////// HELPER METHODS

        private void SetAvailabilityInPool(int objectType, GameObject objectKey, PoolingAvailability newAvailability) {
            //retreives the index of where the object key exists on the object type entry's list
            int objectDataIndex = GetObjectDataIndex(pool[objectType].objects, objectKey, objectType);

            //gets the pooled object data struct and sets the new availability
            if (objectDataIndex != -1) {
            PooledObjectData getObjectData = pool[objectType].objects[objectDataIndex];
            getObjectData.availability = newAvailability;

            //places the modified pooled object data struct back into the object type entry's list
            pool[objectType].objects[objectDataIndex] = getObjectData;
            }
        }

        private void AddToPool(int objectType, GameObject newObject) {
            if (pool.ContainsKey(objectType))
                //if the pool has an entry for that object type, add it in to that entry's list
                AddToEntryInPool(objectType, newObject);
            else
                //if the pool doesn't have an entry, create a new entry initialised with a list containing that new object (count = 1)
                AddNewEntryToPool(objectType, newObject);
        }

        private void AddToEntryInPool(int objectType, GameObject newObject) {
            pool[objectType].objects.Add(new PooledObjectData() {
                pooledObject = newObject,
                availability = PoolingAvailability.OCCUPIED
            });
        }

        private void AddNewEntryToPool(int objectType, GameObject newObject) {
            pool.Add(objectType, new PooledObjectTypeData() {
                objects = new List<PooledObjectData>()
                    {
                        new PooledObjectData ()
                        {
                            pooledObject = newObject,
                            availability = PoolingAvailability.OCCUPIED
                        }
                    }
            });
        }


        ///////// HELPER FUNCTIONS


        private int GetObjectDataIndex(List<PooledObjectData> objectDataList, GameObject objectKey, int entry) {
            
            
            foreach (PooledObjectData pooledObjectData in objectDataList)
                if (pooledObjectData.pooledObject == objectKey)
                    return objectDataList.IndexOf(pooledObjectData);
            if (debug) Debug.LogError("[POOLER] Cannot find the object: " + objectKey + " in the list of objects for the entry: " + entry);
            return -1;
        }

        // gets current number of objects stored in the pool
        private int GetPoolCount(int key) {
            if (pool.ContainsKey(key))
                return pool[key].objects.Count;
            return 0;
        }

        private List<PoolingFlags> GetSettingsFlags(int key) {
            return GetObjectSetting(key).poolingFlags;
        }

        private int GetSettingsMaxCount(int key) {
            return GetObjectSetting(key).maxCount;
        }

        private GameObject GetSettingsPrefab(int key) {
            return GetObjectSetting(key).objectPrefab;
        }

        private PooledObjectSetting GetObjectSetting(int key) {
            foreach (PooledObjectSetting objectSetting in poolSettings)
                if (objectSetting.objectType == key)
                    return objectSetting;
            if (debug) Debug.LogError("[POOLER] Cannot find setting with Object Type: " + key);
            return new PooledObjectSetting();
        }

        public void SetObstacleSpeed(float newSpeed) {
            List<GameObject> obstacles = GetObjects(3); //regular asteroids
            obstacles.AddRange(GetObjects(4)); //boost asteroids
            obstacles.AddRange(GetObjects(2)); //neon rings
            obstacles.AddRange(GetObjects(5)); //large asteroids
            obstacles.AddRange(GetObjects(6)); //homing
            obstacles.AddRange(GetObjects(7)); //big boost
            obstacles.AddRange(GetObjects(8)); //bad rings
            obstacles.AddRange(GetObjects(9)); //slower rings
            obstacles.AddRange(GetObjects(10)); //bones
            obstacles.AddRange(GetObjects(11)); //big bones
            obstacles.AddRange(GetObjects(12)); //skull
            obstacles.AddRange(GetObjects(13)); //necro
            obstacles.AddRange(GetObjects(14)); //giant satan
            obstacles.AddRange(GetObjects(15)); //neon lines
            obstacles.AddRange(GetObjects(16)); //boost bubble
            obstacles.AddRange(GetObjects(17)); //giant skulls
            obstacles.AddRange(GetObjects(18)); //man
            obstacles.AddRange(GetObjects(19)); 
            obstacles.AddRange(GetObjects(19)); //galaxy
            obstacles.AddRange(GetObjects(19)); //planet

            foreach (GameObject obstacle in obstacles) {
                currentForceMultiplier = newSpeed;
                obstacle.GetComponent<Obstacle>().forceMultiplier = newSpeed;
                obstacle.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}