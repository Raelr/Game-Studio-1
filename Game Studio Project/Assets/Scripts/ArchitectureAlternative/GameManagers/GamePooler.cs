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


        public override void Initialise()
        {
            base.Initialise();
        }

        public GameObject RetrieveOrCreate (ObjectType objectType)
        {
            GameObject getObject = Retrieve(objectType, RetrieveMethod.BOTTOM);

            //if it cannot be retrieved because the object pooler requires it to be instantiated
            if (getObject.isNull())
                getObject = Create(objectType);

            //sets the availability to occupied now that the specific object is being used
            SetAvailabilityInPool(objectType, getObject, PoolingAvailability.OCCUPIED);

            return getObject;
        }

        // recycle this object into the pool
        // important: ensure objects you pool have been previously created via the RetrieveOrCreate() method
        public void PoolObject(ObjectType objectType, GameObject obj)
        {
            SetAvailabilityInPool(objectType, obj, PoolingAvailability.POOLED);
            obj.Hide();
        }


        // checks whether it can receive the object from the pool
        private GameObject Retrieve (ObjectType objectType, RetrieveMethod method)
        {
            // retrieve nothing if the pool doesn't even have an entry for that object type
            if (!pool.ContainsKey(objectType))
                return null;

            // retrieve nothing if the pool for that object type hasn't been filled to the brim
            if (GetPoolCount(objectType) < GetSettingsMaxCount(objectType))
                return null;

            switch (method)
            {
                // iterates from bottom to top of the object type's list looking for an object that is 'pooled' to be reused
                case RetrieveMethod.BOTTOM:
                    for (int i = 0; i < pool[objectType].objects.Count; i++)
                    {
                        PooledObjectData pooledObjectData = pool[objectType].objects[i];
                        if (pooledObjectData.availability == PoolingAvailability.POOLED)
                            return pooledObjectData.pooledObject;
                    }
                    Debug.Log("All " + objectType + " objects are currently being used in game");
                    return null;
            }

            // if the pooling method used is undefined / null
            Debug.LogError("Unknown pool retrieving method");
            return null;
        }

        private GameObject Create(ObjectType objectType)
        {
            //instanties the object based off the prefab in the settings
            GameObject newObject = Instantiate(GetSettingsPrefab(objectType));

            //hides the object
            newObject.Hide();

            //adds the object to the pool
            AddToPool(objectType, newObject);

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

            Debug.LogError("Cannot find the object: " + objectKey + " in the list of objects for the entry: " + entry);
            return -1;
        }
        

        // gets current number of objects stored in the pool
        private int GetPoolCount(ObjectType key)
        {
            if (pool.ContainsKey(key))
                return pool[key].objects.Count;
            return 0;
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
            Debug.LogError("Cannot find setting with Object Type: " + key);
            return new PooledObjectSetting();
        }
    }
}