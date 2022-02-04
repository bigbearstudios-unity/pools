# BBUnity Pools

## Concepts

### Pool

A Pool is the main container for all of the Pool Definitions. There are two types of Pools, a Static Pool which must be manually spawned and a TimedPool which will spawn automatically after a given amount of time.

### Pool Definition

A Pool Definition is the definition for a single entity which can be spawned from a pool. Its properties differ slightly for Static / Timed Pool but the shared properties are:

- Name(String): The name of the definition, this is used when spawning manually to find the definition.
- Prefab (GameObject): The GameObject which will be used to instantiate the new object.
- Starting Size (Int): The starting size of the pool. How many objects will be instantiated upon Start.
- Maximum Size (Int): The maximum size of the pool. The pool will never spawn more than this amount.
- Use Disabled Instances (Boolean): Should disabled instances be classified as 'avalible'?

### Pool Behaviour

A Pool Behaviour is the component assigned to every object which is spawned by a Pool. You can add this component yourself, or you can allow BBUnity Events do it for you.

## Basic Usage

To get started with BBUnity Pools simply add a StaticPool or TimedPool component onto the GameObject you wish to use as the pool container. Once you have done this expand the "Pool Definitions" tabs see below for the properties for a Static / Timed Pool and how to use each one.

### Static Pool

Static pools have all of the shared properties outlined above, they require manual spawning via the following interface:

``` csharp

/*
 * First you need to get a reference to the pool. You can do this via a variable and the Unity inspector or via the Find method.
 */

StaticPool pool = StaticPool.Find("Pool Name within Unity");

//Spawn an item with no extra params
pool.Spawn("definition_name");

//Spawn with a given position
pool.Spawn("definition_name", position);

//Spawn with a position and scale
pool.Spawn("definition_name", position, scale);

//Spawn with a parent, position, scale
pool.Spawn("definition_name", parent, position, scale);

//Spawn with a before callback
pool.Spawn("definition_name", (PoolBehaviour behaviour) => {  });

//Spawn with a before callback, after callback
pool.Spawn("definition_name", (PoolBehaviour behaviour) => {  }, (PoolBehaviour behaviour) => {  });

//Using a definition directly
StaticPoolDefinition definition = pool.FindPoolDefinition("Definition Name");

definition.Spawn();
definition.Spawn(position);
pool.Spawn((PoolBehaviour behaviour) => {  }, (PoolBehaviour behaviour) => {  });

```

### Timed Pool

## Callbacks

## Best Practises

