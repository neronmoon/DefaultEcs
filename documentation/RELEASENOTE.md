## DefaultEcs 0.9.1
added debug info for World
added a way to enable/disable an entity without removing it
handled empty struct as special flag case to not waste memory
added a way to enable/disable a component on an entity without removing it

[nuget package](https://www.nuget.org/packages/DefaultEcs/0.9.1)

## DefaultEcs 0.9.0
updated System.Memory reference
added WithAny filter for EntitySetBuilder

Breaking change:
added IsEnabled property on ISystem

[nuget package](https://www.nuget.org/packages/DefaultEcs/0.9.0)

## DefaultEcs 0.8.1
fixed to have metadata documentation for all target frameworks
changed With and Without attribute to accept multiple types

[nuget package](https://www.nuget.org/packages/DefaultEcs/0.8.1)

## DefaultEcs 0.8.0
added SubscribeAttribute to automatically subscribe to decorated methods
changed World.SetMaximumComponentCount return type and added World.GetMaximumComponentCount
broke compatibility with BinarySerializer v0.7.0 produced data

[nuget package](https://www.nuget.org/packages/DefaultEcs/0.8.0)

## DefaultEcs 0.7.0
added IPublisher abstraction on World
fixed IEnumerable<Entity> serialization/deserialization
relaxed World size at creation to allow growth when needed

[nuget package](https://www.nuget.org/packages/DefaultEcs/0.7.0)

## DefaultEcs 0.6.3
reduced cpu usage of multithreading SystemRunner when idling

[nuget package](https://www.nuget.org/packages/DefaultEcs/0.6.3)

## DefaultEcs 0.6.2
made ISystem implements IDisposable
added WithAttribute and WithoutAttribute attributes to define EntitySet from World in AEntitySystem

[nuget package](https://www.nuget.org/packages/DefaultEcs/0.6.2)

## DefaultEcs 0.6.1
added RemoveFromChildrenOf and RemoveFromParentsOf methods on Entity to remove hierarchy of dispose chain
BinarySerializer and TextSerializer now handle abstract types and types with no default constructor
some fixes on BinarySerializer and TextSerializer

[nuget package](https://www.nuget.org/packages/DefaultEcs/0.6.1)

## DefaultEcs 0.6.0
added serialize feature to World and Entity
renamed AEntitySetSystem base class to AEntitySystem
renamed World.SetComponentTypeMaximumCount method to SetMaximumComponentCount
inner improvements

[nuget package](https://www.nuget.org/packages/DefaultEcs/0.6.0)

## DefaultEcs 0.5.0
added base system class to update an EntitySet
added base system class to update a component type on a World
added system class to process custom Action as update
added system class to update sequentially multiple systems
added system class to update in parallel multiple systems
added a way to build entities hierarchy for dispose chain

[nuget package](https://www.nuget.org/packages/DefaultEcs/0.5.0)

## DefaultEcs 0.4.0
Entity.Get is now unsafe: exception is more cryptic if it does not have a component but it's faster so eh
updated System.Memory reference

[nuget package](https://www.nuget.org/packages/DefaultEcs/0.4.0)

## DefaultEcs 0.3.2-alpha
relaxed the need to create EntitySet before creating Entity
added default value to Entity.Set

fixed leak with EntitySetBuilder
fixed Entity.Remove for multiple entity with the same component

[nuget package](https://www.nuget.org/packages/DefaultEcs/0.3.2-alpha)

## DefaultEcs 0.3.1-alpha
fixed refCount bug when disposing Entity

[nuget package](https://www.nuget.org/packages/DefaultEcs/0.3.1-alpha)