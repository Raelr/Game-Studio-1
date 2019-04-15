using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlternativeArchitecture
{

    public enum ObjectType
    {
        PLAYER,
        OBSTACLE_SPHERE,
        OBSTACLE_SPHERE_BIG
    }

    public class GamePooler : InitialisedEntity
    {

        public enum PoolingFlags
        {
            POSITION_TO_ZERO
        }

        public enum RetrieveMethod
        {
            BOTTOM  //retrieves from the bottom to the top of the list
        }

        public enum PoolingAvailability
        {
            OCCUPIED, // currently being used in-game
            POOLED // is finished and is now in the pool
        }



        // used for defining the pooled object settings in inspector
        [System.Serializable]
        public struct PooledObjectSetting
        {
            public ObjectType objectType;
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

        public struct PooledObjectData
        {
            public GameObject pooledObject;
            public PoolingAvailability availability;
        }

        public struct PooledObjectTypeData
        {
            public List<PooledObjectData> objects;
        }

        public Dictionary<ObjectType, PooledObjectTypeData> pool = new Dictionary<ObjectType, PooledObjectTypeData>();



        public struct RetrievedObjectData
        {
            public GameObject retrievedObject;
            public bool allowSpawning;
        }


        public override void Initialise()
        {
            base.Initialise();
            
            PreloadPool();
        }

        private void PreloadPool ()
        {
            foreach (PooledObjectSetting objectSetting in poolSettings)
                if (objectSetting.preload)
                    PreloadObjectType(objectSetting);
        }

        private void PreloadObjectType (PooledObjectSetting objectSetting)
        {
            for (int i = 0; i < objectSetting.maxCount; i++)
            {
                GameObject newObject = RetrieveOrCreate(objectSetting.objectType);
                SetAvailabilityInPool(objectSetting.objectType, newObject, PoolingAvailability.POOLED);
            }
        }

        public GameObject RetrieveOrCreate (ObjectType objectType)
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
        private GameObject SetupRetrieved (ObjectType objectType, RetrievedObjectData getObject)
        {
            //sets the availability to occupied now that the specific object is being used
            SetAvailabilityInPool(objectType, getObject.retrievedObject, PoolingAvailability.OCCUPIED);

            //do any pooling flags that the object settings define
            List<PoolingFlags> poolingFlags = GetSettingsFlags(objectType);
            if (getObject.retrievedObject.isNotNull() && poolingFlags.Count > 0)
            {
                DoFlags(getObject, poolingFlags);
            }

            return getObject.retrievedObject;
        }

        private void DoFlags (RetrievedObjectData getObject, List<PoolingFlags> flags)
        {
            if (flags.Contains(PoolingFlags.POSITION_TO_ZERO))
                getObject.retrievedObject.transform.position = Vector3.zero;
        }

        // recycle this object into the pool
        // important: ensure objects you pool have been previously created via the RetrieveOrCreate() method
        public void PoolObject(ObjectType objectType, GameObject obj)
        {
            SetAvailabilityInPool(objectType, obj, PoolingAvailability.POOLED);
            obj.Hide();
        }


        // checks whether it can receive the object from the pool
        private RetrievedObjectData Retrieve (ObjectType objectType, RetrieveMethod method)
        {
            // retrieve nothing if the pool doesn't even have an entry for that object type
            if (!pool.ContainsKey(objectType))
            {
                Debug.Log("[POOLER] No entry exists for " + objectType);
                return new RetrievedObjectData() { allowSpawning = true };
            }

            // retrieve nothing if the pool for that object type hasn't been filled to the brim
            if (GetPoolCount(objectType) < GetSettingsMaxCount(objectType))
            {
                Debug.Log("[POOLER] " + objectType + " is not at max capacity yet  ("+ GetPoolCount(objectType)  + " < " + GetSettingsMaxCount(objectType) + ")");
                return new RetrievedObjectData() { allowSpawning = true };
            }

            switch (method)
            {
                // iterates from bottom to top of the object type's list looking for an object that is 'pooled' to be reused
                case RetrieveMethod.BOTTOM:
                    for (int i = 0; i < pool[objectType].objects.Count; i++)
                    {
                        PooledObjectData pooledObjectData = pool[objectType].objects[i];
                        if (pooledObjectData.availability == PoolingAvailability.POOLED)
                            return new RetrievedObjectData() { retrievedObject = pooledObjectData.pooledObject };
                    }
                    Debug.Log("[POOLER] All " + objectType + " objects are currently being used in game");
                    return new RetrievedObjectData() { allowSpawning = false };
            }

            // if the pooling method used is undefined / null
            Debug.LogError("[POOLER] Unknown pool retrieving method");
            return new RetrievedObjectData () { allowSpawning = false };
        }

        private GameObject Create(ObjectType objectType)
        {
            Debug.Log("[POOLER] Creating new " + objectType);

            //instanties the object based off the prefab in the settings
            GameObject newObject = Instantiate(GetSettingsPrefab(objectType));

            //hides the object
            newObject.Hide();

            //adds the object to the pool
            AddToPool(objectType, newObject);

            Debug.Log("[POOLER] " + objectType + " now has " + GetPoolCount(objectType) + " object");

            //returns it
            return newObject;
        }







        ///////// HELPER METHODS

        private void SetAvailabilityInPool(ObjectType objectType, GameObject objectKey, PoolingAvailability newAvailability)
        {
            //retreives the index of where the object key exists on the object type entry's list
            int objectDataIndex = GetObjectDataIndex(pool[objectType].objects, objectKey, objectType);

            //gets the pooled object data struct and sets the new availability
            PooledObjectData getObjectData = pool[objectType].objects[objectDataIndex];
            getObjectData.availability = newAvailability;

            //places the modified pooled object data struct back into the object type entry's list
            pool[objectType].objects[objectDataIndex] = getObjectData;
        }

        private void AddToPool(ObjectType objectType, GameObject newObject)
        {
            if (pool.ContainsKey(objectType))
                //if the pool has an entry for that object type, add it in to that entry's list
                AddToEntryInPool(objectType, newObject);
            else
                //if the pool doesn't have an entry, create a new entry initialised with a list containing that new object (count = 1)
                AddNewEntryToPool(objectType, newObject);
        }

        private void AddToEntryInPool (ObjectType objectType, GameObject newObject)
        {
            pool[objectType].objects.Add(new PooledObjectData()
            {
                pooledObject = newObject,
                availability = PoolingAvailability.OCCUPIED
            });
        }

        private void AddNewEntryToPool (ObjectType objectType, GameObject newObject)
        {
            pool.Add(objectType, new PooledObjectTypeData()
            {
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
        


        private int GetObjectDataIndex (List<PooledObjectData> objectDataList, GameObject objectKey, ObjectType entry)
        {
            foreach (PooledObjectData pooledObjectData in objectDataList)
                if (pooledObjectData.pooledObject == objectKey)
                    return objectDataList.IndexOf(pooledObjectData);

            Debug.LogError("[POOLER] Cannot find the object: " + objectKey + " in the list of objects for the entry: " + entry);
            return -1;
        }
        

        // gets current number of objects stored in the pool
        private int GetPoolCount(ObjectType key)
        {
            if (pool.ContainsKey(key))
                return pool[key].objects.Count;
            return 0;
        }

        private List<PoolingFlags> GetSettingsFlags(ObjectType key)
        {
            return GetObjectSetting(key).poolingFlags;
        }

        private int GetSettingsMaxCount(ObjectType key)
        {
            return GetObjectSetting(key).maxCount;
        }

        private GameObject GetSettingsPrefab(ObjectType key)
        {
            return GetObjectSetting(key).objectPrefab;
        }

        private PooledObjectSetting GetObjectSetting (ObjectType key)
        {
            foreach (PooledObjectSetting objectSetting in poolSettings)
                if (objectSetting.objectType == key)
                    return objectSetting;
            Debug.LogError("[POOLER] Cannot find setting with Object Type: " + key);
            return new PooledObjectSetting();
        }
    }
}